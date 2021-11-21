using System;
using System.Collections;
using System.Collections.Generic;
using PokemonCore;
using PokemonCore.Attack;
using PokemonCore.Attack.Data;
using PokemonCore.Combat;
using PokemonCore.Inventory;
using PokemonCore.Utility;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utility;
using Debug = PokemonCore.Debug;


public class BattleUIHandler : MonoBehaviour
{
    #region BattleUI

    public GameObject PokemonStateUIPrefab;
    public GameObject MoveUIPrefab;

    public GameObject BattleUI;
    public GameObject AlliesState;
    public GameObject OpponentState;
    public GameObject MoveUI;

    public TargetChooserHandler TargetChooserHandler;

    #endregion


    public static BattleUIHandler Instance
    {
        get
        {
            if (s_Instance != null) return s_Instance;
            s_Instance = FindObjectOfType<BattleUIHandler>();
            if (s_Instance != null) return s_Instance;
            throw new Exception("No BattleUI Found");
        }
    }

    private static BattleUIHandler s_Instance;

    public void Init(BattleHandler bh)
    {
        gameObject.SetActive(true);
        s_Instance = this;
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }

        List<CombatPokemon> allies = bh.AlliesPokemons;
        List<CombatPokemon> opponents = bh.OpponentPokemons;

        InitStateUI(allies, AlliesState.transform);
        InitStateUI(opponents, OpponentState.transform);


        if (MoveUI.transform.childCount == 1)
        {
            for (int i = 0; i < Game.MaxMovesPerPokemon; i++)
            {
                GameObject state = Instantiate(MoveUIPrefab, MoveUI.transform);
                state.GetComponentInChildren<Text>().text = "";
            }
        }


        PokemonChooserTableUI.Instance.Init(Game.trainer, new[] {"Switch Pokemon", "View Ability", "Items", "Cancel"},
            new Action<int>[] {SwitchPokemon, ViewAbility, ShowItems, ShowBattleMenu});

        TargetChooserHandler.Init(opponents, allies);

        // dialogManager.dialogManagerIn.InitBattle(Game.battleReporter);
        DialogHandler.Instance.InitBattle(Game.battleReporter);

