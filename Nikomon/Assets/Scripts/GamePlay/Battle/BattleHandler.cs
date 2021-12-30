using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GamePlay.UI.BattleUI;
using GamePlay.UI.UIFramework;
using GamePlay.UI.UtilUI;
using PokemonCore;
using PokemonCore.Attack.Data;
using PokemonCore.Combat;
using PokemonCore.Monster.Data;
using PokemonCore.Utility;
using UnityEngine;
using UnityEngine.UI;
using Utility;
using Debug = PokemonCore.Debug;

public class BattleHandler : MonoBehaviour
{
    public List<CombatPokemon> userPokemons => battle?.MyPokemons;
    public List<CombatPokemon> Pokemons => battle?.Pokemons;
    public List<CombatPokemon> AlliesPokemons => battle?.alliesPokemons;
    public List<CombatPokemon> OpponentPokemons => battle?.opponentsPokemons;

    public Battle battle => Game.battle;

    public CombatPokemon OppoPoke;

    public static BattleHandler Instance
    {
        get
        {
            if (s_Instance != null) return s_Instance;
            s_Instance = FindObjectOfType<BattleHandler>();
            if (s_Instance != null) return s_Instance;
            s_Instance = CreateBattleHandler();
            return s_Instance;
        }
    }

    private static BattleHandler s_Instance;

    private void Awake()
    {
        if (s_Instance && s_Instance != this)
        {
            Destroy(this.gameObject);
        }

        if (s_Instance == null)
        {
            s_Instance = this;
        }
    }

    private static BattleHandler CreateBattleHandler()
    {
        GameObject obj = GameObject.Find("Global");
        if (obj == null)
        {
            obj = new GameObject("Global");
        }

        s_Instance = obj.AddComponent<BattleHandler>();
        return s_Instance;
    }


    private Queue moveTimeline;

    public void StartBattle(Battle battle)
    {
        battle.OnThisTurnEnd += OnTurnEnd;
        battle.OnTurnBegin += OnTurnBegin;
        battle.ShowPokeMove += ShowPokeMove;
        battle.OnPokemonFainting += (combatPoke) =>
        {
            EventPool.Schedule(() =>
            {
                BattleFieldHandler.Instance.OnPokemonFainting(combatPoke);
                // if (Game.battle.MyPokemons.Contains(combatPoke))
                //     UIManager.Instance.Show<BattleMenuPanel>(new List<bool>() {false, true, false, true});
            });
        };
        battle.OnReplacePokemon += (p1, p2) =>
        {
            EventPool.Schedule(() =>
            {
                // BattleUIHandler.Instance.OnReplacePokemon(p1,p2);
                UIManager.Instance.Refresh<BattleStatusPanel>();
                BattleFieldHandler.Instance.OnReplacePokemon(p1, p2);
            });
        };
        // UIManager.Instance.Show<BattleUIPanel>(this);

        Game.battleReporter.OnReport += (o) =>
        {
            EventPool.Schedule(() =>
            {
                // print(">>>>>>>>>>Show Dialogue Panel<<<<<<<");
                print($"Dialogue Panel: {o}");
                // UIManager.Instance.Show<DialogPanel>(o);
                UIManager.Instance.Show<BattleDialoguePanel>(o);
            });
        };
        // BattleUIHandler.Instance.Init(this);
        BattleFieldHandler.Instance.Init(AlliesPokemons, OpponentPokemons);

        // DialogHandler.Instance.OnDialogFinished += (o) => { if(Game.battle!=null) BattleUIHandler.Instance.UpdateUI(this);};


        battle.OnMove += OnMove;
        battle.OnHit += OnHit;
        battle.OnHitted += OnHitted;
        battle.OnOneMoveEnd += () => { EventPool.Schedule(() => BattleFieldHandler.Instance.OnOneMoveEnd());};
        OppoPoke = OpponentPokemons[0];
        // print("Complete BattleHandler Init");
        // OnTurnBegin();

        GlobalManager.Instance.CompleteBattleInit();
    }

    public CombatPokemon CurrentPokemon;

    public void ShowPokeMove(CombatPokemon poke)
    {
        // print(">>>>>>show move!<<<<<<<");
        // EventPool.Schedule(() => { BattleUIHandler.Instance.ShowMoves(poke);});
        CurrentPokemon = poke;
        EventPool.Schedule(() =>
        {
            var statusPanel = UIManager.Instance.GetUI<BattleStatusPanel>();
            if (statusPanel != null)
                statusPanel.ActivePokemon(poke);
            else
            {
                UIManager.Instance.Show<BattleStatusPanel>(this);
                statusPanel = UIManager.Instance.GetUI<BattleStatusPanel>();
                if (statusPanel != null)
                    statusPanel.ActivePokemon(poke);
            }

            UIManager.Instance.Show<BattleMenuPanel>();
            UIManager.Instance.Refresh<MovePanel>(poke.pokemon.moves.ToList());
        });
    }

    public void EndBattle(BattleResults results)
    {
        print("End Battle");
        // UIManager.Instance.PopAllUI(UILayer.NormalUI);
        // UIManager.Instance.PopAllUI(UILayer.MainUI);
        BattleFieldHandler.Instance.EndBattle(results);
    }

