using System.Collections;
using System.Collections.Generic;
using GamePlay.UI.UIFramework;
using GamePlay.UI.UtilUI;
using PokemonCore.Monster.Data;
using UnityEngine;

public class SettlementTest : MonoBehaviour
{
    void Start()
    {
        GlobalManager.Instance.LoadAllSaveData();
        Pokemon wyf = new Pokemon(3, 50);
        Pokemon AIfanfan = new Pokemon(7, 50);
        Pokemon AIfanfan2 = new Pokemon(1, 30);
            
        List<PokemonLevelUpState> _PokemonLevelUpStates = new List<PokemonLevelUpState>();
        var wyf_before = new Experience(wyf.Exp);
        wyf.Exp.AddExperience(3000);
        
        PokemonLevelUpState temp1 = new PokemonLevelUpState();
        temp1.Pokemon = wyf;
        Experience exp1 = wyf_before;

        temp1.ExpBefore = exp1;
        _PokemonLevelUpStates.Add(temp1);
        
        PokemonLevelUpState temp2 = new PokemonLevelUpState();
        var AIfanfan_before = new Experience(AIfanfan.Exp);
        AIfanfan.Exp.AddExperience(5000);
        temp2.Pokemon = AIfanfan;
        //exp2.AddExperience(5000);
        temp2.ExpBefore = AIfanfan_before;
        _PokemonLevelUpStates.Add(temp2);
        print(AIfanfan.Exp.Current);

        
        PokemonLevelUpState temp3 = new PokemonLevelUpState();
        var AIfanfan2_before = new Experience(AIfanfan2.Exp);
        temp3.Pokemon = AIfanfan2;
        AIfanfan2.Exp.AddExperience(10000);
        temp3.ExpBefore = AIfanfan2_before;
        _PokemonLevelUpStates.Add(temp3);
        UIManager.Instance.Show<SettlementPanel>(_PokemonLevelUpStates);

    }



   
}
