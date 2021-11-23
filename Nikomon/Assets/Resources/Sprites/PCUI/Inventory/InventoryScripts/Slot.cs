using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay.UI.UIFramework;
using GamePlay.UI.UtilUI;
using UnityEngine;
using UnityEngine.UI;

public class Slot :MonoBehaviour
//PCImage的升级版
{

    public PCItem pcItem;//pcItem有宝可梦的信息。
    public Image slotImage;//储存图片
    public Text slotNum;
    //直接用宝可梦
    //public Pokemon pokemon;
    public GameObject itemInSlot;
    public GameObject menu;
    //public PCManager PCManager;
    public int judge=0;
    public int number = 0;


    public void OnClicked()
    {
        // if (judge == 0)
        // {
        //     //PCManager.refreshMenu();
        //     PCManager.refreshInformation(number);
        //     PCManager.openInform();
        //     judge = 1;
        // }
        // else
        // {
        //     PCManager.closeInform();
        //     judge = 0;
        // }
        Action<int> action = (o) =>
        {
            switch (o)
            {
                case 0:
                    Debug.Log("查看信息");
                    if (judge == 0)
                    {
                        //PCManager.refreshMenu();
                        PCManager.refreshInformation(number);
                        PCManager.openInform();
                        judge = 1;
                    }
                    else
                    {
                        PCManager.closeInform();
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
                    pcItem = null;
                    transform.Find("Item").GetComponent<ItemOnMove>().myPC.itemList[number] = null;
                    PCManager.Refresh();
                    break;
                case 4:
                    Debug.Log("查看能力");
                    break;
                case 5:
                    Debug.Log("取消");
                    break;
            }
        };
        UIManager.Instance.Show<DialogueChooserPanel>(new string[]
        {
            "查看信息", "标记","持有物","放生","查看能力","取消"
        }, new Vector2(0, 1),action, itemInSlot.transform.parent as RectTransform);
    }

    public void SetupSlot(PCItem item,int num)
    {
        number = num;
        if(item == null)//这个格子里面没有精灵的情况
        {
            itemInSlot.SetActive(false);
            return;
        }
        slotImage.sprite = item.itemImage;
        slotNum.text = item.itemNumber.ToString();//应该最后没有用的
        pcItem = item;//为了更新显示信息用的
    }
    public void Update()
    {
        
    }
}
