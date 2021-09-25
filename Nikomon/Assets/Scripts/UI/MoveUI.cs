
using System;
using PokemonCore.Attack;
using PokemonCore.Combat;
using UnityEngine;
using UnityEngine.UI;

public class MoveUI:MonoBehaviour
{
    private Move move;
    private int index;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void Init(Move move,int index)
    {
        this.index = index;
        this.move = move;
        GetComponentInChildren<Text>().text = move._baseData.innerName;
        //TODO:实现其它的漂亮的效果
    }

    private void OnClick()
    {
        print("Click");
        BattleUIHandler.Instance.ChooseMove(move,index);
    }

}
