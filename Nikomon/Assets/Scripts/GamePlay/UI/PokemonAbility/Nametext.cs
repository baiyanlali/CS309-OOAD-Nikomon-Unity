 using System.Collections;
using System.Collections.Generic;
 using GamePlay.Core;
 using GamePlay.Messages;
 using PokemonCore;
 using UnityEngine;
using UnityEngine.UI;

public class Nametext : MonoBehaviour
{
    // Start is called before the first frame update
    //要先检查这些有没有绑定！！！
    public Text nameText, propertyText,ownerText,numberText;
    public Text experienceText, needExperienceText;
    public Image exp,Expcurrent;
    public void Start()//测试
    {
        //TODO: change to translator
        // nameText.text = "222";
        // propertyText.text = "sssss";//????
        // ownerText.text = "sssss";//??????
        // numberText.text = "22";
        // experienceText.text = "333";
        // //poke.Exp.ExperienceNeeded;//还需要的经验值！！！！
        // needExperienceText.text = "555";
        // Expcurrent.fillAmount = 0.5f;

    }
    public void Init(Pokemon pokemon)
    {
        //TODO: change to translator
        nameText.text = Translator.TranslateStr(pokemon.IsNicknamed ? pokemon.Name :pokemon.Name);
        propertyText.text = Game.TypesMap[pokemon.Type1.Value].Name;
        ownerText.text = pokemon.TrainerID.ToString();//??????
        numberText.text = pokemon.AbilityID.ToString();
        experienceText.text = pokemon.Exp.Current.ToString();
        //poke.Exp.ExperienceNeeded;//还需要的经验值！！！！
        int toLv = pokemon.Exp.level + 1;//TODO:是不是需要检查一下是不是满级呢？//这个要做成血条差不多
        needExperienceText.text = pokemon.Exp.ExperienceNeeded(toLv).ToString();
        //TODO:Expcurrent.fillAmount 有问题，这样可能大于1了！！！
        // Expcurrent.fillAmount = pokemon.Exp.Current * 1f / pokemon.Exp.ExperienceNeeded(toLv);
        Expcurrent.fillAmount = pokemon.Exp.Current * 1f / (pokemon.Exp.NextLevelExp-pokemon.Exp.Past);
        // print(pokemon.Exp.NextLevelExp);
        // print(pokemon.Exp.Current);
       
    }

}
