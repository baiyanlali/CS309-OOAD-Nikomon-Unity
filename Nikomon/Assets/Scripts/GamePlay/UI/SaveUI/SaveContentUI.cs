using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveContentUI : TabContent
{
    public List<SaveSlotUI> SaveSlots=new List<SaveSlotUI>();

    public override void OnShow()
    {
        base.OnShow();
        
        Init();
        var datas = GlobalManager.Instance.LoadAllSaveData();
        for (int i = 0; i < datas.Length; i++)
        {
            SaveSlots[i].OnEnter(i,datas[i]);
        }
    }

    public override void Init(params object[] args)
    {
        if (SaveSlots.Count == 0)
        {
            SaveSlots.AddRange(gameObject.GetComponentsInChildren<SaveSlotUI>());
        }
    }
    
}
