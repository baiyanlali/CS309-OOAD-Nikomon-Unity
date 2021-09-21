using System;
using System.Collections.Generic;
using PokemonCore.Attack.Data;
using PokemonCore.Combat;
using PokemonCore.Combat.Interface;
using PokemonCore.Inventory;
using PokemonCore.Monster;
using PokemonCore.Monster.Data;
using PokemonCore.Utility;


namespace PokemonCore
{
    public class Game
    {
        #region DataPath

        public static string DataPath => "Assets//Data//";

        public static readonly int MaxMovesPerPokemon = 4;
        public static readonly int MaxPartyNums = 6;

        public static readonly string TypeFile = "types.json";
        public static readonly string MoveFile = "moves.json";
        public static readonly string PokemonFile = "pokemons.json";
        public static readonly string ExpTableFile = "levelingRate.json";

        #endregion
        

        public static Dictionary<int,Types> TypesMap { get; private set; }
        
        public static Dictionary<int,Ability> AbilitiesData { get; private set; }
        public static Dictionary<int,PokemonData> PokemonsData { get; private set; }
        public static Dictionary<int,int[]> ExperienceTable { get; private set; }
        public static Dictionary<int,Nature> NatureData { get; private set; }
        
        public static Dictionary<int,List<IEffect>> EffectsData { get; private set; }
        
        public static Dictionary<int,MoveData> MovesData { get; private set; }
        
        public static Dictionary<int,Item> ItemsData{get;private set;}
        

        public static Trainer trainer;

        public static Random Random;

        private LoadDataType loadDataType { get; set; }
        
        
        public static Game Instance
        {
            get { return sInstance; }
        }
        private static Game sInstance;

        public Game()
        {
            if (sInstance == null)
                sInstance = this;

            Init();
        }

        public void Init()
        {
            LoadTypes();
            LoadMoves();
            LoadPokemons();
            LoadExperienceTable();
            NatureData = new Dictionary<int, Nature>();
            NatureData.Add(0,new Nature(0,new float[]{0,0,0,0,0}));
        }

        #region LoadDataToDictionary

        

        public void LoadExperienceTable(LoadDataType type = LoadDataType.Json)
        {
            List<int[]> tmp=SaveLoad.Load<List<int[]>>(ExpTableFile);
            ExperienceTable = new Dictionary<int, int[]>();
            for (int i = 0; i < tmp.Count; i++)
            {
                ExperienceTable.Add(i,tmp[i]);
            }
        }

        public void LoadTypes(LoadDataType type = LoadDataType.Json)
        {
            List<Types> tmp = SaveLoad.Load<List<Types>>(TypeFile);
            TypesMap = new Dictionary<int, Types>();
            foreach (var t in tmp)
            {
                TypesMap.Add(t.ID,t);
            }
        }
        
        public void LoadAbilities(LoadDataType type = LoadDataType.Json)
        {
            if (type == LoadDataType.Json)
            {
                // using FileStream openStream = File.OpenRead(DataPath+"Abilities.json");
                // AbilitiesData = await JsonSerializer.DeserializeAsync<Dictionary<int,Ability>>(openStream);
            }
        }
        public void LoadNature(LoadDataType type = LoadDataType.Json)
        {
            if (type == LoadDataType.Json)
            {
                
            }
        }
        public void LoadEffect(LoadDataType type = LoadDataType.Json)
        {
            if (type == LoadDataType.Json)
            {
                
            }
        }
        
        public void LoadMoves(LoadDataType type = LoadDataType.Json)
        {
            List<MoveData> tmp = SaveLoad.Load<List<MoveData>>(MoveFile);
            MovesData = new Dictionary<int, MoveData>();
            foreach (var t in tmp)
            {
                MovesData.Add(t.MoveID,t);
            }
        }
        public void LoadItems(LoadDataType type = LoadDataType.Json)
        {
            if (type == LoadDataType.Json)
            {
                
            }
        }
        

        public void LoadPokemons(LoadDataType type=LoadDataType.Json)
        {
            List<PokemonData> tmp = SaveLoad.Load<List<PokemonData>>( PokemonFile);
            PokemonsData = new Dictionary<int, PokemonData>();
            foreach (var t in tmp)
            {
                PokemonsData.Add(t.ID,t);
            }
        }
        
        #endregion

        
        
    }

    public enum LoadDataType
     {
         Json,
         CSV,
         XML,
         Database
     }
}