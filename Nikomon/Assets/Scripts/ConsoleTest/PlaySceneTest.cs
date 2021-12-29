using System.Collections;
using System.Collections.Generic;
using PokemonCore;
using PokemonCore.Inventory;
using UnityEngine;

public class PlaySceneTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        #if UNITY_EDITOR
            GlobalManager.Instance.game.CreateNewSaveFile("text man",false);
            Game.Instance.AddPokemon(new Pokemon(89,50));
            Game.Instance.AddPokemon(new Pokemon(20,50));
            Game.bag.Add((Item.Tag.Medicine,0)); // (Tag,ItemID)
            Game.bag.Add((Item.Tag.Medicine,1)); // (Tag,ItemID)
            Game.bag.Add((Item.Tag.Medicine,6)); // (Tag,ItemID)
        #endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
