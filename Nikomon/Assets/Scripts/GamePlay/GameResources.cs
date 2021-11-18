using System;
using System.Collections.Generic;
using GamePlay.Core;
using GamePlay.UI.UIFramework;
using Newtonsoft.Json;
using PokemonCore;
using PokemonCore.Attack.Data;
using PokemonCore.Inventory;
using PokemonCore.Monster.Data;
using PokemonCore.Utility;
using UnityEngine;
using UnityEngine.UI;
using Types = PokemonCore.Types;

namespace GamePlay
{
    public static class GameConst
    {
        public static Dictionary<string, string> UI_PATH = new Dictionary<string, string>()
        {
            ["ChooseElement"] = "Prefabs/UI/ChooseElement",
            ["DialogChooser"] = "Prefabs/UI/DialogChooser",
            ["DialogSystem"] = "Prefabs/UI/DialogSystem",
            ["Move"] = "Prefabs/UI/Move",
            ["PokemonChooser"] = "Prefabs/UI/PokemonChooser",
            ["PokemonState"] = "Prefabs/UI/PokemonState",
            
            ["BagContentElement"]="Prefabs/UI/BagSystem/BagContentElement",
            ["BagContents"]="Prefabs/UI/BagSystem/BagContents",
            ["BagTable"]="Prefabs/UI/BagSystem/BagTable",
            ["Table"]="Prefabs/UI/BagSystem/Table",
            
            ["PokemonChooserTable"]="Prefabs/UI/PokemonChooserTable/PokemonChooserTable",
            ["PokemonStatButton"]="Prefabs/UI/PokemonChooserTable/PokemonStatButton",
        };
    }
    
    public static class GameResources
    {
        public static Dictionary<int, GameObject[]> Pokemons;
        public static Dictionary<int, Sprite> PokemonIcons;
        public static Dictionary<Item.Tag, Sprite> BagIcons;
        public static Dictionary<int, Sprite> TypeIcons;
        public static Dictionary<int, Color> TypeColors;

        private static Dictionary<string, GameObject> CachedPrefabs;

        #region Initial Load

        

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
                
                var res = Resources.Load<Sprite>("Sprites/TypeIcons/"+$"{t.ID}{t.Name}");
                TypeIcons.Add(t.ID,res);

            }

            var tmp_dic = JsonConvert.DeserializeObject<Dictionary<int, PokeColor>>(tmp_typeColor);

            foreach (var tm in tmp_dic)
            {
                TypeColors.Add(tm.Key,tm.Value.toColor());
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

        public static BaseUI SpawnUIPrefab(string name)
        {
            if (!GameConst.UI_PATH.ContainsKey(name))
            {
                throw new Exception($"No Keys of {name} founded in ui prefab");
            }
            
            if(!CachedPrefabs.ContainsKey(name))
            {
                var result = Resources.Load<BaseUI>(GameConst.UI_PATH[name]);
                return result;
            }
            else
            {
                return CachedPrefabs[name].GetComponent<BaseUI>();
            }
        }
        
        
    }
}