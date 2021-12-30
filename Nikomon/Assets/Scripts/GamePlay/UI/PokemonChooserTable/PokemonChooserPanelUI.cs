using System;
using System.Collections.Generic;
using GamePlay.UI.UIFramework;
using GamePlay.Utilities;
using PokemonCore.Combat;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.UI.PokemonChooserTable
{
    public class PokemonChooserPanelUI : BaseUI,IUIAnimator
    {
        private GameObject ChooserElement;
        private Trainer _trainer;
        
        public override void Init(params object[] args)
        {
            base.Init(args);
            
            Trainer trainer=args[0] as Trainer;
            _trainer = trainer;
            string[] chooses = args[1] as string[];
            Action<int,int> actions = args[2] as Action<int,int>;
            int lastIndex = -1;
            if (args.Length >= 4)
                lastIndex = (int)args[3];

            ChooserElement = GameResources.SpawnPrefab(typeof(PokemonChooserElementUI));

            List<Selectable> selectables = new List<Selectable>();
        
            int pokes = trainer.bagPokemons;
        
            if (pokes < transform.childCount)
            {
                for (int i = pokes; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            }
            else if(pokes>transform.childCount)
            {
                for (int i = transform.childCount; i < pokes; i++)
                {
                    Instantiate(ChooserElement, transform);
                }
            }
        
            for (int i = 0; i < Mathf.Min(pokes,transform.childCount); i++)
            {
                if (trainer.party[i] == null) break;
                if (FirstSelectable == null) FirstSelectable = transform.GetChild(i).gameObject;
                transform.GetChild(i).gameObject.SetActive(true);
                transform.GetChild(i).GetComponent<PokemonChooserElementUI>().Init(trainer.party[i],i,chooses,actions);
                selectables.Add(transform.GetChild(i).GetComponent<Selectable>());
            }
            
            selectables.AutomateNavigation(DirectionType.Vertical);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args">0 for Trainer, 1 for string[] chooses, 2 for Action actions</param>
        public override void OnEnter(params object[] args)
        {
            base.OnEnter(args);
        }

        public override void OnResume()
        {
            base.OnResume();
            for (int i = 0; i < transform.childCount; i++)
            {
                if (_trainer.party[i] == null) break;
                transform.GetChild(i).gameObject.SetActive(true);
                transform.GetChild(i).GetComponent<PokemonChooserElementUI>().UpdateData(_trainer.party[i]);
            }
            gameObject.SetActive(true);
        }

        public void OnEnterAnimator()
        {
            gameObject.transform.localScale = Vector3.zero;
            LeanTween.scale(gameObject,Vector3.one,0.3f).setOnComplete(() =>
            {

            });
        }

        public void OnExitAnimator()
        {
            gameObject.transform.localScale = Vector3.one;
            LeanTween.scale(gameObject,Vector3.zero,0.3f).setOnComplete(() =>
            {

            });
        }
    }
}
