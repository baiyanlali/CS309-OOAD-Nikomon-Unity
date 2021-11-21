using System;
using System.Collections.Generic;
using GamePlay.UI.PokemonChooserTable;
using GamePlay.UI.UIFramework;
using PokemonCore;
using PokemonCore.Attack;
using PokemonCore.Attack.Data;
using PokemonCore.Combat;
using PokemonCore.Inventory;
using PokemonCore.Utility;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GamePlay.UI.BattleUI
{
    public class BattleUIPanel : BaseUI
    {
        private BattleUIHandler _battleUIHandler;
        public override void Init(params object[] args)
        {
            base.Init();
            CanQuitNow = false;
            BattleHandler bh=args[0] as BattleHandler;
            _battleUIHandler = GET(_battleUIHandler, "BattleCanvas");
            
            _battleUIHandler.Init(bh,this);
            FirstSelectable = GetComponentInChildren<Button>().gameObject;
        }

        public void CanQuit()
        {
            CanQuitNow = true;
        }

        
    }
}