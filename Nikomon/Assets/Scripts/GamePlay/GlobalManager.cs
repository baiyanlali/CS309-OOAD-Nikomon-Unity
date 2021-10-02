using System;
using System.Collections.Generic;
using PokemonCore;
using PokemonCore.Combat;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;


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

    // public Dictionary<int,GameObject> 

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
        game = new Game();
        game.OnDoNotHaveSaveFile +=StartPanel;
        game.Init();
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
    
    
}