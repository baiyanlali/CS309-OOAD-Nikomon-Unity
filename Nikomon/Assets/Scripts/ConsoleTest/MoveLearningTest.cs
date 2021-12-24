using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        Pokemon AIfanfan = new Pokemon(7, 50);
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


        //StartCoroutine(StartTest());
        //UIManager.Instance.Show<MovelearningUI>(wyf,);

    }
}
