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
using UnityEngine;
using XLua;
using Random = System.Random;


namespace PokemonCore
{
    /// <summary>
    /// 整个宝可梦逻辑的类
    /// </summary>
    public class Game
    {

        public const float VERSOIN = 0.1f;
        
        #region DataPath

        public static string DataPath { get; set; }

        public static readonly int MaxMovesPerPokemon = 4;
        public static readonly int MaxPartyNums = 6;

        public static readonly string TypeFile = "types";
        public static readonly string MoveFile = "moves";
        public static readonly string PokemonFile = "pokemons";
        public static readonly string ExpTableFile = "levelingRate";
        public static readonly string SaveFile = "Save";
        public static readonly string ItemFile = "items";

        #endregion


        public static Dictionary<int, Types> TypesMap { get; private set; }

        public static Dictionary<int, Ability> AbilitiesData { get; private set; }
        public static Dictionary<int, PokemonData> PokemonsData { get; private set; }
        public static Dictionary<int, int[]> ExperienceTable { get; private set; }
        public static Dictionary<int, Nature> NatureData { get; private set; }

        public static Dictionary<int, List<string>> EffectsData { get; private set; }

        public static Dictionary<int, MoveData> MovesData { get; private set; }

        public static Dictionary<ValueTuple<Item.Tag,int>, Item> ItemsData { get; private set; }


        public static Trainer trainer;

        public static TrainerBag bag;

        public static PC pc;

        public static Random Random;

        public static LuaEnv LuaEnv;

        private LoadDataType loadDataType { get; set; }

        #region 触发事件

        public Action OnDoNotHaveSaveFile;
        public Action OnHaveSaveFile;

        public Action OnGameQuit= () =>
        {
            if (LuaEnv!=null)
            {
                // LuaEnv.Dispose();
            }
        };

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

            // Init();
        }

        //TODO:目前只存档Trainer信息，剩下的以后再说
        // public bool HaveSave => SaveLoad.Load<GameState>(SaveFile) != null;

        public void SaveData(int slot)
        {
            SaveLoad.Save(string.Concat(SaveFile,slot), GetSave);
        }


        public GameState GetSave
        {
            get
            {
                var state=new GameState(VERSOIN,DateTime.Now,trainer,pc,bag);
                return state;
            }
        }
        


        public void Init(
            Dictionary<int, Types> typesMap, 
            Dictionary<int, Ability> abilities,
            Dictionary<int, PokemonData> pokemonDatas, 
            Dictionary<int, int[]> experienceTable,
            Dictionary<int, Nature> natures,
            Dictionary<int, List<string>> effectsData, 
            Dictionary<int, MoveData> moveDatas,
            Dictionary<ValueTuple<Item.Tag,int>, Item> items)
        {
            Random = new Random();
            TypesMap = typesMap;
            MovesData = moveDatas;
            PokemonsData = pokemonDatas;
            ExperienceTable = experienceTable;

            AbilitiesData = abilities;

            NatureData = natures;

            EffectsData = effectsData;

            ItemsData = items;

            // bag = new TrainerBag();
            // bag.Add(items[(Item.Tag.PokeBalls,0)]);

            // if (HaveSave)
            // {
            //     // trainer = SaveLoad.Load<Trainer>(SaveFile);
            //     // OnHaveSaveFile?.Invoke();
            //     
            //     GameState state = SaveLoad.Load<GameState>(SaveFile);
            //     if (state.VERSION < VERSOIN)
            //     {
            //         UnityEngine.Debug.LogWarning("GameCore version level not equal, may cause some problem");
            //     }
            //     trainer = state.Trainer;
            //     pc = state.PlayerPC;
            //     bag = state.TrainerBag;
            //     
            // }
            // else
            //     OnDoNotHaveSaveFile?.Invoke();

            LuaEnv = new LuaEnv();
            LuaEnv.DoString("require 'Effect'");
            // NatureData = new Dictionary<int, Nature>();
            // NatureData.Add(0, new Nature(0, new float[] {0, 0, 0, 0, 0}));
            // if (HaveSave)
            //     trainer = SaveLoad.Load<Trainer>(SaveFile);
            // else
            //     OnDoNotHaveSaveFile?.Invoke();

            // PC pc = new PC();
        }

        //TODO: for test
        public void CreateNewSaveFile(string name, bool isMale)
        {
            trainer = new Trainer(name, isMale);
            AddPokemon(new Pokemon(1,50));
            bag = new TrainerBag();
            bag.Add(ItemsData[(Item.Tag.PokeBalls,0)]);
            pc = new PC();
            // SaveData(slot);
        }

        // public void LoadSaveFile(int slot)
        // {
        //     // GameState state = SaveLoad.Load<GameState>(string.Concat(SaveFile, slot));
        //     // LoadSaveFile(state);
        // }

