using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using GamePlay;
using GamePlay.Messages;
using GamePlay.UI.BattleUI;
using GamePlay.UI.UIFramework;
using GamePlay.UI.UtilUI;
using PokemonCore;
using PokemonCore.Character;
using PokemonCore.Combat;
using PokemonCore.Inventory;
using UnityEditor.VersionControl;
using Debug = UnityEngine.Debug;

public class PCManager : BaseUI
{

    public PC pc;
    public Trainer trainer;

    public override bool IsOnly => true;

    public Text BoxTitle;

    public GameObject imageGrid;//PC里面的格子。
    public GameObject information;
    public Text Name,HP, ATK, DEF, SPA, SPD, SPE;

    public PartyInPC TableUI;//加了个<PCParty>

    public List<Slot> slots = new List<Slot>();
    public GameObject emptySlot;
    private bool[] judge;
    public bool[] exchangeIndex;//用于交换作用的！！！目前是想设它为26长的一个布尔数组，前六位是背包，后20位是PC
    //public bool[] ifhasPokemon;//用于检测有没有宝可梦的

    public Transform MoveDetial;

    private GameObject MoveElementPrefab;

    private List<MoveElement> _moveElements=new List<MoveElement>();

    private void Awake()
    {
        imageGrid.SetActive(true);
        if (emptySlot == null)
            emptySlot = GameResources.SpawnPrefab("Slot");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args">0 for trainer, 1 for pc</
    
    // public override void OnEnter(params object[] args)
    // {
    //     MoveElementPrefab = GameResources.SpawnPrefab(typeof(MoveElement));
    //     if (_moveElements.Count == 0)
    //     {
    //         for (int i = 0; i < Game.MaxMovesPerPokemon; i++)
    //         {
    //             GameObject obj = Instantiate(MoveElementPrefab, MoveDetial);
    //             obj.name = "Move" + i;
    //             _moveElements.Add(obj.GetComponent<MoveElement>());
    //         }
    //     }
    //     base.OnEnter(args);
    //     if (args != null)
    //     {
    //         trainer=args[0] as Trainer;
    //         pc = args[1] as PC;
    //         judge = new bool[trainer.party.Length];
    //         exchangeIndex = new bool[trainer.party.Length+pc.Pokemons.Length];
    //         for (int i = 0; i < judge.Length; i++)
    //         {
    //             judge[i] = false;
    //         }
    //         for (int i = 0; i < exchangeIndex.Length; i++)
    //         {
    //             exchangeIndex[i] = false;
    //         }
    //         for (int i = 0; i < ifhasPokemon.Length; i++)
    //         {
    //             ifhasPokemon[i] = false;
    //         }
    //     }
    //
    //     
    //     imageGrid.SetActive(true);
    //     if (emptySlot == null)
    //         emptySlot = GameResources.SpawnPrefab("Slot");
    //
    //     OnRefresh();
    //     
    // }
    public override void OnEnter(params object[] args)
    {//12.6开始改的！
        MoveElementPrefab = GameResources.SpawnPrefab(typeof(MoveElement));
        if (_moveElements.Count == 0)
        {
            for (int i = 0; i < Game.MaxMovesPerPokemon; i++)
            {
                GameObject obj = Instantiate(MoveElementPrefab, MoveDetial);
                obj.name = "Move" + i;
                _moveElements.Add(obj.GetComponent<MoveElement>());
            }
        }
        base.OnEnter(args);
        if (args != null)
        {
            trainer=args[0] as Trainer;
            pc = args[1] as PC;
            judge = new bool[trainer.party.Length];
            print(pc.Pokemons.Length * pc.pokemons.Length);//160
            exchangeIndex = new bool[trainer.party.Length+pc.Pokemons.Length * pc.pokemons.Length];
            for (int i = 0; i < judge.Length; i++)
            {
                judge[i] = false;
            }
            for (int i = 0; i < exchangeIndex.Length; i++)
            {
                exchangeIndex[i] = false;
            }
            // for (int i = 0; i < ifhasPokemon.Length; i++)
            // {
            //     ifhasPokemon[i] = false;
            // }
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

        //pc.P
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

        BoxTitle.text = pc.BoxNames[pc.ActiveBox];
        TableUI.Init(trainer,new []{"查看信息", "交换","持有物","放生","放入仓库","取消"},HandleChooserTalbeUI);
    }

    private void HandleChooserTalbeUI(int chooseIndex,int bagIndex)
    {
        // print(chooseIndex+" "+bagIndex);
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
                        if (i == bagIndex)
                        {
                            exchangeIndex[i] = false;
                            return;
                        }
                        if (i <= 5)
                        {//背包里面的交换!
                            exchangeIndex[i] = false;
                            (trainer.party[i], trainer.party[bagIndex]) = (trainer.party[bagIndex], trainer.party[i]);
                            TableUI.ExchangeData(trainer);
                            return;
                        }
                        else
                        {//先标记的PC，然后再选择了背包里面的宝可梦进行交换！！！
                            exchangeIndex[i] = false;
                            // print("PCBoxID");
                            // print( i - 6);
                            //pc.SwitchPCAndPartyPokemon(trainer, bagIndex, i-6);
                            pc.SwitchPartyAndPCPokemon(trainer, bagIndex, i-6);
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
                        // trainer.party[bagIndex] = null;
                        bool result = trainer.RemovePokemon(bagIndex);
                        if (result == false)
                        {
                            UIManager.Instance.Show<ConfirmPanel>("Your party need at least one pokemon");
                            return;
                        }
                        // for (int i = bagIndex; i < trainer.party.Length-1; i++)
                        // {
                        //     trainer.party[i] = trainer.party[i + 1];
                        // }
                        // trainer.party[trainer.party.Length-1] = null;
                        
                        
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
                Debug.Log("放入仓库");
                Pokemon temp4 = trainer.party[bagIndex];
                // trainer.party[bagIndex] = null;
                // for (int i = bagIndex; i < trainer.party.Length-1; i++)
                // {
                //     trainer.party[i] = trainer.party[i + 1];
                // }
                // trainer.party[trainer.party.Length-1] = null;
                bool result = trainer.RemovePokemon(bagIndex);
                if (result == false)
                {
                    UIManager.Instance.Show<ConfirmPanel>("Your party need at least one pokemon");
                    return;
                }
                
                TableUI.UpdateData(trainer);
                // for (int i = 0; i < pc.Pokemons.Length; i++)
                // {
                //     if (pc.Pokemons[i] == null)
                //     {
                //         pc.Pokemons[i] = temp4;
                //         break;
                //     }
                // }
                result = pc.AddPokemon(temp4);
                if (result == false)
                {
                    UIManager.Instance.Show<ConfirmPanel>("Your PC is full which is UNBELIEVEBLE!!!");
                    trainer.AddPokemon(temp4);
                    TableUI.UpdateData(trainer);
                    return;
                }
                
                //pc.Pokemons[19] = temp4;
                UIManager.Instance.Refresh<PCManager>();
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
            Pokemon pokemon = trainer.party[number];
            Name.text = pokemon.Name.ToString();
            HP.text = pokemon.HP.ToString();
            ATK.text = pokemon.ATK.ToString();
            DEF.text = pokemon.DEF.ToString();
            SPA.text = pokemon.SPA.ToString();
            SPD.text = pokemon.SPD.ToString();
            SPE.text = pokemon.SPE.ToString();
            for (int i = 0; i < pokemon.moves.Length; i++)
            {
                if (pokemon.moves[i] == null)
                {
                    _moveElements[i].gameObject.SetActive(false);
                }
                else
                {
                    _moveElements[i].Init(pokemon.moves[i]);
                    _moveElements[i].gameObject.SetActive(true);
                }
                
            }
        }
    }
    public void RefreshInformation(Pokemon pokemon)
    {
        if (pokemon == null)
        {
            Name.text = string.Empty;
            HP.text = string.Empty;
            ATK.text = string.Empty;
            DEF.text = string.Empty;
            SPA.text = string.Empty;
            SPD.text = string.Empty;
            SPE.text = string.Empty;
        }
        else
        {
            Name.text = pokemon.Name;
            HP.text = pokemon.HP.ToString();
            ATK.text = pokemon.ATK.ToString();
            DEF.text = pokemon.DEF.ToString();
            SPA.text = pokemon.SPA.ToString();
            SPD.text = pokemon.SPD.ToString();
            SPE.text = pokemon.SPE.ToString();

            for (int i = 0; i < pokemon.moves.Length; i++)
            {
                if (pokemon.moves[i] == null)
                {
                    _moveElements[i].gameObject.SetActive(false);
                }
                else
                {
                    _moveElements[i].Init(pokemon.moves[i]);
                    _moveElements[i].gameObject.SetActive(true);
                }
                
            }
        }
    }

    public void ShowInfo(bool isShow)
    {
        information.SetActive(isShow);
        
        
    }

    public void NextPC()
    {
        pc.ChangeActiveBox(1);
        OnRefresh();
    }

    public void PreviousPC()
    {
        pc.ChangeActiveBox(-1);
        OnRefresh();
    }


}

