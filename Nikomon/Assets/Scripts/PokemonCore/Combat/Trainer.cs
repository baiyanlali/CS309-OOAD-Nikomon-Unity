using System;
using System.Reflection;
using Newtonsoft.Json;
using PokemonCore.Combat.Interface;

namespace PokemonCore.Combat
{
    [Serializable]
    public class Trainer:IPropertyModify
    {
        public string name { get; private set; }
        public int id { get; private set; }
        public int money { get; private set; }
        public Pokemon[] party { get; set; }
        
        public bool isMale;

        public int pokedexNums;

        [JsonConstructor]
        public Trainer(string name, int id, int money, Pokemon[] party, bool isMale, int pokedexNums)
        {
            this.name = name;
            this.id = id;
            this.money = money;
            this.party = party;
            this.isMale = isMale;
            this.pokedexNums = pokedexNums;
            pokemonOnTheBattle = new bool[party.Length];
        }
        
        public Trainer(string name,bool isMale)
        {
            this.name = name;
            this.isMale = isMale;
            //TODO:Change the id to normal value
            this.id = Game.Random.Next();
            this.money = 3000;
            this.party = new Pokemon[Game.MaxPartyNums];
            pokemonOnTheBattle = new bool[party.Length];
            pokedexNums = 0;
        }

        [JsonIgnore] public bool[] pokemonOnTheBattle;
        
        [JsonIgnore]
        public int pokemonCount
        {
            get
            {
                int num = 0;
                for (int i = 0; i < this.party.Length; i++)
                {
                    if (this.party[i] != null) num++;
                }

                return num;
            }
        }

        /// <summary>
        /// 这里传进来一个Pokemon，然后返回这个Pokemon在Trainer的party中的index
        /// </summary>
        public int PokemonIndex(Pokemon pokemon)
        {
            for (int i = 0; i < party.Length; i++)
            {
                if (party[i] != null)
                {
                    if (party[i] == pokemon) return i;
                }
            }

            return -1;
        }
        
        [JsonIgnore]
        public int ablePokemonCount
        {
            get
            {
                int num = 0;
                for (int i = 0; i < this.party.Length; i++)
                {
                    if (this.party[i] != null && party[i].HP>0) num++;
                }

                return num;
            }
        }

        [JsonIgnore]
        public int lastAblePokemonIndex
        {
            get
            {
                int index = 0;
                for (int i = 0; i < party.Length; i++)
                {
                    //血量大于0而且不在战场上
                    if (party[i] != null && party[i].HP > 0 && pokemonOnTheBattle[i] ==false) return i;
                }

                return -1;
            }
        }
        [JsonIgnore]
        public Pokemon firstParty => party[0];
        [JsonIgnore]
        public Pokemon lastParty => party[party.Length-1];
        [Obsolete]
        //TODO:删除所有反射属性，支持IOS端
        public object this[string propertyName]
        {
            get
            {
                Type t = this.GetType();
                PropertyInfo pi = t.GetProperty(propertyName);
                return pi.GetValue(this, null);
            }
            set
            {
                Type t = this.GetType();
                PropertyInfo pi = t.GetProperty(propertyName);
                pi.SetValue(this, value, null);
            }
        }

    }
}