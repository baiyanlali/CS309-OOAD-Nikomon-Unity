using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Selectable))]
public class TriggerSelect : MonoBehaviour,ISelectHandler, IDeselectHandler
{
    // public UnityEvent onSelect;
    public Action onSelect;
    public Action onDeSelect;

    public void OnSelect(BaseEventData eventData)
    {
        onSelect?.Invoke();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        onDeSelect?.Invoke();
    }
}