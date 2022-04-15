using System;
using System.Collections;
using System.Linq;
using GamePlay.UI.UIFramework;

namespace GamePlay.UI
{
    public class StartMenuUI:BaseUI
    {
        public TabElement SaveContent;
        public override void Init(params object[] args)
        {
            CanQuitNow = false;
            base.Init(args);
            StartCoroutine(showContinue());

        }

        IEnumerator showContinue()
        {
            var datas = GlobalManager.Instance.LoadAllSaveData();
            int count = datas.Count(data => data != null);
            //等三帧，让tabsystem加载完
            yield return null;
            yield return null;
            yield return null;
            if (count != 0)
            {
                print("Have save file");
                GetComponent<TabSystem>().OnChoose(SaveContent);
            }
        }
    }
}