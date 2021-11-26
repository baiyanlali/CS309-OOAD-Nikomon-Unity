using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GamePlay;
using PokemonCore;
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
        Pokemons = Game.PokemonsData.Keys.ToArray();
        
        pokemonIndentities = new List<PokemonIndentity>();
        for (int i = 0; i < Pokemons.Length; i++)
        {
            Pokemon pokemon = new Pokemon(Pokemons[i], Random.Range(minLevel, maxLevel + 1));
            var obj = GameResources.Pokemons[pokemon.ID];
            GameObject poke=null;
            Vector2 pos = Random.insideUnitCircle*GetComponent<SphereCollider>().radius;
            
            if(obj.Length==1)
               poke  = Instantiate(obj[0],new Vector3(pos.x,transform.position.y,pos.y),Quaternion.identity);
            else if (obj.Length == 2)
                poke = Instantiate(obj[pokemon.isMale ? 0 : 1],new Vector3(pos.x,transform.position.y,pos.y),Quaternion.identity);

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
