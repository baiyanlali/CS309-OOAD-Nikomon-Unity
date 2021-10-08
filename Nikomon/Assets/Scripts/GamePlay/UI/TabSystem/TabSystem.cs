using System;
using System.Collections;
using System.Collections.Generic;
using PokemonCore.Inventory;
using PokemonCore.Utility;
using UnityEngine;

public class TabSystem : MonoBehaviour
{
    public float ScaleSize=1.5f;

    public Action<TabElement> OnChoosed;
    private List<TabElement> TabElements;
    private Transform TableContent;
    private Transform Tables;
    private TabElement CurrentTabElement;

    private List<Transform> tableContents;

    private void OnEnable()
    {
        TabElements = new List<TabElement>();
        TabElements.AddRange(GetComponentsInChildren<TabElement>());
        TableContent = transform.Find("TableContent");
        Tables = transform.Find("Tables");
        if (TableContent != null)
        {
            tableContents = new List<Transform>();
            for (int i = 0; i < TableContent.childCount; i++)
            {
                tableContents.Add(TableContent.GetChild(i));
            }
        }
        
        foreach (var tabElement in TabElements.OrEmptyIfNull())
        {
            tabElement.OnChoose = OnChoose;
        }

    }

    /// <summary>
    /// 必须确保TableContent下没有子物体，命令才有效
    /// </summary>
    /// <param name="tabElements"></param>
    public void Init(Dictionary<TabElement,GameObject> tabElements)
    {
        // print($"Tab system init {tabElements.Count}");
        for (int i = 0; i < TableContent.childCount; i++)
        {
            Destroy(TableContent.GetChild(i).gameObject);
        }

        for (int i = 1; i < Tables.childCount-1; i++)
        {
            Destroy(Tables.GetChild(i).gameObject);
        }
        
        tableContents = new List<Transform>();
        TabElements = new List<TabElement>();
        
        foreach (var tab in tabElements)
        {
            this.TabElements.Add(tab.Key);
            tab.Key.gameObject.transform.SetParent(Tables);
            tab.Key.transform.SetSiblingIndex(Tables.childCount-2);
            tab.Key.OnChoose = OnChoose;
            tableContents.Add(tab.Value.transform);
            tab.Value.transform.SetParent(TableContent,false);
        }
        
        OnChoose(TabElements[0]);
    }

    public void Show(Boolean[] tags)
    {
        for (int i = 0; i < Mathf.Min(tags.Length,TabElements.Count); i++)
        {
            if (tags[i] == false)
            {
                TabElements[i].gameObject.SetActive(false);
            }
            else
            {
                TabElements[i].gameObject.SetActive(true);
            }
        }
    }
    
    

    void OnChoose(TabElement element)
    {
        if (element == CurrentTabElement) return;
        
        RectTransform rect=element.transform as RectTransform;
        LeanTween.size(rect, rect.sizeDelta*ScaleSize, 0.2f);

        if (CurrentTabElement != null)
        {
            RectTransform r=CurrentTabElement.transform as RectTransform;
            LeanTween.size(r, r.sizeDelta/ScaleSize, 0.2f);
        }
        
        CurrentTabElement = element;

        for (int i = 0; i < tableContents.Count; i++)
        {
            //因为有左箭头，这里的index从1开始
            if (element.transform.GetSiblingIndex()-1 == i)
            {
                tableContents[i].gameObject.SetActive(true);
            }
            else
            {
                tableContents[i].gameObject.SetActive(false);
            }
        }
        
        OnChoosed?.Invoke(element);
    }
    
}