        EventSystem.current.SetSelectedGameObject(BattleUI.gameObject.transform.GetChild(0).gameObject);
    }


    /// <summary>
    /// 目前战斗UI界面中已经有了部分素材，下次战斗时，会有几种情况
    /// 1.宝可梦数量等于上一局，什么都不用改
    /// 2.宝可梦数量多于上一局，那就多Instantiate
    /// 3.宝可梦数量少于上一局，那就Hide起来
    /// 可以写得更简洁，但是我懒
    /// </summary>
    /// <param name="pokemons"></param>
    /// <param name="parent"></param>
    private void InitStateUI(List<CombatPokemon> pokemons, Transform parent)
    {
        if (pokemons.Count == parent.transform.childCount)
        {
            for (int i = 0; i < pokemons.Count; i++)
            {
                parent.transform.GetChild(i).GetComponent<BattlePokemonStateUI>().Init(pokemons[i]);
            }
        }
        else if (pokemons.Count > parent.transform.childCount)
        {
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                parent.transform.GetChild(i).GetComponent<BattlePokemonStateUI>().Init(pokemons[i]);
            }

            for (int i = parent.transform.childCount; i < pokemons.Count; i++)
            {
                GameObject state = Instantiate(PokemonStateUIPrefab, parent.transform);
                state.name = pokemons[i].Name;
                state.GetComponent<BattlePokemonStateUI>().Init(pokemons[i]);
            }
        }
        else if (pokemons.Count < parent.transform.childCount)
        {
            for (int i = 0; i < pokemons.Count; i++)
            {
                parent.transform.GetChild(i).GetComponent<BattlePokemonStateUI>().Init(pokemons[i]);
            }

            for (int i = pokemons.Count; i < parent.transform.childCount; i++)
            {
                parent.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }


    public void EndBattle()
    {
        // dialogManager.dialogManagerIn.EndBattle();
        this.gameObject.SetActive(false);
    }

    public void UpdateUI(BattleHandler bh)
    {
        ShowBattleMenu();
        int count = AlliesState.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            AlliesState.transform.GetChild(i).GetComponent<BattlePokemonStateUI>()?.UpdateState();
        }

        count = OpponentState.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            OpponentState.transform.GetChild(i).GetComponent<BattlePokemonStateUI>()?.UpdateState();
        }
    }

    public void UpdateStatus(Action onComplete = null)
    {
        // ShowBattleMenu();
        int count = AlliesState.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            var ui = AlliesState.transform.GetChild(i).GetComponent<BattlePokemonStateUI>();
            ui.UpdateState();
        }

        count = OpponentState.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            var ui = OpponentState.transform.GetChild(i).GetComponent<BattlePokemonStateUI>();

            ui.UpdateState();
        }
        
        onComplete?.Invoke();
    }

    public void UpdateStatus(CombatPokemon poke, int hp, Action onComplete)
    {
        ShowBattleMenu();
        int count = AlliesState.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            var ui = AlliesState.transform.GetChild(i).GetComponent<BattlePokemonStateUI>();
            if (ui.pokemon.CombatID == poke.CombatID)
            {
                ui.UpdateState(hp, onComplete);
            }
        }

        count = OpponentState.transform.childCount;
        for (int i = 0; i < count; i++)
        {
            var ui = OpponentState.transform.GetChild(i).GetComponent<BattlePokemonStateUI>();
            if (ui.pokemon.CombatID == poke.CombatID)
            {
                ui.UpdateState(hp, onComplete);
            }
        }
    }

    public void OnReplacePokemon(CombatPokemon p1, CombatPokemon p2)
    {
        gameObject.SetActive(true);
        // for (int i = 0; i < transform.childCount; i++)
        // {
        //     transform.GetChild(i).gameObject.SetActive(true);
        // }

        List<CombatPokemon> allies = BattleHandler.Instance.AlliesPokemons;
        List<CombatPokemon> opponents = BattleHandler.Instance.OpponentPokemons;

        InitStateUI(allies, AlliesState.transform);
        InitStateUI(opponents, OpponentState.transform);

        // PokemonChooserTableUI.Instance.Init(Game.trainer, new[] {"Switch Pokemon", "View Ability", "Items", "Cancel"},
        // new Action<int>[] {SwitchPokemon, ViewAbility, ShowItems, ShowBattleMenu});

        TargetChooserHandler.Init(opponents, allies);
    }


    public void ShowBattleMenu(int o)
    {
        ShowBattleMenu();
    }

    public void ShowBattleMenu()
    {
        BattleUI.SetActive(true);
        MoveUI.SetActive(false);
        PokemonChooserTableUI.Instance.gameObject.SetActive(false);
        DialogChooserUI.Instance.gameObject.SetActive(false);
    }

    private CombatPokemon currentPoke;

    public void ShowMoves(CombatPokemon poke)
    {
        currentPoke = poke;
        if (poke.HP <= 0)
        {
            BattleUI.SetActive(true);
            BattleUI.transform.Find("Fight").gameObject.SetActive(false);
            BattleUI.transform.Find("Bag").gameObject.SetActive(false);
            LayoutRebuilder.ForceRebuildLayoutImmediate(BattleUI.GetComponent<RectTransform>());
            return;
        }

        BattleUI.SetActive(true);
        BattleUI.transform.Find("Fight").gameObject.SetActive(true);
        BattleUI.transform.Find("Bag").gameObject.SetActive(true);
        LayoutRebuilder.ForceRebuildLayoutImmediate(BattleUI.GetComponent<RectTransform>());
        Move[] moves = poke.pokemon.moves;

        //有一个back按钮
        for (int i = 0; i < moves.Length; i++)
        {
            if (moves[i] == null)
            {
                MoveUI.transform.GetChild(i + 1).gameObject.SetActive(false);
            }
            else
            {
                GameObject obj = MoveUI.transform.GetChild(i + 1).gameObject;
                obj.SetActive(true);
                obj.GetComponent<MoveUI>().Init(moves[i], i);
            }
        }

        MoveUI.SetActive(false);
    }


    public void ViewAbility(int i)
    {
    }

    public void ShowItems(int i)
    {
    }

    public void ChooseMove(Move move, int index)
    {
        //如果技能效果是针对对面宝可梦而且宝可梦只有一个的话
        if (move._baseData.Target == Targets.SELECTED_OPPONENT_POKEMON &&
            BattleHandler.Instance.OpponentPokemons.Count == 1)
        {
            Instruction instruction =
                new Instruction(currentPoke.CombatID, Command.Move, index,
                    BattleHandler.Instance.OpponentPokemons[0].CombatID);
            BuildInstrustruction(instruction);
        }
        else
        {
            TargetChooserHandler.ShowTargetChooser(move._baseData.Target);
            TargetChooserHandler.OnCancelChoose = () => { MoveUI.SetActive(true); };

            MoveUI.SetActive(false);
            TargetChooserHandler.OnChooseTarget = (target) =>
                {
                    UnityEngine.Debug.Log(target.ConverToString());
                    Instruction instruction =
                        new Instruction(currentPoke.CombatID, Command.Move, index,
                            target);
                    BuildInstrustruction(instruction);
                    BattleUI.SetActive(false); //TODO:这里有bug！
                }
                ;
        }
    }

    public void SwitchPokemon(int index)
    {
        ShowBattleMenu();
        UnityEngine.Debug.Log($"Choose switch to index:{index}");
        if (Game.trainer.party[index] == null || Game.trainer.pokemonOnTheBattle[index]) return;
        Instruction ins = new Instruction(currentPoke.CombatID, Command.SwitchPokemon, index,
            null);
        BuildInstrustruction(ins);
    }

    public void UseItem(Item item, int target)
    {
        UseItem(item, new List<int>() {target});
    }

    public void UseItem(Item item, List<int> target)
    {
        target.Insert(0, item.ID);
        Instruction ins = new Instruction(currentPoke.CombatID, Command.Items, (int) item.tag,
            target);
        BuildInstrustruction(ins);
    }

    public void Run()
    {
        Instruction ins = new Instruction(currentPoke.CombatID, Command.Run, Game.trainer.id,
            null);
        BuildInstrustruction(ins);
    }

    public void BuildInstrustruction(Instruction instruction)
    {
        BattleHandler.Instance.ReceiveInstruction(instruction);
    }


    private void Update()
    {
        if (NicomonInputSystem.Instance.back)
        {
            BattleUI.SetActive(true);
            EventSystem.current.SetSelectedGameObject(BattleUI.gameObject.transform.GetChild(0).gameObject);
            MoveUI.SetActive(false);
            TargetChooserHandler.Instance.gameObject.SetActive(false);
            BagUI.Instance.gameObject.SetActive(false);
            PokemonChooserTableUI.Instance.gameObject.SetActive(false);
        }
    }
}