using System;
using System.Collections.Generic;
using System.Linq;
using GamePlay.UI.UIFramework;
using GamePlay.Utilities;
using PokemonCore;
using PokemonCore.Attack;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.UI.BattleUI
{
    public class MovePanel : BaseUI
    {
        private GameObject MoveUIPrefab;
        private Move[] moveCache;
        /// <summary>
        /// 现在init有两种情况，一种是只传List，另外一种是只传Action
        /// </summary>
        /// <param name="args">List Move</param>
        public override void Init(params object[] args)
        {
            Move[] moves = (args[0] as List<Move>)?.ToArray();
            Action<int> OnChooseMove = null;
            if(moves==null)
            {
                OnChooseMove = (Action<int>) args[0];
            }
            if (moveCache == null && moves == null)
            {
                throw new Exception("Need moves");
            }
            else
            {
                if (moves != null)
                {
                    if (moveCache == null || moveCache != moves)
                        moveCache = moves;
                }
            }
            MoveUIPrefab = GameResources.SpawnPrefab(typeof(MoveElement));
            base.Init(args);


            for (int i = transform.childCount; i < Game.MaxMovesPerPokemon+1; i++)
            {
                GameObject state = Instantiate(MoveUIPrefab, transform);
                // state.GetComponentInChildren<Text>().text = "";
            }

            List<Selectable> selectables = new List<Selectable>();
            selectables.Add(transform.GetChild(0).GetComponent<Selectable>());
            if (moves != null)
            {
                for (int i = 0; i < moves.Length; i++)
                {
                    if (moves[i] == null)
                    {
                        transform.GetChild(i + 1).gameObject.SetActive(false);
                    }
                    else
                    {
                        GameObject obj = transform.GetChild(i + 1).gameObject;
                        obj.SetActive(true);
                        obj.GetComponent<MoveElement>().Init(moves[i]);
                        obj.GetComponent<Button>().onClick.RemoveAllListeners();
                        int index = i;
                        obj.GetComponent<Button>().onClick.AddListener(() =>
                        {
                            OnChooseMove?.Invoke(index);
                        });
                    }
                }
            }
            else
            {
                for (int i = 0; i < moveCache.Length; i++)
                {
                    if (moveCache[i] == null)
                    {
                        transform.GetChild(i + 1).gameObject.SetActive(false);
                    }
                    else
                    {
                        GameObject obj = transform.GetChild(i + 1).gameObject;
                        obj.SetActive(true);
                        if (FirstSelectable == null) FirstSelectable = obj;
                        obj.GetComponent<MoveElement>().Init(moveCache[i]);
                        var btn = obj.GetComponent<Button>();
                        selectables.Add(btn);
                        int index = i;
                        btn.onClick.RemoveAllListeners();
                        btn.onClick.AddListener(() =>
                        {
                            OnChooseMove(index);
                            // print("Move Button!");
                        });
                    }
                }
            }
            
            selectables.AutomateNavigation(DirectionType.Vertical);
           
        }

        public override void OnRefresh(params object[] args)
        {
            base.OnRefresh(args);
            moveCache = (args[0] as List<Move>)?.ToArray();
        }
    }
}