using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CancelTrigger : MonoBehaviour, ICancelHandler
{
    public Action<BaseEventData> cancel;

    public void OnCancel(BaseEventData eventData)
    {
        cancel?.Invoke(eventData);
    }
}
