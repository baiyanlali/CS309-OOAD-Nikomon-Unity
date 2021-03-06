using System.Collections;
using System.Collections.Generic;
using PokemonCore;
using PokemonCore.Inventory;
using UnityEngine;

public class PlaySceneTest : MonoBehaviour
{
    // Start is called before the first frame update
#if UNITY_EDITOR

    void Awake()
    {
            GlobalManager.Instance.game.CreateNewSaveFile("text man",false);
            
            Game.Instance.AddPokemon(new Pokemon(6,50));
            Game.Instance.AddPokemon(new Pokemon(25,50));
            Game.Instance.AddPokemon(new Pokemon(20,50));
            Game.Instance.AddPokemon(new Pokemon(4,11));
            Game.Instance.AddPokemon(new Pokemon(1, 24));
            
            Game.trainer.RemovePokemon(0);
            Game.bag.Add((Item.Tag.Medicine,0)); // (Tag,ItemID)
            Game.bag.Add((Item.Tag.Medicine,1)); // (Tag,ItemID)
            Game.bag.Add((Item.Tag.Medicine,6)); // (Tag,ItemID)
    }
#endif

    // Update is called once per frame
    void Update()
    {
        
    }
}
