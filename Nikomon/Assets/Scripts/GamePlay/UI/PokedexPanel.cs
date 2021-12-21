using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay;
using GamePlay.UI.BattleUI;
using GamePlay.UI.UIFramework;
using PokemonCore;
using PokemonCore.Combat;
using PokemonCore.Monster.Data;
using UnityEngine;
using UnityEngine.UI;

public class PokedexPanel : BaseUI
{
    public Trainer trainer;
    private GameObject PokemondexPrefab;
    public Transform PokedexDetial;
    private List<PokemondexElement> _pokemondexElements=new List<PokemondexElement>();
    public GameObject pokenmons;
    public Transform _Scrollbar;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="args">0 for trainer</
    /// 
    public override void OnEnter(params object[] args)
    {
        PokemondexPrefab = GameResources.SpawnPrefab(typeof(PokemondexElement));
        print(_pokemondexElements.Count);
        if (_pokemondexElements.Count == 0)
        {
            for (int i = 0; i < Game.PokemonsData.Count; i++)//Game.PokemonsData.Count
            {
                GameObject obj = Instantiate(PokemondexPrefab, PokedexDetial);
                obj.transform.SetAsLastSibling();
                obj.GetComponent<PokemondexElement>().number = -1;
                obj.GetComponent<TriggerSelect>().onSelect = () =>
                {
                    _Scrollbar.GetComponent<Scrollbar>().value = 
                        (Game.PokemonsData.Count - obj.GetComponent<PokemondexElement>().number-0.001f) 
                        / Game.PokemonsData.Count;
                    if (_Scrollbar.GetComponent<Scrollbar>().value < 0.14)
                        _Scrollbar.GetComponent<Scrollbar>().value -= 0.03f;
                    if (_Scrollbar.GetComponent<Scrollbar>().value > 0.86)
                        _Scrollbar.GetComponent<Scrollbar>().value += 0.03f;
                     print(_Scrollbar.GetComponent<Scrollbar>().value);
                    //StartCoroutine(Normal(obj));
                    if (obj.GetComponent<PokemondexElement>().number == -1)
                    {
                        //TODO:显示一个傻逼的图片来区分？或者不显示   目前有大问题number这个变量有问题！！！！！！！
                        foreach (Transform child in pokenmons.transform)
                        {
                            // print(child.name);
                            child.gameObject.SetActive(false);
                        }
                        print(obj.GetComponent<PokemondexElement>().number);
                    }
                    else
                    {
                        int num = obj.GetComponent<PokemondexElement>().number;
                        string str = obj.GetComponent<PokemondexElement>().name;
                        foreach (Transform child in pokenmons.transform)
                        {
                            // print(child.name);
                            child.gameObject.SetActive(false);
                        }
                        string name = num.ToString() + str;
                        pokenmons.SetActive(true);
                        print(name);
                        pokenmons.transform.Find(name).gameObject.SetActive(true);
                    }
                    
                    
                };
                // obj.GetComponent<TriggerSelect>().onDeSelect = () =>
                // {
                //     _Scrollbar.GetComponent<Scrollbar>().value = 
                //         (Game.PokemonsData.Count - obj.GetComponent<PokemondexElement>().number-0.001f) 
                //         / Game.PokemonsData.Count;
                //     if (_Scrollbar.GetComponent<Scrollbar>().value < 0.14)
                //         _Scrollbar.GetComponent<Scrollbar>().value -= 0.03f;
                //     StopCoroutine(Normal(obj));
                // };
                obj.name = "Pokedex" + i;
                if (i == 0)
                    base.FirstSelectable = obj;
                print(obj.name);
                
                _pokemondexElements.Add(obj.GetComponent<PokemondexElement>());
            }
        }
        
        base.OnEnter(args);
        if (args != null)
        {
            trainer=args[0] as Trainer;
        }
        print(211);
        int j = 0;
        foreach (var i in trainer.PokemonCountered)
        {
            PokemonData temp = Game.PokemonsData[i];
            _pokemondexElements[j].Init(temp);
            j++;
            print(i);
        }

    }

    IEnumerator Normal(GameObject obj)
    {
        yield return new WaitForSeconds(1f);
        if (obj.GetComponent<PokemondexElement>().number == -1)
        {
            //TODO:显示一个傻逼的图片来区分？或者不显示
        }
        else
        {
            int num = obj.GetComponent<PokemondexElement>().number;
            string str = obj.GetComponent<PokemondexElement>().name;
            foreach (Transform child in pokenmons.transform)
            {
                // print(child.name);
                child.gameObject.SetActive(false);
            }
            string name = num.ToString() + str;
            pokenmons.SetActive(true);
            print(name);
            pokenmons.transform.Find(name).gameObject.SetActive(true);
        }

    }
    
}
