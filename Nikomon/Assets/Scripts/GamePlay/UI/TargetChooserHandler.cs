using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GamePlay;
using PokemonCore;
using PokemonCore.Attack.Data;
using PokemonCore.Combat;
using PokemonCore.Utility;
using UnityEngine;
using UnityEngine.UI;

public class TargetChooserHandler : MonoBehaviour
{
    public GameObject TargetChooserPrefab;
    public GameObject Panel;
    public Transform Allies;
    public Transform Opponents;
    public Button Submit;
    public Button Cancel;
    private List<TargetChooserUI> oppoToggle;
    private List<TargetChooserUI> allyToggle;
    private List<TargetChooserUI> userToggle;

    public Action<List<int>> OnChooseTarget;
    public Action OnCancelChoose;

    public static TargetChooserHandler Instance
    {
        get
        {
            return sInstance;
        }
    }
    private static TargetChooserHandler sInstance;
    
    public void Init(List<CombatPokemon> opponents,List<CombatPokemon> allies)
    {
        sInstance = this;
        oppoToggle = new List<TargetChooserUI>();
        allyToggle = new List<TargetChooserUI>();
        userToggle = new List<TargetChooserUI>();
        InitToggleUI(opponents,Opponents,oppoToggle);
        InitToggleUI(allies,Allies,allyToggle);
        Panel.SetActive(false);
        Cancel.onClick.AddListener(() =>
        {
            Panel.SetActive(false);
            OnCancelChoose?.Invoke();
        });
    }
    
    //TODO:这里可以改得更好看一些
    private void InitToggleUI(List<CombatPokemon> pokemons, Transform parent,List<TargetChooserUI> toggle)
    {

        if (pokemons.Count < parent.transform.childCount)
        {
            for (int i = 0; i < parent.transform.childCount; i++)
            {
                parent.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        else if (pokemons.Count > parent.transform.childCount)
        {
            GameObject o = Instantiate(TargetChooserPrefab, parent);
        }

        for (int i = 0; i < pokemons.Count; i++)
        {
            var targetChooserUI=parent.transform.GetChild(i).GetComponent<TargetChooserUI>();
            targetChooserUI.gameObject.SetActive(true);
            targetChooserUI.Init(pokemons[i]);
            toggle.Add(targetChooserUI.GetComponent<TargetChooserUI>());
            if(pokemons[i].TrainerID==Game.trainer.id)
                userToggle.Add(targetChooserUI.GetComponent<TargetChooserUI>());
        }
    }


    private void ClearToggle()
    {
        foreach (var toggle in allyToggle)
        {
            toggle.toggle.isOn = false;
        }
        foreach (var toggle in oppoToggle)
        {
            toggle.toggle.isOn = false;
        }
    }

    //TODO: 完善这里！
    public void ShowTargetChooser(Targets target)
    {
        
        List<int> targets = new List<int>();
        Panel.SetActive(true);
        
        oppoToggle.Off();
        allyToggle.Off();
        
        targets.Clear();
        
        switch (target)
        {
            case Targets.ALLY:
                oppoToggle.Disable();
                allyToggle.Enable();
                userToggle.Disable();
                break;
            case Targets.USER:
                oppoToggle.Disable();
                allyToggle.Disable();
                userToggle.Enable();
                break;
            case Targets.ALL_POKEMON:
                oppoToggle.Enable();
                allyToggle.Enable();
                break;
            case Targets.USER_OR_ALLY:
                oppoToggle.Disable();
                allyToggle.Enable();
                break;
            case Targets.ALL_OPPONENTS:
                oppoToggle.Disable();
                allyToggle.Disable();
                oppoToggle.On();
                break;
            case Targets.ENTIRE_FIELD:
                oppoToggle.Disable();
                allyToggle.Disable();
                oppoToggle.On();
                allyToggle.On();
                break;
            case Targets.USER_AND_ALLIES:
                oppoToggle.Disable();
                allyToggle.Disable();
                allyToggle.On();
                break;
            case Targets.SELECTED_OPPONENT_POKEMON:
                oppoToggle.Enable();
                allyToggle.Disable();
                Submit.gameObject.SetActive(false);
                oppoToggle.OnSelected(OnSelected);
                break;
            case Targets.ALL_OTHER_POKEMON:
                oppoToggle.Disable();
                allyToggle.Disable();
                oppoToggle.On();
                allyToggle.On();
                userToggle.Off();
                break;
            case Targets.RANDOM_OPPONENT:
                //TODO；还没想好怎么实现更好
                break;
            case Targets.SELECTED_POKEMON:
                oppoToggle.Enable();
                allyToggle.Enable();
                
                Submit.gameObject.SetActive(false);
                allyToggle.OnSelected(() =>
                {
                    targets.AddRange(from o in oppoToggle where o.toggle.isOn select o.Pokemon.CombatID);
                    targets.AddRange(from o in allyToggle where o.toggle.isOn select o.Pokemon.CombatID);
                    ClearToggle();
                    OnChooseTarget?.Invoke(targets);
                    Panel.SetActive(false);
                });
                break;
        }
        
        if(Submit.gameObject.activeSelf)
            Submit.onClick.AddListener(() =>
                {
                    targets.AddRange(from o in oppoToggle where o.toggle.isOn == true select o.Pokemon.CombatID);
                    targets.AddRange(from o in allyToggle where o.toggle.isOn == true select o.Pokemon.CombatID);
                    OnChooseTarget?.Invoke(targets);
                    Panel.SetActive(false);
                }
            );
        
    }


    public void OnSelected()
    {
        List<int> targets = new List<int>();
        targets.AddRange(from o in oppoToggle where o.toggle.isOn select o.Pokemon.CombatID);
        targets.AddRange(from o in allyToggle where o.toggle.isOn select o.Pokemon.CombatID);
        ClearToggle();
        OnChooseTarget?.Invoke(targets);
        Panel.SetActive(false);
    }
    
}
