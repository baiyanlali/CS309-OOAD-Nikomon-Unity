 using System.Collections;
using System.Collections.Generic;
 using GamePlay.Core;
 using GamePlay.Messages;
 using PokemonCore;
 using PokemonCore.Combat;
 using UnityEngine;
using UnityEngine.UI;

public class Nametext : MonoBehaviour
{
    // Start is called before the first frame update
    //要先检查这些有没有绑定！！！
    public Text nameText, propertyText,ownerText,numberText;
    public Text experienceText, needExperienceText;
    //public Text NatureContent;
    public Image exp,Expcurrent;

    public void Init(Pokemon pokemon,Trainer trainer)
    {
        //TODO: change to translator
        nameText.text = pokemon.IsNicknamed ? pokemon.Name : Messages.Get(pokemon.Name);
        propertyText.text =Messages.Get( Game.TypesMap[pokemon.Type1.Value].Name);
        // ownerText.text = pokemon.TrainerID.ToString();
        ownerText.text = trainer.name;
        // numberText.text = pokemon.AbilityID.ToString();
        numberText.text = pokemon.ID.ToString();
        experienceText.text = pokemon.Exp.Current.ToString();
        //poke.Exp.ExperienceNeeded;//还需要的经验值！！！！
        int toLv = pokemon.Exp.level + 1;
        needExperienceText.text = pokemon.Exp.ExperienceNeeded(toLv).ToString();
        //TODO:Expcurrent.fillAmount 有问题，这样可能大于1了！！！
        Expcurrent.fillAmount = pokemon.Exp.Current * 1f / (pokemon.Exp.NextLevelExp-pokemon.Exp.Past);
        //NatureContent.text = Game.NatureData[pokemon.NatureID].ToString();


    }

}
