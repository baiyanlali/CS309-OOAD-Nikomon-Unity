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
    public GameObject _Button;
    
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
        
        _Button.SetActive(false);
        
        EvolutionBalls.transform.localScale=Vector3.zero;
        LeanTween.scale(EvolutionBalls, Vector3.one, 2).setOnComplete(() =>
        {
            pokenmons.transform.Find(_PokemonOrignalName).gameObject.SetActive(false);
            pokenmons.transform.Find(_PokemonEvolutedName).gameObject.SetActive(true);
            LeanTween.scale(EvolutionBalls, Vector3.one, 2).setOnComplete(() =>
            {
                LeanTween.scale(EvolutionBalls, Vector3.zero, 2).setOnComplete(() =>
                {
                    //pokenmons.transform.Find(_PokemonEvolutedName).gameObject.SetActive(true);
                    _Button.SetActive(true);
                });
            });
            
        });
        
        ExitBtn.onClick.RemoveAllListeners();
        ExitBtn.onClick.AddListener(() =>
        {
            // print("Back Evolution Panel");
            UIManager.Instance.Hide(this);
            OnComplete?.Invoke();
        });
        Vector3 temp = new Vector3();
        temp.x = 0;
        temp.y = 0;
        temp.z = -180;
        LeanTween.rotate(EvolutionBalls, temp, 1).setOnComplete(() =>
        {
            temp.z = 0;
            LeanTween.rotate(EvolutionBalls, temp, 1).setOnComplete(() =>
            {
                temp.z = 390;
                LeanTween.rotate(EvolutionBalls, temp, 0.5f).setOnComplete(() =>
                {
                    temp.z = 360;
                    LeanTween.rotate(EvolutionBalls, temp, 0.5f).setOnComplete(() =>
                    {
                        temp.z = 390;
                        LeanTween.rotate(EvolutionBalls, temp, 0.5f).setOnComplete(() =>
                        {
                            temp.z = 360;
                            LeanTween.rotate(EvolutionBalls, temp, 0.5f).setOnComplete(() =>
                            {
                                temp.z = 180;
                                LeanTween.rotate(EvolutionBalls, temp, 1).setOnComplete(() =>
                                {
                                    temp.z = 0;
                                    LeanTween.rotate(EvolutionBalls, temp, 1).setOnComplete(() =>
                                    {

                                    });
                                });
                            });
                        });
                    });
                });
            });

        });
    }
}
