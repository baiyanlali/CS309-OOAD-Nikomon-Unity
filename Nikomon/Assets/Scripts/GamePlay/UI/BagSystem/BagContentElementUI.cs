using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay;
using GamePlay.UI.UIFramework;
using GamePlay.UI.UtilUI;
using PokemonCore.Attack.Data;
using PokemonCore.Combat;
using PokemonCore.Inventory;
using UnityEngine;
using UnityEngine.UI;

public class BagContentElementUI : MonoBehaviour
{
    public Text Name;
    public Text Number;
    public Image ItemIcon;
    private Button _button;

    public void Init(Item item, int num,List<string> options,Action<int,Item> action)
    {
        // Name = Name ? Name : transform.Find("Name").GetComponent<Text>();
        // Number = Number ? Number : transform.Find("Number").GetComponent<Text>();
        // ItemIcon = ItemIcon ? ItemIcon : transform.Find("Icon").GetComponent<Image>();
        // _button = _button ? _button : GetComponent<Button>();
        //
        // Name.text = item.name;
        // Number.text = $"X  {num}";
        //
        // _button.onClick.RemoveAllListeners();a
        // _button.onClick.AddListener(() =>
        // {
        //     UIManager.Instance.Show<DialogueChooserPanel>(options, new Vector2(0,1),(Action<int>)((index)=>action(index,item)),transform as RectTransform);
        // });
        Number = Number ? Number : transform.Find("Number").GetComponent<Text>();
        ItemIcon.sprite =  GameResources.ItemIcons[(item.tag,item.ID)];
        _button = _button ? _button : GetComponent<Button>();

        Name.text = item.name;
        Number.text = $"X  {num}";
        
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() =>
        {
            UIManager.Instance.Show<DialogueChooserPanel>(options, new Vector2(0,1),(Action<int>)((index)=>action(index,item)),transform as RectTransform);
        });
    }
}