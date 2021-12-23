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
    public int number;

    public Pokemon pokemon;

    public Action<Pokemon> RefreshInformation;
    public Action<bool> ShowInfo;
    private PC _pc;
    private Trainer _trainer;

    public void OnClicked()
    {
        Action<int> action = (o) =>
        {
            _pc ??= UIManager.Instance.GetUI<PCManager>().pc;
            _trainer ??= UIManager.Instance.GetUI<PCManager>().trainer;
            switch (o)
            {
                case 0:
                    Debug.Log("查看信息");
                    UIManager.Instance.Show<AbilityPanel>(_trainer,pokemon);
                    // if (judge == 0)
                    // {
                    //     //PCManager.refreshMenu();
                    //     // PCManager.refreshInformation(number);
                    //     RefreshInformation(pokemon);
                    //     // PCManager.openInform();
                    //     ShowInfo(true);
                    //     judge = 1;
                    // }
                    // else
                    // {
                    //     // PCManager.closeInform();
                    //     ShowInfo(false);
                    //     judge = 0;
                    // }
                    break;
                case 1:
                    Debug.Log("交换1");
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
                                _pc.SwapPokemon((i-6)/_pc.Pokemons.Length, (i-6)%_pc.Pokemons.Length, _pc.ActiveBox, index);
                                UIManager.Instance.Refresh<PCManager>();
                                return;
                            }
                        }
                    }
                    exchangeIndex[_pc.ActiveBox * _pc.Pokemons.Length + index+ 6] = true;//因为前六个是背包！！！
                    //exchangeIndex[index+ 6] = true;//因为前六个是背包！！！
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
                            _pc.RemovePokemon(_pc.ActiveBox,index);
                    
                            UIManager.Instance.Refresh<PCManager>();
                        }
                        else
                        {
                    
                        }
                    });
                    UIManager.Instance.Show<ConfirmPanel>("Are you sure to release this pokemon?", action);
                    
                    break;
                case 4:
                    Debug.Log("加入背包");
                    for (int i = 0; i < _trainer.party.Length; i++)
                    {
                        if (_trainer.party[i] == null)
                        {
                            (_trainer.party[i], _pc.Pokemons[index]) = (_pc.Pokemons[index],_trainer.party[i]);
                            UIManager.Instance.Refresh<PCManager>();
                            return;
                        }
                    }
                    
                    UIManager.Instance.Show<ConfirmPanel>("Your party is full");
                    
                    break;
                case 5:
                    Debug.Log("取消");
                    break;
            }
        };
        if(pokemon == null)
            print("wyf6888");
        if (pokemon != null)
        {
            UIManager.Instance.Show<DialogueChooserPanel>(new List<string>
            {
                "查看信息", "标记", "持有物", "放生", "加入背包", "取消"
            }, new Vector2(0, 1), action, itemInSlot.transform.parent as RectTransform);
        }
    }

    public void SetupSlot(Pokemon item,int num,Action<Pokemon> refresh,Action<bool> showinfo)
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
