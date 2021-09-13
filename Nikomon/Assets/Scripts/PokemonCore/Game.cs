using System.Collections.Generic;
using PokemonCore.Monster;
using PokemonCore.Monster.Data;

namespace PokemonCore
{
    public class Game
    {
        public static Dictionary<int,PokemonData> PokemonData { get; private set; }
        public static Dictionary<int,int[]> ExperienceTable { get; private set; }
        public static Dictionary<int,Nature> NatureData { get; private set; }
    }
}