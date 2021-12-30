using System.Collections;
using System.Collections.Generic;
using GamePlay.UI;
using GamePlay.UI.UIFramework;
using UnityEngine;

public class ConnectTest : MonoBehaviour
{
    void Start()
    {
        GlobalManager.Instance.LoadAllSaveData();
        UIManager.Instance.Show<ConnectPanel>();
        
    }
}
