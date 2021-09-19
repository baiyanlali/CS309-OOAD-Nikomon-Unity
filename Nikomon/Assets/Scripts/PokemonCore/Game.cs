﻿using System.Collections.Generic;
using System.IO;
using PokemonCore.Attack;
using PokemonCore.Attack.Data;
using PokemonCore.Combat;
using PokemonCore.Combat.Interface;
using PokemonCore.Inventory;
using PokemonCore.Monster;
using PokemonCore.Monster.Data;
using PokemonCore.Utility;

// using System.Text.Json;
// using System.Text.Json.Serialization;
// using Newtonsoft.Json;

namespace PokemonCore
{
    public class Game
    {
        #region DataPath

        public static string DataPath => "Assets//Data//";

        public static readonly string TypeFile = "types.json";
        public static readonly string MoveFile = "moves.json";
        public static readonly string PokemonFile = "pokemons.json";

        #endregion
        

        public static Dictionary<int,Types> TypesMap { get; private set; }
        
        public static Dictionary<int,Ability> AbilitiesData { get; private set; }
        public static Dictionary<int,PokemonData> PokemonsData { get; private set; }
        public static Dictionary<int,int[]> ExperienceTable { get; private set; }
        public static Dictionary<int,Nature> NatureData { get; private set; }
        
        public static Dictionary<int,List<IEffect>> EffectsData { get; private set; }
        
        public static Dictionary<int,MoveData> MovesData { get; private set; }
        
        public static Dictionary<int,Item> ItemsData{get;private set;}
        

        public Trainer trainer;

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
        }

        #region LoadDataToDictionary

        

        public void LoadExperienceTable(LoadDataType type = LoadDataType.Json)
        {
            if (type == LoadDataType.Json)
            {
                
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