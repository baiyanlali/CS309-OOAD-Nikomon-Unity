using System;
using System.Collections;
using System.Collections.Generic;
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

    public CombatPokemon CurrentPokemon =>
        CurrentMyPokemonIndex >= userPokemons.Count ? null : userPokemons[CurrentMyPokemonIndex];

    [SerializeField] public Battle battle => Game.battle;

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

    private int CurrentMyPokemonIndex = 0;

    public void StartBattle(Battle battle)
    {
        battle.OnThisTurnEnd += OnTurnEnd;
        battle.OnTurnBegin += OnTurnBegin;
        battle.OnPokemonChooseHandled += OnPokemonChooseHandled;
        battle.OnReplacePokemon += (p1, p2) =>
        {
            BattleUIHandler.Instance.OnReplacePokemon(p1,p2);
            BattleFieldHandler.Instance.OnReplacePokemon(p1, p2);
        };
        BattleUIHandler.Instance.Init(this);
        BattleFieldHandler.Instance.Init(AlliesPokemons, OpponentPokemons);

        DialogHandler.Instance.OnDialogFinished += (o) => { if(Game.battle!=null) BattleUIHandler.Instance.UpdateUI(this);};
        
        OnTurnBegin();
    }
    
    public void EndBattle()
    {
        BattleUIHandler.Instance.EndBattle();
        BattleFieldHandler.Instance.EndBattle();
    }
    

    public void OnTurnEnd()
    {
        UnityEngine.Debug.Log("Turn End");
        
        // EventPool.Schedule(() => { BattleUIHandler.Instance.UpdateUI(this); });
    }

    public void OnTurnBegin()
    {
        UnityEngine.Debug.Log("Your move");

        EventPool.Schedule(() =>
        {
            CurrentMyPokemonIndex = 0;
            BattleUIHandler.Instance.BattleUI.SetActive(true);
            BattleUIHandler.Instance.ShowMoves();
        });
        
        
        // print($"Current Pokemon Index: {CurrentMyPokemonIndex}");
    }

    //TODO: 增加当宝可梦回合跳过
    public void OnPokemonChooseHandled(int combatID)
    {
    }

    public void ReceiveInstruction(Instruction instruction)
    {
        if (CurrentMyPokemonIndex + 1 == userPokemons.Count)
        {
            battle.ReceiveInstruction(instruction, true); //很多时候用了这个命令就直接进入到下一个TurnEnd和TurnBegin了，所以要提前考虑
        }
        else
        {
            battle.ReceiveInstruction(instruction, true);
            CurrentMyPokemonIndex++;

            EventPool.Schedule(() => { BattleUIHandler.Instance.ShowMoves(); });
        }
    }

   
}