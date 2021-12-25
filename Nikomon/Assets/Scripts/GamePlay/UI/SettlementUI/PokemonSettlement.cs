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
    
    public void Init(Pokemon pokemon)
    {
        this.pokemon = pokemon;
        NameText.text = pokemon.Name;
        LevelText.text = "Lv." + pokemon.Level;
        level = pokemon.Level;
        ExpSlider.value = pokemon.Exp.Current / (float) pokemon.Exp.NextLevelExp;
        icon.sprite = GameResources.PokemonIcons[pokemon.ID];
        LevelAddText.text = null;
        ExpCur = pokemon.Exp.Current;
    }

    public void addExp(Experience ExpAfter, Experience ExpBefore)
    {
        int experience = ExpAfter.Total - ExpBefore.Total+70000;
        print(ExpBefore.NextLevelExp);
        LevelAddText.text = "+" + experience;
        print("进来了");
        LevelText.text = "Lv." + pokemon.Level;
        
        //ExpBefore.ExperienceNeeded(level + 1);
        while (ExpCur + experience >= ExpBefore.ExperienceNeeded(level + 1))
        {
            //有bug 一秒出来！！！不知道为什么
            print("我是傻逼");
            LeanTween.value(ExpSlider.value, 1, 1).setOnUpdate(
                f =>
                {
                    print("我是吴一凡");
                    ExpSlider.value = f;
                });
            ExpSlider.value = 0;
            experience = experience - (ExpBefore.ExperienceNeeded(level + 1) - ExpCur);
            ExpBefore.AddExperience(ExpBefore.ExperienceNeeded(level + 1) - ExpCur);
            ExpCur = 0;
            level += 1;
            LevelText.text = "Lv." + level;
        }
        LeanTween.value(ExpSlider.value, ExpBefore.Current / (float) ExpBefore.NextLevelExp, 1).setOnUpdate(
            f =>
            {
                ExpSlider.value = f;
            });
        
        // //ExpBefore.ExperienceNeeded(pokemon.Level + 1);
        // while (ExpCur + experience >= ExpBefore.NextLevelExp)
        // {
        //     //有bug
        //     print("我是傻逼");
        //     LeanTween.value(ExpSlider.value, 1, 1f).setOnUpdate(
        //         f =>
        //         {
        //             ExpSlider.value = f;
        //         });
        //     ExpSlider.value = 0;
        //     experience = experience - (ExpBefore.NextLevelExp - ExpCur);
        //     ExpBefore.AddExperience(ExpBefore.NextLevelExp - ExpCur);
        //     //NextLevelExp是啥？？？？为啥从31跳到了39！！！！！
        //     ExpCur = 0;
        //     level += 1;
        //     LevelText.text = "Lv." + level;
        // }
        // LeanTween.value(ExpSlider.value, ExpBefore.Current / (float) ExpBefore.NextLevelExp, 1f).setOnUpdate(
        //     f =>
        //     {
        //         ExpSlider.value = f;
        //     });
    }

    
    
    
    
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
