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

    public GameObject Bag;
    public GameObject Party;
    public GameObject Save;
    public GameObject Setting;

    #endregion
    
    public override UILayer Layer => UILayer.NormalUI;
    public override bool IsOnly => true;
    public override bool IsBlockPlayerControl => true;

    public override void Init(params object[] args)
    {
        base.Init(args);
        Bag = GET(Bag ,nameof(Bag ),GET_TYPE.GameObject);
        Party = GET(Party ,nameof(Party ),GET_TYPE.GameObject);
        Save = GET(Save ,nameof(Save ),GET_TYPE.GameObject);
        Setting = GET(Setting ,nameof(Setting ),GET_TYPE.GameObject);
    }

    public override void OnEnter(params object[] args)
    {
        base.OnEnter(args);
        Bag.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
        Bag.GetComponentInChildren<Button>().onClick.AddListener(OpenBag);
        Party.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
        Party.GetComponentInChildren<Button>().onClick.AddListener(OpenParty);
        Save.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
        Save.GetComponentInChildren<Button>().onClick.AddListener(OpenSave);
        Setting.GetComponentInChildren<Button>().onClick.RemoveAllListeners();
        Setting.GetComponentInChildren<Button>().onClick.AddListener(OpenSetting);
    }

    public override void OnExit()
    {
        base.OnExit();
    }

    public override void OnPause()
    {
        base.OnPause();
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
        print("Open Save");
        UIManager.Instance.Show<SavePanelUI>();
    }
    
    
}
