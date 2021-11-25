using System;
using System.Reflection;
using Newtonsoft.Json;
using PokemonCore.Combat.Interface;

namespace PokemonCore.Combat
{
    [Serializable]
    public class Trainer
    {
        public string name { get; private set; }
        public int id { get; private set; }

        private int money;

        [JsonIgnore]
        public int Money
        {
            get
            {
                return money;
            }
            set
            {
                if (value < 0)
                    money = 0;
                else
                {
                    money = value;
                }
            }
        }

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

        public Trainer(string name, bool isMale)
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

        [JsonIgnore] public bool isPartyFull => pokemonCount >= party.Length;

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
                    if (this.party[i] != null && party[i].HP > 0) num++;
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
                    if (party[i] != null && party[i].HP > 0 && pokemonOnTheBattle[i] == false) return i;
                }

                return -1;
            }
        }

        [JsonIgnore] public Pokemon firstParty => party[0];
        
        
        [JsonIgnore] public Pokemon firstAbleParty{
            get
            {
                for (int i = 0; i < party.Length; i++)
                {
                    if (party[i].HP > 0) return party[i];
                }

                return null;
            }
        }
        [JsonIgnore] public Pokemon lastParty => party[party.Length - 1];


        /// <summary>
        /// 显示背包里实际上有多少只宝可梦
        /// </summary>
        [JsonIgnore]
        public int bagPokemons
        {
            get
            {
                int num = 0;
                for (int i = 0; i < party.Length; i++)
                {
                    if (party[i] != null) num++;
                    else break;
                }

                return num;
            }
        }

        public void AddPokemon(Pokemon poke)
        {
            if (isPartyFull) return;
            poke.TrainerID = this.id;
            party[pokemonCount] = poke;
        }

        /// <summary>
        /// 这里我们假设只出现一个null,我们先找到这个null，然后让它和下一个元素交换，直到下一个元素为null
        /// </summary>
        public void ReArrayParty()
        {
            int index = 0;
            for (int i = 0; i < party.Length; i++)
            {
                if (party[i] == null)
                {
                    index = i;
                    break;
                }
            }

            for (int i = index; i < party.Length - 1; i++)
            {
                if (party[i + 1] != null)
                {
                    (party[i], party[i + 1]) = (party[i + 1], party[i]);
                }
                else break;
            }
        }

        public bool RemovePokemon(Pokemon poke)
        {
            if (pokemonCount <= 1||poke==null)
            {
                return false;
            }
            for (int i = 0; i < pokemonCount; i++)
            {
                if (poke == party[i])
                {
                    party[i] = null;
                    ReArrayParty();
                    return true;

                }
            }

            return false;
        }

        public bool RemovePokemon(int index)
        {
            for (int i = 0; i < pokemonCount; i++)
            {
                if (i == index)
                {
                   return RemovePokemon(party[i]);
                }
            }

            return false;
        }
    }
}