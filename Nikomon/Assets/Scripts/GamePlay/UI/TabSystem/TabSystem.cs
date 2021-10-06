using System;
using System.Collections;
using System.Collections.Generic;
using PokemonCore.Utility;
using UnityEngine;

public class TabSystem : MonoBehaviour
{
    public List<TabElement> TabElements;
    public Transform TableContent;

    private List<Transform> tableContents;

    private void OnEnable()
    {
        TabElements = new List<TabElement>();
        TabElements.AddRange(GetComponentsInChildren<TabElement>());
        TableContent = transform.Find("TableContent");

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

    void OnChoose(TabElement element)
    {
        
    }
    
}
