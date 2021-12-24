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
    private Action onClicked;

    public Action OnClicked
    {
        set
        {
            onClicked = value;
            GetComponent<Button>().onClick.RemoveAllListeners();
            GetComponent<Button>().onClick.AddListener(() =>
            {
                onClicked?.Invoke();
            });
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        onSelect?.Invoke();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        onDeSelect?.Invoke();
    }
    // public void onClicked(BaseEventData eventData)
    // {
    //     onClicked?.Invoke();
    // }
}