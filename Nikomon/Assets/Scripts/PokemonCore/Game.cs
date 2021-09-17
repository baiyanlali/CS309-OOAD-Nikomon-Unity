using System.Collections.Generic;
using System.IO;
using PokemonCore.Attack;
using PokemonCore.Combat;
using PokemonCore.Combat.Interface;
using PokemonCore.Inventory;
using PokemonCore.Monster;
using PokemonCore.Monster.Data;
// using System.Text.Json;
// using System.Text.Json.Serialization;
// using Newtonsoft.Json;

namespace PokemonCore
{
    public enum LoadDataType
     {
         Json,
         CSV,
         XML,
         Database
     }
    public class Game
    {
        private static string loadDataPath = "..//..//Data//";

        public static Dictionary<int,Types> TypesMap { get; private set; }
        
        public static Dictionary<int,Ability> AbilitiesData { get; private set; }
        public static Dictionary<int,PokemonData> PokemonData { get; private set; }
        public static Dictionary<int,int[]> ExperienceTable { get; private set; }
        public static Dictionary<int,Nature> NatureData { get; private set; }
        
        public static Dictionary<int,List<IEffect>> EffectsData { get; private set; }
        
        public static Dictionary<int,Move> MovesData { get; private set; }
        
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
            

        }

        public void Init()
        {
            
        }

        #region LoadDataToDictionary

        

        public void LoadExperienceTable(LoadDataType type = LoadDataType.Json)
        {
            if (type == LoadDataType.Json)
            {
                
            }
        }

        public async void LoadTypes(LoadDataType type = LoadDataType.Json)
        {
            if (type == LoadDataType.Json)
            {
                using FileStream openStream = File.OpenRead(loadDataPath+"Types.json");
                // TypesMap = await JsonSerializer.DeserializeAsync<Dictionary<int,Types>>(openStream);
            }
        }
        
        public async void LoadAbilities(LoadDataType type = LoadDataType.Json)
        {
            if (type == LoadDataType.Json)
            {
                using FileStream openStream = File.OpenRead(loadDataPath+"Abilities.json");
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
            if (type == LoadDataType.Json)
            {
                
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
            if (type == LoadDataType.Json)
            {
                
            }
        }
        
        #endregion

        
        
    }
}