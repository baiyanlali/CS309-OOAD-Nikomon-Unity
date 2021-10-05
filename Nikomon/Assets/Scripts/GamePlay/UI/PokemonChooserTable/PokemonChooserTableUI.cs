using System;
using System.Collections;
using System.Collections.Generic;
using PokemonCore.Combat;
using UnityEngine;

public class PokemonChooserTableUI : MonoBehaviour
{
    private GameObject ChooserElement;

    public static PokemonChooserTableUI Instance
    {
        get
        {
            if (sInstance != null) return sInstance;
            sInstance = FindObjectOfType<PokemonChooserTableUI>();
            if (sInstance != null) return sInstance;
            throw new Exception("懒得写了");
        }
    }
    public static PokemonChooserTableUI sInstance;

    public void Init(Trainer trainer,string[] chooses,Action<int>[] actions)
    {
        ChooserElement = ChooserElement
            ? ChooserElement
            : Resources.Load<GameObject>("Prefabs/UI/PokemonChooserTable/PokemonStatButton");

        int pokes = trainer.bagPokemons;
        
        if (pokes < transform.childCount)
        {
            for (int i = pokes; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        else if(pokes>transform.childCount)
        {
            for (int i = transform.childCount; i < pokes; i++)
            {
                Instantiate(ChooserElement, transform);
            }
        }
        
        for (int i = 0; i < Mathf.Min(pokes,transform.childCount); i++)
        {
            if (trainer.party[i] == null) break;
            transform.GetChild(i).gameObject.SetActive(true);
            transform.GetChild(i).GetComponent<PokemonChooserElementUI>().Init(trainer.party[i],i,chooses,actions);
        }
        
        gameObject.SetActive(false);
    }
}