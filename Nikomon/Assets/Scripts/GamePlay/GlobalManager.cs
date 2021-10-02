using System;
using System.Collections.Generic;
using PokemonCore;
using PokemonCore.Attack.Data;
using PokemonCore.Combat;
using PokemonCore.Monster;
using PokemonCore.Monster.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Windows;
using Debug = UnityEngine.Debug;
using Types = PokemonCore.Types;

using Newtonsoft.Json;


/// <summary>
/// 负责与PokemonCore交互，所有unity相关的对象都通过它与Pokemon Core交互
/// </summary>
public class GlobalManager : MonoBehaviour
{
    public static GlobalManager Instance
    {
        get
        {
            if (s_Instance != null) return s_Instance;
            s_Instance = FindObjectOfType<GlobalManager>();
            if(s_Instance!=null) return s_Instance;
            s_Instance = CreateGlobalManager();
            return s_Instance;
        }
    }

    private static GlobalManager s_Instance;
    
    private Game game;

    #region 存储各种美术资源

    public Dictionary<int, GameObject> Pokemons;

    #endregion

    private static GlobalManager CreateGlobalManager()
    {
        GameObject obj = GameObject.Find("Global");
        if (obj == null)
        {
            obj = new GameObject("Global");
        }
        
        GlobalManager gm = obj.AddComponent<GlobalManager>();
        return gm;
        
    }
    
    private void Awake()
    {
        if (s_Instance != null && s_Instance != this)
        {
            Destroy(this.gameObject);
        }
        s_Instance = this;


        DontDestroyOnLoad(this);
        InitGame();
        // Game.DataPath = Application.dataPath+@"/Data/";
        // print(Game.DataPath);
        // game = new Game();
        // game.OnDoNotHaveSaveFile +=StartPanel;
        // game.Init();
    }

    private void InitGame()
    {
        Game.DataPath = Application.persistentDataPath;

        game = new Game();
        // var types = JsonConvert.DeserializeObject<Dictionary<int, Types>>(Resources.Load<TextAsset>("PokemonData/" + Game.TypeFile)
        //     .text);
        // var moves = JsonConvert.DeserializeObject<Dictionary<int,MoveData>>(Resources.Load<TextAsset>("PokemonData/" + Game.MoveFile).text);
        // var pokes = JsonConvert.DeserializeObject<Dictionary<int,PokemonData>>(Resources.Load<TextAsset>("PokemonData/" + Game.PokemonsData).text);
        // var exps = JsonConvert.DeserializeObject<Dictionary<int,int[]>>(Resources.Load<TextAsset>("PokemonData/" + Game.ExpTableFile).text);
        var natures = new Dictionary<int, Nature>();
        natures.Add(0, new Nature(0, new float[] {0, 0, 0, 0, 0}));

        game.OnDoNotHaveSaveFile +=StartPanel;
        game.Init(LoadTypes(),null,LoadPokemons(),LoadExperienceTable(),natures,null,LoadMoves(),null);
    }

    void StartPanel()
    {
        Debug.Log("No Save data found");
        GameObject obj=GameObject.Find("Canvas");
        obj.transform.Find("StartCanvas").gameObject.SetActive(true);
        obj.transform.Find("Start").gameObject.SetActive(false);
    }

    public void CreateNewTrainer(bool isMale)
    {
        InputField text = GameObject.Find("NameText").GetComponent<InputField>();
        game.CreateNewSaveFile(text.text,isMale);
        
        StartGame();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
        SceneManager.sceneLoaded += OnSceneLoaded;
        
    }

    //TODO：目前无法实现主角控制双打情况！
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Game.trainer.party[0]=new Pokemon(Game.PokemonsData[4], "",Game.trainer, 50, 0);
        Game.trainer.party[1]=new Pokemon(Game.PokemonsData[1], "",Game.trainer, 50, 0);
        Trainer trainer = new Trainer("Computer",false);
        trainer.party[0] = new Pokemon(Game.PokemonsData[17], "",trainer, 50, 0);
        trainer.party[1] = new Pokemon(Game.PokemonsData[7], "",trainer, 50, 0);
        List<Trainer> trainers = new List<Trainer>();
        trainers.Add(trainer);
        StartBattle(null,trainers,true,2);
    }

    public void StartBattle(List<Trainer> allies,List<Trainer> oppo,bool isHost,int pokemonPerTrainer=1)
    {
        game.StartBattle(allies,oppo,oppo,null,isHost,pokemonPerTrainer);
        BattleHandler.Instance.StartBattle(Game.battle);
    }

    public void StartBattle(Trainer oppo)
    {
        List<Trainer> trainers = new List<Trainer>();
        trainers.Add(oppo);
        StartBattle(null,trainers,true);
    }
    
    
    
    
    #region LoadDataToDictionary

        public Dictionary<int,int[]> LoadExperienceTable()
        {
            List<int[]> tmp = JsonConvert.DeserializeObject<List<int[]>>(Resources.Load<TextAsset>("PokemonData/"+Game.ExpTableFile).text);
            var ExperienceTable = new Dictionary<int, int[]>();
            for (int i = 0; i < tmp.Count; i++)
            {
                ExperienceTable.Add(i, tmp[i]);
            }

            return ExperienceTable;
        }

        public Dictionary<int,Types> LoadTypes()
        {
            List<Types> tmp = JsonConvert.DeserializeObject<List<Types>>(Resources.Load<TextAsset>("PokemonData/"+Game.TypeFile).text);
            var TypesMap = new Dictionary<int, Types>();
            foreach (var t in tmp)
            {
                TypesMap.Add(t.ID, t);
            }

            return TypesMap;
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

        public Dictionary<int,MoveData> LoadMoves()
        {
            List<MoveData> tmp = JsonConvert.DeserializeObject<List<MoveData>>(Resources.Load<TextAsset>("PokemonData/"+Game.MoveFile).text );
            var MovesData = new Dictionary<int, MoveData>();
            foreach (var t in tmp)
            {
                MovesData.Add(t.MoveID, t);
            }

            return MovesData;
        }

        public void LoadItems(LoadDataType type = LoadDataType.Json)
        {
            if (type == LoadDataType.Json)
            {
            }
        }


        public Dictionary<int,PokemonData> LoadPokemons()
        {
            List<PokemonData> tmp =JsonConvert.DeserializeObject<List<PokemonData>>(Resources.Load<TextAsset>("PokemonData/"+Game.PokemonFile).text);
            var PokemonsData = new Dictionary<int, PokemonData>();
            foreach (var t in tmp)
            {
                PokemonsData.Add(t.ID, t);
            }

            return PokemonsData;
        }

        #endregion
    
    
}