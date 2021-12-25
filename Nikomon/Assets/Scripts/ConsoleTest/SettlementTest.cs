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
        wyf.Exp.AddExperience(500);
        PokemonLevelUpState temp1 = new PokemonLevelUpState();
        temp1.Pokemon = wyf;
        Experience exp1 = wyf.Exp;
        exp1.AddExperience(5000);
        temp1.ExpAfter = exp1;
        print(wyf.Exp.Current);
        print(exp1.Current);
        _PokemonLevelUpStates.Add(temp1);
        
        PokemonLevelUpState temp2 = new PokemonLevelUpState();
        temp2.Pokemon = AIfanfan;
        Experience exp2 = AIfanfan.Exp;
        exp2.AddExperience(5000);
        temp2.ExpAfter = exp2;
        _PokemonLevelUpStates.Add(temp2);
        print(AIfanfan.Exp.Current);
        print(exp2.Current);
        
        PokemonLevelUpState temp3 = new PokemonLevelUpState();
        temp3.Pokemon = AIfanfan2;
        Experience exp3 = AIfanfan2.Exp;
        exp3.AddExperience(5000);
        temp3.ExpAfter = exp3;
        _PokemonLevelUpStates.Add(temp3);
        print(AIfanfan2.Exp.Current);
        print(exp3.Current);
        UIManager.Instance.Show<SettlementPanel>(_PokemonLevelUpStates);

    }



   
}
