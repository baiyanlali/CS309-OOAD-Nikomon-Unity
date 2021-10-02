using System;
using System.Collections;
using System.Collections.Generic;
using PokemonCore;
using PokemonCore.Combat;
using UnityEngine;
using Debug = PokemonCore.Debug;

public class BattleHandler : MonoBehaviour
{
    public List<CombatPokemon> userPokemons => battle?.MyPokemons;
    public List<CombatPokemon> Pokemons => battle?.Pokemons;
    public List<CombatPokemon> AlliesPokemons => battle?.alliesPokemons;
    public List<CombatPokemon> OpponentPokemons => battle?.opponentsPokemons;

    public CombatPokemon CurrentPokemon=>CurrentMyPokemonIndex>=userPokemons.Count?null:userPokemons[CurrentMyPokemonIndex];

    [SerializeField]
    public Battle battle;

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

    private int CurrentMyPokemonIndex = 0;
    public void StartBattle(Battle battle)
    {
        this.battle = battle;
        battle.OnThisTurnEnd += OnTurnEnd;
        battle.OnTurnBegin += OnTurnBegin;
        battle.OnPokemonChooseHandled += OnPokemonChooseHandled;
        BattleUIHandler.Instance.Init(this);
        BattleFieldHandler.Instance.Init(AlliesPokemons,OpponentPokemons);
        OnTurnBegin();
    }

    public void OnTurnEnd()
    {
        UnityEngine.Debug.Log("Turn End");
        BattleUIHandler.Instance.UpdateUI(this);
    }
    
    public void OnTurnBegin()
    {
        UnityEngine.Debug.Log("Your move");

        CurrentMyPokemonIndex = 0;
        BattleUIHandler.Instance.ShowMoves();
        print($"Current Pokemon Index: {CurrentMyPokemonIndex}");
    }

    //TODO: 增加当宝可梦回合跳过
    public void OnPokemonChooseHandled(int combatID)
    {
        
    }

    public void ReceiveInstruction(Instruction instruction)
    {
        if (CurrentMyPokemonIndex + 1 == userPokemons.Count)
        {
            battle.ReceiveInstruction(instruction);//很多时候用了这个命令就直接进入到下一个TurnEnd和TurnBegin了，所以要提前考虑

        }
        else
        {
            battle.ReceiveInstruction(instruction);
            CurrentMyPokemonIndex++;

            BattleUIHandler.Instance.ShowMoves();
        }
    }
    public void EndBattle()
    {
    }
}