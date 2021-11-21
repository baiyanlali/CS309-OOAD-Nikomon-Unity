using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuManager : MonoBehaviour
{
    private void Start()
    {
        
    }


    public void StartGameWithSlot(int slot)
    {
        GlobalManager.Instance.LoadSaveData(slot);
        //SceneManager.LoadSceneAsync(1);
        SceneTransmitor.LoadSceneID(1);
    }

    public void StartGameWithNew(Text text)
    {
        if (string.IsNullOrWhiteSpace(text.text))
        {
            return;
        }
        GlobalManager.Instance.game.CreateNewSaveFile(text.text,false);
        //SceneManager.LoadSceneAsync(1);
        SceneTransmitor.LoadSceneID(1);
    }

    
}
