using System.Collections;
using System.Collections.Generic;
using System.IO;
using PokemonCore.Combat;
using UnityEngine;

public class dialogManager : MonoBehaviour
{

    //对话框消失的方式，目前分为按Accept消失和按时间Automatic消失两种
    
    public static dialogManager dialogManagerIn;
    private string[] dialogList;
    private dialogEngine de;

    private void Awake()
    {
        dialogManagerIn = this;
        de = transform.GetComponent<dialogEngine>();
        this.gameObject.SetActive(false);
    }

    public void InitBattle(BattleReporter br)
    {
        br.OnReport += onDialogText;
    }

    public void EndBattle()
    {
        gameObject.SetActive(false);
    }
    
    //display battle discription
    public void onDialogText(string text)
    {
        gameObject.SetActive(true);
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

    public IEnumerator textDraw(string text)
    {
        de.DrawDialogBox();
        de.StartCoroutine("DrawText", text);
        yield return null;
    }
    public IEnumerator textUnDraw()
    {
        de.StartCoroutine("fadeEffect",0);
        yield return null;
    }
}
