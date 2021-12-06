using System;
using Newtonsoft.Json;
using PokemonCore.Combat;

namespace PokemonCore.Character
{
    [Serializable]
    public class PC
    {
        [JsonProperty]
        private int maxPokemonsPerBox, maxBox;

        [JsonProperty]
        public Pokemon[][] pokemons;
        

        [JsonIgnore]
        public Pokemon[] Pokemons
        {
            get
            {
                return pokemons[(int) this.ActiveBox];
            }
        }

        [JsonProperty]
        public int ActiveBox { get;private set; }

        public void ChangeActiveBox(int offset)
        {
            ActiveBox += offset;
            ActiveBox %= maxBox;
            if (ActiveBox < 0)
                ActiveBox += maxBox;

        }

        public void SetActiveBox(int index)
        {
            if (index >= maxBox || index < 0) return;
            ActiveBox = index;
        }
        
        [JsonProperty]
        public string[] BoxNames { get; private set; }
        


        [JsonConstructor]
        public PC(int maxPokemonsPerBox,int maxBox,Pokemon[][] pokemons,int activeBox,string[] boxNames)
        {
            this.maxPokemonsPerBox = maxPokemonsPerBox;
            this.maxBox = maxBox;
            this.pokemons = pokemons;
            this.ActiveBox = activeBox;
            this.BoxNames = boxNames;
        }
        
        public PC(int maxPokemonsPerBox = 20, int maxBox = 8)
        {
            this.maxPokemonsPerBox = maxPokemonsPerBox;
            this.maxBox = maxBox;
            this.pokemons = new Pokemon[maxBox][];
            ActiveBox = 0;
            for (int i = 0; i < this.pokemons.Length; i++)
            {
                this.pokemons[i] = new Pokemon[maxPokemonsPerBox];
            }
            this.BoxNames = new string[maxBox];
            for (int index1 = 0; index1 < maxBox; index1++)
            {
                
                for (int index2 = 0; index2 < maxPokemonsPerBox; index2++)
                {
                    this.pokemons[index1][index2] = null;
                }

                BoxNames[index1] = $"Box {index1 + 1}";
            }
        }

        public PC(
            Pokemon[][] pokemons,
            byte? box = null,
            string[] names = null) : this()
        {
            if (names != null)
                BoxNames = names;
            if (box.HasValue)
                this.ActiveBox = (byte) ((uint) box.Value % maxBox);
            if (pokemons != null) return;
            this.pokemons = pokemons;
        }

        public bool RemovePokemon(int boxID, int pokemonID)
        {
            if (pokemons[boxID][pokemonID] == null) return false;
            this.pokemons[boxID][pokemonID] = null;
            return true;
        }
        
        public bool SwitchPCAndPartyPokemon(Trainer trainer,int partyID,int PCBoxID)
        {
            Pokemon pokemon = trainer.party[partyID];
            if (pokemon == null) return false;
            //TODO: Make this method into static
            trainer.party[partyID] = Pokemons[PCBoxID];
            pokemons[(int) this.ActiveBox][ PCBoxID] = pokemon;
            return true;
        }
        
        public bool SwitchPartyAndPCPokemon(Trainer trainer,int partyID,int PCnum)
        {
            Pokemon pokemon = trainer.party[partyID];
            if (pokemon == null) return false;
            //TODO: Make this method into static
            trainer.party[partyID] = pokemons[PCnum / Pokemons.Length][PCnum % Pokemons.Length];
            pokemons[PCnum / Pokemons.Length][PCnum % Pokemons.Length] = pokemon;
            return true;
        }

        public bool AddPokemon(int boxID, int pokemonID, Pokemon pokemon)
        {
            if (pokemons[boxID][ pokemonID] != null) return false;
            pokemons[boxID][ pokemonID] = pokemon;
            return true;
        }
        
        public void SwapPokemon(int box1, int poke1, int box2, int poke2)
        {
            (pokemons[box1][ poke1], pokemons[box2][ poke2]) = (pokemons[box2][ poke2], pokemons[box1][ poke1]);
        }

        public bool AddPokemon(Pokemon pokemon)
        {
            int box,index;
            (box, index) = FindNextSuitableSlot();
            UnityEngine.Debug.Log((box,index));
            if (box == -1 || index == -1)
            {
                return false;
            }

            AddPokemon(box, index, pokemon);
            return true;
        }
        
        //box index, in box index
        public ValueTuple<int, int> FindNextSuitableSlot()
        {
            int currentIndex = ActiveBox;
            int result = -1;
            while (true)
            {
                result = FindSuitableSlotInBox(currentIndex);
                
                if (result >= 0)
                {
                    return (currentIndex, result);
                }
                
                
                currentIndex += 1;
                currentIndex %= maxBox;
                
                if (currentIndex == ActiveBox)
                {
                    return (-1, -1);
                }
            }
        }

        private int FindSuitableSlotInBox(int boxIndex)
        {
            if (boxIndex >= maxBox||boxIndex<0)
            {
                return -2;
            }
            Pokemon[] pokes = pokemons[boxIndex];
            for (int i = 0; i < maxPokemonsPerBox; i++)
            {
                if (pokes[i] == null) return i;
            }

            return -1;
        }
    }
}