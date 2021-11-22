using System.Collections;
using System.Collections.Generic;
using GamePlay.UI.UIFramework;
using UnityEngine;
using UnityEngine.EventSystems;

public class TabContent : BaseUI
{
    public override void Init(params object[] args)
    {
        var cancelTrigger = gameObject.GetComponentsInChildren<CancelTrigger>();
        foreach (var trigger in cancelTrigger)
        {
            trigger.cancel += OnBack;
        }
    }

    
    public virtual void OnShow(params  object[] args)
    {
    }

    private GameObject lastTab;
    public override void OnEnter(params object[] args)
    {
        lastTab = EventSystem.current.currentSelectedGameObject;
        if (FirstSelectable != null)
        {
            EventSystem.current.SetSelectedGameObject(FirstSelectable.gameObject);
        }
        Init();
    }

    //BaseUI 的 OnExit它不能用因为它不走UIManager这条路
    public virtual void OnBack(BaseEventData data)
    {
        EventSystem.current.SetSelectedGameObject(lastTab);
    }
    
}
