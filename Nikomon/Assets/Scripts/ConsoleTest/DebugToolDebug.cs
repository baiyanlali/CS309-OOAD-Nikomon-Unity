using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GamePlay.UI.UIFramework;

public class DebugToolDebug : MonoBehaviour
{
    private void Start()
    {
        NicomonInput input = new NicomonInput();
        input.Enable();

        input.Player.Menu.started += (o) =>
        {
            UIManager.Instance.Show<DebugPanel>();   
        };
    }
}
