using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay.UI.UIFramework;
using GamePlay.UI.UtilUI;
using UnityEngine;

public class UIFrameworkTest : MonoBehaviour
{
    private NicomonInput _input;
    void Start()
    {
        print("start");
        _input = new NicomonInput();
        _input.Enable();
        _input.Player.Back.started += (o) =>
        {
            print("Back Pressed");
            Action<bool> callback = (o) =>
            {
                print($"receive {o}");
            };
            UIManager.Instance.Show<ConfirmPanel>("Heiheihei",callback);
        };
    }

    void Update()
    {
        
    }
}
