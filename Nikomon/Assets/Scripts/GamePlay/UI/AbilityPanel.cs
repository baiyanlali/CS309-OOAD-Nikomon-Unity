using System;
using System.Collections.Generic;
using GamePlay.UI.BattleUI;
using GamePlay.UI.UIFramework;
using PokemonCore;
using PokemonCore.Character;
using PokemonCore.Combat;
using UnityEngine;
using UnityEngine.UI;
using Debug = PokemonCore.Debug;

namespace GamePlay.UI.UtilUI
{
    public class AbilityPanel : BaseUI
    {
        public GameObject abilityTable;
        private List<MoveElement> _moveElements=new List<MoveElement>();
        private GameObject MoveElementPrefab;
        public Transform MoveDetial;
        public Trainer trainer;
        public Pokemon Pokemon;
        public Text Name,HP, ATK, DEF, SPA, SPD, SPE;
        public RadarTest _radarTest;
        public Nametext _nametext;
        private void Awake()
        {
            // abilityTable.SetActive(true);
            //_radarTest.pokemon = Pokemon;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args">0 for trainer,1 for pokenmon</
        public override void OnEnter(params object[] args)
        {
            //12.6开始改的！
            MoveElementPrefab = GameResources.SpawnPrefab(typeof(MoveElement));
            if (_moveElements.Count == 0)
            {
                for (int i = 0; i < Game.MaxMovesPerPokemon; i++)
                {
                    GameObject obj = Instantiate(MoveElementPrefab, MoveDetial);
                    obj.name = "Move" + i;
                    _moveElements.Add(obj.GetComponent<MoveElement>());
                }
            }

            base.OnEnter(args);
            if (args != null)
            {
                trainer=args[0] as Trainer;
                Pokemon=args[1] as Pokemon;
            }
            print(Pokemon.Name);
            _radarTest.Init(Pokemon);
            _nametext.Init(Pokemon);
            print(Pokemon.Name);
            
        }
        


        // public void RefreshInformation(Pokemon pokemon)
        // {
        //     if (pokemon == null)
        //     {
        //         Name.text = string.Empty;
        //         HP.text = string.Empty;
        //         ATK.text = string.Empty;
        //         DEF.text = string.Empty;
        //         SPA.text = string.Empty;
        //         SPD.text = string.Empty;
        //         SPE.text = string.Empty;
        //     }
        //     else
        //     {
        //         Name.text = pokemon.Name;
        //         HP.text = pokemon.HP.ToString();
        //         ATK.text = pokemon.ATK.ToString();
        //         DEF.text = pokemon.DEF.ToString();
        //         SPA.text = pokemon.SPA.ToString();
        //         SPD.text = pokemon.SPD.ToString();
        //         SPE.text = pokemon.SPE.ToString();
        //
        //         for (int i = 0; i < pokemon.moves.Length; i++)
        //         {
        //             if (pokemon.moves[i] == null)
        //             {
        //                 _moveElements[i].gameObject.SetActive(false);
        //             }
        //             else
        //             {
        //                 _moveElements[i].Init(pokemon.moves[i]);
        //                 _moveElements[i].gameObject.SetActive(true);
        //             }
        //         
        //         }
        //     }
        // }
        
        
        
    }
}