using System;
using GamePlay.UI.UIFramework;

namespace GamePlay.UI
{
    public class StartMenuUI:BaseUI
    {

        public override void Init(params object[] args)
        {
            CanQuitNow = false;
            base.Init(args);
        }
    }
}