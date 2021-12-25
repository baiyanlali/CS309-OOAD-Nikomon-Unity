using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GamePlay;
using GamePlay.UI.UIFramework;
using PokemonCore;
using PokemonCore.Attack.Data;
using PokemonCore.Combat;
using PokemonCore.Monster.Data;
using UnityEngine;

public class EvolutionTest : MonoBehaviour
{
    void Start()
    {
        GlobalManager.Instance.LoadAllSaveData();
        Trainer trainer = new Trainer("JDY", true);
        Pokemon wyf = new Pokemon(3, 50);
        Pokemon AIfanfan = new Pokemon(3, 50);
        Pokemon AIfanfan2 = new Pokemon(1, 30);
        Pokemon AIfanfan3 = new Pokemon(89, 30);
        // MoveData newmove = new MoveData()
        Game game = new Game();
        Game.trainer = trainer;
        
        //StartCoroutine(StartTest());
        UIManager.Instance.Show<EvolutionPanel>(AIfanfan2._base,AIfanfan._base);

    }
}