    public void OnBattleFieldEnd(BattleResults results)
    {
        UIManager.Instance.PopAllUI(UILayer.NormalUI);
        UIManager.Instance.PopAllUI(UILayer.MainUI);
        if (results == BattleResults.Succeed || results == BattleResults.Captured)
        {
            List<PokemonLevelUpState> levelUpStates = new List<PokemonLevelUpState>();
            List<(PokemonData, PokemonData, Pokemon)> pokesEvoluting = new List<(PokemonData, PokemonData, Pokemon)>();
            List<(Pokemon, MoveData)> pokesMovesData = new List<(Pokemon, MoveData)>();
            for (int i = 0; i < Game.trainer.party.Length; i++)
            {
                if (Game.trainer.party[i] == null) break;
                Pokemon poke = Game.trainer.party[i];
                levelUpStates.Add(new PokemonLevelUpState()
                {
                    ExpBefore = new Experience(poke.Exp),
                    Pokemon = poke
                });
                var (evolutions, movesData) = Game.trainer.party[i].AddExperience((Game.trainer.party[i].Exp.NextLevelExp-
                    Game.trainer.party[i].Exp.CurLevelExp)/2 + 1);
                //var (evolutions, movesData) = Game.trainer.party[i].AddExperience(500000);
                if (evolutions != null && evolutions.Count > 0)
                {
                    pokesEvoluting.Add((poke._base, Game.PokemonsData[evolutions[0]], poke));
                    // UIManager.Instance.Show<EvolutionPanel>(poke._base,Game.PokemonsData[evolutions[0]]);
                }


                if (movesData != null && movesData.Count > 0)
                {
                    pokesMovesData.Add((poke, movesData[0]));
                }
            }

            if (levelUpStates.Count != 0)
            {

                    UIManager.Instance.Show<SettlementPanel>(levelUpStates, (Action) (() =>
                    {
                        if (pokesEvoluting.Count != 0)
                        {
                            void ShowEvolution()
                            {
                                if (pokesEvoluting.Count == 0)
                                {
                                    UIManager.Instance.PopAllUI(UILayer.MainUI);
                                    UIManager.Instance.PopAllUI(UILayer.NormalUI);
                                    UIManager.Instance.PopAllUI(UILayer.PopupUI);
                                    GlobalManager.Instance.CanPlayerControlled = true;
                                    // BattleUIHandler.Instance.EndBattle();
                                    CurrentPokemon = null;
                                    return;
                                }
                                print(111);

                                
                                var evolve = pokesEvoluting[0];
                                pokesEvoluting.RemoveAt(0);

                                UIManager.Instance.Show<EvolutionPanel>(evolve.Item1, evolve.Item2, (Action) (() =>
                                {
                                    evolve.Item3.Evolve(evolve.Item2);
                                    ShowEvolution();
                                }));
                            }

                            ShowEvolution();
                        }
                        else
                        {
                            void ShowMoves()
                            {
                                if (pokesMovesData.Count == 0)
                                {
                                    UIManager.Instance.PopAllUI(UILayer.MainUI);
                                    UIManager.Instance.PopAllUI(UILayer.NormalUI);
                                    UIManager.Instance.PopAllUI(UILayer.PopupUI);
                                    GlobalManager.Instance.CanPlayerControlled = true;
                                    // BattleUIHandler.Instance.EndBattle();
                                    CurrentPokemon = null;
                                    return;
                                }

                                var movesData = pokesMovesData[0];
                                pokesMovesData.RemoveAt(0);
                                if (movesData.Item1.MoveCount() < Game.MaxMovesPerPokemon)
                                {
                                    movesData.Item1.AddMove(movesData.Item2);
                                }
                                else
                                {
                                    UIManager.Instance.Show<MovelearningUI>(movesData.Item1, movesData.Item2,
                                        (Action) ShowMoves);
                                }
                            }

                            ShowMoves();
                        }
                        // GlobalManager.Instance.CanPlayerControlled = true;
                        // // BattleUIHandler.Instance.EndBattle();
                        // CurrentPokemon = null;
                    }));
                
                
            }
            
        }
        else
        {
            UIManager.Instance.PopAllUI(UILayer.MainUI);
            UIManager.Instance.PopAllUI(UILayer.NormalUI);
            GlobalManager.Instance.CanPlayerControlled = true;
            // BattleUIHandler.Instance.EndBattle();
            CurrentPokemon = null;
        }
    }

    public void OnHit(Damage dmg)
    {
        EventPool.Schedule(() =>
        {
            BattleFieldHandler.Instance.OnHit(dmg);
        });
    }

    public void OnHitted(CombatPokemon pokemon)
    {
        EventPool.Schedule(() =>
        {
            BattleFieldHandler.Instance.OnHitted(pokemon);
        });
    }

    public void OnMove(CombatAction move)
    {
        EventPool.Schedule(() =>
        {
            BattleFieldHandler.Instance.OnMove(move);
        });
    }

    public void OnTurnEnd()
    {
        EventPool.Schedule(() =>
        {
            BattleFieldHandler.Instance.OnTurnEnd();
        });
        // UnityEngine.Debug.Log("Turn End");

        // EventPool.Schedule(() => { BattleUIHandler.Instance.UpdateUI(this); });
    }

    public void OnTurnBegin()
    {
        // UnityEngine.Debug.Log("Your move");

        EventPool.Schedule(() =>
        {
            // BattleUIHandler.Instance.BattleUI.SetActive(true);
            UIManager.Instance.Show<BattleMenuPanel>();
        });


        // print($"Current Pokemon Index: {CurrentMyPokemonIndex}");
    }


    public void ReceiveInstruction(Instruction instruction)
    {
        battle.ReceiveInstruction(instruction, true);
    }
}