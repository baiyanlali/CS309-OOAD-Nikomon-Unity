using System.Collections.Generic;
using System.Linq;
using PokemonCore.Attack.Data;
using PokemonCore.Monster;
using PokemonCore.Monster.Data;
using PokemonCore.Utility;
using UnityEditor;
using UnityEngine;
using Types = PokemonCore.Types;

namespace Editor
{
    public class PokemonEditor : EditorWindow
    {
        public List<PokemonCore.Types> types;
        public string fileName;

        #region PokemonVar

        public List<PokemonData> PokemonDatas;
        public Vector2 pokemonScrollBar;
        public int PokemonID;
        public int PokemonIndex;
        public Vector2 levelMoveScroll;
        public int level;
        public int move;

        public PokemonData CurrentPokemon
        {
            get => PokemonIndex < PokemonDatas.Count && PokemonIndex >= 0 ? PokemonDatas[PokemonIndex] : null;
        }

        #endregion


        [MenuItem("PokemonTools/Edit Pokemon")]
        private static void ShowWindow()
        {
            var window = GetWindow<PokemonEditor>();
            window.titleContent = new GUIContent("Pokemon Editor");
            window.Show();
        }

        private void CreateGUI()
        {
            PokemonDatas = new List<PokemonData>();
            SpeciesList = new List<Species>();
            levelingRate = new List<int[]>();

            types = SaveLoad.Load<List<PokemonCore.Types>>("types.json");
            // types = null;
        }

        public int CurrentSelectIndex;
        public string[] ToolBars = { "Pokemon", "Species", "Ability", "Leveling Rate", "Evolution", "Nature" };

        private void OnGUI()
        {
            GUILayout.BeginVertical();
            {
                CurrentSelectIndex = GUILayout.Toolbar(CurrentSelectIndex, ToolBars);
                GUILayout.Space(10f);

                switch (CurrentSelectIndex)
                {
                    case 0:
                        Pokemon();
                        break;
                    case 1:
                        Species();
                        break;
                    case 2:
                        break;
                    case 3:
                        LevelingRate();
                        break;
                    case 4:
                        break;
                    case 5:
                        break;
                }
            }
            GUILayout.EndVertical();
        }

        private void Pokemon()
        {
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical(GUILayout.MaxWidth(100));
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Pokemon ID:");
                PokemonID = EditorGUILayout.IntField(PokemonID);
                if (GUILayout.Button("Add"))
                {
                    //这里是计算是否有重复的moveID，如果没有才加入
                    // Debug.Log((from movedata in movedatas where this.MoveID == movedata.MoveID select movedata)
                    // .ToArray());
                    if ((from pokemonData in PokemonDatas where pokemonData.ID == PokemonID select pokemonData)
                        .ToArray().Length == 0)
                    {
                        PokemonData md = new PokemonData(PokemonID);
                        PokemonDatas.Add(md);
                        PokemonID++;
                        this.Repaint();
                    }
                    else
                    {
                        Debug.Log("This ID has been used, select another one");
                    }
                }

                GUILayout.EndHorizontal();

                pokemonScrollBar = GUILayout.BeginScrollView(pokemonScrollBar);

                string[] pokemonName =
                    (from pokemonData in PokemonDatas select pokemonData.ID + "||" + pokemonData.innerName).ToArray();
                if (pokemonName.Length != 0)
                    PokemonIndex = GUILayout.SelectionGrid(PokemonIndex, pokemonName, 1);

                GUILayout.EndScrollView();


                GUILayout.FlexibleSpace();

                GUILayout.BeginHorizontal();
                GUILayout.Label("File Name:");
                fileName = EditorGUILayout.TextField(fileName);
                if (GUILayout.Button("Save"))
                {
                    SaveLoad.Save(fileName, PokemonDatas);
                }

                GUILayout.Space(20);
                if (GUILayout.Button("Load"))
                {
                    PokemonDatas = SaveLoad.Load<List<PokemonData>>(fileName);
                }

                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();

            if (CurrentPokemon == null || PokemonDatas.Count == 0)
            {
            }
            else
            {
                GUILayout.BeginVertical(GUILayout.Width(150));
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label($"Edit Pokemon : {CurrentPokemon.ID}");
                    GUILayout.FlexibleSpace();
                    if (GUILayout.Button("Remove"))
                    {
                        PokemonDatas.Remove(CurrentPokemon);
                        this.Repaint();
                    }

                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("inner Name:");
                    CurrentPokemon.innerName = EditorGUILayout.TextField(CurrentPokemon.innerName);
                    GUILayout.EndHorizontal();

                    if (types == null || types.Count == 0)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Type1:");
                        CurrentPokemon.type1 =
                            EditorGUILayout.IntField(CurrentPokemon.type1.HasValue ? CurrentPokemon.type1.Value : 0);
                        if (CurrentPokemon.type1 == -1) CurrentPokemon.type1 = null;
                        GUILayout.Label("Type2:");
                        CurrentPokemon.type2 =
                            EditorGUILayout.IntField(CurrentPokemon.type2.HasValue ? CurrentPokemon.type2.Value : 0);
                        if (CurrentPokemon.type2 == -1) CurrentPokemon.type2 = null;
                        GUILayout.EndHorizontal();
                    }
                    else
                    {
                        GUILayout.BeginHorizontal();
                        List<string> strs = new List<string>();
                        strs.AddRange((from type in types select type.Name).ToArray());
                        strs.Add("NULL");
                        // Debug.Log($"String len: {strs.Length}");
                        GUILayout.Label("Type1:");
                        CurrentPokemon.type1 = EditorGUILayout.Popup(
                            CurrentPokemon.type1.HasValue ? CurrentPokemon.type1.Value : 0,
                            strs.Where(s => !s.Equals("NULL")).ToArray());
                        GUILayout.Label("Type2:");
                        CurrentPokemon.type2 =
                            EditorGUILayout.Popup(CurrentPokemon.type2.HasValue ? CurrentPokemon.type2.Value : strs.Count-1,
                                strs.ToArray());
                        if (CurrentPokemon.type2 >= types.Count) CurrentPokemon.type2 = null;
                        GUILayout.EndHorizontal();
                    }

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("1st Ability:");
                    CurrentPokemon.Ability1 = EditorGUILayout.IntField(CurrentPokemon.Ability1);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("2st Ability:");
                    CurrentPokemon.Ability2 = EditorGUILayout.IntField(CurrentPokemon.Ability2);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Hidden Ability:");
                    CurrentPokemon.AbilityHidden = EditorGUILayout.IntField(CurrentPokemon.AbilityHidden);
                    GUILayout.EndHorizontal();
                }
                // GUILayout.EndVertical();
                //
                // GUILayout.BeginVertical();
                GUILayout.Space(30);

