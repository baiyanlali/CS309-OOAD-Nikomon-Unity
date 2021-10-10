using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PokemonCore.Character;
using PokemonCore.Combat;
using PokemonCore.Inventory;
using PokemonCore.Monster;
using PokemonCore.Utility;

namespace PokemonCore.Saving
{
    [Serializable]
    public struct GameState
    {
        // public DateTime TimeCreated { get; private set; }
        public Trainer Trainer { get; private set; }
        public PC PlayerPC { get; private set; }
        public TrainerBag TrainerBag { get; private set; }
        // public int[] PlayerBag { get; private set; }
//TODO：Player Bag,Time Created
        [JsonConstructor]
        public GameState(
            Trainer trainer,
            PC pc,
            TrainerBag trainerBag
            )
        {
            
            Trainer = trainer;
            PlayerPC = pc;
            TrainerBag = trainerBag;
        }
       
    }
}