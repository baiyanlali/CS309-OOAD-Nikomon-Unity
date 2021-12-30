using System;
using System.Collections.Generic;
using System.Linq;
using GamePlay.Core;
using GamePlay.UI;
using GamePlay.UI.BagSystem;
using GamePlay.UI.BattleUI;
using GamePlay.UI.MainMenuUI;
using GamePlay.UI.PokemonChooserTable;
using GamePlay.UI.UIFramework;
using GamePlay.UI.UtilUI;
using Newtonsoft.Json;
using PokemonCore;
using PokemonCore.Attack.Data;
using PokemonCore.Inventory;
using PokemonCore.Monster.Data;
using PokemonCore.Utility;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Types = PokemonCore.Types;

namespace GamePlay
{
    public static class GameConst
    {
        public static readonly string SaveFilePath = Application.persistentDataPath;
        public static readonly string SaveFileName = "Save";
        public static readonly int SaveMaxFileNum = 3;

        public static Dictionary<string, string> PrefabPathStr = new Dictionary<string, string>()
        {
            //BagSystem
            ["Table"] = "Prefabs/UI/BagSystem/Table",
            ["BagContents"] = "Prefabs/UI/BagSystem/BagContents",
            ["ChooseElement"] = "Prefabs/UI/ChooseElement",
            ["Slot"]="Prefabs/UI/PC/slot"
        };

        public static Dictionary<Type, string> PrefabPath = new Dictionary<Type, string>()
        {
            /*-----------------GameObject------------------ - */
            [typeof(GlobalManager)] = "Prefabs/Global", 


            /*-----------------UI-------------------*/
            [typeof(UIManager)] = "Prefabs/UI/UIManager", 
            [typeof(MainMenuUI)] = "Prefabs/UI/MainMenu/MainMenu", 
            [typeof(StartMenuUI)] = "Prefabs/UI/MainMenu/StartMenu", 
            [typeof(SavePanelUI)] = "Prefabs/UI/MainMenu/SavePanelUI", 
            [typeof(ConfirmPanel)] = "Prefabs/UI/UtilUI/ConfirmPanel", 
            //bag
            [typeof(BagPanelUI)] = "Prefabs/UI/BagSystem/BagTable", 
            [typeof(BagContentElementUI)] = "Prefabs/UI/BagSystem/BagContentElement", 
            [typeof(StorePanelUI)]="Prefabs/UI/BagSystem/Store",
            [typeof(StoreContentElement)]="Prefabs/UI/BagSystem/StoreContentElement",

            //PokemonChooserPanel
            [typeof(PokemonChooserPanelUI)] = "Prefabs/UI/PokemonChooserTable/PokemonChooserTable", 
            [typeof(PokemonChooserElementUI)] = "Prefabs/UI/PokemonChooserTable/PokemonStatButton", 

            //DialogueChooser
            [typeof(DialogueChooserPanel)] = "Prefabs/UI/DialogChooser",
            //TargetChooser
            [typeof(TargetChooserPanel)] = "Prefabs/UI/BattleUI/PokemonChooser", 
            [typeof(TargetChooserUI)] = "Prefabs/UI/BattleUI/TargetChooserElement", 
            [typeof(TargetChooserHandler)] = "Prefabs/UI/BattleUI/PokemonChooserElement",
            //Battle
            [typeof(MovePanel)]="Prefabs/UI/BattleUI/MovePanel",
            [typeof(BattlePokemonStateUI)]="Prefabs/UI/BattleUI/PokemonState",
            [typeof(BattleStatusPanel)]="Prefabs/UI/BattleUI/BattleStatusPanel",
            [typeof(BattleMenuPanel)]="Prefabs/UI/BattleUI/BattleMenuUI",
            [typeof(BattleDialoguePanel)]="Prefabs/UI/BattleUI/BattleDialoguePanel",
            [typeof(MoveElement)]="Prefabs/UI/BattleUI/MoveElement",
            //DebugTool
            [typeof(DebugPanel)]="Prefabs/UI/DebugTool",
            //Dialogue
            [typeof(DialogPanel)]="Prefabs/UI/DialogPanel",
            //PC
            [typeof(PCManager)]="Prefabs/UI/PC/PC",
            [typeof(PCParty)]="Prefabs/UI/PC/Party",
            [typeof(Slot)]="Prefabs/UI/PC/slot",
            //Ability
            [typeof(AbilityPanel)]="Prefabs/UI/Ability/PokemonAbilityTable",
            //Pokedex
            [typeof(PokedexPanel)]="Prefabs/UI/Pokemondex/Pokedex",
            [typeof(PokemondexElement)]="Prefabs/UI/Pokemondex/Pokemondex",
            //Movelearing
            [typeof(MovelearningUI)]="Prefabs/UI/MoveLearning/MoveLearnPanel",
            //Setting
            [typeof(SettingUI)]="Prefabs/UI/SettingUI/SettingPanel",
            //Evolution
            [typeof(EvolutionPanel)]="Prefabs/UI/EvolutionUI/EvolutionPanel",
            //Settlement
            [typeof(SettlementPanel)]="Prefabs/UI/SettlementUI/SettlementPanel",
            [typeof(PokemonSettlement)]="Prefabs/UI/SettlementUI/PokemonSettlement",
            //Connect
            [typeof(ConnectPanel)]="Prefabs/UI/ConnectUI/ConnectUI",
            
            
        };
    }

