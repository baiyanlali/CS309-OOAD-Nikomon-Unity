using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay;
using GamePlay.UI.UIFramework;
using GamePlay.UI.UtilUI;
using PokemonCore.Character;
using PokemonCore.Combat;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Slot :MonoBehaviour
//PCImage的升级版
{

    // public PCItem pcItem;//pcItem有宝可梦的信息。
    public Image slotImage;//储存图片
    //直接用宝可梦
    //public Pokemon pokemon;
    public GameObject itemInSlot;
    //public PCManager PCManager;
    public int judge=0;
    public int index = 0;

    public Pokemon pokemon;

    public Action<int> RefreshInformation;
    public Action<bool> ShowInfo;
    private PC _pc;
    private Trainer _trainer;

    public void OnClicked()
    {
        Action<int> action = (o) =>
        {
            switch (o)
            {
                case 0:
                    Debug.Log("查看信息");
                    if (judge == 0)
                    {
                        //PCManager.refreshMenu();
                        // PCManager.refreshInformation(number);
                        RefreshInformation(index);
                        // PCManager.openInform();
                        ShowInfo(true);
                        judge = 1;
                    }
                    else
                    {
                        // PCManager.closeInform();
                        ShowInfo(false);
                        judge = 0;
                    }
                    break;
                case 1:
                    Debug.Log("交换1");
                    _pc ??= UIManager.Instance.GetUI<PCManager>().pc;
                    _trainer ??= UIManager.Instance.GetUI<PCManager>().trainer;
                    bool[] exchangeIndex = UIManager.Instance.GetUI<PCManager>().exchangeIndex;
                    for (int i = 0; i < exchangeIndex.Length; i++)
                    {
                        if (exchangeIndex[i])
                        {
                            if (i <= 5)
                            {
                                //i<=5说明上一个选择的宝可梦是位于背包的。这种情况是先标记背包再标记PC的时候
                                exchangeIndex[i] = false;
                                _pc.SwitchPCAndPartyPokemon(_trainer, i, index);
                                UIManager.Instance.Refresh<PCManager>();
                                return;
                            }
                            else
                            {
                                //这种情况是PC内部进行交换的！！！
                                exchangeIndex[i] = false;
                                _pc.SwapPokemon(_pc.ActiveBox, i-6, _pc.ActiveBox, index);
                                UIManager.Instance.Refresh<PCManager>();
                                return;
                            }
                        }
                    }
                    exchangeIndex[index+ 6] = true;//因为前六个是背包！！！
                    break;
                case 2:
                    Debug.Log("持有物");
                    break;
                case 3:
                    Debug.Log("放生");
                    Action<bool> action = (Action<bool>) ((o) =>
                    {
                        if (o == true)
                        {
                            _pc ??= UIManager.Instance.GetUI<PCManager>().pc;
                            _pc.Pokemons[index] = null;
                    
                            UIManager.Instance.Refresh<PCManager>();
                        }
                        else
                        {
                    
                        }
                    });
                    UIManager.Instance.Show<ConfirmPanel>("Are you sure to release this pokemon?", action);
                    
                    break;
                case 4:
                    Debug.Log("查看能力");
                    break;
                case 5:
                    Debug.Log("取消");
                    break;
            }
        };
        UIManager.Instance.Show<DialogueChooserPanel>(new List<string>
        {
            "查看信息", "标记","持有物","放生","查看能力","取消"
        }, new Vector2(0, 1),action, itemInSlot.transform.parent as RectTransform);
    }

    public void SetupSlot(Pokemon item,int num,Action<int> refresh,Action<bool> showinfo)
    {
        pokemon = item;
        index = num;
        this.RefreshInformation = refresh;
        this.ShowInfo = showinfo;
        if(item == null)//这个格子里面没有精灵的情况
        {
            itemInSlot.SetActive(false);
            return;
        }
        itemInSlot.SetActive(true);
        slotImage.sprite = GameResources.PokemonIcons[item.ID];
        
        
    }

}
