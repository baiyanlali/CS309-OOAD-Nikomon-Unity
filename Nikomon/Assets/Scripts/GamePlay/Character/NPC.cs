using System.Collections;
using System.Collections.Generic;
using PokemonCore.Combat;
using UnityEngine;
using Yarn.Unity;

public class NPC : MonoBehaviour,IInteractive
{
    private Trainer _trainer;

    public string dialogueNode;
    // Start is called before the first frame update
    void Start()
    {
        _trainer = new Trainer("WYF", true);
        _trainer.party[0] = new Pokemon(1, 20);
        _trainer.party[1] = new Pokemon(8, 20);
    }
    

    public void OnInteractive()
    {
        // DialogHandler.Instance.InitBattle(null);
        // GlobalManager.Instance.StartBattle(null,new List<Trainer>(){_trainer},false,1);
    }

    public void OnInteractive(GameObject obj)
    {
        if (obj.GetComponent<PlayerMovement>()!=null)
        {
            if(!string.IsNullOrEmpty(dialogueNode)) 
                FindObjectOfType<DialogueRunner>().StartDialogue(dialogueNode);
        }
    }
}
