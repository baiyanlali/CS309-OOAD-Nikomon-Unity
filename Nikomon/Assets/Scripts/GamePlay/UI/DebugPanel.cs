using GamePlay.UI.UIFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugPanel :BaseUI
{
    Button Submit;
    Button Cancel;
    InputField InputField;
    XLua.LuaEnv luaEnv;

    // public override bool IsBlockPlayerControl => true;

    public override void Init(params object[] args)
    {
        Submit = GET(Submit, nameof(Submit));
        Cancel = GET(Cancel, nameof(Cancel));

        FirstSelectable = Cancel.gameObject;

        ExitBtn = Cancel;
        base.Init(args);
        

        InputField = GET(InputField, nameof(InputField));
        luaEnv = new XLua.LuaEnv();

        luaEnv.DoString("require 'Main'");
    }


    public override void OnEnter(params object[] args)
    {
        base.OnEnter(args);

        Submit.onClick.RemoveAllListeners();
        Submit.onClick.AddListener(()=>
        {
            luaEnv.DoString(InputField.text);
        });
    }

    public override void OnExit()
    {
        luaEnv.Dispose();
        base.OnExit();
    }

}
