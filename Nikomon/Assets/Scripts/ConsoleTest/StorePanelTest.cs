using System.Collections;
using System.Collections.Generic;
using GamePlay.UI.BagSystem;
using GamePlay.UI.UIFramework;
using UnityEngine;

public class StorePanelTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UIManager.Instance.Show<StorePanelUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
