using System;
using System.Collections.Generic;
using PokemonCore.Character;
using PokemonCore.Combat;
using PokemonCore.Inventory;
using PokemonCore.Monster;
using PokemonCore.Utility;

namespace PokemonCore.Saving
{
    [Serializable]
    public struct GameState:IEquatable<GameState>,IEqualityComparer<GameState>
    {
        // public DateTime TimeCreated { get; private set; }
        public Trainer Trainer { get; private set; }
        public PC PlayerPC { get; private set; }
        // public int[] PlayerBag { get; private set; }
//TODO：Player Bag,Time Created
        public GameState(
            Trainer trainer,
            PC pc
            )
        {
            
            Trainer = trainer;
            PlayerPC = pc;
        }
        public bool Equals(GameState obj)
        {
            return Equals(obj);
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (typeof(Game) == obj.GetType()) return this.Equals(obj);
            if (typeof(GameState) == obj.GetType()) return this.Equals(obj);
            return base.Equals(obj);
        }

        
        public bool Equals(GameState x, GameState y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(GameState obj)
        {
            return Trainer.id.GetHashCode();
        }
    }
}