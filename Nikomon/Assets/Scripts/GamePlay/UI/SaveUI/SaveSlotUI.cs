
using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay;
using GamePlay.UI.UIFramework;
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

    private Image[] PokemonImages;

    public override void Init(params object[] args)
    {
        Title = GET(Title,nameof(Title));
        LastTimeSaved = GET(LastTimeSaved,nameof(LastTimeSaved));
        Name = GET(Name,nameof(Name));
        Money = GET(Money,nameof(Money));
        PokemonParty = GET(PokemonParty,nameof(PokemonParty));

        PokemonImages = PokemonParty.GetComponentsInChildren<Image>();
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
            Money.text = "Money:" + data.GameState.Trainer.money;

            for (int i = 0; i < PokemonImages.Length; i++)
            {
                PokemonImages[i].gameObject.SetActive(false);
            }
            //TODO:目前没法在理论上支持更高的Pokemon party数量
            for (int i = 0; i < Math.Min(PokemonImages.Length,data.GameState.Trainer.party.Length); i++)
            {
                if (data.GameState.Trainer.party[i] == null) break;
                PokemonImages[i].gameObject.SetActive(true);
                PokemonImages[i].sprite = GameResources.PokemonIcons[data.GameState.Trainer.party[i].ID];
            }
        }
        else
        {
            HasFile = false;
            Title.text = "No File Here";
            LastTimeSaved.text = "";
            Name.text = "";
            Money.text = "";
            for (int i = 0; i < PokemonImages.Length; i++)
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
