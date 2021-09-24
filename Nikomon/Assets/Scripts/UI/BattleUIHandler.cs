using System;
using System.Collections;
using System.Collections.Generic;
using PokemonCore;
using PokemonCore.Attack;
using PokemonCore.Combat;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIHandler : MonoBehaviour
{
    #region BattleUI

    public GameObject PokemonStateUIPrefab;
    public GameObject MoveUIPrefab;

    public GameObject BattleUI;
    public GameObject AlliesState;
    public GameObject OpponentState;
    public GameObject MoveUI;

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
    }

    public void ShowMoves()
    {
        Move[] moves = BattleHandler.Instance.CurrentPokemon.pokemon.moves;

        for (int i = 0; i < moves.Length; i++)
        {
            if (moves[i] == null) break;
            MoveUI.transform.GetChild(i).GetComponentInChildren<Text>().text = moves[i]._baseData.innerName;
        }
    }
}