    public static class GameResources
    {
        public static Dictionary<int, GameObject[]> Pokemons;
        public static Dictionary<int, Sprite> PokemonIcons;
        public static Dictionary<Item.Tag, Sprite> BagIcons;
        public static Dictionary<int, Sprite> TypeIcons;
        public static Dictionary<int, Color> TypeColors;
        public static Dictionary<(Item.Tag, int), Sprite> ItemIcons;

        private static Dictionary<Type, GameObject> CachedPrefabs = new Dictionary<Type, GameObject>();
        private static Dictionary<string, GameObject> CachedPrefabsStr = new Dictionary<string, GameObject>();


        #region Initial Load

        public static Dictionary<string, string> LoadLocalization(string culture)
        {
            var text = Resources.Load<TextAsset>($"Localization/{culture}/name_{culture}").text;
            var dics = JsonConvert.DeserializeObject<Dictionary<string, string>>(text);
            text = Resources.Load<TextAsset>($"Localization/{culture}/skill_{culture}").text;
            var tmp = JsonConvert.DeserializeObject<Dictionary<string, string>>(text);
            foreach (var t in tmp)
            {
                dics.Add(t.Key,t.Value);
            }
            
            text = Resources.Load<TextAsset>($"Localization/{culture}/tool_{culture}").text;
            tmp = JsonConvert.DeserializeObject<Dictionary<string, string>>(text);
            foreach (var t in tmp)
            {
                if(! dics.ContainsKey(t.Key))
                    dics.Add(t.Key,t.Value);
            }
            
            text = Resources.Load<TextAsset>($"Localization/{culture}/ui_{culture}").text;
            tmp = JsonConvert.DeserializeObject<Dictionary<string, string>>(text);
            foreach (var t in tmp)
            {
                if(! dics.ContainsKey(t.Key))
                    dics.Add(t.Key,t.Value);
            }
            text = Resources.Load<TextAsset>($"Localization/{culture}/move_description_{culture}").text;
            tmp = JsonConvert.DeserializeObject<Dictionary<string, string>>(text);
            foreach (var t in tmp)
            {
                if(! dics.ContainsKey(t.Key))
                    dics.Add(t.Key,t.Value);
            }
            return dics;
        }

        public static Dictionary<int, int[]> LoadExperienceTable()
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

        public static Dictionary<int, Types> LoadTypes()
        {
            List<Types> tmp =
                JsonConvert.DeserializeObject<List<Types>>(Resources.Load<TextAsset>("PokemonData/" + Game.TypeFile)
                    .text);
            var tmp_typeColor = Resources.Load<TextAsset>("PokemonData/typesColor").text;

            TypeIcons = new Dictionary<int, Sprite>();
            TypeColors = new Dictionary<int, Color>();
            var typesMap = new Dictionary<int, Types>();
            foreach (var t in tmp)
            {
                typesMap.Add(t.ID, t);

                var res = Resources.Load<Sprite>("Sprites/TypeIcons/" + $"{t.ID}{t.Name}");
                TypeIcons.Add(t.ID, res);
            }

            var tmp_dic = JsonConvert.DeserializeObject<Dictionary<int, PokeColor>>(tmp_typeColor);

            foreach (var tm in tmp_dic)
            {
                TypeColors.Add(tm.Key, tm.Value.toColor());
            }

            return typesMap;
        }

