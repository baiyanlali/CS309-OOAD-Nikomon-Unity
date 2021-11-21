using System.Collections;
using System.Collections.Generic;
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
        if (judge == 0)
        {
            PCManager.refreshMenu();
            PCManager.refreshInformation(number);
            DialogChooserUI.Instance.transform.SetParent(itemInSlot.transform.parent.parent);
            DialogChooserUI.Instance.ShowChooser(new string[] { "Open", "Close" }, new Vector2(0, 0), (o) =>
            {
                switch (o)
                {
                    case 0:
                        Debug.Log(0);
                        break;
                    case 1:
                        Debug.Log(1);
                        break;
                }
            }, itemInSlot.transform.parent as RectTransform);
            PCManager.openInform();
            judge = 1;
        }
        else
        {
            Debug.Log("111111111");
            //menu.SetActive(false);
            PCManager.closeInform();

            judge = 0;
        }
    }

    public void SetupSlot(PCItem item,int num)
    {
        if(item == null)//这个格子里面没有精灵的情况
        {
            itemInSlot.SetActive(false);
            return;
        }
        slotImage.sprite = item.itemImage;
        slotNum.text = item.itemNumber.ToString();//应该最后没有用的
        pcItem = item;//为了更新显示信息用的
        number = num;
    }
    public void Update()
    {
        
    }
}
