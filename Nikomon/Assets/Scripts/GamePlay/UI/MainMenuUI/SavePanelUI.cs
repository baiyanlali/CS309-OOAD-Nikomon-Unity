using System;
using System.Collections.Generic;
using GamePlay.UI.UIFramework;
using GamePlay.UI.UtilUI;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GamePlay.UI.MainMenuUI
{
    public class SavePanelUI : BaseUI
    {
        public override UILayer Layer => UILayer.NormalUI;

        public Button ExitButton;
        public List<SaveSlotUI> SaveSlots = new List<SaveSlotUI>();


        
        public override void OnEnter(params object[] args)
        {
            base.OnEnter();
            var datas = GlobalManager.Instance.LoadAllSaveData();
            for (int i = 0; i < datas.Length; i++)
            {
                SaveSlots[i].OnEnter(i, datas[i]);
            }

        }

        private int saveIndex;
        public override void Init(params object[] args)
        {
            base.Init();
            if (SaveSlots.Count == 0)
            {
                SaveSlots.AddRange(gameObject.GetComponentsInChildren<SaveSlotUI>());
            }

            for (int i = 0; i < SaveSlots.Count; i++)
            {
                int index = i;
                SaveSlots[i].GetComponent<Button>().onClick.RemoveAllListeners();
                SaveSlots[i].GetComponent<Button>().onClick.AddListener(() =>
                {
                    Action<bool> action = (isSave) =>
                    {
                        ConfirmSave(isSave, index);
                    };
                    if (SaveSlots[index].HasFile)
                    {
                        saveIndex = index;
                        UIManager.Instance.Show<ConfirmPanel>("SavePanelUI.HasFile", action);
                    }
                    else
                    {
                        saveIndex = index;
                        // GlobalManager.Instance.SaveSaveData(index);
                        UIManager.Instance.Show<ConfirmPanel>("SavePanelUI.SureToSave",action);
                    }
                });
            }
            
            ExitButton = GET(ExitButton, nameof(ExitButton));
            ExitBtn = ExitButton;

        }
        

        public void ConfirmSave(bool isConfirmed,int index)
        {
            if(isConfirmed)
            {
                var save = GlobalManager.Instance.SaveSaveData(saveIndex);
                SaveSlots[index].OnEnter(index,save);
                // UIManager.Instance.Show<ConfirmPanel>("SavePanelUI.SaveSuccessfully",null);
            }
        }
    }
}