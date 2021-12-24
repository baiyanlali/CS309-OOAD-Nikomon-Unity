using System.Collections;
using System.Collections.Generic;
using GamePlay.UI;
using GamePlay.UI.UIFramework;
using UnityEngine;

public class SettingTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GlobalManager.Instance.LoadAllSaveData();
        UIManager.Instance.Show<SettingUI>();
        
    }
    
}
