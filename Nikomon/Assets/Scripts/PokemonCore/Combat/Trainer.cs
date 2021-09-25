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

        public Trainer(string name,bool isMale)
        {
            this.name = name;
            this.isMale = isMale;
            //TODO:Change the id to normal value
            this.id = Game.Random.Next();
            this.money = 3000;
            this.party = new Pokemon[Game.MaxPartyNums];
            pokedexNums = 0;
        }

        public int Money
        {
            get => this.money;
            set => this.money = Math.Max(value, 0);
        }
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