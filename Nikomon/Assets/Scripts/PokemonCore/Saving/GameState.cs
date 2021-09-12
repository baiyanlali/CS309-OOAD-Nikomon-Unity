using System;
using System.Collections.Generic;
using PokemonCore.Character;
using PokemonCore.Combat;
using PokemonCore.Inventory;
using PokemonCore.Monster;
using PokemonCore.Utility;

namespace PokemonCore.Saving
{
    public struct GameState:IEquatable<GameState>,IEqualityComparer<GameState>
    {
        public DateTime TimeCreated { get; private set; }
        public string PlayerName { get; private set; }
        public int TrainerID { get; private set; }
        public int SecretID { get; private set; }
        public bool IsMale { get; private set; }
        public TimeSpan PlayTime { get; private set; }
        public byte[][] Pokedex { get; private set; }
        public int PlayerMoney { get; private set; }
        public int PlayerSavings { get; private set; }
        public int PokeCenterID { get; private set; }
        public int ActiveMapID { get; private set; }
        public Vector PlayerPosition { get; private set; }
        public byte FollowerPokemonID { get; private set; }
        public IPokemon PokemonParty { get; private set; }
        public PC PlayerPC { get; private set; }
        public int[] PlayerBag { get; private set; }//存储每个item的id！
//TODO：Not Finish yet!
        // public GameState(
        //     Trainer trainer,
        //     PC pc
        //     )
        // {
        //     PlayerPC = pc;
        // }
        public bool Equals(GameState other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(GameState x, GameState y)
        {
            throw new NotImplementedException();
        }

        public int GetHashCode(GameState obj)
        {
            throw new NotImplementedException();
        }
    }
}