        public void LoadSaveFile(GameState gameState)
        {
            trainer = gameState.Trainer;
            bag = gameState.TrainerBag.ToTrainerBag();
            pc = gameState.PlayerPC;
        }

        public static Battle battle { get; private set; }
        public static BattleReporter battleReporter { get; private set; }


        public void AddPokemon(Pokemon poke)
        {
            if (!trainer.isPartyFull)
            {
                trainer.AddPokemon(poke);
            }
            else
            {
                pc.AddPokemon(poke);
            }

            trainer.PokemonCountered.Add(poke.ID);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="allies">这里的ally是没有trainer的！</param>
        /// <param name="opponent"></param>
        /// <param name="AI"></param>
        /// <param name="UserInternet"></param>
        /// <param name="isHost"></param>
        /// <param name="pokemonPerTrainer"></param>
        public void StartBattle(List<Trainer> allies, List<Trainer> opponent, List<Trainer> AI,
            List<Trainer> UserInternet = null, bool isHost = true, int pokemonPerTrainer = 1)
        {
            battle = new Battle(isHost);
            if (allies == null) allies = new List<Trainer>();
            allies.Add(trainer);
            List<Pokemon> alliesPoke = null;
            List<Pokemon> oppoPoke = null;
            if (pokemonPerTrainer == 1)
            {
                alliesPoke = (from pokea in allies select pokea.firstAbleParty).ToList();
                oppoPoke = (from pokea in opponent select pokea.firstAbleParty).ToList();
            }
            else if (pokemonPerTrainer > 1)
            {
                alliesPoke = new List<Pokemon>();
                oppoPoke = new List<Pokemon>();
                for (int i = 0; i < pokemonPerTrainer; i++)
                {
                    alliesPoke.AddRange(from pokea in allies select pokea.party[i]);
                    oppoPoke.AddRange(from pokea in opponent select pokea.party[i]);
                }
            }

            battle.OnBattleEnd += (o) =>
            {
                UnityEngine.Debug.Log($"Battle Results: {o}");
                battle = null;
                Random = new Random();
            };
            battle.StartBattle(alliesPoke, oppoPoke, allies, opponent);
            BattleAI ai;
            if (AI != null)
                ai = new BattleAI(battle, AI);
            battleReporter = new BattleReporter(battle);
            
        }

        public void CompleteBattleInit()
        {
            battle.CompleteInit();
        }

        #region LoadDataToDictionary

        // public void LoadExperienceTable(LoadDataType type = LoadDataType.Json)
        // {
        //     List<int[]> tmp = SaveLoad.Load<List<int[]>>(ExpTableFile);
        //     ExperienceTable = new Dictionary<int, int[]>();
        //     for (int i = 0; i < tmp.Count; i++)
        //     {
        //         ExperienceTable.Add(i, tmp[i]);
        //     }
        // }
        //
        // public void LoadTypes(LoadDataType type = LoadDataType.Json)
        // {
        //     List<Types> tmp = SaveLoad.Load<List<Types>>(TypeFile);
        //     TypesMap = new Dictionary<int, Types>();
        //     foreach (var t in tmp)
        //     {
        //         TypesMap.Add(t.ID, t);
        //     }
        // }
        //
        // public void LoadAbilities(LoadDataType type = LoadDataType.Json)
        // {
        //     if (type == LoadDataType.Json)
        //     {
        //         // using FileStream openStream = File.OpenRead(DataPath+"Abilities.json");
        //         // AbilitiesData = await JsonSerializer.DeserializeAsync<Dictionary<int,Ability>>(openStream);
        //     }
        // }
        //
        // public void LoadNature(LoadDataType type = LoadDataType.Json)
        // {
        //     if (type == LoadDataType.Json)
        //     {
        //     }
        // }
        //
        // public void LoadEffect(LoadDataType type = LoadDataType.Json)
        // {
        //     if (type == LoadDataType.Json)
        //     {
        //     }
        // }
        //
        // public void LoadMoves(LoadDataType type = LoadDataType.Json)
        // {
        //     List<MoveData> tmp = SaveLoad.Load<List<MoveData>>(MoveFile);
        //     MovesData = new Dictionary<int, MoveData>();
        //     foreach (var t in tmp)
        //     {
        //         MovesData.Add(t.MoveID, t);
        //     }
        // }
        //
        // public void LoadItems(LoadDataType type = LoadDataType.Json)
        // {
        //     if (type == LoadDataType.Json)
        //     {
        //     }
        // }
        //
        //
        // public void LoadPokemons(LoadDataType type = LoadDataType.Json)
        // {
        //     List<PokemonData> tmp = SaveLoad.Load<List<PokemonData>>(PokemonFile);
        //     PokemonsData = new Dictionary<int, PokemonData>();
        //     foreach (var t in tmp)
        //     {
        //         PokemonsData.Add(t.ID, t);
        //     }
        // }

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