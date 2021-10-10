using System;
using System.Collections.Generic;
using GamePlay;
using PokemonCore;
using PokemonCore.Attack.Data;
using PokemonCore.Combat;
using PokemonCore.Monster;
using PokemonCore.Monster.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using Types = PokemonCore.Types;
using Newtonsoft.Json;
using PokemonCore.Inventory;
using PokemonCore.Network;
using PokemonCore.Utility;
using Utility;


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
            if (s_Instance != null) return s_Instance;
            s_Instance = CreateGlobalManager();
            return s_Instance;
        }
    }

    public static bool isBattling;

    private static GlobalManager s_Instance;

    private Game game;

    public ConfigSettings Config;

    #region 存储各种美术资源

    public Dictionary<int, GameObject[]> Pokemons;
    public Dictionary<int, Sprite> PokemonIcons;
    public Dictionary<Item.Tag, Sprite> BagIcons;

    #endregion

    private void OnDestroy()
    {
        game?.OnGameQuit();
    }
    

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
    }

    private void InitGame()
    {
        Game.DataPath = Application.persistentDataPath;

        Config = SaveLoad.Load<ConfigSettings>("Settings.json", Application.persistentDataPath) ?? new ConfigSettings();

        game = new Game();
        var natures = new Dictionary<int, Nature>();
        natures.Add(0, new Nature(0, new float[] {0, 0, 0, 0, 0}));

        Pokemons = new Dictionary<int, GameObject[]>();
        PokemonIcons = new Dictionary<int, Sprite>();

        game.OnDoNotHaveSaveFile += StartPanel;
        
        LoadResources();

        game.OnHaveSaveFile += () =>
        {
            SceneManager.sceneLoaded += (o1, o2) =>
            {
                BagUI.Instance?.Init(Game.bag);
            };

            SceneManager.LoadScene(1);
        };

        
        game.Init(LoadTypes(), null, LoadPokemons(), LoadExperienceTable(), natures, null, LoadMoves(), LoadItems());
        isBattling = false;
    }

    void StartPanel()
    {
        Debug.Log("No Save data found");
        GameObject obj = GameObject.Find("Canvas");
        obj.transform.Find("StartCanvas").gameObject.SetActive(true);
        obj.transform.Find("Start").gameObject.SetActive(false);
    }

    public void CreateNewTrainer(bool isMale)
    {
        InputField inputField = GameObject.Find("NameText").GetComponent<InputField>();
        game.CreateNewSaveFile(inputField.text, isMale);

        StartGame();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
        SceneManager.sceneLoaded += StartBattleFastTest;
    }

    public void StartNetworkBattle(int trainersNum = 2, int pokemonPerTrainer = 1, string password = "")
    {
        // NetworkLogic.StartLocalNetwork();
        NetworkLogic.OnStartBattle = StartBattle;
        NetworkLogic.PairOnBattle(trainersNum, pokemonPerTrainer, password);
    }

    public void StopPairNetworkBattle()
    {
        NetworkLogic.PairOff();
    }

    public void StartFastNetworkBattle()
    {
        SceneManager.LoadScene(1);
        SceneManager.sceneLoaded += (a, b) => { StartNetworkBattle(); };
    }

    void StartBattleFastTest(Scene scene, LoadSceneMode mode)
    {
        Game.trainer.party[0] = new Pokemon(Game.PokemonsData[4], "", Game.trainer, 50, 0);
        Game.trainer.party[1] = new Pokemon(Game.PokemonsData[1], "", Game.trainer, 50, 0);
        Trainer trainer = new Trainer("Computer", false);
        trainer.party[0] = new Pokemon(Game.PokemonsData[17], "", trainer, 50, 0);
        trainer.party[1] = new Pokemon(Game.PokemonsData[7], "", trainer, 50, 0);
        List<Trainer> trainers = new List<Trainer>();
        trainers.Add(trainer);
        game.SaveData();
        StartBattle(null, trainers, true, 2);
    }

    public void StartBattle(List<Trainer> allies, List<Trainer> oppo, bool isHost, int pokemonPerTrainer = 1)
    {
        StartBattle(allies, oppo, oppo, null, isHost, pokemonPerTrainer);
    }

    /// <summary>
    /// 适用于野外对战
    /// </summary>
    /// <param name="pokemon">野外对战遇到的仅有的一只宝可梦</param>
    public void StartBattle(Pokemon pokemon)
    {
        Trainer trainer = new Trainer(pokemon.Name, pokemon.isMale);
        trainer.party[0] = pokemon;
        pokemon.TrainerID = trainer.id;
        StartBattle(null, new List<Trainer>(new[] {trainer}), true);
    }

    public void StartBattle(List<Trainer> allies, List<Trainer> oppos, List<Trainer> AI, List<Trainer> userInternet,
        bool isHost,
        int pokemonPerTrainer)
    {
        isBattling = true;
        game.StartBattle(allies, oppos, AI, userInternet, isHost, pokemonPerTrainer);
        Game.battle.OnBattleEnd += (o) =>
        {
            isBattling = false;
            print("Global: Battle END;");
            BattleHandler.Instance.EndBattle();
        };
        BattleHandler.Instance.StartBattle(Game.battle);
    }

    private void OnApplicationQuit()
    {
        NetworkLogic.PairOff();
    }


    public void Update()
    {
        EventPool.Tick();
    }

    #region LoadDataToDictionary

    public Dictionary<int, int[]> LoadExperienceTable()
    {
        List<int[]> tmp =
            JsonConvert.DeserializeObject<List<int[]>>(Resources.Load<TextAsset>("PokemonData/" + Game.ExpTableFile)
                .text);
        var ExperienceTable = new Dictionary<int, int[]>();
        for (int i = 0; i < tmp.Count; i++)
        {
            ExperienceTable.Add(i, tmp[i]);
        }

        return ExperienceTable;
    }

    public Dictionary<int, Types> LoadTypes()
    {
        List<Types> tmp =
            JsonConvert.DeserializeObject<List<Types>>(Resources.Load<TextAsset>("PokemonData/" + Game.TypeFile).text);
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

    public Dictionary<int, MoveData> LoadMoves()
    {
        List<MoveData> tmp =
            JsonConvert.DeserializeObject<List<MoveData>>(
                Resources.Load<TextAsset>("PokemonData/" + Game.MoveFile).text);
        var MovesData = new Dictionary<int, MoveData>();
        foreach (var t in tmp)
        {
            MovesData.Add(t.MoveID, t);
        }

        return MovesData;
    }

    public Dictionary<ValueTuple<Item.Tag, int>, Item> LoadItems()
    {
        string str = Resources.Load<TextAsset>("PokemonData/" + Game.ItemFile).text;
        Dictionary<Item.Tag, List<Item>> items =
            JsonConvert.DeserializeObject<Dictionary<Item.Tag, List<Item>>>(str);
        var itemData = new Dictionary<ValueTuple<Item.Tag, int>, Item>();
        foreach (var item in items)
        {
            var tag = item.Key;
            var list = item.Value;
            foreach (var i in list.OrEmptyIfNull())
            {
                int id = i.ID;
                itemData.Add((tag, id), i);
            }
        }

        return itemData;
    }


    public Dictionary<int, PokemonData> LoadPokemons()
    {
        List<PokemonData> tmp =
            JsonConvert.DeserializeObject<List<PokemonData>>(Resources
                .Load<TextAsset>("PokemonData/" + Game.PokemonFile).text);
        var PokemonsData = new Dictionary<int, PokemonData>();


        foreach (var t in tmp)
        {
            PokemonsData.Add(t.ID, t);


            GameObject obj = Resources.Load<GameObject>("Prefabs/Pokemons/" + t.ID + t.innerName);
            if (obj == null)
            {
                obj = Resources.Load<GameObject>("Prefabs/Pokemons/" + t.ID + t.innerName + "M");
                if (obj == null) throw new Exception($"Pokemon Prefabs Not Found:{t.ID},{t.innerName}");
                var obj2 = Resources.Load<GameObject>("Prefabs/Pokemons/" + t.ID + t.innerName + "F");
                Pokemons.Add(t.ID, new[] {obj, obj2});
            }
            else
            {
                Pokemons.Add(t.ID, new[] {obj});
            }


            PokemonIcons.Add(t.ID, Resources.Load<Sprite>("Sprites/PokemonIcons/" + t.ID + t.innerName));
        }

        return PokemonsData;
    }

    public void LoadResources()
    {
        BagIcons = new Dictionary<Item.Tag, Sprite>();
        string[] strs = Enum.GetNames(typeof(Item.Tag));
        foreach (var str in strs)
        {
            Sprite spr = Resources.Load<Sprite>("Sprites/BagIcons/"+str);
            BagIcons.Add((Item.Tag)Enum.Parse(typeof(Item.Tag),str),spr);
        }
    }

    #endregion
}