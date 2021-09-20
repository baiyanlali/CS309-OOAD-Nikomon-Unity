using System;
using PokemonCore.Combat;

namespace PokemonCore.Character
{
    [Serializable]
    public class PC
    {
        private int maxPokemonsPerBox, maxBox;

        private Pokemon[,] pokemons;

        public Pokemon[][] AllBoxes
        {
            get
            {
                Pokemon[][] arr=new Pokemon[maxBox][];
                for (int i = 0; i < maxBox; i++)
                {
                    arr[i] = new Pokemon[maxPokemonsPerBox];
                    for (int j = 0; j < maxPokemonsPerBox; j++)
                    {
                        arr[i][j] = pokemons[i, j];
                    }
                }

                return arr;
            }
        }

        public Pokemon[] Pokemons
        {
            get
            {
                Pokemon[] arr = new Pokemon[maxPokemonsPerBox];
                for (int i = 0; i < maxBox; i++)
                {
                    arr[i] = this.pokemons[(int)this.ActiveBox, i];
                }

                return arr;
            }
        }

        public byte ActiveBox { get; private set; }
        public string[] BoxNames { get; private set; }


        public PC this[byte i]
        {
            get
            {
                this.ActiveBox = (byte)((uint)i % maxBox);
                return this;
            }
            
        }
        public PC(int maxPokemonsPerBox = 20, int maxBox = 40)
        {
            this.pokemons = new Pokemon[maxBox, maxPokemonsPerBox];
            this.BoxNames = new string[maxBox];
            for (int index1 = 0; index1 < maxBox; index1++)
            {
                for (int index2 = 0; index2 < maxPokemonsPerBox; index2++)
                {
                    this.pokemons[index1, index2] = null;
                }

                BoxNames[index1] = $"Box {index1 + 1}";
            }
        }

        public PC(
            Pokemon[][] pokemons = null,
            byte? box = null,
            string[] names = null) : this(20, 40)
        {
            if (names != null)
                BoxNames = names;
            if (box.HasValue)
                this.ActiveBox = (byte) ((uint) box.Value % maxBox);
            if (pokemons != null) return;
            for (int index1 = 0; index1 < maxBox; index1++)
            {
                for (int index2 = 0; index2 < maxPokemonsPerBox; index2++)
                {

                    if (index1 > pokemons.Length - 1 && index2 > pokemons[index1].Length - 1)
                        this.pokemons[index1, index2] = pokemons[index1][index2];
                    else this.pokemons[index1, index2] = null;
                }
            }
        }

        public bool removePokemon(int boxID, int pokemonID)
        {
            if (this[Convert.ToByte(boxID)].pokemons[boxID,pokemonID] == null) return false;
            this.pokemons[boxID,pokemonID] = null;
            return true;
        }
        
        public bool switchPCAndPartyPokemon(Trainer trainer,int partyID,int PCBoxID)
        {
            Pokemon pokemon = trainer.party[partyID];
            if (pokemon == null) return false;
            //TODO: Make this method into static
            trainer.party[partyID] = Pokemons[PCBoxID];
            pokemons[(int) this.ActiveBox, PCBoxID] = pokemon;
            return true;
        }

        public bool addPokemon(int boxID, int pokemonID, Pokemon pokemon)
        {
            if (pokemons[boxID, pokemonID] != null) return false;
            pokemons[boxID, pokemonID] = pokemon;
            return true;
        }
        
        public void swapPokemon(int box1, int poke1, int box2, int poke2)
        {
            Pokemon pokemon = pokemons[box1, poke1];
            pokemons[box1, poke1] = pokemons[box2, poke2];
            pokemons[box2, poke2] = pokemon;
        }

    }
}