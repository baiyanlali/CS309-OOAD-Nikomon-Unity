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
        // print("New Item On Move" + NameIndexed);
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
        else if (obj.GetComponent<PCParty>())
        {
            PCParty chooser = obj.GetComponent<PCParty>();
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
    }
}