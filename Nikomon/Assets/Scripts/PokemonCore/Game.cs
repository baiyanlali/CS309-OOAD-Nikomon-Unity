using System;
using System.Collections.Generic;
using System.Linq;
using PokemonCore.Attack.Data;
using PokemonCore.Character;
using PokemonCore.Combat;
using PokemonCore.Combat.Interface;
using PokemonCore.Inventory;
using PokemonCore.Monster;
using PokemonCore.Monster.Data;
using PokemonCore.Saving;
using PokemonCore.Utility;


namespace PokemonCore
{
    /// <summary>
    /// 整个宝可梦逻辑的类
    /// </summary>
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
        public static readonly string SaveFile = "Save.json";

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

        #region 触发事件

        public Action OnDoNotHaveSaveFile;

        #endregion
        
        
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

        //TODO:目前只存档Trainer信息，剩下的以后再说
        public bool HaveSave => SaveLoad.Load<Trainer>(SaveFile) != null;

        public void SaveData()
        {
            SaveLoad.Save(SaveFile, trainer);
        }

        public void Init()
        {
            Random = new Random();
            LoadTypes();
            LoadMoves();
            LoadPokemons();
            LoadExperienceTable();
            NatureData = new Dictionary<int, Nature>();
            NatureData.Add(0,new Nature(0,new float[]{0,0,0,0,0}));
            if (HaveSave)
                trainer = SaveLoad.Load<Trainer>(SaveFile);
            else
                OnDoNotHaveSaveFile?.Invoke();

            // PC pc = new PC();
        }

        public void CreateNewSaveFile(string name,bool isMale)
        {
            trainer = new Trainer(name,isMale);
            SaveData();
        }

        public static Battle battle { get; private set; }
        public static BattleReporter battleReporter { get; private set; }
        public void StartBattle(List<Trainer> allies,List<Trainer> opponent, bool isHost=true)
        {
            battle = new Battle(isHost);
            if (allies == null) allies = new List<Trainer>();
            allies.Add(trainer);
            var alliesPoke = (from pokea in allies select pokea.firstParty).ToList();
            var oppoPoke = (from pokea in opponent select pokea.firstParty).ToList();
            battle.StartBattle(alliesPoke, oppoPoke, allies, opponent);
            battleReporter = new BattleReporter(battle);
        }
        
        public void StartBattle(List<Trainer> allies,List<Trainer> opponent,List<Trainer> AI,List<Trainer> UserInternet=null, bool isHost=true,int pokemonPerTrainer=1)
        {
            battle = new Battle(isHost);
            if (allies == null) allies = new List<Trainer>();
            allies.Add(trainer);
            List<Pokemon> alliesPoke =null;
            List<Pokemon> oppoPoke =null;
            if (pokemonPerTrainer == 1)
            {
                alliesPoke = (from pokea in allies select pokea.firstParty).ToList();
                oppoPoke = (from pokea in opponent select pokea.firstParty).ToList();
            }
            else if(pokemonPerTrainer>1)
            {
                alliesPoke = new List<Pokemon>();
                oppoPoke = new List<Pokemon>();
                for (int i = 0; i < pokemonPerTrainer; i++)
                {
                    alliesPoke.AddRange(from pokea in allies select pokea.party[i]);
                    oppoPoke.AddRange(from pokea in opponent select pokea.party[i]);
                }
            }

            BattleAI ai;
            battle.StartBattle(alliesPoke, oppoPoke, allies, opponent);
            if (AI != null)
                ai= new BattleAI(battle,AI);
            battleReporter = new BattleReporter(battle);
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