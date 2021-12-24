using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GamePlay;
using GamePlay.Core;
using GamePlay.Messages;
using GamePlay.UI.SaveUI;
using GamePlay.UI.UIFramework;
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
using Yarn.Unity;


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

    public Game game;

    public ConfigSettings Config;

    public DialogueRunner DialogueRunner;

    public int CurrentDataSlot { get; private set; }
    
    
    public bool CanPlayerControlled
    {
        get{
            return NicomonInputSystem.Instance.NicomonInput.Player.enabled;
        }
        set
        {
            if(value)NicomonInputSystem.Instance.NicomonInput.Player.Enable();
            else
                NicomonInputSystem.Instance.NicomonInput.Player.Disable();
        }
    }
    

    private void OnDestroy()
    {
        game?.OnGameQuit();
    }


    private static GlobalManager CreateGlobalManager()
    {
        // GameObject obj = GameObject.Find("Global");
        // if (obj == null)
        // {
        //     obj = new GameObject("Global");
        // }
        //
        // GlobalManager gm = obj.AddComponent<GlobalManager>();
        GameObject obj = GameResources.SpawnPrefab(typeof(GlobalManager));

        obj = Instantiate<GameObject>(obj);
        obj.name = nameof(GlobalManager);
        GlobalManager gm = obj.GetComponent<GlobalManager>();
        
        return gm;
    }

    private void Awake()
    {
        // print("Init");
        if (s_Instance != null && s_Instance != this)
        {
            Destroy(this.gameObject);
        }

        s_Instance = this;


        DontDestroyOnLoad(this);
        InitGame();

        Translator.Translate = GameResources.LoadLocalization(Messages.Current_culture);
        GetComponent<LineProviderBehaviour>().textLanguageCode = Messages.Current_culture;
        DialogueRunner = GetComponent<DialogueRunner>();
        DialogueRunner.lineProvider = GetComponent<LineProviderBehaviour>();
        SetUpDiagoueRunnder(DialogueRunner);

        void SetUpDiagoueRunnder(DialogueRunner runner)
        {
            runner.onNodeComplete.AddListener((s) => { visitedNode.Add(s);});
            
            runner.AddFunction("visited",(Func<string,bool>)visited);
            runner.AddCommandHandler("start_battle",(Action<string>)startBattleFromDialogue);
            // runner.AddCommandHandler();

            bool visited(string node)
            {
                // print("Add visited:(node)");
                return visitedNode.Contains(node);
            }

            void startBattleFromDialogue(string trainerName)
            {
                GameObject.Find(trainerName).GetComponent<NPC>()?.StartBattle();
            }
        }
    }

    private HashSet<string> visitedNode =new HashSet<string>();

    private void InitGame()
    {
        Game.DataPath = Application.persistentDataPath;

        Config = SaveLoad.Load<ConfigSettings>("Settings.json", Application.persistentDataPath) ?? new ConfigSettings();

        game = new Game();
        var natures = new Dictionary<int, Nature>();
        natures.Add(0, new Nature(0, new float[] {0, 0, 0, 0, 0}));

        GameResources.Pokemons = new Dictionary<int, GameObject[]>();
        GameResources.PokemonIcons = new Dictionary<int, Sprite>();

        // game.OnDoNotHaveSaveFile += StartPanel;

        GameResources.LoadResources();




        game.Init(GameResources.LoadTypes(), null, GameResources.LoadPokemons(), GameResources.LoadExperienceTable(),
            natures, null, GameResources.LoadMoves(), GameResources.LoadItems());
        isBattling = false;
        GameResources.LoadPokeItemicons();

        // GetComponent<InMemoryVariableStorage>().SetValue("$player",Game.trainer.name);

    }

    public void SaveSaveData()
    {
        SaveSaveData(CurrentDataSlot);
    }

    public SaveData SaveSaveData(int slot)
    {
        var state = game.GetSave;
        Vector3 position=Vector3.zero;
        var player = FindObjectOfType<PlayerMovement>();
        if (player != null)
            position = player.transform.position;
        //TODO
        SaveData save=new SaveData(state,"TODO",position,SceneManager.GetActiveScene().buildIndex,Config);
        SaveLoad.Save(GameConst.SaveFileName+slot,save,GameConst.SaveFilePath);

        return save;
        // game.SaveData(slot);

        // UIManager.Instance.Show<SavingSignatureUI>();
    }

    public SaveData[] LoadAllSaveData()
    {
        SaveData[] saves = new SaveData[GameConst.SaveMaxFileNum];
        for (int i = 0; i < saves.Length; i++)
        {
            //may be cause null
            saves[i] = LoadSaveData(i);
        }

        return saves;
    }

    /// <summary>
    /// 这里只是完成Game的数据初始化
    /// </summary>
    /// <param name="index"></param>
    public void InitGameWithDataIndex(int index)
    {
        var data = LoadSaveData(index);
        InitGameWithData(data);
    }

    public void InitGameWithData(SaveData data)
    {
        Config = data.Settings;
        game.LoadSaveFile(data.GameState);
        
        SceneManager.LoadScene(data.SceneLoaded);
        location = data.PlayerPosition.ToVec3();
        SceneManager.sceneLoaded +=  OnLoadedFromSave;
    }

    private Vector3 location;

    public void OnLoadedFromSave(Scene s,LoadSceneMode m)
    {
        GetComponent<InMemoryVariableStorage>().SetValue("$player",Game.trainer.name);
        GameObject.FindWithTag("Player").transform.position = location;
        SceneManager.sceneLoaded -=  OnLoadedFromSave;
    }

    public SaveData LoadSaveData(int slot)
    {
        var result = SaveLoad.Load<SaveData>( string.Concat(GameConst.SaveFileName,slot),GameConst.SaveFilePath);
        return result;
    }

   

    // public void StartGame()
    // {
    //     SceneManager.LoadScene(1);
    //     SceneManager.sceneLoaded += StartBattleFastTest;
    // }

    public void StartNetworkBattle(int trainersNum = 2, int pokemonPerTrainer = 1, string password = "")
    {
        // NetworkLogic.StartLocalNetwork();
        NetworkLogic.OnStartBattle = StartBattle;
        NetworkLogic.PairOnBattle(trainersNum, pokemonPerTrainer, password);
    }
    //
    public void StopPairNetworkBattle()
    {
        NetworkLogic.PairOff();
    }
    //
    // public void StartFastNetworkBattle()
    // {
    //     SceneManager.LoadScene(1);
    //     SceneManager.sceneLoaded += (a, b) => { StartNetworkBattle(); };
    // }

    // void StartBattleFastTest(Scene scene, LoadSceneMode mode)
    // {
    //     Game.trainer.party[0] = new Pokemon(Game.PokemonsData[4], "", Game.trainer, 50, 0);
    //     Game.trainer.party[1] = new Pokemon(Game.PokemonsData[1], "", Game.trainer, 50, 0);
    //     Trainer trainer = new Trainer("Computer", false);
    //     trainer.party[0] = new Pokemon(Game.PokemonsData[17], "", trainer, 50, 0);
    //     trainer.party[1] = new Pokemon(Game.PokemonsData[7], "", trainer, 50, 0);
    //     List<Trainer> trainers = new List<Trainer>();
    //     trainers.Add(trainer);
    //     game.SaveData();
    //     StartBattle(null, trainers, true, 2);
    // }

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
        Trainer trainer = Trainer.ProduceWild(pokemon.Name, pokemon.isMale);
        trainer.party[0] = pokemon;
        pokemon.TrainerID = trainer.id;
        
        StartBattle(null, new List<Trainer>(new[] {trainer}), true);
    }

    public void StartBattle(NPC oppoTrainer)
    {
        StartBattle(null,new List<Trainer>(){oppoTrainer._trainer},true,oppoTrainer.pokemonPerTrainer);
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
            BattleHandler.Instance.EndBattle(o);
        };
        BattleHandler.Instance.StartBattle(Game.battle);
    }

    public void CompleteBattleInit()
    {
        game.CompleteBattleInit();
    }

    private void OnApplicationQuit()
    {
        NetworkLogic.PairOff();
    }


    public void Update()
    {
        EventPool.Tick();
    }
}