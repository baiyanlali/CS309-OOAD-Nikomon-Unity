using System;
using GamePlay.UI.UIFramework;
using GamePlay.UI.UtilUI;
using PokemonCore;
using PokemonCore.Inventory;
using UnityEngine;

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
            UIManager.Instance.Show<DialogPanel>($"You get {item.name}",
                DialogPanel.FadeType.Button);
            Destroy(this.gameObject);
        }

        public void OnInteractive(GameObject obj)
        {
            this.OnInteractive();
        }
    }
}