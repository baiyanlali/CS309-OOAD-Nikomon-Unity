using System.Collections;
using System.Collections.Generic;
using GamePlay.UI.UIFramework;
using PokemonCore;
using PokemonCore.Character;
using PokemonCore.Combat;
using UnityEngine;

public class PCTest : MonoBehaviour
{
    public PCManager PCManager;
    // Start is called before the first frame update
    void Start()
    {
        GlobalManager.Instance.LoadAllSaveData();
        Trainer trainer = new Trainer("JDY", true);
        Pokemon wyf = new Pokemon(1, 50);
        Pokemon AIfanfan = new Pokemon(7, 50);
        Pokemon AIfanfan2 = new Pokemon(1, 30);
        // trainer.AddPokemon(AIfanfan);
        // trainer.AddPokemon(AIfanfan2);
        trainer.AddPokemon(wyf);
        trainer.AddPokemon(AIfanfan);
        trainer.AddPokemon(AIfanfan2);
        trainer.AddPokemon(AIfanfan);
        // PCManager.TableUI.Init(trainer,new []{"Show Ability","Cancel"},HandleTableUI);

        PC pc = new PC();

        for (int i = 0; i < 5; i++)
        {
            pc.Pokemons[i]=new Pokemon(1,Game.Random.Next(99));
        }

        for (int i = 6; i < 12; i++)
        {
            pc.Pokemons[i]=new Pokemon(7,Game.Random.Next(99));
        }
        
        UIManager.Instance.Show<PCManager>(trainer,pc);
        
        // UIManager.Instance.GetUI<PCManager>().TableUI.Init(trainer,new []{"Show Ability","Cancel"},HandleTableUI);
    }

    void HandleTableUI(int index)
    {
        print(index);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