        public static void LoadAbilities(LoadDataType type = LoadDataType.Json)
        {
            if (type == LoadDataType.Json)
            {
                // using FileStream openStream = File.OpenRead(DataPath+"Abilities.json");
                // AbilitiesData = await JsonSerializer.DeserializeAsync<Dictionary<int,Ability>>(openStream);
            }
        }

        public static void LoadNature(LoadDataType type = LoadDataType.Json)
        {
            if (type == LoadDataType.Json)
            {
            }
        }

        public static void LoadEffect(LoadDataType type = LoadDataType.Json)
        {
            if (type == LoadDataType.Json)
            {
            }
        }
        
        //TODO:!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        public static void LoadPokeItemicons()
        {

            ItemIcons = new Dictionary<(Item.Tag, int), Sprite>();
            var tmp = Game.ItemsData;
            // from t in tmp where t.Key==Item.Tag.PokeBalls
            // foreach (var item in tmp)
            // {
            //     // Debug.Log(item.Key.Item1+item.Key.Item2.ToString());
            //     // ItemIcons.Add(item.Key,Resources.Load<Sprite>("Sprites/ItemIcons/"+item.Key.Item1+item.Key.Item2.ToString()));
            // }
            foreach (var item in tmp)
            {
                ItemIcons.Add(item.Key,Resources.Load<Sprite>("Sprites/ItemIcons/"+item.Key.Item1+item.Key.Item2.ToString()));
            }
            // for (int i = 0; i < ; i++)
            //     
            // {
            //     PokeBallIcons.Add(i, Resources.Load<Sprite>("Sprites/PokemonIcons/" + t.ID + t.innerName));
            // }
            //PokemonIcons.Add(t.ID, Resources.Load<Sprite>("Sprites/PokemonIcons/" + t.ID + t.innerName));
        }

        public static Dictionary<int, MoveData> LoadMoves()
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
        

        public static Dictionary<ValueTuple<Item.Tag, int>, Item> LoadItems()
        {
            string str = Resources.Load<TextAsset>("PokemonData/" + Game.ItemFile).text;
            Dictionary<Item.Tag, List<Item>> itemDictionary =
                JsonConvert.DeserializeObject<Dictionary<Item.Tag, List<Item>>>(str);
            
            var itemData = new Dictionary<ValueTuple<Item.Tag, int>, Item>();
            foreach (var item in itemDictionary)
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


        public static Dictionary<int, PokemonData> LoadPokemons()
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
                    if (obj == null) UnityEngine.Debug.LogError($"Pokemon Prefabs Not Found:{t.ID},{t.innerName}");
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

        public static void LoadResources()
        {
            BagIcons = new Dictionary<Item.Tag, Sprite>();
            string[] strs = Enum.GetNames(typeof(Item.Tag));
            foreach (var str in strs)
            {
                Sprite spr = Resources.Load<Sprite>("Sprites/BagIcons/" + str);
                BagIcons.Add((Item.Tag) Enum.Parse(typeof(Item.Tag), str), spr);
            }
        }

        #endregion


        public static GameObject SpawnPrefab(string name)
        {
            if (!GameConst.PrefabPathStr.ContainsKey(name))
            {
                throw new Exception($"No Keys of {name} founded in prefab");
            }

            if (!CachedPrefabsStr.ContainsKey(name))
            {
                var result = Resources.Load<GameObject>(GameConst.PrefabPathStr[name]);
                CachedPrefabsStr.Add(name, result);
                return result;
            }
            else
            {
                return CachedPrefabsStr[name];
            }
        }

        public static GameObject SpawnPrefab(Type name)
        {
            if (!GameConst.PrefabPath.ContainsKey(name))
            {
                throw new Exception($"No Keys of {name} founded in prefab");
            }

            if (!CachedPrefabs.ContainsKey(name))
            {
                var result = Resources.Load<GameObject>(GameConst.PrefabPath[name]);
                CachedPrefabs.Add(name, result);
                return result;
            }
            else
            {
                return CachedPrefabs[name];
            }
        }
    }
}