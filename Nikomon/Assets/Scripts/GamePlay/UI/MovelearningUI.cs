using System;
using System.Collections;
using System.Collections.Generic;
using GamePlay;
using GamePlay.Messages;
using GamePlay.UI.BattleUI;
using GamePlay.UI.UIFramework;
using GamePlay.UI.UtilUI;
using PokemonCore;
using PokemonCore.Attack;
using PokemonCore.Attack.Data;
using PokemonCore.Combat;
using PokemonCore.Monster.Data;
using UnityEngine;
using UnityEngine.UI;

public class MovelearningUI : BaseUI
{
    public Pokemon _Pokemon;
    public MoveData _MoveData;
    private GameObject MoveElementPrefab;
    public Transform MovesList;
    private List<MoveElement> _moveElements=new List<MoveElement>();
    public GameObject pokenmons;
    public Text movecontent;
    public Text Type,Power,Accuracy;
    public Text Title;
    private bool judge = false;
    public Action OnComplete;
    public override bool IsOnly { get; } = true;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="args">0 for Pokemon,1 for new movedata, 2 for oncomplete</param>
    /// 
    public override void OnEnter(params object[] args)
    {
        MoveElementPrefab = GameResources.SpawnPrefab(typeof(MoveElement));
        print(_moveElements.Count);
        base.OnEnter(args);
        if (args != null)
        {
            _Pokemon=args[0] as Pokemon;
            _MoveData=args[1] as MoveData;
            OnComplete = args[2] as Action;
        }

        judge = true;

        Title.text = _Pokemon.Name;
        if (_moveElements.Count == 0)
        {
            for (int i = 0; i < Game.MaxMovesPerPokemon + 1 ; i++)//有一个是新技能
            {
                GameObject obj = Instantiate(MoveElementPrefab, MovesList);
                obj.transform.SetAsLastSibling();
                obj.GetComponent<MoveElement>().index = i;
                obj.GetComponent<TriggerSelect>().onSelect = () =>
                {
                    Vector3 v = new Vector3();
                    v.x = 1.1f;
                    v.y = 1.1f;
                    v.z = 1.1f;
                    obj.transform.localScale = v;
                    //movecontent.text = obj.GetComponent<MoveElement>()._move._baseData.description;
                    movecontent.text = Messages.Get("Move" + obj.GetComponent<MoveElement>()._move.moveID);
                    Type.text = Game.TypesMap[obj.GetComponent<MoveElement>()._move._baseData.Type].Name;
                    Power.text = obj.GetComponent<MoveElement>()._move._baseData.Power.ToString();
                    Accuracy.text = obj.GetComponent<MoveElement>()._move._baseData.Accuracy.ToString();
                };
                obj.GetComponent<TriggerSelect>().onDeSelect = () =>
                {
                    Vector3 v = new Vector3();
                    v.x = 1f;
                    v.y = 1f;
                    v.z = 1f;
                    obj.transform.localScale = v;
                };
                obj.GetComponent<TriggerSelect>().OnClicked= () =>
                {
                    Action<bool> action = (Action<bool>) ((o) =>
                    {
                        if (o == true)
                        {
                            if(obj.GetComponent<MoveElement>().index == Game.MaxMovesPerPokemon)
                                UIManager.Instance.Hide(this);
                            print(obj.GetComponent<MoveElement>().index);
                            _Pokemon.ReplaceMove(obj.GetComponent<MoveElement>().index,_MoveData);
                            // UIManager.Instance.Refresh<MovelearningUI>();
                            print(1111111);
                            UIManager.Instance.Hide(this);
                            // trainer.party[bagIndex] = null;
                            // if (judge == false)
                            // {
                            //     UIManager.Instance.Show<ConfirmPanel>("you can't forget this move");
                            //     return;
                            // }
                            //_moveElements[obj.GetComponent<MoveElement>().index].gameObject.SetActive(false);
                            // _Pokemon.moves[obj.GetComponent<MoveElement>().index] = 

                            //judge = false;

                            //TableUI.UpdateData(trainer);
                        }
                        else
                        {
                    
                        }
                    });
                    UIManager.Instance.Show<ConfirmPanel>("Are you sure you want to forget this move?", action);
                };
                obj.name = "Move" + i;
                if (i == 0)
                {
                    base.FirstSelectable = obj;
                }

                print(obj.name);
                string name = _Pokemon.ID.ToString() + _Pokemon._base.innerName;
                pokenmons.SetActive(true);
                print(name);
                pokenmons.transform.Find(name).gameObject.SetActive(true);
                
                _moveElements.Add(obj.GetComponent<MoveElement>());
            }
        }
        for (int i = 0; i < _Pokemon.moves.Length + 1; i++)
        {
            if(i == _Pokemon.moves.Length)
                _moveElements[i].Init(new Move(_MoveData));
            else
                _moveElements[i].Init(_Pokemon.moves[i]);
        }
        
        ExitBtn.onClick.RemoveAllListeners();
        ExitBtn.onClick.AddListener(() =>
        {
            UIManager.Instance.Hide(this);
            OnComplete?.Invoke();
        });

    }
}
