using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay.Character;
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


    private void OnCollisionEnter(Collision other)
    {
        // OnInteractive(other.gameObject);
    }
    
    
    public void OnInteractive()
    {
        // DialogHandler.Instance.InitBattle(null);
        // GlobalManager.Instance.StartBattle(null,new List<Trainer>(){_trainer},false,1);
    }

    public void OnInteractive(GameObject obj)
    {
        var runner = FindObjectOfType<DialogueRunner>();

        if (obj.GetComponent<PlayerController>() != null)
        {
            transform.LookAt(obj.transform,Vector3.up);
            obj.transform.LookAt(this.transform, Vector3.up);
            if (!string.IsNullOrEmpty(dialogueNode))
                if (runner.CurrentNodeName != dialogueNode)
                {
                    // transform.rotation.SetLookRotation(obj.transform.position,Vector3.up);
                    // obj.transform.rotation.SetLookRotation(transform.position,Vector3.up);
                    FindObjectOfType<CameraDirector>().SetTargetCinemachine(this.gameObject,obj);
                    UIManager.Instance.PopAllUI(UILayer.MainUI);
                    runner.StartDialogue(dialogueNode);
                    runner.onDialogueComplete.AddListener(() =>
                    {
                        FindObjectOfType<CameraDirector>().ResetTargetCinemachine();
                    });
                }
        }
    }

    public void StartBattle()
    {
        // UIManager.Instance.Hide<DialogPanel>();
        UIManager.Instance.PopAllUI(UILayer.MainUI);
        var transform1 = transform;
        var player = FindObjectOfType<PlayerController>();
        player.Teleport(transform1.position + transform1.forward * 8);
        player.FaceTo(this.transform.position,Vector3.up);
        // FindObjectOfType<PlayerController>().transform.position = transform.position + transform.forward * 8;
        var battleField = FindObjectOfType<BattleFieldHandler>();
        battleField.transform.position = transform1.position + transform1.forward * 4f;
        battleField.transform.rotation = FindObjectOfType<PlayerController>().transform.rotation;
        // GameObject.Find("BattleField").transform.position = transform.position + transform.forward * 4f;
        // GameObject.FindGameObjectWithTag("BattleField").transform.rotation = FindObjectOfType<PlayerController>().transform.rotation;
        GlobalManager.Instance.StartBattle(this);
    }
    
    
    // protected void LateUpdate()
    // {
    //     transform.localEulerAngles = new Vector3(0, 0, transform.localEulerAngles.z);
    // }
}