using System.Collections;
using System.Collections.Generic;
using PokemonCore.Combat;
using UnityEngine;

public class NPC : MonoBehaviour,IInteractive
{
    private Trainer _trainer;
    // Start is called before the first frame update
    void Start()
    {
        _trainer = new Trainer("WYF", true);
        _trainer.party[0] = new Pokemon(1, 20);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnInteractive()
    {
        // DialogHandler.Instance.InitBattle(null);
        // GlobalManager.Instance.StartBattle(null,new List<Trainer>(){_trainer},false,1);
    }

    public void OnInteractive(GameObject obj)
    {
        if (obj.name == "")
        {
            
        }
    }
}
