using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay;
using PokemonCore.Monster.Data;
using UnityEngine;
using UnityEngine.UI;

public class PokemonSettlement : MonoBehaviour
{
    public bool isOpponent;
    public Text NameText;
    public Text LevelText;

    public Text LevelAddText;

    //public Text HealthText;
    //public Slider HealthSlider;
    public Slider ExpSlider;
    public Pokemon pokemon;
    public Image icon;
    public int ExpCur;
    public int level;

    public void Init(Pokemon pokemon, Experience expBefore)
    {
        this.pokemon = pokemon;
        NameText.text = pokemon.Name;
        LevelText.text = "Lv." + expBefore.level;
        level = expBefore.level;
        ExpSlider.value = expBefore.Current / (float) (expBefore.NextLevelExp - expBefore.CurLevelExp);
        icon.sprite = GameResources.PokemonIcons[pokemon.ID];
        LevelAddText.text = null;
        ExpCur = expBefore.Current;
    }

    public void addExp(Experience ExpAfter, Experience ExpBefore)
    {
        LevelAddText.text = "+" + (ExpAfter.Total -ExpBefore.Total).ToString();
        LevelText.text = "Lv." + level;

        int step = ExpAfter.level - ExpBefore.level;
        // print(step);
        
        LevelUpdateTween(step, () =>
        {
            LeanTween.value(ExpSlider.value, ExpAfter.Current / (float) (ExpAfter.NextLevelExp - ExpAfter.CurLevelExp),
                1).setOnUpdate(
                f => { ExpSlider.value = f; });
        });
        void LevelUpdateTween(int step,Action onComplete)
        {
            if (step == 0)
            {
                onComplete?.Invoke();
                return;
            }
            LeanTween.value(ExpSlider.value, 1f, 1)
                .setOnUpdate(f => { ExpSlider.value = f; })
                .setOnComplete(() =>
                {
                    ExpSlider.value = 0;
                    level += 1;
                    LevelText.text = "Lv." + level;
                    LevelUpdateTween(step-1,onComplete);
                });
        }
    }
}