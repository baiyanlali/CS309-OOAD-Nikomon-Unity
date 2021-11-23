using GamePlay.UI.UIFramework;
using GamePlay.UI.UtilUI;
using PokemonCore;
using PokemonCore.Inventory;
using UnityEngine.UI;

namespace GamePlay.UI.BagSystem
{
    public class StorePanelUI:BaseUI
    {
        private Text MoneyNumber;
        public override void Init(params object[] args)
        {
            MoneyNumber = GET(MoneyNumber, "MoneyPanel/Text");
            base.Init(args);
        }

        public override void OnEnter(params object[] args)
        {
            base.OnEnter(args);

            // MoneyNumber.text ="Money:"+ Game.trainer.Money;
            MoneyNumber.text ="Money:12345";
            
            // UIManager.Instance.Show<ConfirmPanel>("Are your sure to buy?");
        }

        public override void OnRefresh(params object[] args)
        {
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
        
        

        public void OnBuy(Item.Tag tag,int id)
        {
            Game.bag.Add((tag,id));
            Game.trainer.Money -= 1000;
        }
    }
}