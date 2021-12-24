using System.Collections.Generic;
using GamePlay.UI.BattleUI;
using GamePlay.UI.UIFramework;
using GamePlay.UI.UtilUI;
using PokemonCore;
using PokemonCore.Combat;
using PokemonCore.Inventory;
using UnityEngine;
using UnityEngine.UI;

namespace GamePlay.UI.BagSystem
{
    public class StorePanelUI : BaseUI
    {
        public Text MoneyNumber;
        public Trainer trainer;
        public TrainerBag bag;
        private List<StoreContentElement> _storeElements=new List<StoreContentElement>();
        private GameObject StorePrefab;
        public Transform ItemList;
        public Transform _Scrollbar;
        public Image IconImage;
        public override bool IsOnly { get; } = true;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args">0 for trainer,1 for bag</
        /// 
        public override void OnEnter(params object[] args)
        {
            StorePrefab = GameResources.SpawnPrefab(typeof(StoreContentElement));
            print(_storeElements);
            if (_storeElements.Count == 0)
            {
                int i = 0;
                foreach (var vItem in Game.ItemsData)
                {
                    GameObject obj = Instantiate(StorePrefab, ItemList);
                    //print(i);
                    obj.transform.SetAsLastSibling();
                    obj.GetComponent<StoreContentElement>().number = i;
                    var item = vItem.Value;
                    obj.GetComponent<StoreContentElement>().Init(vItem.Key,vItem.Value);
                    obj.GetComponent<TriggerSelect>().onSelect = () =>
                    {
                        IconImage.sprite = obj.GetComponent<StoreContentElement>().ItemIcon.sprite;
                        _Scrollbar.GetComponent<Scrollbar>().value = 
                            (Game.ItemsData.Count + 1 - obj.GetComponent<StoreContentElement>().number-0.001f) 
                            / (Game.ItemsData.Count + 1);
                        
                        if (_Scrollbar.GetComponent<Scrollbar>().value < 0.14)
                            _Scrollbar.GetComponent<Scrollbar>().value -= 0.1f;
                        if (_Scrollbar.GetComponent<Scrollbar>().value > 0.86)
                            _Scrollbar.GetComponent<Scrollbar>().value += 0.03f;
                        print(_Scrollbar.GetComponent<Scrollbar>().value);
                        Vector3 v = new Vector3();
                        v.x = 1.1f;
                        v.y = 1.1f;
                        v.z = 1.1f;
                        obj.transform.localScale = v;


                    };
                    obj.GetComponent<TriggerSelect>().onDeSelect = () =>
                    {
                        Vector3 v = new Vector3();
                        v.x = 1f;
                        v.y = 1f;
                        v.z = 1f;
                        obj.transform.localScale = v;
                    };
                    obj.name = "PokeStoreItem" + i;
                    if (i == 0)
                    {
                        base.FirstSelectable = obj;
                        //print(333);
                    }
                    _storeElements.Add(obj.GetComponent<StoreContentElement>());
                    i++;
                }
            }
            
            
            base.OnEnter(args);
            if (args != null)
            {
                trainer=args[0] as Trainer;
                bag=args[1] as TrainerBag;
            }

            int money = trainer.Money;
            // MoneyNumber.text ="Money:"+ Game.trainer.Money;
            MoneyNumber.text =trainer.Money.ToString();
            
            // UIManager.Instance.Show<ConfirmPanel>("Are your sure to buy?");
        }

        public void buy(int price)
        {
            trainer.Money -= price;
            MoneyNumber.text = trainer.Money.ToString();
        }

        public override void OnRefresh(params object[] args)
        {
            MoneyNumber.text = trainer.Money.ToString();
            base.OnRefresh(args);
        }

        public override void OnPause()
        {
            base.OnPause();
        }

        public override void OnResume()
        {
            base.OnResume();
        }
        
        

        public void OnBuy((Item.Tag, int) key)
        {
            //TODO:和背包互动！！！！
            print(11111111);
            print(22222222);
            
            bag.Add(key);
            foreach (var temp in bag.Items)
            {
                print(temp);
            }
        }
    }
}