using GamePlay.UI;
using GamePlay.UI.UIFramework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuManager : MonoBehaviour
{
    private void Start()
    {
        UIManager.Instance.Show<StartMenuUI>();
    }


    public static void StartGameWithSlot(int slot)
    {
        GlobalManager.Instance.LoadSaveData(slot);
        //SceneManager.LoadSceneAsync(1);
        SceneTransmitor.LoadSceneID(1);
    }

    public static void StartGameWithNew(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            print("No name given");
            return;
        }
        GlobalManager.Instance.game.CreateNewSaveFile(text,false);
        //SceneManager.LoadSceneAsync(1);
        // print("start");
        SceneTransmitor.LoadSceneID(1);
        
    }

    
}
