using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using GamePlay;
using GamePlay.Messages;
using GamePlay.UI.UIFramework;
using GamePlay.UI.UtilUI;
using PokemonCore.Character;
using PokemonCore.Combat;
using PokemonCore.Inventory;
using UnityEditor.VersionControl;

public class PCManager : BaseUI
{

    public PC pc;
    public Trainer trainer;

    public GameObject imageGrid;//PC里面的格子。
    public GameObject information;
    public Text Name,HP, ATK, DEF, SPA, SPD, SPE;

    public PokemonChooserTableUI TableUI;
    //public PCImage imagePrefab;
    //public Text information;//介绍宝可梦的信息。(先不要)

    public List<Slot> slots = new List<Slot>();
    public GameObject emptySlot;
    private bool[] judge;
    public bool[] exchangeIndex;//用于交换作用的！！！目前是想设它为26长的一个布尔数组，前六位是背包，后20位是PC

    private void Awake()
    {
        imageGrid.SetActive(true);
        if (emptySlot == null)
            emptySlot = GameResources.SpawnPrefab("Slot");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args">0 for trainer, 1 for pc</param>
    public override void OnEnter(params object[] args)
    {
        base.OnEnter(args);
        if (args != null)
        {
            trainer=args[0] as Trainer;
            pc = args[1] as PC;
            judge = new bool[trainer.party.Length];
            exchangeIndex = new bool[trainer.party.Length+pc.Pokemons.Length];
            for (int i = 0; i < judge.Length; i++)
            {
                judge[i] = false;
            }
            for (int i = 0; i < exchangeIndex.Length; i++)
            {
                exchangeIndex[i] = false;
            }
        }

        
        imageGrid.SetActive(true);
        if (emptySlot == null)
            emptySlot = GameResources.SpawnPrefab("Slot");

        OnRefresh();
        
    }

    private void OnEnable()
    {
        imageGrid.SetActive(true);
        // OnRefresh();
    }

    public override void OnRefresh(params object[] args)
    {
        base.OnRefresh(args);

        if (slots.Count == 0)
        {
            for (int i = 0; i < pc.Pokemons.Length; i++)
            {
                slots.Add(Instantiate(emptySlot).GetComponent<Slot>());
                slots[i].transform.SetParent(imageGrid.transform);
                slots[i].GetComponent<Slot>().SetupSlot(pc.Pokemons[i],i,RefreshInformation,ShowInfo);
            }
        }
        else
        {
            for (int i = 0; i < pc.Pokemons.Length; i++)
            {
                slots[i].GetComponent<Slot>().SetupSlot(pc.Pokemons[i],i,RefreshInformation,ShowInfo);
            }
        }
        
        // for (int i = 0; i < imageGrid.transform.childCount; i++)
        // {
        //     if(imageGrid.transform.childCount == 0)
        //         break;
        //     Destroy(imageGrid.transform.GetChild(i).gameObject);
        // }
        // slots.Clear();
        // Debug.Log(imageGrid.transform.childCount);
        // for (int i = 0; i < pc.Pokemons.Length; i++)
        // {
        //     slots.Add(Instantiate(emptySlot).GetComponent<Slot>());
        //     slots[i].transform.SetParent(imageGrid.transform);
        //     slots[i].GetComponent<Slot>().SetupSlot(pc.Pokemons[i],i,RefreshInformation,ShowInfo);
        // }
        
        TableUI.Init(trainer,new []{"查看信息", "标记","持有物","放生","查看能力","取消"},HandleChooserTalbeUI);
    }

    private void HandleChooserTalbeUI(int chooseIndex,int bagIndex)
    {
        print(chooseIndex+" "+bagIndex);
        switch (chooseIndex)
        {
            case 0:
                Debug.Log("查看信息");
                if (!judge[bagIndex] )
                {
                    RefreshInformationButton(bagIndex);
                    ShowInfo(true);
                    for (int i = 0; i < judge.Length; i++)
                    {
                        judge[i] = false;
                    }
                    judge[bagIndex] = true;
                }
                else
                {
                    ShowInfo(false);
                    judge[bagIndex] = false;
                }
                break;
            case 1:
                Debug.Log("交换");
                for (int i = 0; i < exchangeIndex.Length; i++)
                {
                    if (exchangeIndex[i])
                    {
                        if (i <= 5)
                        {//背包里面的交换!
                            exchangeIndex[i] = false;
                            Pokemon temp = trainer.party[i];
                            trainer.party[i] = trainer.party[bagIndex];
                            trainer.party[bagIndex] = temp;
                            TableUI.ExchangeData(trainer);
                            return;
                        }
                        else
                        {//先标记的PC，然后再选择了背包里面的宝可梦进行交换！！！
                            exchangeIndex[i] = false;
                            pc.SwitchPCAndPartyPokemon(trainer, bagIndex, i-6);
                            UIManager.Instance.Refresh<PCManager>();
                            return;
                        }
                    }
                }
                exchangeIndex[bagIndex] = true;
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
                        trainer.party[bagIndex] = null;
                        for (int i = bagIndex; i < trainer.party.Length-1; i++)
                        {
                            trainer.party[i] = trainer.party[i + 1];
                        }
                        trainer.party[trainer.party.Length-1] = null;
                        
                        //TableUI.Init(trainer,new []{"查看信息", "标记","持有物","放生","查看能力","取消"},HandleChooserTalbeUI);
                        TableUI.UpdateData(trainer);
                    
                        //UIManager.Instance.Refresh<PCManager>();
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
        
    }
    

    public void RefreshInformationButton(int number)
    {
        if (trainer.party[number] == null)
        {
            Name.text = number.ToString();
            HP.text = "bbb";
            ATK.text = "ccc";
            DEF.text = "ddd";
            SPA.text = "eee";
            SPD.text = "fff";
            SPE.text = "ggg";
        }
        else
        {
            Pokemon temp = trainer.party[number];
            Name.text = temp.Name.ToString();
            HP.text = temp.HP.ToString();
            ATK.text = temp.ATK.ToString();
            DEF.text = temp.DEF.ToString();
            SPA.text = temp.ToString();
            SPD.text = temp.ToString();
            SPE.text = temp.ToString();
        }
    }
    public void RefreshInformation(int number)
    {
        if (slots[number].pokemon == null)
        {
            Name.text = slots[number].index.ToString();
            HP.text = Messages.Get(slots[number].pokemon.Name);
            ATK.text = slots[number].pokemon.ATK.ToString();
            DEF.text = "ddd";
            SPA.text = "eee";
            SPD.text = "fff";
            SPE.text = "ggg";
        }
        else
        {
            Name.text = slots[number].pokemon.Name.ToString();
            HP.text = slots[number].pokemon.HP.ToString();
            ATK.text = slots[number].pokemon.ATK.ToString();
            DEF.text = slots[number].pokemon.DEF.ToString();
            SPA.text = slots[number].pokemon.SPA.ToString();
            SPD.text = slots[number].pokemon.SPD.ToString();
            SPE.text = slots[number].pokemon.SPE.ToString();
        }
    }

    public void ShowInfo(bool isShow)
    {
        information.SetActive(isShow);
    }


}

