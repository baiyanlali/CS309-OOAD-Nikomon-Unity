using System;

namespace PokemonCore.Combat
{
    [Serializable]
    public class Trainer
    {
        public string name { get; private set; }
        public int id { get; private set; }
        public int money { get; private set; }
        public Pokemon[] party { get; set; }

        public int pokedexNums;

        public Trainer(string name)
        {
            this.name = name;
            //TODO:Change the id to normal value
            this.id = 0;
            this.money = 3000;
            this.party = new Pokemon[6];
            pokedexNums = 0;
        }

        public int Money
        {
            get => this.money;
            set => this.money = Math.Max(value, 0);
        }

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

        public Pokemon firstParty => party[0];
        public Pokemon lastParty => party[party.Length-1];

    }
}