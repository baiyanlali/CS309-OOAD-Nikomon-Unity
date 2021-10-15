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


    public void StartBattle(Battle battle)
    {
        battle.OnThisTurnEnd += OnTurnEnd;
        battle.OnTurnBegin += OnTurnBegin;
        battle.ShowPokeMove += ShowPokeMove;
        battle.OnPokemonFainting += (combatPoke) =>
        {
            BattleFieldHandler.Instance.OnPokemonFainting(combatPoke);
        };
        battle.OnReplacePokemon += (p1, p2) =>
        {
            BattleUIHandler.Instance.OnReplacePokemon(p1,p2);
            BattleFieldHandler.Instance.OnReplacePokemon(p1, p2);
        };
        BattleUIHandler.Instance.Init(this);
        BattleFieldHandler.Instance.Init(AlliesPokemons, OpponentPokemons);

        DialogHandler.Instance.OnDialogFinished += (o) => { if(Game.battle!=null) BattleUIHandler.Instance.UpdateUI(this);};

        battle.OnMove += OnMove;
        battle.OnHit += OnHit;
        
        
        // OnTurnBegin();
        
        GlobalManager.Instance.CompleteBattleInit();
    }

    public void ShowPokeMove(CombatPokemon poke)
    {
        print("show move!");
        EventPool.Schedule(() => { BattleUIHandler.Instance.ShowMoves(poke);});
    }
    
    public void EndBattle()
    {
        BattleUIHandler.Instance.EndBattle();
        BattleFieldHandler.Instance.EndBattle();
    }

    public string OnHit(Damage dmg)
    {
        
        return null;
    }

    public void OnMove(CombatMove move)
    {
        BattleFieldHandler.Instance.OnMove(move);

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
            BattleUIHandler.Instance.BattleUI.SetActive(true);
        });
        
        
        // print($"Current Pokemon Index: {CurrentMyPokemonIndex}");
    }



    public void ReceiveInstruction(Instruction instruction)
    {
        battle.ReceiveInstruction(instruction, true);
    }

   
}