
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GamePlay;
using GamePlay.UI.UIFramework;
using GamePlay.UI.UtilUI;
using UnityEngine;
using UnityEngine.UI;

public class SaveSlotUI : BaseUI
{
    public Text Title;
    public Text LastTimeSaved;
    public Text Name;
    public Text Money;
    public GameObject PokemonParty;

    public bool HasFile { get; private set; }

    private List<Image> PokemonImages;

    public override void Init(params object[] args)
    {
        base.Init(args);
        Title = GET(Title,nameof(Title),GET_TYPE.Component);
        LastTimeSaved = GET(LastTimeSaved,nameof(LastTimeSaved),GET_TYPE.Component);
        Name = GET(Name,nameof(Name),GET_TYPE.Component);
        Money = GET(Money,nameof(Money),GET_TYPE.Component);
        PokemonParty = GET(PokemonParty,nameof(PokemonParty),GET_TYPE.GameObject);
        // for (int i = 0; i < PokemonParty.transform.childCount; i++)
        // {
        //     PokemonParty.transform.GetChild(i).gameObject.SetActive(true);
        // }
        if (PokemonImages == null)
        {
            PokemonImages = PokemonParty.GetComponentsInChildren<Image>().ToList();
            PokemonImages.RemoveAt(0);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args">0 for index, 1 for save data</param>
    public override void OnEnter(params object[] args)
    {
        base.OnEnter(args);
        
        int? slot = args[0] as int?;
        SaveData data=args[1] as SaveData;
        if (data != null && data.GameState!=null)
        {
            HasFile = true;
            Title.text = "Save " + slot;
            LastTimeSaved.text = data.GameState.TimeModified.ToString("yyyy-MM-dd HH:mm");
            Name.text = data.GameState.Trainer.name;
            Money.text = "Money:" + data.GameState.Trainer.Money;

            for (int i = 0; i < PokemonImages.Count; i++)
            {
                PokemonImages[i].gameObject.SetActive(false);
            }
            //TODO:目前没法在理论上支持更高的Pokemon party数量
            for (int i = 0; i < Math.Min(PokemonImages.Count,data.GameState.Trainer.party.Length); i++)
            {
                if (data.GameState.Trainer.party[i] == null) break;
                PokemonImages[i].gameObject.SetActive(true);
                PokemonImages[i].sprite = GameResources.PokemonIcons[data.GameState.Trainer.party[i].ID];
            }
            
            
        }
        else
        {
            HasFile = false;
            Title.text = "Save " + slot;
            LastTimeSaved.text = "No File Here";
            Name.text = "";
            Money.text = "";
            for (int i = 0; i < PokemonImages.Count; i++)
            {
                PokemonImages[i].gameObject.SetActive(false);
            }
            
            
        }
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}
