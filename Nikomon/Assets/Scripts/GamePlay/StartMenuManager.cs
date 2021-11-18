using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuManager : MonoBehaviour
{

    

    public void StartGameWithSlot(int slot)
    {
        GlobalManager.Instance.game.LoadSaveFile(slot);
        SceneManager.LoadScene(1);
    }

    public void StartGameWithNew(Text text)
    {
        if (string.IsNullOrWhiteSpace(text.text))
        {
            return;
        }
        GlobalManager.Instance.game.CreateNewSaveFile(text.text,false);
        SceneManager.LoadScene(1);
    }

    
}
