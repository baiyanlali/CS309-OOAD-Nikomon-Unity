using System.Collections;
using System.Collections.Generic;
using GamePlay;
using UnityEngine;
using UnityEngine.UI;
using GamePlay.UI.UIFramework;
public class PCPanel : BaseUI
{
    GameObject Tables;
    GameObject Titles;

    Text text;
    PCManager PCManager;

    public override void Init(params object[] args)
    {
        base.Init(args);
        Tables = GET(Tables, "Tables", GET_TYPE.GameObject);
        Titles = GET(Titles, "TableContent/Title", GET_TYPE.GameObject);
        text = GET(text, "TableContent/Title/Text");
        PCManager = GET(PCManager, "TableContent");//绑定脚本!!!
        PCManager.Refresh();
        GameObject obj = GameResources.SpawnPrefab("PCSlot");
        Instantiate(obj);

    }
    public override void OnEnter(params object[] args)
    {
        base.OnEnter(args);


    }

    public override void OnExit()
    {
        base.OnExit();
        
    }
}
