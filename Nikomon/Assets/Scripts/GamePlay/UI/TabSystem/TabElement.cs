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
        OnChoose?.Invoke(this);
    }
}
