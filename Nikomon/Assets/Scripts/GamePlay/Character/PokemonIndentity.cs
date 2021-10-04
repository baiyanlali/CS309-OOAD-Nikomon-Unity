using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonIndentity : MonoBehaviour,IInteractive
{
    public Pokemon pokemon;
    

    public void OnInteractive()
    {
        GlobalManager.Instance.StartBattle(pokemon);
    }


    private void Update()
    {
        
    }
}
