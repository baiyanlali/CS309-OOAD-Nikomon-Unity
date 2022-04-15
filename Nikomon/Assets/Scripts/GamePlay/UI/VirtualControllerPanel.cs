using System.Collections;
using System.Collections.Generic;
using GamePlay.UI.UIFramework;
using UnityEngine;

public class VirtualControllerPanel : BaseUI
{
    public override UILayer Layer => UILayer.MainUI;

    //这里重写了空方法是为了避免调用父类时加入Canvas Group
    public override void OnPause()
    {
    }
    public override void OnResume()
    {
    }
}
