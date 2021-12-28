using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PokemonCore.Attack.Data;
using PokemonCore.Combat.Interface;
using PokemonCore.Utility;

namespace PokemonCore.Inventory
{
    [System.Serializable]
    public class Item
    {
        public enum Tag
        {
            Medicine,//
            PokeBalls,//精灵球
            BattleItems,//
            Berries,
            OtherItems,
            TMs,
            Treasures,
            Ingredients,
            KeyItems
        }
        
        public Tag tag { get;  set; }
        public int ID { get;  set; }
        public string name { get;  set; }
        public int effectId { get; set; }

        public Object para { get; }

        /// <summary>
        /// 用作买卖
        /// </summary>
        public int value { get; set; }
        
        [JsonConstructor]
        public Item(
            Tag tag = Tag.OtherItems,
            int id = 0,
            string name = "",
            int value = 0,
            int effectId = 0,
            Object para = null
        )
        {
            this.tag = tag;
            this.ID = id;
            this.name = name;
            this.value = value;
            this.effectId = effectId;
            this.para = para;
        }

        public void useItem(params object[] args)
        {
            Effect e = (Game.LuaEnv.Global.Get<Effect>("effect"+effectId));//string plus int (id)
        }
        
        
        
        
    }
}