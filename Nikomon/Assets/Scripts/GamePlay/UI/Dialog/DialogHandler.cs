using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using PokemonCore.Combat;
using UnityEngine;
using UnityEngine.UI;

public class DialogHandler : MonoBehaviour
{
    public enum FadeType
    {
        Button,
        Automatic
    }

    public static DialogHandler Instance
    {
        get
        {
            if (sInstance != null) return sInstance;
            sInstance = FindObjectOfType<DialogHandler>();
            return sInstance;
        }
    }

    public Text DialogText;
    public NicomonInputSystem nicoInput;
    public GameObject ShowNext;
    private static DialogHandler sInstance;
    private Queue<string> reports;
    private string currentReport;
    private bool isDrawing = false;
    public Action<string> OnDialogFinished;

    private void Start()
    {
        sInstance = this;
        gameObject.SetActive(false);
    }

    public void InitBattle(BattleReporter br)
    {
        nicoInput = nicoInput ? nicoInput : FindObjectOfType<NicomonInputSystem>();

        
        if(reports==null)
            emptied = true;
        if (reports == null)
            reports = new Queue<string>();
        if (DialogText == null)
        {
            DialogText = gameObject.GetComponentInChildren<Text>();
        }

        if (ShowNext == null)
        {
            ShowNext = transform.Find("ShowNext").gameObject;
        }

        if (currentReport == null || string.IsNullOrEmpty(currentReport))
            currentReport = "";

        br.OnReport = OnReport;

#if UNITY_EDITOR
        br.OnReport += ConsoleDebug.Console.SendUDP;
#endif
        // gameObject.SetActive(false);
        isDrawing = false;

    }

    private bool emptied=false;
    private void OnReport(string report)
    {
        reports.Enqueue(report);
        if (emptied)
        {
            emptied = false;
            currentReport = reports.Dequeue();
            gameObject.SetActive(true);
            StartDialog(currentReport);
        }
    }

    private void StartDialog(string str)
    {
        StartCoroutine(DrawDialog(str));
    }

    private void StartDialogImmediate(string str)
    {
        StopAllCoroutines();
        DialogText.text = str;
        isDrawing = false;
    }

    private bool isAccept;

    private void Update()
    {
        if (nicoInput)
        {
            if (isAccept && !nicoInput.accept)
            {
                if (isDrawing)
                {
                    StartDialogImmediate(currentReport);
                }
                else
                {
                    if (reports.Count == 0)
                    {
                        OnDialogFinished?.Invoke("");
                        gameObject.SetActive(false);
                        emptied = true;
                        return;
                    }

                    currentReport = reports.Dequeue();
                    StartDialog(currentReport);
                }
            }

            isAccept = nicoInput.accept;
        }

        ShowNext?.SetActive(!isDrawing);
    }

    private IEnumerator DrawDialog(string str)
    {
        isDrawing = true;
        StringBuilder sb = new StringBuilder();
        int index = 0;
        while (index < str.Length)
        {
            sb.Append(str[index]);
            DialogText.text = sb.ToString();
            yield return new WaitForSeconds(0.1f);
            index++;
        }

        isDrawing = false;
    }
}