using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay.UI.UIFramework;
using GamePlay.UI.UtilUI;
using PokemonCore.Combat;
using UnityEngine;
using Yarn.Unity;

public class NPC : MonoBehaviour, IInteractive
{
    [Serializable]
    public struct PokemonInfo
    {
        public int id;
        public int initLevel;
    }

    [HideInInspector] public Trainer _trainer;
    public int pokemonPerTrainer = 1;
    public PokemonInfo[] PokemonInfos;

    public string dialogueNode;

    // Start is called before the first frame update
    void Start()
    {
        _trainer = new Trainer(dialogueNode, true);
        if (PokemonInfos != null)
        {
            for (int i = 0; i < PokemonInfos.Length; i++)
            {
                _trainer.party[i] = new Pokemon(PokemonInfos[i].id, PokemonInfos[i].initLevel,_trainer.id);
            }
        }
    }


    public void OnInteractive()
    {
        // DialogHandler.Instance.InitBattle(null);
        // GlobalManager.Instance.StartBattle(null,new List<Trainer>(){_trainer},false,1);
    }

    public void OnInteractive(GameObject obj)
    {
        if (obj.GetComponent<PlayerMovement>() != null)
        {
            transform.LookAt(obj.transform,Vector3.up);
            if (!string.IsNullOrEmpty(dialogueNode))
                FindObjectOfType<DialogueRunner>().StartDialogue(dialogueNode);
        }
    }

    public void StartBattle()
    {
        UIManager.Instance.Hide<DialogPanel>();
        FindObjectOfType<PlayerMovement>().transform.position = transform.position + transform.forward * 8;
        GameObject.Find("BattleField").transform.position = transform.position + transform.forward * 4f;
        GameObject.FindGameObjectWithTag("BattleField").transform.rotation = FindObjectOfType<PlayerMovement>().transform.rotation;
        GlobalManager.Instance.StartBattle(this);
    }
}