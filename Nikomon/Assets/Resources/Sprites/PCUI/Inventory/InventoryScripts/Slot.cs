using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay;
using GamePlay.UI.UIFramework;
using GamePlay.UI.UtilUI;
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
                    Debug.Log("标记");
                    break;
                case 2:
                    Debug.Log("持有物");
                    break;
                case 3:
                    Debug.Log("放生");
                    UIManager.Instance.Show<ConfirmPanel>("Are you sure to release this pokemon?", (Action<bool>)((o) =>
                    {
                        if (o == true)
                        {
                            // pcItem = null;
                            
                            // transform.Find("Item").GetComponent<ItemOnMove>().myPC.itemList[number] = null;
                            // PCManager.Refresh();
                            UIManager.Instance.Refresh<PCManager>();
                        }
                        else
                        {
                            
                        }
                    }));
                    
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
