using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabElement : Button
{
    public Action<TabElement> OnChoose;

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        // print("select!");
        OnChoose?.Invoke(this);
    }
    
    
    
    public virtual void DeSelect()
    {
        
    }
    
}
