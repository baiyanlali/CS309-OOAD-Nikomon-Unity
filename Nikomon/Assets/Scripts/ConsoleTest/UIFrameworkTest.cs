using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay.UI.UIFramework;
using GamePlay.UI.UtilUI;
using PokemonCore;
using PokemonCore.Combat;
using PokemonCore.Inventory;
using UnityEngine;

public class UIFrameworkTest : MonoBehaviour
{
    private NicomonInput _input;
    void Start()
    {
        print("start");
        _input = new NicomonInput();
        _input.Enable();
        GlobalManager.Instance.CanPlayerControlled=false;
        TestMainMenu();
    }

<<<<<<< HEAD
=======

    public void TestBattleMenu()
    {
        _input.Player.Menu.started += (o) =>
        {
            // UIManager.Instance.Show<BattleUIPanel>();
        };
    }
    
>>>>>>> parent of 89f344b5 (重要UI组件基本重写完毕，仅剩Dialogue)
    public void TestMainMenu()
    {
        _input.Player.Menu.started += (o) =>
        {
            TrainerBag bag = new TrainerBag();
            bag.Add((Item.Tag.PokeBalls,0),2);
            Game.bag = bag;
            Game.Random = new System.Random();
            Game.trainer = new Trainer("Balala", true);
            Game.trainer.AddPokemon(new Pokemon(1,30));
            Game.trainer.AddPokemon(new Pokemon(7,40));
            Game.trainer.AddPokemon(new Pokemon(17,30));
            UIManager.Instance.Show<MainMenuUI>();
        };
    }

    void Update()
    {
        
    }
}
