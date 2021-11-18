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
    public class GameState
    {
        public float VERSION ;
        public DateTime TimeModified { get; private set; }
        public Trainer Trainer { get; private set; }
        public PC PlayerPC { get; private set; }
        public TrainerBag TrainerBag { get; private set; }
        // public int[] PlayerBag { get; private set; }
        [JsonConstructor]
        public GameState(
            float version,
            DateTime timeModified,
            Trainer trainer,
            PC pc,
            TrainerBag trainerBag
            )
        {
            this.VERSION = version;
            this.TimeModified = timeModified;
            Trainer = trainer;
            PlayerPC = pc;
            TrainerBag = trainerBag;
        }
       
    }
}