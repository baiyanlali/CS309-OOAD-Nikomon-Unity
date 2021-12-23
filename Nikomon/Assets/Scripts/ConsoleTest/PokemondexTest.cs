using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GamePlay.UI.UIFramework;
using GamePlay.UI.UtilUI;
using PokemonCore;
using PokemonCore.Combat;
using UnityEngine;

public class PokemondexTest : MonoBehaviour
{
    void Start()
    {
        GlobalManager.Instance.LoadAllSaveData();
        Trainer trainer = new Trainer("JDY", true);
        Pokemon wyf = new Pokemon(3, 50);
        Pokemon AIfanfan = new Pokemon(7, 50);
        Pokemon AIfanfan2 = new Pokemon(1, 30);
        Pokemon AIfanfan3 = new Pokemon(89, 30);
        Game game = new Game();
        Game.trainer = trainer;
            
        wyf.Exp.AddExperience(500);
        // trainer.AddPokemon(AIfanfan);
        // trainer.AddPokemon(AIfanfan2);
        game.AddPokemon(wyf);
        game.AddPokemon(AIfanfan);
        game.AddPokemon(AIfanfan2);
        game.AddPokemon(AIfanfan);
        game.AddPokemon(AIfanfan3);
        // trainer.AddPokemon(wyf);
        // trainer.AddPokemon(AIfanfan);
        // trainer.AddPokemon(AIfanfan2);
        // trainer.AddPokemon(AIfanfan);
        // PCManager.TableUI.Init(trainer,new []{"Show Ability","Cancel"},HandleTableUI);
        
        //PC pc = new PC();
        
        var pokes = Game.PokemonsData.Keys;
        var pokesID = pokes.ToList();
        
        // for (int i = 0; i < 20; i++)
        // {
        //     pc.Pokemons[i]=new Pokemon(pokesID.RandomPickOne(),Game.Random.Next(40));
        // }
        

        //StartCoroutine(StartTest());
        UIManager.Instance.Show<PokedexPanel>(Game.trainer);

        // UIManager.Instance.GetUI<PCManager>().TableUI.Init(trainer,new []{"Show Ability","Cancel"},HandleTableUI);
    }

    IEnumerator StartTest()
    {
        yield return null;
        GlobalManager.Instance.game.CreateNewSaveFile("Hello",false);
        yield return null;
        foreach (var pokemonData in Game.PokemonsData)
        {
            Game.trainer.PokemonCountered.Add(pokemonData.Key);
        }
        UIManager.Instance.Show<PokedexPanel>(Game.trainer);
    }
}
