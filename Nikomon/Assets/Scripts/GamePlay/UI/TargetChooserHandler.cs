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
    public void Init(List<CombatPokemon> opponents,List<CombatPokemon> allies)
    {
        oppoToggle = new List<TargetChooserUI>();
        allyToggle = new List<TargetChooserUI>();
        userToggle = new List<TargetChooserUI>();
        foreach (var oppo in opponents.OrEmptyIfNull())
        {
            GameObject o = Instantiate(TargetChooserPrefab, Opponents);
            o.GetComponent<TargetChooserUI>()?.Init(oppo);
            oppoToggle.Add(o.GetComponent<TargetChooserUI>());
        }
        foreach (var ally in allies.OrEmptyIfNull())
        {
            GameObject o = Instantiate(TargetChooserPrefab, Allies);
            o.GetComponent<TargetChooserUI>()?.Init(ally);
            allyToggle.Add(o.GetComponent<TargetChooserUI>());
            if(ally.TrainerID==Game.trainer.id)
                userToggle.Add(o.GetComponent<TargetChooserUI>());
        }
        Panel.SetActive(false);
        Cancel.onClick.AddListener(() =>
        {
            Panel.SetActive(false);
            OnCancelChoose?.Invoke();
        });
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
                oppoToggle.OnSelected(() =>
                {
                    targets.AddRange(from o in oppoToggle where o.toggle.isOn select o.Pokemon.CombatID);
                    targets.AddRange(from o in allyToggle where o.toggle.isOn select o.Pokemon.CombatID);
                    OnChooseTarget?.Invoke(targets);
                    Panel.SetActive(false);
                });
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
    
    
}
