using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DialogChooserUI : MonoBehaviour
{
    public static DialogChooserUI Instance
    {
        get
        {
            if (sInstance != null) return sInstance;
            sInstance = FindObjectOfType<DialogChooserUI>();
            if (sInstance != null) return sInstance;
            CreateDialogChooserUI();
            return sInstance;
        }
    }

    private static DialogChooserUI sInstance;
    private Transform Content;


    private static void CreateDialogChooserUI()
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/UI/DialogChooser"));
        sInstance = obj.GetComponent<DialogChooserUI>();
    }

    private void Start()
    {
        sInstance = this;
        gameObject.SetActive(false);
    }

    public void ShowChooser(string[] chooser,Action<int> OnChoose=null, RectTransform rectTransform = null)
    {
        // UnityEngine.Debug.Log("Show Chooser!");
        gameObject.SetActive(true);

        if (Content == null)
        {
            Content = transform.Find("Content");
        }
        if (rectTransform != null)
        {
            GetComponent<RectTransform>().position = rectTransform.position;
        }


        if (chooser.Length > Content.childCount)
        {
            GameObject obj = Resources.Load<GameObject>("Prefabs/UI/ChooseElement");

            for (int i = Content.childCount; i < chooser.Length; i++)
            {
                var btn = Instantiate(obj, Content);
            }
        }
        else if (chooser.Length < Content.childCount)
        {
            for (int i = chooser.Length; i < Content.childCount; i++)
            {
                Content.GetChild(i).gameObject.SetActive(false);
            }
        }

        for (int i = 0; i < Mathf.Min(chooser.Length,Content.childCount); i++)
        {
            Content.GetChild(i).gameObject.SetActive(true);
            Content.GetChild(i).GetComponentInChildren<Text>().text = chooser[i];
            Content.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
            int c = i;
            Content.GetChild(i).GetComponent<Button>().onClick.AddListener(() => { OnChoose?.Invoke(c); });
        }
        
        // if (chooser.Length == Content.childCount)
        // {
        //     for (int i = 0; i < Content.childCount; i++)
        //     {
        //         Content.GetChild(i).gameObject.SetActive(true);
        //         Content.GetChild(i).GetComponent<Text>().text = chooser[i];
        //         Content.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
        //         Content.GetChild(i).GetComponent<Button>().onClick.AddListener(() => { OnChoose?.Invoke(i); });
        //     }
        // }
        // else if (chooser.Length < Content.childCount)
        // {
        //     for (int i = 0; i < chooser.Length; i++)
        //     {
        //         Content.GetChild(i).gameObject.SetActive(true);
        //         Content.GetChild(i).GetComponent<Text>().text = chooser[i];
        //         Content.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
        //         Content.GetChild(i).GetComponent<Button>().onClick.AddListener(() => { OnChoose?.Invoke(i); });
        //     }
        //
        //     for (int i = chooser.Length; i < Content.childCount; i++)
        //     {
        //         Content.GetChild(i).gameObject.SetActive(false);
        //     }
        // }
        // else if (chooser.Length > Content.childCount)
        // {
        //     for (int i = 0; i < Content.childCount; i++)
        //     {
        //         Content.GetChild(i).gameObject.SetActive(true);
        //         Content.GetChild(i).GetComponent<Text>().text = chooser[i];
        //         Content.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners();
        //         Content.GetChild(i).GetComponent<Button>().onClick.AddListener(() => { OnChoose?.Invoke(i); });
        //     }
        //
        //     GameObject obj = Resources.Load<GameObject>("Prefabs/UI/ChooseElement");
        //
        //     for (int i = Content.childCount; i < chooser.Length; i++)
        //     {
        //         var btn = Instantiate(obj, Content);
        //         btn.GetComponentInChildren<Text>().text = chooser[i];
        //         btn.GetComponent<Button>().onClick.RemoveAllListeners();
        //         btn.GetComponent<Button>().onClick.AddListener(() => { OnChoose?.Invoke(i); });
        //     }
        // }
    }
}