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

    public void ShowChooser(string[] chooser,Vector2 wantedPivot,Action<int> OnChoose=null, RectTransform rectTransform = null)
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
            Content.GetChild(i).GetComponent<Button>().onClick.AddListener(() =>
            {
                OnChoose?.Invoke(c); 
                gameObject.SetActive(false);
            });
        }

        
        
        
        int width = Screen.width;
        int height = Screen.height;
        RectTransform rectTrans=GetComponent<RectTransform>();

        rectTrans.pivot = wantedPivot;
        
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTrans);
        LayoutRebuilder.ForceRebuildLayoutImmediate(rectTrans);
        
        Rect rect = rectTrans.rect;

        Vector2 vec=rectTrans.position;

        var pivot = rectTrans.pivot;
        float left = vec.x - rect.width * pivot.x;
        float right = vec.x + rect.width * (1 - pivot.x);
        float down = vec.y - rect.height * pivot.y;
        float up = vec.y + rect.height * (1 - pivot.y);

        Vector2 finalPos=vec;
        if (left <= 0)
        {
            left = 0;
            finalPos.x = left + rect.width * pivot.x;
        }else if (right > width)
        {
            right = width;
            finalPos.x = right - rect.width * (1 - pivot.x);
        }
        
        if (down <= 0)
        {
            down = 0;
            finalPos.y =down + rect.height * pivot.y;
        }else if (up>height)
        {
            up = height;
            finalPos.y = up- rect.height * (1 - pivot.y);
        }

        rectTrans.anchoredPosition = finalPos;
        // LayoutRebuilder.ForceRebuildLayoutImmediate(rectTrans);
    }
}