using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PokemonCore.Utility;

namespace PokemonCore.Inventory
{
    public class TrainerBag
    {
        /// <summary>
        /// Tuple用来确定是哪一个，最后一个int表示物品数量
        /// </summary>
        public Dictionary<ValueTuple<Item.Tag, int>, int> Items;

        [JsonConstructor]
        public TrainerBag(Dictionary<ValueTuple<Item.Tag, int>, int> items=null)
        {
            if (items != null)
            {
                this.Items = items;
            }
            else
            {
                this.Items = new Dictionary<ValueTuple<Item.Tag, int>, int>();
            }
        }

        [JsonIgnore]
        public List<Item> this[Item.Tag tag] => ShowItems(tag);
        
        [JsonIgnore]
        public int this[Item item]=>Items[(item.tag,item.ID)];

        public List<Item> ShowItems(Item.Tag tag)
        {
            List<Item> items = new List<Item>();
            foreach (var item in Items)
            {
                if(item.Key.Item1==tag)items.Add(Game.ItemsData[item.Key]);
            }
            return items;
        }
        
        public void Add(Item item,int nums=1)
        {
            Add((item.tag,item.ID),nums);
        }

        public void Add(ValueTuple<Item.Tag, int> item,int nums=1)
        {
            if(Items.ContainsKey(item))
                Items[item]+=nums;
            else
            {
                Items.Add(item,nums);
            }
        }

        public void AddRange(List<Item> items)
        {
            foreach (var item in items.OrEmptyIfNull())
            {
                Add(item);
            }
        }

        public bool Decrease(Item item, int nums = 1)
        {
            return Decrease((item.tag, item.ID), nums);
        }

        public bool Decrease(ValueTuple<Item.Tag, int> item,int nums=1)
        {
            if (nums > Items[item]) return false;
            Items[item] -= nums;
            return true;
        }
        
    }
}