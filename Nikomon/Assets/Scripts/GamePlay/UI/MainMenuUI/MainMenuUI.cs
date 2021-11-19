using System.Collections;
using System.Collections.Generic;
using GamePlay.UI;
using GamePlay.UI.MainMenuUI;
using GamePlay.UI.UIFramework;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : BaseUI
{
    #region UIAsset

    public Button Bag;
    public Button Party;
    public Button Save;
    public Button Setting;

    #endregion
    
    public override UILayer Layer => UILayer.NormalUI;
    public override bool IsOnly => true;
    public override bool IsBlockPlayerControl => true;

    public override void Init(params object[] args)
    {
        base.Init(args);
        Bag = GET(Bag ,nameof(Bag )).GetComponentInChildren<Button>();
        Party = GET(Party ,nameof(Party )).GetComponentInChildren<Button>();
        Save = GET(Save ,nameof(Save )).GetComponentInChildren<Button>();
        Setting = GET(Setting ,nameof(Setting )).GetComponentInChildren<Button>();
    }

    public override void OnEnter(params object[] args)
    {
        base.OnEnter(args);
        Bag.onClick.RemoveAllListeners();
        Bag.onClick.AddListener(OpenBag);
        Party.onClick.RemoveAllListeners();
        Party.onClick.AddListener(OpenParty);
        Save.onClick.RemoveAllListeners();
        Save.onClick.AddListener(OpenSave);
        Setting.onClick.RemoveAllListeners();
        Setting.onClick.AddListener(OpenSetting);
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void OnPause()
    {
        base.OnPause();
        Bag.interactable = true;
    }

    public override void OnResume()
    {
        base.OnResume();
    }

    public void Resume()
    {
        UIManager.Instance.Hide(this);
    }

    public void OpenBag()
    {
        
    }

    public void OpenSetting()
    {
        UIManager.Instance.Show<SettingUI>();
    }

    public void OpenParty()
    {
        
    }
    public void OpenSave()
    {
        UIManager.Instance.Show<SavePanelUI>();
    }
    
    
}
