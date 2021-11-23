using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay;
using GamePlay.UI.UIFramework;
using GamePlay.UI.UtilUI;
using UnityEngine;
using UnityEngine.UI;
using Debug = PokemonCore.Debug;

public class PokemonChooserElementUI : MonoBehaviour
{
    public Image PokemonIcon;
    public Text PokemonName;
    public Slider HealthBar;
    public Text HealthText;
    public Text LevelText;
    public Pokemon Poke;

    public void Init(Pokemon pokemon, int index, string[] dialogChoose, Action<int> actions)
    {
        Poke = pokemon;
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
            // UnityEngine.Debug.Log("Click!");
            // DialogChooserUI.Instance.ShowChooser(dialogChoose,new Vector2(0,1),
            //     (o) =>
            //     {
            //         //这里是宝可梦的index和选项的index
            //         actions[o]?.Invoke(Index);
            //     },
            //     transform as RectTransform);
            Action<int> action = (o) => { actions?.Invoke(o); };
            UIManager.Instance.Show<DialogueChooserPanel>(dialogChoose, new Vector2(0, 1), action, transform as RectTransform);
        });
    }

    public void UpdateData(Pokemon pokemon)
    {
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