using System;
using System.Collections;
using System.Collections.Generic;
using PokemonCore.Combat;
using UnityEngine;
using UnityEngine.UI;

public class BattlePokemonStateUI : MonoBehaviour
{
    public bool isOpponent;
    public Text NameText;
    public Text LevelText;
    public Text HealthText;
    public Slider HealthSlider;
    public Slider ExpSlider;
    public CombatPokemon pokemon;

    public void Init(CombatPokemon pokemon)
    {
        if (this.pokemon == null || this.pokemon!=pokemon)
        {
            this.pokemon = pokemon;
            NameText.text = pokemon.Name;
            LevelText.text = "Lv." + pokemon.Level;
            HealthText.text = pokemon.HP + "/" + pokemon.TotalHP;
            HealthSlider.value = pokemon.HP / (float) pokemon.TotalHP;
            ExpSlider.value = pokemon.pokemon.Exp.Current / (float) pokemon.pokemon.Exp.NextLevelExp;
            ChangeHealthSliderColor();

        }
        else
        {
            LevelText.text = "Lv." + pokemon.Level;
            LeanTween.value(HealthSlider.value, pokemon.HP / (float) pokemon.TotalHP, 1f).setOnUpdate(
                f =>
                {
                    HealthSlider.value = f;
                    ChangeHealthSliderColor();
                    HealthText.text = (int)(f*pokemon.TotalHP) + "/" + pokemon.TotalHP;
                });
        }

    }

    private void ChangeHealthSliderColor()
    {
        if (HealthSlider.value <= 0.2f)
        {
            HealthSlider.transform.Find("Fill Area/Fill").GetComponent<Image>().color = Color.red;
        }
        else if (HealthSlider.value <= 0.5f)
        {
            HealthSlider.transform.Find("Fill Area/Fill").GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            HealthSlider.transform.Find("Fill Area/Fill").GetComponent<Image>().color = Color.green;
        }
    }

    // public void UpdateState()
    // {
    //     LevelText.text = "Lv." + pokemon.Level;
    //     HealthText.text = pokemon.HP + "/" + pokemon.TotalHP;
    //     // HealthSlider.value = pokemon.HP / (float)pokemon.TotalHP;
    //     LeanTween.value(HealthSlider.value, pokemon.HP / (float) pokemon.TotalHP, 1f).setOnUpdate(
    //         f =>
    //         {
    //             HealthSlider.value = f;
    //
    //
    //             ChangeHealthSliderColor();
    //         });
    // }
    //
    // public void UpdateState(Action onComplete)
    // {
    //     UpdateState();
    //     // onComplete?.Invoke();
    // }
    //
    // public void UpdateState(int hp, Action onComplete)
    // {
    //     Debug.Log($"Update State!! from :{gameObject.name}");
    //     LevelText.text = "Lv." + pokemon.Level;
    //     HealthText.text = pokemon.HP + "/" + pokemon.TotalHP;
    //     // HealthSlider.value = pokemon.HP / (float)pokemon.TotalHP;
    //     LeanTween.value(HealthSlider.value, hp / (float) pokemon.TotalHP, 1f)
    //         .setOnUpdate(f =>
    //         {
    //             HealthSlider.value = f;
    //             ChangeHealthSliderColor();
    //         }).setOnComplete(() =>
    //         {
    //             Debug.Log("Update state complete");
    //             onComplete?.Invoke();
    //         });
    //     
    // }
}