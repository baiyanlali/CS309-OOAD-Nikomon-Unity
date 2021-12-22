using System.Collections;
using System.Collections.Generic;
using GamePlay.UI.BagSystem;
using GamePlay.UI.UIFramework;
using PokemonCore.Combat;
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
        UIManager.Instance.Show<StorePanelUI>(trainer);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
