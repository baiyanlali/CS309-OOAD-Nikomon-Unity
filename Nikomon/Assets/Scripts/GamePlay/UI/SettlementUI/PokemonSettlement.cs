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
        // this.pokemon = pokemon;
        // NameText.text = pokemon.Name;
        // LevelText.text = "Lv." + pokemon.Level;
        // level = pokemon.Level;
        // ExpSlider.value = pokemon.Exp.Current / (float) pokemon.Exp.NextLevelExp;
        // icon.sprite = GameResources.PokemonIcons[pokemon.ID];
        // LevelAddText.text = null;
        // ExpCur = pokemon.Exp.Current;
        this.pokemon = pokemon;
        NameText.text = pokemon.Name;
        LevelText.text = "Lv." + expBefore.level;
        level = expBefore.level;
        ExpSlider.value = expBefore.Current / (float) (expBefore.NextLevelExp - expBefore.CurLevelExp);
        // ExpSlider.value = pokemon.Exp.Current / (float) pokemon.Exp.NextLevelExp;
        // ExpSlider.value = pokemon.Exp.Current / (float) pokemon.Exp.ExperienceNeeded(pokemon.Level + 1);
        icon.sprite = GameResources.PokemonIcons[pokemon.ID];
        LevelAddText.text = null;
        ExpCur = expBefore.Current;
    }

    public void addExp(Experience ExpAfter, Experience ExpBefore)
    {
        // int experience = ExpAfter.Total - ExpBefore.Total+70000;
        // int experience = ExpAfter.Total - ExpBefore.Total + 7000;
        print(ExpBefore.NextLevelExp);
        LevelAddText.text = "+" + (ExpAfter.Total -ExpBefore.Total).ToString();
        print("进来了");
        LevelText.text = "Lv." + level;

        int step = ExpAfter.level - ExpBefore.level;
        print(step);
        
        LevelUpdateTween(step, () =>
        {
            // ExpBefore.AddExperience(experience);
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
                    LevelUpdateTween(step-1,onComplete);
                });
            level += 1;
            LevelText.text = "Lv." + level;
        }
        
        //ExpBefore.ExperienceNeeded(level + 1);
        
        // int i = 0;
        // while (experience >= ExpBefore.ExperienceNeeded(level + 1))
        // {
        //     //有bug 一秒出来！！！不知道为什么
        //     print(i);
        //     LeanTween.value(ExpSlider.value, 1f, 1).setOnUpdate(
        //         f => { ExpSlider.value = f; });
        //     ExpSlider.value = 0;
        //     experience = experience - (ExpBefore.ExperienceNeeded(level + 1));
        //     ExpBefore.AddExperience(ExpBefore.ExperienceNeeded(level + 1));
        //     ExpCur = 0;
        //     level += 1;
        //     LevelText.text = "Lv." + level;
        //     i++;
        // }

        // ExpBefore.AddExperience(experience);
        // print(111);
        // //LeanTween.value(ExpSlider.value, ExpAfter.Current /(float) (ExpAfter.NextLevelExp-ExpAfter.CurLevelExp), 1).setOnUpdate(
        // // LeanTween.value(ExpSlider.value, ExpBefore.Current /(float) (ExpBefore.NextLevelExp-ExpBefore.CurLevelExp), 1).setOnUpdate(
        // LeanTween.value(ExpSlider.value, ExpBefore.Current / (float) (ExpBefore.NextLevelExp - ExpBefore.CurLevelExp),
        //     1).setOnUpdate(
        //     f => { ExpSlider.value = f; });
    }

    // public void addExp(Experience ExpAfter, Experience ExpBefore)
    // {
    //     // int experience = ExpAfter.Total - ExpBefore.Total+70000;
    //     int experience = ExpAfter.Total - ExpBefore.Total + 7000;
    //     print(ExpBefore.NextLevelExp);
    //     LevelAddText.text = "+" + experience;
    //     print("进来了");
    //     LevelText.text = "Lv." + level;
    //     
    //     //ExpBefore.ExperienceNeeded(level + 1);
    //     int i = 0;
    //     while (ExpCur + experience >= ExpBefore.ExperienceNeeded(level + 1))
    //     {
    //         //有bug 一秒出来！！！不知道为什么
    //         print(i);
    //         LeanTween.value(ExpSlider.value, 1f, 1).setOnUpdate(
    //             f =>
    //             {
    //                 //print("我是吴一凡");
    //                 ExpSlider.value = f;
    //             });
    //         ExpSlider.value = 0;
    //         experience = experience - (ExpBefore.ExperienceNeeded(level + 1) - ExpCur);
    //         ExpBefore.AddExperience(ExpBefore.ExperienceNeeded(level + 1) - ExpCur);
    //         ExpCur = 0;
    //         level += 1;
    //         LevelText.text = "Lv." + i;
    //         i++;
    //     }
    //     print(111);
    //     LeanTween.value(ExpSlider.value, ExpBefore.Current / (float) ExpBefore.ExperienceNeeded(level + 1), 1).setOnUpdate(
    //         f =>
    //         {
    //             ExpSlider.value = f;
    //         });
    //     
    //     // //ExpBefore.ExperienceNeeded(pokemon.Level + 1);
    //     // while (ExpCur + experience >= ExpBefore.NextLevelExp)
    //     // {
    //     //     //有bug
    //     //     print("我是傻逼");
    //     //     LeanTween.value(ExpSlider.value, 1, 1f).setOnUpdate(
    //     //         f =>
    //     //         {
    //     //             ExpSlider.value = f;
    //     //         });
    //     //     ExpSlider.value = 0;
    //     //     experience = experience - (ExpBefore.NextLevelExp - ExpCur);
    //     //     ExpBefore.AddExperience(ExpBefore.NextLevelExp - ExpCur);
    //     //     //NextLevelExp是啥？？？？为啥从31跳到了39！！！！！
    //     //     ExpCur = 0;
    //     //     level += 1;
    //     //     LevelText.text = "Lv." + level;
    //     // }
    //     // LeanTween.value(ExpSlider.value, ExpBefore.Current / (float) ExpBefore.NextLevelExp, 1f).setOnUpdate(
    //     //     f =>
    //     //     {
    //     //         ExpSlider.value = f;
    //     //     });
    // }


    // public void Init(Pokemon pokemon)
    // {
    //     if (this.pokemon == null || this.pokemon!=pokemon)
    //     {
    //         this.pokemon = pokemon;
    //         NameText.text = pokemon.Name;
    //         LevelText.text = "Lv." + pokemon.Level;
    //         ExpSlider.value = pokemon.Exp.Current / (float) pokemon.Exp.NextLevelExp;
    //
    //     }
    //     else{
    //         //TODO:要考虑进化，升级的情况！
    //         LevelText.text = "Lv." + pokemon.Level;
    //         LeanTween.value(ExpSlider.value, pokemon.Exp.Current / (float) pokemon.Exp.NextLevelExp, 1f).setOnUpdate(
    //             f =>
    //             {
    //                 ExpSlider.value = f;
    //             });
    //     }
    //
    // }
}