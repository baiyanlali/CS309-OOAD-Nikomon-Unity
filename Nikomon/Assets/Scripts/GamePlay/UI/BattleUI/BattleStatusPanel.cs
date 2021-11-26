using System;
using System.Collections.Generic;
using GamePlay.UI.UIFramework;
using PokemonCore.Combat;
using UnityEngine;

namespace GamePlay.UI.BattleUI
{
    public class BattleStatusPanel:BaseUI
    {
        public GameObject AlliesState;
        public GameObject OpponentState;
        public GameObject PokemonStateUIPrefab;
        // public override bool IsBlockPlayerControl => true;
        public override void Init(params object[] args)
        {
            AlliesState = GET(AlliesState, nameof(AlliesState), GET_TYPE.GameObject);
            OpponentState = GET(OpponentState, nameof(OpponentState), GET_TYPE.GameObject);

            PokemonStateUIPrefab = GameResources.SpawnPrefab(typeof(BattlePokemonStateUI));
            
            OnRefresh(args);
            
            base.Init(args);
        }

        private BattleHandler bh;
        public override void OnRefresh(params object[] args)
        {
            base.OnRefresh(args);
            if(args.Length>=1)
                bh=args[0] as BattleHandler;
            else if (bh == null)
                throw new Exception("No Battle Field Found");

            List<CombatPokemon> allies = bh.AlliesPokemons;
            List<CombatPokemon> opponents = bh.OpponentPokemons;

            InitStateUI(allies, AlliesState.transform);
            InitStateUI(opponents, OpponentState.transform);
        }
        
        

        /// <summary>
            /// 目前战斗UI界面中已经有了部分素材，下次战斗时，会有几种情况
            /// 1.宝可梦数量等于上一局，什么都不用改
            /// 2.宝可梦数量多于上一局，那就多Instantiate
            /// 3.宝可梦数量少于上一局，那就Hide起来
            /// 可以写得更简洁，但是我懒
            /// </summary>
            /// <param name="pokemons"></param>
            /// <param name="parent"></param>
        private void InitStateUI(List<CombatPokemon> pokemons, Transform parent)
            {
                if (pokemons.Count == parent.transform.childCount)
                {
                    for (int i = 0; i < pokemons.Count; i++)
                    {
                        parent.transform.GetChild(i).GetComponent<BattlePokemonStateUI>().Init(pokemons[i]);
                    }
                }
                else if (pokemons.Count > parent.transform.childCount)
                {
                    for (int i = 0; i < parent.transform.childCount; i++)
                    {
                        parent.transform.GetChild(i).GetComponent<BattlePokemonStateUI>().Init(pokemons[i]);
                    }
        
                    for (int i = parent.transform.childCount; i < pokemons.Count; i++)
                    {
                        GameObject state = Instantiate(PokemonStateUIPrefab, parent.transform);
                        state.name = pokemons[i].Name;
                        state.GetComponent<BattlePokemonStateUI>().Init(pokemons[i]);
                    }
                }
                else if (pokemons.Count < parent.transform.childCount)
                {
                    for (int i = 0; i < pokemons.Count; i++)
                    {
                        parent.transform.GetChild(i).GetComponent<BattlePokemonStateUI>().Init(pokemons[i]);
                    }
        
                    for (int i = pokemons.Count; i < parent.transform.childCount; i++)
                    {
                        parent.transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
            }


        public override void OnEnter(params object[] args)
        {
            base.OnEnter(args);
        }
    }
}