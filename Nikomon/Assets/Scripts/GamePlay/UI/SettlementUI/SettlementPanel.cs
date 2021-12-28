using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay;
using GamePlay.UI.BattleUI;
using GamePlay.UI.UIFramework;
using GamePlay.UI.UtilUI;
using PokemonCore;
using PokemonCore.Monster.Data;
using UnityEngine;

public class PokemonLevelUpState
{
    public Pokemon Pokemon;//升级后的宝可梦！！！
    public Experience ExpBefore;
}

public class SettlementPanel : BaseUI
{
// Start is called before the first frame update
    public Transform PokemonList;
    private GameObject SettlementPrefab;
    private List<PokemonSettlement> _storeElements = new List<PokemonSettlement>();
    public List<PokemonLevelUpState> _PokemonLevelUpStates = new List<PokemonLevelUpState>();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args">0 for pokemondata1</
    /// 
    public override void OnEnter(params object[] args)
    {
        SettlementPrefab = GameResources.SpawnPrefab(typeof(PokemonSettlement));
        

        base.OnEnter(args);
        if (args != null)
        {
            _PokemonLevelUpStates = args[0] as List<PokemonLevelUpState>;
        }
        
        // _storeElements.Clear();
        // print(_PokemonLevelUpStates.Count - PokemonList.childCount);

        int num = _PokemonLevelUpStates.Count - PokemonList.childCount;
        for (int i = 0; i < num; i++)//有一个是新技能
        {
            GameObject obj = Instantiate(SettlementPrefab, PokemonList);

            obj.name = "Pokemon" + i;
            print(obj.name);
            _storeElements.Add(obj.GetComponent<PokemonSettlement>());
        }

        foreach (var temp in _storeElements)
        {
            temp.GetComponent<PokemonSettlement>().gameObject.SetActive(false);
        }

        int j = 0;
        foreach (var temp in _PokemonLevelUpStates)
        {
            PokemonList.GetChild(j).gameObject.SetActive(true);
            _storeElements[j].Init(temp.Pokemon,temp.ExpBefore);
            j++;
        }

        j = 0;
        foreach (var temp in _PokemonLevelUpStates)
        {
            _storeElements[j].addExp( temp.Pokemon.Exp,temp.ExpBefore);
            j++;
        }


    }
}
