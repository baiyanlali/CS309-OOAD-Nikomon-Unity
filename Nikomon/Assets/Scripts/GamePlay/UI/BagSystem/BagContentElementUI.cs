using System.Collections;
using System.Collections.Generic;
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
            DialogChooserUI.Instance.ShowChooser(new[] {"Use", "Cancel"}, (i) =>
            {
                BagUI.Instance.Hide();
                switch (i)
                {
                    case 0:
                        TargetChooserHandler.Instance.OnChooseTarget = (list) =>
                        {
                            BattleUIHandler.Instance.UseItem(item, list);
                            TargetChooserHandler.Instance.OnChooseTarget = null;
                        };
                        TargetChooserHandler.Instance.ShowTargetChooser(Targets.SELECTED_OPPONENT_POKEMON);
                        break;
                    case 1:
                        break;
                }
            },transform as RectTransform);
        });
        //TODO: ADD ItemIcon
    }
}