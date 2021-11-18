using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TabElement : MonoBehaviour,IPointerDownHandler
{
    public Action<TabElement> OnChoose;
    public void OnPointerDown(PointerEventData eventData)
    {
        
        // print("OnPointerDown");
        OnChoose?.Invoke(this);
    }

    public virtual void Select()
    {
        
    }

    public virtual void DeSelect()
    {
        
    }
}
