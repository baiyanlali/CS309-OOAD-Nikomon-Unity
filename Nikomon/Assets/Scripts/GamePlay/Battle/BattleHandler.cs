using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GamePlay.UI.BattleUI;
using GamePlay.UI.UIFramework;
using GamePlay.UI.UtilUI;
using PokemonCore;
using PokemonCore.Combat;
using UnityEngine;
using Utility;
using Debug = PokemonCore.Debug;

public class BattleHandler : MonoBehaviour
{
    public List<CombatPokemon> userPokemons => battle?.MyPokemons;
    public List<CombatPokemon> Pokemons => battle?.Pokemons;
    public List<CombatPokemon> AlliesPokemons => battle?.alliesPokemons;
    public List<CombatPokemon> OpponentPokemons => battle?.opponentsPokemons;

    public Battle battle => Game.battle;

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
            BattleFieldHandler.Instance.OnPokemonFainting(combatPoke);
            if(Game.battle.MyPokemons.Contains(combatPoke))
                UIManager.Instance.Show<BattleMenuPanel>(new List<bool>() {false, true, false, true});
        };
        battle.OnReplacePokemon += (p1, p2) =>
        {
            // BattleUIHandler.Instance.OnReplacePokemon(p1,p2);
            UIManager.Instance.Refresh<BattleStatusPanel>();
            BattleFieldHandler.Instance.OnReplacePokemon(p1, p2);
        };
        // UIManager.Instance.Show<BattleUIPanel>(this);

        Game.battleReporter.OnReport += (o) =>
        {
            // print(">>>>>>>>>>Show Dialogue Panel<<<<<<<");
            print($"Dialogue Panel: {o}");
            UIManager.Instance.Show<DialogPanel>(o);
        };
        // BattleUIHandler.Instance.Init(this);
        BattleFieldHandler.Instance.Init(AlliesPokemons, OpponentPokemons);

        // DialogHandler.Instance.OnDialogFinished += (o) => { if(Game.battle!=null) BattleUIHandler.Instance.UpdateUI(this);};


        battle.OnMove += OnMove;
        battle.OnHit += OnHit;
        battle.OnHitted += OnHitted;
        battle.OnOneMoveEnd += () => { BattleFieldHandler.Instance.OnOneMoveEnd(); };

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
            if(statusPanel!=null)
                statusPanel.ActivePokemon(poke);
            else
            {
                UIManager.Instance.Show<BattleStatusPanel>(this);
                statusPanel = UIManager.Instance.GetUI<BattleStatusPanel>();
                if(statusPanel!=null)
                    statusPanel.ActivePokemon(poke);
            }

            UIManager.Instance.Show<BattleMenuPanel>();
            UIManager.Instance.Refresh<MovePanel>(poke.pokemon.moves.ToList());
        });
    }

    public void EndBattle(BattleResults results)
    {
        print("End Battle");
        UIManager.Instance.PopAllUI(UILayer.NormalUI);
        UIManager.Instance.PopAllUI(UILayer.MainUI);
        GlobalManager.Instance.CanPlayerControlled = true;
        // BattleUIHandler.Instance.EndBattle();
        CurrentPokemon = null;

        BattleFieldHandler.Instance.EndBattle(results);
    }

    public void OnHit(Damage dmg)
    {
        BattleFieldHandler.Instance.OnHit(dmg);
    }

    public void OnHitted(CombatPokemon pokemon)
    {
        BattleFieldHandler.Instance.OnHitted(pokemon);
    }

    public void OnMove(CombatAction move)
    {
        BattleFieldHandler.Instance.OnMove(move);
    }

    public void OnTurnEnd()
    {
        BattleFieldHandler.Instance.OnTurnEnd();
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