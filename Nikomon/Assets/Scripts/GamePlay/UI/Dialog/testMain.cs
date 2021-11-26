using System.Collections;
using GamePlay.UI.Dialog;
using GamePlay.UI.UIFramework;
using GamePlay.UI.UtilUI;
using UnityEngine;

public class testMain : MonoBehaviour
{

    void Start()
    {
       // dm = DialogManager.dialogManagerIn;
       //UIManager.Instance.Init();
       
       UIManager.Instance.Show<DialoguePanel>("test content");
       //UIManager.Instance.Show<ConfirmPanel>();
    }


    

}
