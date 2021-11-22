using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditemToPCInventory : MonoBehaviour
{
    // Start is called before the first frame update
    //先随便绑定一个物体，回来再细看
    public PCInventory myPC;
    public PCItem item;
    public void AddNewItem(Pokemon pokemon)
    {
        //没有number的时候就是先找NONe的去添加，找不到了再去itemList.add这样去添加（add现在还没写）
        item.Init(pokemon);
        for(int i = 0;i<myPC.itemList.Count; i++)
        {
            if(myPC.itemList[i] == null)
            {
                myPC.itemList[i] = item;//先找是None的
                break;
            }
        }
    }
    
    public void AddNewItem(Pokemon pokemon,int number)
    {
        //有number的情况，目的是为了ItemonMove的情况，用按钮里面的pokemon去更新PC里面的精灵，这时候是指定的位置！！
        item.Init(pokemon);
        myPC.itemList[number] = item;//先找是None的
    }
}
