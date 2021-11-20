using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay.UI.UIFramework;
using GamePlay.UI.UtilUI;
using PokemonCore;
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
        TestMainMenu();
    }

    public void TestMainMenu()
    {
        _input.Player.Menu.started += (o) =>
        {
            TrainerBag bag = new TrainerBag();
            bag.Add((Item.Tag.PokeBalls,0),2);
            Game.bag = bag;
            UIManager.Instance.Show<MainMenuUI>();
        };
    }

    void Update()
    {
        
    }
}
