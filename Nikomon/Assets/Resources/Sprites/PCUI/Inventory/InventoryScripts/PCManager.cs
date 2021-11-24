using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using GamePlay;
using GamePlay.Messages;
using GamePlay.UI.UIFramework;
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
        // print(index);
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

