using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay;
using GamePlay.UI.UIFramework;
using PokemonCore;
using PokemonCore.Character;
using PokemonCore.Combat;
using UnityEngine;
using UnityEngine.EventSystems;
public class PartyInPC : MonoBehaviour
//public class PokemonChooserTableUI: MonoBehaviour 
{
    private GameObject ChooserElement;
    private PCManager _pcManager;

    public void Init(Trainer trainer,string[] chooses,Action<int,int> actions)
    {
        // ChooserElement = ChooserElement
        //     ? ChooserElement
        //     : Resources.Load<GameObject>("Prefabs/UI/PokemonChooserTable/PokemonStatButton");
        _pcManager ??= UIManager.Instance.GetUI<PCManager>();
        ChooserElement = GameResources.SpawnPrefab(typeof(PCParty));

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
            transform.GetChild(i).GetComponent<PCParty>().Init(trainer.party[i],i,chooses,actions);
            GameObject obj = transform.GetChild(i).gameObject;
            obj.GetComponent<PCParty>().IndexInBag = i;
            obj.GetComponent<TriggerSelect>().onSelect = () =>
            {
                _pcManager.ShowInfo(true);
                _pcManager.GameObjectdesc.SetActive(false);
                _pcManager.RefreshInformation(trainer.party[obj.GetComponent<PCParty>().IndexInBag]);
            };
        }
        
        // gameObject.SetActive(false);
        // EventSystem.current.SetSelectedGameObject(transform.GetChild(0).gameObject);
    }

    public void ExchangeData(Trainer trainer)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (trainer.party[i] == null) break;
            transform.GetChild(i).gameObject.SetActive(true);
            transform.GetChild(i).GetComponent<PCParty>().UpdateData(trainer.party[i]);
        }
        gameObject.SetActive(true);
    }
    public void UpdateData(Trainer trainer)
    {
        // for (int i = 0; i < transform.childCount; i++)
        // {
        //     if (trainer.party[i] == null)
        //     {
        //         break;
        //     }
        //     transform.GetChild(i).gameObject.SetActive(true);
        //     transform.GetChild(i).GetComponent<PokemonChooserElementUI>().UpdateData(trainer.party[i]);
        // }
        //Destroy(transform.GetChild(bagIndex).gameObject);
        Destroy(transform.GetChild(transform.childCount-1).gameObject);
        for (int i = 0; i < transform.childCount; i++)
        {
            if (trainer.party[i] == null) break;
            transform.GetChild(i).gameObject.SetActive(true);
            transform.GetChild(i).GetComponent<PCParty>().UpdateData(trainer.party[i]);
        }
        gameObject.SetActive(true);
    }

    
}



// public class PokemonChooserTableUI : MonoBehaviour 
// //public class PokemonChooserTableUI: MonoBehaviour 
// {
//     private GameObject ChooserElement;
//
//     public void Init(Trainer trainer,string[] chooses,Action<int,int> actions)
//     {
//         // ChooserElement = ChooserElement
//         //     ? ChooserElement
//         //     : Resources.Load<GameObject>("Prefabs/UI/PokemonChooserTable/PokemonStatButton");
//
//         //ChooserElement = GameResources.SpawnPrefab(typeof(PartyElement));
//         ChooserElement = GameResources.SpawnPrefab(typeof(PartyElement));
//
//         int pokes = trainer.bagPokemons;
//         
//         if (pokes < transform.childCount)
//         {
//             for (int i = pokes; i < transform.childCount; i++)
//             {
//                 transform.GetChild(i).gameObject.SetActive(false);
//             }
//         }
//         else if(pokes>transform.childCount)
//         {
//             for (int i = transform.childCount; i < pokes; i++)
//             {
//                 Instantiate(ChooserElement, transform);
//             }
//         }
//         
//         for (int i = 0; i < Mathf.Min(pokes,transform.childCount); i++)
//         {
//             if (trainer.party[i] == null) break;
//             transform.GetChild(i).gameObject.SetActive(true);
//             transform.GetChild(i).GetComponent<PartyElement>().Init(trainer.party[i],i,chooses,actions);
//         }
//         
//         // gameObject.SetActive(false);
//         // EventSystem.current.SetSelectedGameObject(transform.GetChild(0).gameObject);
//     }
//
//     public void ExchangeData(Trainer trainer)
//     {
//         for (int i = 0; i < transform.childCount; i++)
//         {
//             if (trainer.party[i] == null) break;
//             transform.GetChild(i).gameObject.SetActive(true);
//             transform.GetChild(i).GetComponent<PartyElement>().UpdateData(trainer.party[i]);
//         }
//         gameObject.SetActive(true);
//     }
//     public void UpdateData(Trainer trainer)
//     {
//         Destroy(transform.GetChild(transform.childCount-1).gameObject);
//         for (int i = 0; i < transform.childCount; i++)
//         {
//             if (trainer.party[i] == null) break;
//             transform.GetChild(i).gameObject.SetActive(true);
//             transform.GetChild(i).GetComponent<PartyElement>().UpdateData(trainer.party[i]);
//         }
//         gameObject.SetActive(true);
//     }
//
//     
// }