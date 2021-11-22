using System;
using System.Collections.Generic;
using System.Linq;
using GamePlay.UI.UIFramework;
using GamePlay.Utilities;
using PokemonCore;
using PokemonCore.Attack.Data;
using PokemonCore.Combat;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.UI.BattleUI

{
    public class TargetChooserPanel : BaseUI
    {
        public GameObject TargetChooserPrefab;
        public Transform Allies;
        public Transform Opponents;
        public Button Submit;
        public Button Cancel;
        private List<TargetChooserUI> oppoToggle;
        private List<TargetChooserUI> allyToggle;
        private List<TargetChooserUI> userToggle;

        public Action<List<int>> OnChooseTarget;
        public Action OnCancelChoose;
        public override void Init(params object[] args)
        {
            Allies = GET(Allies, "Panel/Ally");
            Opponents = GET(Opponents, "Panel/Opponent");
            Submit = GET(Submit, "Panel/Submit");
            Cancel = GET(Cancel, "Panel/Cancel");
            
            ExitBtn = Cancel;

            base.Init(args);

            TargetChooserPrefab = GameResources.SpawnPrefab(typeof(PokemonChooserElementUI));
            
            List<CombatPokemon> opponents = args[0] as List<CombatPokemon>;
            List<CombatPokemon> allies = args[1] as List<CombatPokemon>;
            
            oppoToggle = new List<TargetChooserUI>();
            allyToggle = new List<TargetChooserUI>();
            userToggle = new List<TargetChooserUI>();
            var op = InitToggleUI(opponents, Opponents, oppoToggle);
            var al = InitToggleUI(allies, Allies, allyToggle);
            op.LinkNavigation(al,DirectionType.Horizontal);
            al.LinkNavigation(new List<Button>(){Cancel,Submit},DirectionType.Horizontal);
            Targets targets =(Targets)args[2];
            ShowTargetChooser(targets);

            if (args.Length >= 4)
            {
                OnChooseTarget = args[3] as Action<List<int>>;
                if (args.Length >= 5)
                {
                    OnCancelChoose = args[4] as Action;
                }
            }

        }

        
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args">0 and 1 for opponent and ally pokemon, 2 for target, 3 for onchoose target, 4 for cancel</param>
        public override void OnEnter(params object[] args)
        {
            base.OnEnter(args);
            
        }

        private List<Toggle> InitToggleUI(List<CombatPokemon> pokemons, Transform parent, List<TargetChooserUI> toggle)
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

            List<Toggle> toggles = new List<Toggle>();
            for (int i = 0; i < pokemons.Count; i++)
            {
                var targetChooserUI = parent.transform.GetChild(i).GetComponent<TargetChooserUI>();
                targetChooserUI.gameObject.SetActive(true);
                toggles.Add(parent.transform.GetChild(i).GetComponent<Toggle>());
                targetChooserUI.Init(pokemons[i]);
                toggle.Add(targetChooserUI.GetComponent<TargetChooserUI>());
                if (pokemons[i].TrainerID == Game.trainer.id)
                    userToggle.Add(targetChooserUI.GetComponent<TargetChooserUI>());
            }
            toggles.AutomateNavigation(DirectionType.Horizontal);

            return toggles;
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

        public void ShowTargetChooser(Targets target)
        {
            List<int> targets = new List<int>();

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
                        UIManager.Instance.Hide(this);
                        // Panel.SetActive(false);
                    });
                    break;
            }

            if(FirstSelectable==null)
                FirstSelectable = oppoToggle.First((o) =>
                {
                    return o.toggle.interactable;
                }).gameObject;
            if (FirstSelectable == null)
            {
                FirstSelectable = allyToggle.First((o) =>
                {
                    return o.toggle.interactable;
                }).gameObject;
            }

            if (FirstSelectable == null)
            {
                FirstSelectable = Cancel.interactable ? Cancel.gameObject : Submit.gameObject;
            }
            if (Submit.gameObject.activeSelf)
                Submit.onClick.AddListener(() =>
                    {
                        targets.AddRange(from o in oppoToggle where o.toggle.isOn == true select o.Pokemon.CombatID);
                        targets.AddRange(from o in allyToggle where o.toggle.isOn == true select o.Pokemon.CombatID);
                        OnChooseTarget?.Invoke(targets);
                        UIManager.Instance.Hide(this);
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
            UIManager.Instance.Hide(this);
        }
    }
}