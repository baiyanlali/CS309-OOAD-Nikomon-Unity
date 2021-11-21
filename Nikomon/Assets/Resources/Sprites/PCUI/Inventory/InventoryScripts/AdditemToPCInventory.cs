using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdditemToPCInventory : MonoBehaviour
{
    // Start is called before the first frame update
    //还没用过！！先写一下
    public PCInventory myPC;
    public PCItem item;
    public void AddNewItem(Pokemon pokemon)
    {
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
}
