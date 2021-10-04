using System.Collections;
using System.Collections.Generic;
using GamePlay;
using UnityEngine;

public class PokemonGenerator : MonoBehaviour
{
    public int[] Pokemons;
    
    public int maxLevel;

    public int minLevel;

    public int maxPokemonNum;

    private List<PokemonIndentity> pokemonIndentities;
    void Start()
    {
        pokemonIndentities = new List<PokemonIndentity>();
        for (int i = 0; i < maxPokemonNum; i++)
        {
            Pokemon pokemon = new Pokemon(Pokemons.RandomPickOne(), Random.Range(minLevel, maxLevel + 1));
            var obj = GlobalManager.Instance.Pokemons[pokemon.ID];
            GameObject poke=null;
            if(obj.Length==1)
               poke  = Instantiate(obj[0]);
            else if (obj.Length == 2)
                poke = Instantiate(obj[pokemon.isMale ? 0 : 1]);

            if (poke != null)
            {
                var identity = poke.gameObject.AddComponent<PokemonIndentity>();
                identity.pokemon = pokemon;
                pokemonIndentities.Add(identity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
