using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GamePlay;
using GamePlay.UI.UIFramework;
using PokemonCore;
using PokemonCore.Attack.Data;
using PokemonCore.Combat;
using UnityEngine;

public class MoveLearningTest : MonoBehaviour
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
            
        wyf.Exp.AddExperience(500);
        game.AddPokemon(wyf);
        game.AddPokemon(AIfanfan);
        game.AddPokemon(AIfanfan2);
        game.AddPokemon(AIfanfan);
        game.AddPokemon(AIfanfan3);
        var movedatas = Game.MovesData.Keys.ToArray();
        var movedata = Game.MovesData.Values.ToArray();
        for (int i = 0; i < Game.MaxMovesPerPokemon; i++)
        {
            AIfanfan.AddMove(movedatas[i]);
        }


        //StartCoroutine(StartTest());
        UIManager.Instance.Show<MovelearningUI>(AIfanfan,movedata[4],(Action)(() => { }));

    }
}
