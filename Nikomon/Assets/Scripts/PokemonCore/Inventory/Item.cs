using Newtonsoft.Json;

namespace PokemonCore.Inventory
{
    [System.Serializable]
    public class Item
    {
        public enum Tag
        {
            Medicine,
            PokeBalls,
            BattleItems,
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
        
        /// <summary>
        /// 用作买卖
        /// </summary>
        public int value { get; set; }

        [JsonConstructor]
        public Item(Tag t,int id,string name)
        {
            this.tag = t;
            this.ID = id;
            this.name = name;
        }


        public Item()
        {
            tag =  Tag.Berries;
            ID = 1;
            name = "Change this";
        }
        
        
    }
}