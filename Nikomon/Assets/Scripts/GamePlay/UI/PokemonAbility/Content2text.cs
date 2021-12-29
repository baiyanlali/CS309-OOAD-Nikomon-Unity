using System.Collections;
using System.Collections.Generic;
using GamePlay.Core;
using GamePlay.Messages;
using PokemonCore;
using PokemonCore.Combat;
using UnityEngine;
using UnityEngine.UI;

public class Content2text : MonoBehaviour
{
    public Text NatureContent;

    public void Init(Pokemon pokemon)
    {
        //NatureContent.text = Game.NatureData[pokemon.NatureID].ToString();
        // gameObject.GetComponent<TriggerSelect>().onDeSelect = () =>
        // {
        //     obj.transform.localScale = v;
        // };

    }

}