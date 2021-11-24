using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GamePlay.UI.UIFramework;
using P3DS2U.Editor.SPICA;
using PokemonCore.Character;
using PokemonCore.Combat;
using PokemonCore.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemOnMove : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform originalParent;
    private int currentItemID;
    public bool judge = true;
    public Pokemon temp;
    public Pokemon TempPokemon;

    private Trainer _trainer;
    private PC _pc;


    public static int NameIndexed = 0;

    private void Start()
    {
        print("New Item On Move" + NameIndexed);
        gameObject.name = "Item" + NameIndexed.ToString();
        NameIndexed += 1;
    }

    private void OnDestroy()
    {
        print("Destroy:" + gameObject.name);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log(currentItemID);
        temp = transform.parent.GetComponent<Slot>().pokemon; //新加的不知道对错，准备让pcItem随着transform的移动而移动！！！
        TempPokemon = transform.parent.GetComponent<Slot>().pokemon;
        // transform.parent.GetComponent<Slot>().pokemon=null;//新加的不知道对错
        originalParent = transform.parent;
        // Debug.Log(currentItemID);
        currentItemID = originalParent.GetComponent<Slot>().index;
        transform.SetParent(transform.parent.parent.parent.parent); //原来两个parent！！
        transform.position = eventData.position;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        // Debug.Log(eventData.pointerCurrentRaycast.gameObject?.name);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _pc ??= UIManager.Instance.GetUI<PCManager>().pc;
        _trainer ??= UIManager.Instance.GetUI<PCManager>().trainer;
        Slot mySlot = originalParent.GetComponent<Slot>();
        GameObject obj = eventData.pointerCurrentRaycast.gameObject;
        if (obj == null)
        {
        }
        else if (obj.GetComponentInParent<Slot>())
        {
            Slot slot = obj.GetComponentInParent<Slot>();
            _pc.SwapPokemon(_pc.ActiveBox, mySlot.index, _pc.ActiveBox, slot.index);
            // (mySlot.pokemon, slot.pokemon) = (slot.pokemon, mySlot.pokemon);
        }
        else if (obj.GetComponent<PokemonChooserElementUI>())
        {
            PokemonChooserElementUI chooser = obj.GetComponent<PokemonChooserElementUI>();
            int index = -1;
            mySlot.pokemon = chooser.Poke;
            for (int i = 0; i < _trainer.party.Length; i++)
            {
                if (_trainer.party[i] == chooser.Poke)
                {
                    index = i;
                    break;
                }
            }

            if (index != -1)
                _pc.SwitchPCAndPartyPokemon(_trainer, index, mySlot.index);
        }

        transform.position = originalParent.position;
        transform.SetParent(originalParent);
        transform.parent.GetComponent<Slot>().pokemon = temp;

        GetComponent<CanvasGroup>().blocksRaycasts = true;

        UIManager.Instance.Refresh<PCManager>();

        // if (eventData.pointerCurrentRaycast.gameObject.name == "Image") //移到的位置有精灵
        // {
        //     Debug.Log(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.name);
        //     transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent);
        //     transform.position = eventData.pointerPressRaycast.gameObject.transform.parent.parent.position;
        //     //改变myPC里面的list！
        //     
        //     var token = myPC.itemList[currentItemID];
        //     myPC.itemList[currentItemID] =
        //         myPC.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().number];
        //     myPC.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().number] = token;
        //     //
        //     eventData.pointerCurrentRaycast.gameObject.transform.parent.position = originalParent.position;
        //     eventData.pointerCurrentRaycast.gameObject.transform.parent.SetParent(originalParent);
        //     eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.GetComponent<Slot>().pokemon =
        //         transform.parent.GetComponent<Slot>().pokemon;
        //     transform.parent.GetComponent<Slot>().pokemon = temp;
        //     GetComponent<CanvasGroup>().blocksRaycasts = true;
        //
        //     // PCManager.Refresh(); //为了更新显示信息！！！
        //     UIManager.Instance.Refresh<PCManager>();
        //     return;
        // }
        // else if (eventData.pointerCurrentRaycast.gameObject.name == "slot(Clone)") //移到的位置没有精灵
        // {
        //     if (eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().number == currentItemID)
        //     {
        //         //要考虑移动回自己位置的情况！！！！
        //         transform.position = originalParent.position;
        //         transform.SetParent(originalParent);
        //         transform.parent.GetComponent<Slot>().pokemon = temp; //新加的不知道对错
        //         GetComponent<CanvasGroup>().blocksRaycasts = true;
        //         return;
        //     }
        //
        //     transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
        //     transform.position = eventData.pointerPressRaycast.gameObject.transform.position;
        //     //改变myPC里面的list！
        //     myPC.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().number] =
        //         myPC.itemList[currentItemID];
        //     myPC.itemList[currentItemID] = null;
        //
        //     //
        //     transform.parent.GetComponent<Slot>().pokemon = temp; 
        //     eventData.pointerCurrentRaycast.gameObject.transform.Find("Item").position = originalParent.position;
        //     eventData.pointerCurrentRaycast.gameObject.transform.Find("Item").SetParent(originalParent);
        //     GetComponent<CanvasGroup>().blocksRaycasts = true;
        //     UIManager.Instance.Refresh<PCManager>();
        //     // PCManager.Refresh(); //为了可以正常显示information！！！
        //     return;
        // }
        // else if (eventData.pointerCurrentRaycast.gameObject.name == "PokemonStatButton")
        // {
        //     //TODO：按钮和item的转化！！！
        //     Debug.Log("shit");
        //     transform.position = originalParent.position; //transform是item
        //     transform.SetParent(originalParent);
        //     var token = eventData.pointerCurrentRaycast.gameObject.GetComponent<PokemonChooserElementUI>().Poke;
        //     if (token == null)
        //     {
        //         Debug.Log("pokenmon in PokemonChooserElementUI is shit");
        //     }
        //     if (TempPokemon == null)
        //     {
        //         Debug.Log("pokenmon in item is shit");
        //     }
        //     eventData.pointerCurrentRaycast.gameObject.GetComponent<PokemonChooserElementUI>().UpdateData(
        //         TempPokemon); //用pcItem中的pokenmon来更新按钮里面的信息的,但是不知道对不对（目前的item里面的pokemon都是null）所以目前没法debug！
        //     
        //     //还缺用按钮里面的pokemon去更新PC里面的精灵
        //     AdditemToPCInventory additemToPCInventory = new AdditemToPCInventory();//这样初始化不知道对不对
        //     additemToPCInventory.AddNewItem(token, currentItemID);
        //     UIManager.Instance.Refresh<PCManager>();
        //     // PCManager.Refresh();
        //     /*
        //     myPC.itemList[eventData.pointerCurrentRaycast.gameObject.GetComponentInParent<Slot>().number] =
        //     myPC.itemList[currentItemID];
        //     myPC.itemList[currentItemID] = null;
        //      */
        //     //上面的是把PC里面的宝可梦移到没有宝可梦的格子里面的方法可以参考一下更改itemlist！！！！
        //     //eventData.pointerCurrentRaycast.gameObject.transform.Find("Item").SetParent(originalParent);
        //     GetComponent<CanvasGroup>().blocksRaycasts = true;
        //     return;
        // }
        // else if (eventData.pointerCurrentRaycast.gameObject == null) //目前没有用，可以补充！！！！
        // {
        //     transform.position = originalParent.position;
        //     transform.SetParent(originalParent);
        //     transform.parent.GetComponent<Slot>().pokemon = temp; 
        //     GetComponent<CanvasGroup>().blocksRaycasts = true;
        // }
        // else
        // {
        //     transform.position = originalParent.position;
        //     transform.SetParent(originalParent);
        //     transform.parent.GetComponent<Slot>().pokemon = temp; 
        //     GetComponent<CanvasGroup>().blocksRaycasts = true;
        // }


        // catch (NullReferenceException e)
        // {
        //     //有可能是拖拽的最终位置是个NULLReference，亦或者var temp = eventData.pointerCurrentRaycast.gameObject.GetComponent<PokemonChooserElementUI>().Poke;是null！
        //     //两种来到这里的可能
        //     Debug.Log("NullReferenceException");
        //     transform.position = originalParent.position;
        //     transform.SetParent(originalParent);
        //     transform.parent.GetComponent<Slot>().pokemon = temp; //新加的不知道对错
        //     GetComponent<CanvasGroup>().blocksRaycasts = true;
        // }
    }
}