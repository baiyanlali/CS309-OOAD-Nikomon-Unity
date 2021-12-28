using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay;
using GamePlay.UI.BattleUI;
using GamePlay.UI.UIFramework;
using PokemonCore;
using PokemonCore.Combat;
using PokemonCore.Monster.Data;
using UnityEngine;
using UnityEngine.UI;

public class EvolutionPanel : BaseUI
{
    // Start is called before the first frame update
    public GameObject pokenmons;
    public PokemonData _PokemonOrignal;
    public PokemonData _PokemonEvoluted;

    public GameObject EvolutionBalls;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="args">0 for pokemondata1,1 for pokemondata2,2 for oncomplete</param>
    /// 
    public override void OnEnter(params object[] args)
    {
        //TODO:背景图片这些都要继续美化

        base.OnEnter(args);
        Action OnComplete = null;
        if (args != null)
        {
            _PokemonOrignal = args[0] as PokemonData;
            _PokemonEvoluted = args[1] as PokemonData;
            OnComplete = args[2] as Action;
        }
        foreach (Transform child in pokenmons.transform)
        {
            child.gameObject.SetActive(false);
        }

        string _PokemonOrignalName = _PokemonOrignal.ID.ToString() + _PokemonOrignal.innerName;
        string _PokemonEvolutedName = _PokemonEvoluted.ID.ToString() + _PokemonEvoluted.innerName;
        pokenmons.transform.Find(_PokemonOrignalName).gameObject.SetActive(true);
        //int EvolutionID = _PokemonOrignal._base.EvoChainID;
        //GameResources.Pokemons[_PokemonOrignal.ID][0];
        //print(211);
        //_PokemonOrignal()
        
        EvolutionBalls.transform.localScale=Vector3.zero;
        LeanTween.scale(EvolutionBalls, Vector3.one, 1).setOnComplete(() =>
        {
            pokenmons.transform.Find(_PokemonOrignalName).gameObject.SetActive(false);
            pokenmons.transform.Find(_PokemonEvolutedName).gameObject.SetActive(true);
            LeanTween.scale(EvolutionBalls, Vector3.zero, 1).setOnComplete(() =>
            {
                //pokenmons.transform.Find(_PokemonEvolutedName).gameObject.SetActive(true);
            });
            
        });
        
        ExitBtn.onClick.RemoveAllListeners();
        ExitBtn.onClick.AddListener(() =>
        {
            // print("Back Evolution Panel");
            UIManager.Instance.Hide(this);
            OnComplete?.Invoke();
        });

    }
}