                {
                    GUILayout.Label("Ratio's and Value's");

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Male Ratio:");
                    CurrentPokemon.MaleRatio = EditorGUILayout.IntSlider(CurrentPokemon.MaleRatio, 0, 100);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Catch Rate:");
                    CurrentPokemon.CatchRate = EditorGUILayout.IntSlider(CurrentPokemon.CatchRate, 0, 100);
                    GUILayout.EndHorizontal();

                    CurrentPokemon.BasicExp = EditorGUILayout.IntField("Basic Experience:", CurrentPokemon.BasicExp);

                    if (levelingRate == null || levelingRate.Count==0)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Leveling Rate:");
                        CurrentPokemon.GrowthRate = EditorGUILayout.IntField(CurrentPokemon.GrowthRate);
                        GUILayout.EndHorizontal();
                    }
                    else
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Leveling Rate:");
                        string[] levelingRateName =
                            (from levelingRate in Enumerable.Range(0, levelingRate.Count)
                                select levelingRate.ToString()).ToArray();
                        CurrentPokemon.GrowthRate = EditorGUILayout.Popup(CurrentPokemon.GrowthRate,levelingRateName);
                        GUILayout.EndHorizontal();
                    }

                    GUILayout.Space(30);
                    GUILayout.Label("Pokedex Info");

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Height:");
                    CurrentPokemon.Height = EditorGUILayout.FloatField(CurrentPokemon.Height);
                    GUILayout.Label("Weight:");
                    CurrentPokemon.Weight = EditorGUILayout.FloatField(CurrentPokemon.Weight);
                    GUILayout.EndHorizontal();


                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Species:");
                    CurrentPokemon.Species = EditorGUILayout.IntField(CurrentPokemon.Species);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Base Friendship:");
                    CurrentPokemon.BaseFriendship = EditorGUILayout.IntField(CurrentPokemon.BaseFriendship);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Evolution ID:");
                    CurrentPokemon.EvoChainID = EditorGUILayout.IntField(CurrentPokemon.EvoChainID);
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Evolution Method:");
                    CurrentPokemon.EvolutionMethod =
                        (EvolutionMethod)EditorGUILayout.EnumPopup(CurrentPokemon.EvolutionMethod);
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                {
                    GUILayout.Space(30);
                    GUILayout.Label("Base stats");

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("HP:");
                    CurrentPokemon.BaseStatsHP = EditorGUILayout.IntField(CurrentPokemon.BaseStatsHP);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Attack:");
                    CurrentPokemon.BaseStatsATK = EditorGUILayout.IntField(CurrentPokemon.BaseStatsATK);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Defense:");
                    CurrentPokemon.BaseStatsDEF = EditorGUILayout.IntField(CurrentPokemon.BaseStatsDEF);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("SP Atk:");
                    CurrentPokemon.BaseStatsSPA = EditorGUILayout.IntField(CurrentPokemon.BaseStatsSPA);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("SP def:");
                    CurrentPokemon.BaseStatsSPD = EditorGUILayout.IntField(CurrentPokemon.BaseStatsSPD);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Speed:");
                    CurrentPokemon.BaseStatsSPE = EditorGUILayout.IntField(CurrentPokemon.BaseStatsSPE);
                    GUILayout.EndHorizontal();


                    GUILayout.Space(20);
                    GUILayout.Label("ev yield stats");

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("HP:");
                    CurrentPokemon.evYieldHP = EditorGUILayout.IntField(CurrentPokemon.evYieldHP);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Attack:");
                    CurrentPokemon.evYieldATK = EditorGUILayout.IntField(CurrentPokemon.evYieldATK);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Defense:");
                    CurrentPokemon.evYieldDEF = EditorGUILayout.IntField(CurrentPokemon.evYieldDEF);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("SP Atk:");
                    CurrentPokemon.evYieldSPA = EditorGUILayout.IntField(CurrentPokemon.evYieldSPA);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("SP def:");
                    CurrentPokemon.evYieldSPD = EditorGUILayout.IntField(CurrentPokemon.evYieldSPD);
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Speed:");
                    CurrentPokemon.evYieldSPE = EditorGUILayout.IntField(CurrentPokemon.evYieldSPE);
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                {
                    GUILayout.Label("Level Move");
                    level = EditorGUILayout.IntField("Level:", level);
                    if (GUILayout.Button("Add Level Move"))
                    {
                        if (CurrentPokemon.LevelMoves.ContainsKey(level))
                        {
                            List<int> levelMoves;
                            if (CurrentPokemon.LevelMoves.TryGetValue(level, out levelMoves))
                            {
                                levelMoves.Add(move);
                            }
                        }
                        else
                        {
                            CurrentPokemon.LevelMoves.Add(level, new List<int>(new int[] { move }));
                        }
                    }

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Level");
                    GUILayout.FlexibleSpace();
                    GUILayout.Label("Move");
                    GUILayout.EndHorizontal();

                    GUILayout.BeginScrollView(levelMoveScroll);
                    foreach (var key in CurrentPokemon.LevelMoves)
                    {
                        foreach (var move in key.Value)
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Label($"{key.Key}");
                            GUILayout.FlexibleSpace();
                            GUILayout.Label($"{move}");
                            GUILayout.EndHorizontal();
                        }
                    }

                    GUILayout.EndScrollView();
                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                {
                    GUILayout.Label("");
                    move = EditorGUILayout.IntField("Move", move);
                    GUILayout.Label("");
                    GUILayout.Label("Remove Moves");
                    GUILayout.BeginScrollView(levelMoveScroll);
                    bool isBreak = false;
                    foreach (var key in CurrentPokemon.LevelMoves)
                    {
                        foreach (var move in key.Value)
                        {
                            //如果MoveList中只有一个值，那直接删除键值对，否则删掉List中某个特定值
                            if (GUILayout.Button("X"))
                            {
                                if (key.Value.Count == 1)
                                    CurrentPokemon.LevelMoves.Remove(key.Key);
                                else
                                    key.Value.Remove(move);
                                isBreak = true;
                                break;
                            }
                        }

                        if (isBreak) break;
                    }

                    GUILayout.EndScrollView();
                }
                GUILayout.EndVertical();
            }

