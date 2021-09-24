using System.Collections;
using System.Collections.Generic;
using PokemonCore;
using PokemonCore.Combat;
using UnityEngine;

public class BattleHandler : MonoBehaviour
{
    public List<CombatPokemon> userPokemons => battle == null ? null : battle.MyPokemons;
    public List<CombatPokemon> Pokemons => battle == null ? null : battle.Pokemons;
    public List<CombatPokemon> AlliesPokemons => battle == null ? null : battle.alliesPokemons;
    public List<CombatPokemon> OpponentPokemons => battle == null ? null : battle.opponentsPokemons;

    public CombatPokemon CurrentPokemon;
    
    private Battle battle;

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
        this.battle = battle;
        CurrentPokemon = userPokemons[0];
        BattleUIHandler.Instance.Init(this);
        BattleUIHandler.Instance.ShowMoves();
    }

    public void EndBattle()
    {
    }
}