using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemOnMove : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler
{
    public Transform originalParent;
    public PCInventory myPC;
    private int currentItemID;
    public bool judge = true;
    public PCItem temp;
    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log(currentItemID);
        temp = transform.parent.GetComponent<Slot>().pcItem;//新加的不知道对错，准备让pcItem随着transform的移动而移动！！！
        transform.parent.GetComponent<Slot>().pcItem=null;//新加的不知道对错
        originalParent = transform.parent;
        Debug.Log(currentItemID);
        currentItemID = originalParent.GetComponent<Slot>().number;
        transform.SetParent(transform.parent.parent.parent.parent);//原来两个parent！！
        transform.position = eventData.position;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position; 
        Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        try
        {
            if (eventData.pointerCurrentRaycast.gameObject.name == "Image") //移到的位置有精灵
            {
                Debug.Log(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.name);
                transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent);
                transform.position = eventData.pointerPressRaycast.gameObject.transform.parent.parent.position;
                //改变myPC里面的list！
                var token = myPC.itemList[currentItemID];
                myPC.itemList[currentItemID] =
                    myPC.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().number];
                myPC.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().number] = token;
                //
                eventData.pointerCurrentRaycast.gameObject.transform.parent.position = originalParent.position;
                eventData.pointerCurrentRaycast.gameObject.transform.parent.SetParent(originalParent);
                eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<Slot>().pcItem =
                    transform.parent.GetComponent<Slot>().pcItem;
                transform.parent.GetComponent<Slot>().pcItem = temp;
                GetComponent<CanvasGroup>().blocksRaycasts = true;

                PCManager.Refresh(); //为了更新显示信息！！！

                return;
            }
            else if (eventData.pointerCurrentRaycast.gameObject.name == "slot(Clone)") //移到的位置没有精灵
            {
                if (eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().number == currentItemID)
                {
                    //要考虑移动回自己位置的情况！！！！
                    transform.position = originalParent.position;
                    transform.SetParent(originalParent);
                    transform.parent.GetComponent<Slot>().pcItem = temp; //新加的不知道对错
                    GetComponent<CanvasGroup>().blocksRaycasts = true;
                    return;
                }

                transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
                transform.position = eventData.pointerPressRaycast.gameObject.transform.position;
                //改变myPC里面的list！
                myPC.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().number] =
                    myPC.itemList[currentItemID];
                myPC.itemList[currentItemID] = null;

                //
                transform.parent.GetComponent<Slot>().pcItem = temp; 
                eventData.pointerCurrentRaycast.gameObject.transform.Find("Item").position = originalParent.position;
                eventData.pointerCurrentRaycast.gameObject.transform.Find("Item").SetParent(originalParent);
                GetComponent<CanvasGroup>().blocksRaycasts = true;
                PCManager.Refresh(); //为了可以正常显示information！！！
                return;
            }
            else if (eventData.pointerCurrentRaycast.gameObject.name == "PokemonStatButton")
            {
                //TODO：按钮和item的转化！！！
                Debug.Log("shit");
                transform.position = originalParent.position; //transform是item
                transform.SetParent(originalParent);
                // var temp = eventData.pointerCurrentRaycast.gameObject.GetComponent<PokemonChooserElementUI>().Poke;
                eventData.pointerCurrentRaycast.gameObject.GetComponent<PokemonChooserElementUI>().UpdateData(
                    transform.GetComponentInParent<Slot>().pcItem
                        .pokemon); //用pcItem中的pokenmon来更新按钮里面的信息的,但是不知道对不对（目前的item里面的pokemon都是null）所以目前没法debug！
                
                //还缺用按钮里面的pokemon去更新PC里面的精灵
                AdditemToPCInventory additemToPCInventory = new AdditemToPCInventory();//这样初始化不知道对不对
                // additemToPCInventory.AddNewItem(temp, currentItemID);
                /*
                myPC.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().number] =
                myPC.itemList[currentItemID];
                myPC.itemList[currentItemID] = null;
                 */
                //上面的是把PC里面的宝可梦移到没有宝可梦的格子里面的方法可以参考一下更改itemlist！！！！
                //eventData.pointerCurrentRaycast.gameObject.transform.Find("Item").SetParent(originalParent);
                GetComponent<CanvasGroup>().blocksRaycasts = true;
                return;
            }
            else if (eventData.pointerCurrentRaycast.gameObject == null) //目前没有用，可以补充！！！！
            {
                transform.position = originalParent.position;
                transform.SetParent(originalParent);
                transform.parent.GetComponent<Slot>().pcItem = temp; 
                GetComponent<CanvasGroup>().blocksRaycasts = true;
            }
            else
            {
                transform.position = originalParent.position;
                transform.SetParent(originalParent);
                transform.parent.GetComponent<Slot>().pcItem = temp; 
                GetComponent<CanvasGroup>().blocksRaycasts = true;
            }

        }
        catch (NullReferenceException e)
        {
            //有可能是拖拽的最终位置是个NULLReference，亦或者var temp = eventData.pointerCurrentRaycast.gameObject.GetComponent<PokemonChooserElementUI>().Poke;是null！
            //两种来到这里的可能
            Debug.Log("NullReferenceException");
            transform.position = originalParent.position;
            transform.SetParent(originalParent);
            transform.parent.GetComponent<Slot>().pcItem = temp; //新加的不知道对错
            GetComponent<CanvasGroup>().blocksRaycasts = true;
        }

       
    }

}
