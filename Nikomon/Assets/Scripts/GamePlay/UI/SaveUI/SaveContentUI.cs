using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
            int index = i;
            SaveSlots[i].OnEnter(i,datas[i]);
            SaveSlots[i].GetComponent<Button>().onClick.RemoveAllListeners();
            SaveSlots[i].GetComponent<Button>().onClick.AddListener(() =>
            {
                GlobalManager.Instance.InitGameWithDataIndex(index);
            });
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
