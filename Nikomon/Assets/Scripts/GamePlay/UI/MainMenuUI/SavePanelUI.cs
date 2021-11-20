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

        public List<SaveSlotUI> SaveSlots = new List<SaveSlotUI>();


        public override void OnEnter(params object[] args)
        {
            base.OnEnter();

            Init();
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
                    Action<bool> act = ConfirmSave;
                    if (SaveSlots[index].HasFile)
                    {
                        saveIndex = index;
                        UIManager.Instance.Show<ConfirmPanel>("SavePanelUI.HasFile",act);
                    }
                    else
                    {
                        saveIndex = index;
                        // GlobalManager.Instance.SaveSaveData(index);
                        UIManager.Instance.Show<ConfirmPanel>("SavePanelUI.SureToSave",act);
                    }
                });
            }
        }

        public override void OnPause()
        {
            base.OnPause();
        }

        public override void OnResume()
        {
            base.OnResume();
            print("save panel resume");
            var datas = GlobalManager.Instance.LoadAllSaveData();
            for (int i = 0; i < datas.Length; i++)
            {
                SaveSlots[i].OnEnter(i, datas[i]);
            }
        }

        public void ConfirmSave(bool isConfirmed)
        {
            if(isConfirmed)
            {
                GlobalManager.Instance.SaveSaveData(saveIndex);
                UIManager.Instance.Show<ConfirmPanel>("SavePanelUI.SaveSuccessfully",null);
            }
        }
    }
}