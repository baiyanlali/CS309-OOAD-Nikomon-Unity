
using System;
using GamePlay;
using PokemonCore.Attack;
using PokemonCore.Combat;
using UnityEngine;
using UnityEngine.UI;

public class MoveUI:MonoBehaviour
{
    private Move move;
    private int index;
    
    private Text NameText;
    private Text PPText;

    private Image TypeIcon;
    private Image BG;

    private void Start()
    {
        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void Init(Move move,int index)
    {
        NameText = NameText ? NameText : transform.Find("Name").GetComponent<Text>();
        PPText = PPText? PPText : transform.Find("PP").GetComponent<Text>();
        TypeIcon=TypeIcon?TypeIcon:
        transform.Find("TypeIcon").GetComponent<Image>();
        BG = BG ? BG : GetComponent<Image>();

        TypeIcon.color=new Color(TypeIcon.color.r,TypeIcon.color.g,TypeIcon.color.b,0.8f);
        this.index = index;
        this.move = move;
        NameText.text = move._baseData.innerName;
        PPText.text = $"{move.PP} / {move.TotalPP}";
        TypeIcon.sprite = GameResources.TypeIcons[move._baseData.Type];
        BG.color = GameResources.TypeColors[move._baseData.Type];
        // print($"{move._baseData.innerName} :{move._baseData.Type}");
        // GetComponentInChildren<Text>().text = move._baseData.innerName;
        //TODO:实现其它的漂亮的效果

    }

    private void OnClick()
    {
        // print("Click");
        BattleUIHandler.Instance.ChooseMove(move,index);
    }

}
