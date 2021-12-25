using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay;
using PokemonCore;
using PokemonCore.Combat;
using UnityEngine;
using UnityEngine.UI;

public class TargetChooserUI : MonoBehaviour
{

    public Image pokeImage;
    public Text pokeName;
    public Text trainerName;
    public Toggle toggle;
    public CombatPokemon Pokemon;
    public Action OnSelected;

    public void Init(CombatPokemon pokemon)
    {
        this.Pokemon = pokemon;
        pokeImage.sprite = GameResources.PokemonIcons[pokemon.pokemon.ID];
        toggle = GetComponent<Toggle>();
        pokeName.text = pokemon.Name;
        trainerName.text = Game.battle.getTrainerByID(pokemon.TrainerID);
        toggle.onValueChanged.AddListener((e)=>
        {
            if(e==true)
                OnSelected?.Invoke();
        });
    }

    public void Disable()
    {
        toggle.isOn = false;
        toggle.interactable = false;
    }

    public void On()
    {
        toggle.isOn = true;
    }
    
    public void Off()
    {
        toggle.isOn = false;
    }

    public void Enable()
    {
        toggle.interactable = true;
    }
}
