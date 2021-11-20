using System;
using System.Collections;
using System.Collections.Generic;
using PokemonCore.Inventory;
using PokemonCore.Utility;
using UnityEngine;
using UnityEngine.EventSystems;

public class TabSystem : MonoBehaviour
{

    public Action<TabElement> OnChoosed;
    private List<TabElement> TabElements;
    public Transform TableContent;
    public Transform Tables;
    private TabElement CurrentTabElement;
    private TabContent CurrentTableContent;

    private List<TabContent> tableContents;

    // private bool HaveEnterScope = false;


    private void Start()
    {
        if (hasInit) return;
        TabElements = new List<TabElement>();
        TabElements.AddRange(GetComponentsInChildren<TabElement>());
        TableContent = TableContent ? TableContent : transform.Find("TableContent");
        Tables = Tables ? Tables : transform.Find("Tables");
        if (TableContent != null)
        {
            tableContents = new List<TabContent>();
            for (int i = 0; i < TableContent.childCount; i++)
            {
                tableContents.Add(TableContent.GetChild(i).GetComponent<TabContent>());
            }
        }

        for (int i = 0; i < TabElements.Count; i++)
        {
            TabElements[i].OnChoose = OnChoose;
            TabElements[i].onClick.RemoveAllListeners();
            var i1 = i;
            if(i1<tableContents.Count)
                TabElements[i].onClick.AddListener(()=>tableContents[i1].OnEnter());
        }
        
    }

    private bool hasInit = false;

    /// <summary>
    /// 必须确保TableContent下没有子物体，命令才有效
    /// </summary>
    /// <param name="tabElements"></param>
    public void Init(Dictionary<TabElement,GameObject> tabElements)
    {
        hasInit = true;
        // print($"Tab system init {tabElements.Count}");
        for (int i = 0; i < TableContent.childCount; i++)
        {
            Destroy(TableContent.GetChild(i).gameObject);
        }
        
        for (int i = 0; i < Tables.childCount; i++)
        {
            Transform obj = Tables.GetChild(i);
            if(obj.GetComponent<TabElement>()!=null)
                Destroy(obj.gameObject);
        }
        
        
        tableContents = new List<TabContent>();
        TabElements = new List<TabElement>();
        
        foreach (var tab in tabElements)
        {
            this.TabElements.Add(tab.Key);
            tab.Key.gameObject.transform.SetParent(Tables);
            tab.Key.transform.SetSiblingIndex(Tables.childCount-2);
            tab.Key.OnChoose = OnChoose;
            tableContents.Add(tab.Value.transform.GetComponent<TabContent>());
            tab.Value.transform.SetParent(TableContent,false);
            
            tab.Key.onClick.RemoveAllListeners();
            tab.Key.onClick.AddListener(()=>tab.Value.transform.GetComponent<TabContent>().OnEnter());
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
        // LeanTween.size(rect, rect.sizeDelta*ScaleSize, 0.2f);

        if (CurrentTabElement != null)
        {
            CurrentTabElement.DeSelect();
        }
        
        CurrentTabElement = element;
            
        element.Select();

        //检测当前content是否真的切换了，因为有可能当前没有content可以用，这时就直接将current table content 设为null
        bool flag = false;
        for (int i = 0; i < tableContents.Count; i++)
        {
            if (TabElements.IndexOf(element.transform.GetComponent<TabElement>()) == i)
            {
                CurrentTableContent = tableContents[i].gameObject.GetComponent<TabContent>();
                
                tableContents[i].gameObject.SetActive(true);
                CurrentTableContent.OnShow();
                flag = true;
            }
            else
            {
                tableContents[i].gameObject.SetActive(false);
            }
        }

        if (!flag) CurrentTableContent = null;
        OnChoosed?.Invoke(element);
    }
    
    public void OnCancel()
    {
        if(CurrentTableContent!=null)
            CurrentTableContent.OnExit();
        EventSystem.current.SetSelectedGameObject(TabElements[0].gameObject);
        OnChoose(TabElements[0]);
        // HaveEnterScope = false;
    }
    
    public void OnSubmit()
    {
        // print("Submit!");
        if (CurrentTableContent == null || CurrentTableContent.FirstSelectable == null) return;
        // EventSystem.current.SetSelectedGameObject(CurrentTableContent.FirstSelectedObject);
        CurrentTableContent.OnEnter();
        // HaveEnterScope = true;
    }
}
