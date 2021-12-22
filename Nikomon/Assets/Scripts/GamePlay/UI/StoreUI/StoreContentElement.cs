using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay;
using GamePlay.UI.BagSystem;
using GamePlay.UI.UIFramework;
using GamePlay.UI.UtilUI;
using PokemonCore.Combat;
using PokemonCore.Inventory;
using UnityEngine;
using UnityEngine.UI;

public class StoreContentElement : MonoBehaviour
{
    public Text Name;
    public Text Number;
    public Image ItemIcon;
    public Button _button;
    public int number;
    public GameObject obj;
    public int price;
    private Trainer _trainer;
    private (Item.Tag, int) _key;

    public void Init((Item.Tag, int) key,Item item)
    {
        ItemIcon.sprite = GameResources.ItemIcons[key];
        Name.text = item.name;
        Number.text = "$" + item.value.ToString();
        price = item.value;//商品的价格
        _key = key;//存疑？？？
        Action<int> action = (o) =>
        {
            _trainer ??= UIManager.Instance.GetUI<StorePanelUI>().trainer;
            switch (o)
            {
                case 0:
                    Debug.Log("购买");
                    
 
                    // if(_trainer.Money >= 500)
                    //     _trainer.Money -= 500;
                    if(_trainer.Money >= price)
                        _trainer.Money -= price;
                    else
                    {
                        UIManager.Instance.Show<ConfirmPanel>("You don't have enough money");
                        break;
                    }
                    //这里为啥不刷新呀？？？
                    UIManager.Instance.GetUI<StorePanelUI>().OnBuy(_key);
                    //OnBuy 是不是就好了呀？？？？
                    UIManager.Instance.GetUI<StorePanelUI>().OnRefresh();
                    break;
                case 1:
                    Debug.Log("取消");
                    break;
            }
        };
        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(() =>
        {
            UIManager.Instance.Show<DialogueChooserPanel>(new List<string>
            {
                "Buy","Cancel"
            }, new Vector2(0, 1),action,obj.transform as RectTransform);
        });

    }
}
