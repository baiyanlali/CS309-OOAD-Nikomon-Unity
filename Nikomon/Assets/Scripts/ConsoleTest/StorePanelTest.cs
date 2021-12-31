using System.Collections;
using System.Collections.Generic;
using GamePlay.UI.BagSystem;
using GamePlay.UI.UIFramework;
using PokemonCore;
using PokemonCore.Combat;
using PokemonCore.Inventory;
using UnityEngine;

public class StorePanelTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GlobalManager.Instance.LoadAllSaveData();
        StartCoroutine(SetUp());
    }

    IEnumerator SetUp()
    {
        yield return null;
        Trainer trainer = new Trainer("JDY", true);
        trainer.Money = 3000;
        TrainerBag bag = new TrainerBag();
        UIManager.Instance.Show<StorePanelUI>(trainer,bag);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
