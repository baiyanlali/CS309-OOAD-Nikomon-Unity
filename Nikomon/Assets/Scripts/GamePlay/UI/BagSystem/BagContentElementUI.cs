using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay.UI.Dialog;
using GamePlay.UI.UIFramework;
using GamePlay.UI.UtilUI;
using PokemonCore.Attack.Data;
using PokemonCore.Combat;
using PokemonCore.Inventory;
using UnityEngine;
using UnityEngine.UI;

public class BagContentElementUI : MonoBehaviour
{
    private Text Name;
    private Text Number;
    private Image ItemIcon;
    private Button _button;

    public void Init(Item item, int num)
    {
        Name = Name ? Name : transform.Find("Name").GetComponent<Text>();
        Number = Number ? Number : transform.Find("Number").GetComponent<Text>();
        ItemIcon = ItemIcon ? ItemIcon : transform.Find("Icon").GetComponent<Image>();
        _button = _button ? _button : GetComponent<Button>();

        Name.text = item.name;
        Number.text = $"X  {num}";
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() =>
        {
            Action<int> action = (i) =>
            {
                // BagUI.Instance.Hide();
                
                switch (i)
                {
                    case 0:
                        print("TODO");
                        // TargetChooserHandler.Instance.OnChooseTarget = (list) =>
                        // {
                        //     // BattleUIHandler.Instance.UseItem(item, list);
                        //     TargetChooserHandler.Instance.OnChooseTarget = null;
                        // };
                        // TargetChooserHandler.Instance.ShowTargetChooser(Targets.SELECTED_OPPONENT_POKEMON);
                        break;
                    case 1:
                        UIManager.Instance.Hide<BagPanelUI>();
                        break;
                }
            };
            UIManager.Instance.Show<DialogueChooserPanel>(new[] {"Use", "Cancel"}, new Vector2(0,1),action,transform as RectTransform);
            // DialogChooserUI.Instance.ShowChooser(new[] {"Use", "Cancel"}, new Vector2(0,1),(i) =>
            // {
            //     BagUI.Instance.Hide();
            //     switch (i)
            //     {
            //         case 0:
            //             // TargetChooserHandler.Instance.OnChooseTarget = (list) =>
            //             // {
            //             //     // BattleUIHandler.Instance.UseItem(item, list);
            //             //     TargetChooserHandler.Instance.OnChooseTarget = null;
            //             // };
            //             // TargetChooserHandler.Instance.ShowTargetChooser(Targets.SELECTED_OPPONENT_POKEMON);
            //             break;
            //         case 1:
            //             break;
            //     }
            // },transform as RectTransform);
        });
        //TODO: ADD ItemIcon
    }
}