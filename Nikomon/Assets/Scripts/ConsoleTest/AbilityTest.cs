using System.Linq;
using GamePlay.UI.UIFramework;
using GamePlay.UI.UtilUI;
using PokemonCore;
using PokemonCore.Character;
using PokemonCore.Combat;
using PokemonCore.Utility;
using UnityEngine;

public class AbilityTest : MonoBehaviour
    {
        void Start()
        {
            GlobalManager.Instance.LoadAllSaveData();
            Trainer trainer = new Trainer("JDY", true);
            Pokemon wyf = new Pokemon(1, 50);
            Pokemon AIfanfan = new Pokemon(7, 50);
            Pokemon AIfanfan2 = new Pokemon(1, 30);
            
            wyf.Exp.AddExperience(500);
            // trainer.AddPokemon(AIfanfan);
            // trainer.AddPokemon(AIfanfan2);
            trainer.AddPokemon(wyf);
            trainer.AddPokemon(AIfanfan);
            trainer.AddPokemon(AIfanfan2);
            trainer.AddPokemon(AIfanfan);
            // PCManager.TableUI.Init(trainer,new []{"Show Ability","Cancel"},HandleTableUI);

            //PC pc = new PC();
        
            var pokes = Game.PokemonsData.Keys;
            var pokesID = pokes.ToList();

            // for (int i = 0; i < 20; i++)
            // {
            //     pc.Pokemons[i]=new Pokemon(pokesID.RandomPickOne(),Game.Random.Next(40));
            // }
        
        
        
            UIManager.Instance.Show<AbilityPanel>(trainer,wyf);
        
            // UIManager.Instance.GetUI<PCManager>().TableUI.Init(trainer,new []{"Show Ability","Cancel"},HandleTableUI);
        }
        
}