            GUILayout.EndHorizontal();
        }

        #region SpeciesVar

        public List<Species> SpeciesList;
        public int SpeciesID;
        public int SpeciesIndex;
        public Vector2 SpeciesScrollBar;

        public Species CurrentSpecies
        {
            get => SpeciesIndex < SpeciesList.Count && SpeciesIndex >= 0 ? SpeciesList[SpeciesIndex] : null;
        }

        #endregion

        void Species()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical(GUILayout.MaxWidth(100));
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Species ID:");
                    SpeciesID = EditorGUILayout.IntField(SpeciesID);
                    if (GUILayout.Button("Add"))
                    {
                        //这里是计算是否有重复的moveID，如果没有才加入
                        // Debug.Log((from movedata in movedatas where this.MoveID == movedata.MoveID select movedata)
                        // .ToArray());
                        if ((from species in SpeciesList where species.ID == SpeciesID select species)
                            .ToArray().Length == 0)
                        {
                            SpeciesList.Add(new Species(SpeciesID));
                            SpeciesID++;
                            this.Repaint();
                        }
                        else
                        {
                            Debug.Log("This ID has been used, select another one");
                        }
                    }

                    GUILayout.EndHorizontal();

                    SpeciesScrollBar = GUILayout.BeginScrollView(SpeciesScrollBar);

                    string[] speciesName =
                        (from species in SpeciesList select species.ID + "||" + species.Description)
                        .ToArray();
                    if (speciesName.Length != 0)
                        SpeciesIndex = GUILayout.SelectionGrid(SpeciesIndex, speciesName, 1);

                    GUILayout.EndScrollView();


                    GUILayout.FlexibleSpace();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("File Name:");
                    fileName = EditorGUILayout.TextField(fileName);
                    if (GUILayout.Button("Save"))
                    {
                        SaveLoad.Save(fileName, SpeciesList);
                    }

                    GUILayout.Space(20);
                    if (GUILayout.Button("Load"))
                    {
                        SpeciesList = SaveLoad.Load<List<Species>>(fileName);
                    }

                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
                GUILayout.BeginVertical();

                {
                    if (CurrentSpecies != null)
                    {
                        GUILayout.Label($"Species ID: {SpeciesID}");
                        CurrentSpecies.Description =
                            EditorGUILayout.TextField("Description:", CurrentSpecies.Description);
                    }
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }

        void Ability()
        {
        }

        #region LevelVar

        private List<int[]> levelingRate;
        public int levelingRateID;
        public int levelingRateIndex;
        public Vector2 levelingRateScroll;

        public int[] CurrentLevelingRate
        {
            get => levelingRateIndex < levelingRate.Count && levelingRateIndex >= 0
                ? levelingRate[levelingRateIndex]
                : null;
        }

        #endregion

        void LevelingRate()
        {
            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical(GUILayout.MaxWidth(100));
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label($"LevelingRate ID:{levelingRate.Count}");
                    if (GUILayout.Button("Add"))
                    {
                        levelingRate.Add(new int[100]);
                        this.Repaint();
                    }

                    GUILayout.EndHorizontal();

                    SpeciesScrollBar = GUILayout.BeginScrollView(SpeciesScrollBar);

                    //也是列举有多少中Leveling Rate
                    string[] levelingRateName =
                        (from levelingRate in Enumerable.Range(0, levelingRate.Count)
                            select levelingRate.ToString()).ToArray();
                    
                    if (levelingRateName.Length != 0)
                        levelingRateIndex = GUILayout.SelectionGrid(levelingRateIndex, levelingRateName, 1);

                    GUILayout.EndScrollView();


                    GUILayout.FlexibleSpace();

                    GUILayout.BeginHorizontal();
                    GUILayout.Label("File Name:");
                    fileName = EditorGUILayout.TextField(fileName);
                    if (GUILayout.Button("Save"))
                    {
                        SaveLoad.Save(fileName, levelingRate);
                    }

                    GUILayout.Space(20);
                    if (GUILayout.Button("Load"))
                    {
                        levelingRate = SaveLoad.Load<List<int[]>>(fileName);
                    }

                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();

                if (CurrentLevelingRate == null)
                {
                    
                }
                else
                {
                     GUILayout.BeginVertical();
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label($"Leveling Rate : {levelingRateIndex}");
                        if (GUILayout.Button("Remove"))
                        {
                            levelingRate.Remove(CurrentLevelingRate);
                            return;
                        }
                        GUILayout.EndHorizontal();
                        levelingRateScroll = GUILayout.BeginScrollView(levelingRateScroll);

                        for (int i = 0; i < CurrentLevelingRate.Length; i++)
                        {
                            CurrentLevelingRate[i] =
                                EditorGUILayout.IntField($"Level {i + 1}:", CurrentLevelingRate[i]);
                        }
                        
                        GUILayout.EndScrollView();
                    }
                    GUILayout.EndVertical();
                }
                
               
            }
            GUILayout.EndHorizontal();
        }

        void Evolution()
        {
            
        }

        void Nature()
        {
            
        }
    }
}