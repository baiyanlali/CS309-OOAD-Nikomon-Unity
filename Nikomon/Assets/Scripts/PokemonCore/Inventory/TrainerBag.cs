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
            if (Items[item] == 0)
            {
                Items.Remove(item);
            }
            return true;
        }
        
    }

/// <summary>
/// 因为Trainer Bag的value Tuple没法直接反序列化，所以写一个新的可以用于序列化的类
/// </summary>
    public class SeriTrainerBag
    {
        public struct SeriItem
        {
            public Item.Tag Tag;
            public int Index;
            public int Count;

            public SeriItem(Item.Tag tag, int index, int count)
            {
                this.Tag = tag;
                this.Index = index;
                this.Count = count;
            }
            
        }

        public List<SeriItem> items;

        public SeriTrainerBag(TrainerBag bag)
        {
            items = new List<SeriItem>();
            foreach (var item in bag.Items)
            {
                items.Add(new SeriItem(item.Key.Item1,item.Key.Item2,item.Value));
            }
        }

        [JsonConstructor]
        public SeriTrainerBag(SeriItem[] items)
        {
            this.items = new List<SeriItem>();
            this.items.AddRange(items);
        }

        public TrainerBag ToTrainerBag()
        {
            Dictionary<ValueTuple<Item.Tag, int>, int> Items = new Dictionary<(Item.Tag, int), int>();
            foreach (var item in this.items)
            {
                Items[(item.Tag, item.Index)] = item.Count;
            }

            return new TrainerBag(Items);
        }
        
    }
}