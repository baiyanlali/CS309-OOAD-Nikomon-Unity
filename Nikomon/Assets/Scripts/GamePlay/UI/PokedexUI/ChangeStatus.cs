using System.Collections;
using System.Collections.Generic;
using PokemonCore;
using UnityEngine;
using UnityEngine.UI;

public class ChangeStatus : MonoBehaviour
{
    public Transform temp;

    public void changeValue()
    {
        int totalnumber = Game.PokemonsData.Count;
        //this.gameObject.GetComponentInParent<>()
        
        temp.GetComponent<Scrollbar>().value = 1/totalnumber;
        print(temp.GetComponent<Scrollbar>().value);
        
    }


}
