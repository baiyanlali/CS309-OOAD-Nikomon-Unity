using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class dialogManager : MonoBehaviour
{
    public static dialogManager dialogManagerIn;
    private string[] dialogList;
    private string[] choiceList;
    private dialogEngine de;
    private int nextNode = -1;

    private void Awake()
    {
        dialogManagerIn = this;
        de = transform.GetComponent<dialogEngine>();
        dialogManagerIn = this;
        initialList();
    }
    public static dialogManager getDialogManager()
    {
        return dialogManagerIn;
    }

    /// <summary>
    /// display certain dialogNode in dialog
    /// </summary>
    /// <param name="dialogId"></param>
    public void onDialogId()
    {
        if (nextNode != -1)
            onDialogId(nextNode);
        else
            offDialog();
    }
    public void onDialogId(int dialogId)
    {
        dialogNode dnTemp = readNode(dialogId);
        onDialogText(dnTemp.content, dnTemp.style);
        if (dnTemp.invokeChoice)
        {

        }
        else
        {
            if (dnTemp.nodeIdList.Count == 0)
            {
                nextNode = -1;
            }
            else
            {
                nextNode = dnTemp.nodeIdList[0];
            }
        }
    }
    
    /// <summary>
    /// display certain string in dialog
    /// </summary>
    /// <param name="text"></param>
    public void onDialogText(string text)
    {
        onDialogText(text, 0);
    }
    public void onDialogText(string text,int style)
    {
        if (de.dialogBoxOn())
        {
            de.StartCoroutine("DrawText", text);
        }
        else
        {
            de.DrawDialogBox(style);
            de.StartCoroutine("DrawText", text);
        }
    }

    /// <summary>
    /// close the dialog with the fade Effect
    /// </summary>
    public void offDialog()
    {
        de.StartCoroutine("fadeEffect", 0);
    }
    
    private void initialList()
    {
        dialogList = File.ReadAllLines("Assets/Resources/DialogTexts/dialogContent.txt");
        choiceList = File.ReadAllLines("Assets/Resources/DialogTexts/choiceContent.txt");
    }
    private dialogNode readNode(int id)
    {
        string[] m_list = dialogList[id].Split('^');
        dialogNode dn = new dialogNode(m_list[0], m_list[1], m_list[2], m_list[3], m_list[4]);

        return dn;
    }


}
