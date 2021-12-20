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
        public RadarTest _radarTest;
        public Nametext _nametext;
        public GameObject pokenmons;
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
            for (int i = 0; i < Pokemon.moves.Length; i++)
            {
                if (Pokemon.moves[i] == null)
                {
                    _moveElements[i].gameObject.SetActive(false);
                }
                else
                {
                    _moveElements[i].Init(Pokemon.moves[i]);
                    _moveElements[i].gameObject.SetActive(true);
                }
                
            }
            foreach (Transform child in pokenmons.transform)
            {
                // print(child.name);
                child.gameObject.SetActive(false);
            }
            string name = Pokemon.ID.ToString() + Pokemon.Name;
            pokenmons.SetActive(true);
            print(name);
            pokenmons.transform.Find(name).gameObject.SetActive(true);
            _radarTest.Init(Pokemon);
            _nametext.Init(Pokemon,trainer);

        }

        
        
        
    }
}