using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GamePlay;
using GamePlay.UI.UIFramework;
using GamePlay.UI.UtilUI;
// using Resources.Sprites.PCUI.Inventory.InventoryScripts;
using UnityEngine;
using UnityEngine.UI;
using Debug = PokemonCore.Debug;

public class PokemonChooserElementUI : PartyElement
{
    public Image PokemonIcon;
    public Text PokemonName;
    public Slider HealthBar;
    public Text HealthText;
    public Text LevelText;
    public Pokemon Poke;
    public int IndexInBag;

    public override void Init(Pokemon pokemon, int index, string[] dialogChoose, Action<int,int> actions)
    {
        base.Init(pokemon, index, dialogChoose, actions);
        Poke = pokemon;
        IndexInBag = index;
        PokemonIcon = PokemonIcon ? PokemonIcon : transform.Find("PokemonIcon").GetComponent<Image>();
        PokemonName = PokemonName ? PokemonName : transform.Find("PokemonName").GetComponent<Text>();
        HealthBar = HealthBar ? HealthBar : transform.Find("HealthBar").GetComponent<Slider>();
        HealthText = HealthText ? HealthText : transform.Find("HealthText").GetComponent<Text>();
        LevelText = LevelText ? LevelText : transform.Find("LevelText").GetComponent<Text>();

        PokemonIcon.sprite = GameResources.PokemonIcons[pokemon.ID];
        PokemonName.text = pokemon.Name;
        HealthBar.value = pokemon.HP / (float) pokemon.TotalHp;
        HealthText.text = $"{pokemon.HP}/{pokemon.TotalHp}";
        LevelText.text = $"Lv.{pokemon.Level}";

        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(() =>
        {
            //不仅回传选了选项中第几个，还回传现在是背包中第几个宝可梦
            Action<int> action = (o) => { actions?.Invoke(o,index); };
            UIManager.Instance.Show<DialogueChooserPanel>(dialogChoose.ToList(), new Vector2(0, 1), action, transform as RectTransform);
        });
    }

    public override void UpdateData(Pokemon pokemon)
    {
        base.UpdateData(pokemon);
        PokemonIcon = PokemonIcon ? PokemonIcon : transform.Find("PokemonIcon").GetComponent<Image>();
        PokemonName = PokemonName ? PokemonName : transform.Find("PokemonName").GetComponent<Text>();
        HealthBar = HealthBar ? HealthBar : transform.Find("HealthBar").GetComponent<Slider>();
        HealthText = HealthText ? HealthText : transform.Find("HealthText").GetComponent<Text>();
        LevelText = LevelText ? LevelText : transform.Find("LevelText").GetComponent<Text>();

        PokemonIcon.sprite = GameResources.PokemonIcons[pokemon.ID];
        PokemonName.text = pokemon.Name;
        HealthBar.value = pokemon.HP / (float) pokemon.TotalHp;
        HealthText.text = $"{pokemon.HP}/{pokemon.TotalHp}";
        LevelText.text = $"Lv.{pokemon.Level}";
    }
}