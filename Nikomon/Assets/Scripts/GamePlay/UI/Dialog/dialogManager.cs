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


    public void onDialogId(int dialogId)
    {
        onDialogId(dialogId, 0);
    }
    public void onDialogId(int dialogId, int style)
    {
        
    }
    //display battle discription
    public void onDialogText(string text)
    {
        onDialogText(text, 0);
    }
    public void onDialogText(string text,int style)
    {
        StartCoroutine(textDraw(text));
    }
    public void offDialog()
    {
        StartCoroutine(textUnDraw());
    }
    
    private void initialList()
    {
        //dialogList = File.ReadAllLines("dialogContent");
        //choiceList = File.ReadAllLines("choiceContent");
    }
    private void readFile()
    {

    }

    public IEnumerator textDraw(string text)
    {
        de.DrawDialogBox();
        de.StartCoroutine("DrawText", text);
        yield return null;
    }
    public IEnumerator textUnDraw()
    {
        de.fadeEffect(0,7f);
        yield return null;
    }
}
