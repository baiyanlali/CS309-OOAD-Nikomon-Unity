using System;
using GamePlay.UI;
using GamePlay.UI.UIFramework;
using GamePlay.UI.UtilUI;
using PokemonCore;
using PokemonCore.Inventory;
using UnityEngine;
using Yarn.Unity;

namespace GamePlay.Objects
{
    public class CollectableItem:MonoBehaviour,IInteractive
    {
        public Item.Tag itemTag;
        public int itemID;
        public void OnInteractive()
        {
            Item item = Game.ItemsData[(itemTag, itemID)];
            Game.bag.Add(item);
            UIManager.Instance.Show<InformPanel>($"You get {item.name}", null, "OK");
            Destroy(this.gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.GetComponent<PlayerController>()!=null)
                UIManager.Instance.Show<InteractPanel>("Get",gameObject.transform, (Action) OnInteractive);
        }
        
        private void OnTriggerExit(Collider other)
        {
            UIManager.Instance.Hide<InteractPanel>();
        }

        public void OnInteractive(GameObject obj)
        {
            this.OnInteractive();
        }

        
    }
}