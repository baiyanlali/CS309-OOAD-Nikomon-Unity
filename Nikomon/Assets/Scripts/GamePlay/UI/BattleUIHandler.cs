using System;
using System.Collections;
using System.Collections.Generic;
using PokemonCore;
using PokemonCore.Attack;
using PokemonCore.Attack.Data;
using PokemonCore.Combat;
using PokemonCore.Utility;
using UnityEditor.Experimental;
using UnityEngine;
using UnityEngine.Events;
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

    private BattleHandler battleHandler;

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
        // s_Instance = this;
        battleHandler = bh;
        List<CombatPokemon> allies = bh.AlliesPokemons;
        List<CombatPokemon> opponents = bh.OpponentPokemons;
        foreach (var ally in allies)
        {
            GameObject state = Instantiate(PokemonStateUIPrefab, AlliesState.transform);
            state.name = ally.Name;
            state.GetComponent<BattlePokemonStateUI>().Init(ally);
        }

        foreach (var opponent in opponents)
        {
            GameObject state = Instantiate(PokemonStateUIPrefab, OpponentState.transform);
            state.name = opponent.Name;
            state.GetComponent<BattlePokemonStateUI>().Init(opponent);
        }

        for (int i = 0; i < Game.MaxMovesPerPokemon; i++)
        {
            GameObject state = Instantiate(MoveUIPrefab, MoveUI.transform);
            state.GetComponentInChildren<Text>().text = "";
        }
        
        
        TargetChooserHandler.Init(opponents,allies);

    }


    public void UpdateUI(BattleHandler bh)
    {
        //因为只能从main thread调用set active，所以换成这种写法
        BattleUI.SetActive(true);
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

    public void ShowMoves()
    {
        Move[] moves = BattleHandler.Instance.CurrentPokemon.pokemon.moves;

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


    public void HideInstruction()
    {
        UnityEngine.Debug.Log("Hide Instruction");
    }

    public void ChooseMove(Move move, int index)
    {
        //如果技能效果是针对对面宝可梦而且宝可梦只有一个的话
        if (move._baseData.Target == Targets.SELECTED_OPPONENT_POKEMON &&
            BattleHandler.Instance.OpponentPokemons.Count == 1)
        {
            Instruction instruction =
                new Instruction(BattleHandler.Instance.CurrentPokemon.CombatID, Command.Move, index,
                    BattleHandler.Instance.OpponentPokemons[0].CombatID);
            BuildInstrustruction(instruction);
        }
        else
        {
            TargetChooserHandler.ShowTargetChooser(move._baseData.Target);
            TargetChooserHandler.OnCancelChoose = () => { MoveUI.SetActive(true); };

            MoveUI.SetActive(false);
            TargetChooserHandler.OnChooseTarget=(target)=>
                {
                    UnityEngine.Debug.Log(target.ConverToString());
                    Instruction instruction =
                    new Instruction(BattleHandler.Instance.CurrentPokemon.CombatID, Command.Move, index,
                        target);
                    BuildInstrustruction(instruction);
                    BattleUI.SetActive(true);//TODO:这里有bug！
                }
                ;
        }
    }


    public void BuildInstrustruction(Instruction instruction)
    {
        BattleHandler.Instance.ReceiveInstruction(instruction);
    }
}