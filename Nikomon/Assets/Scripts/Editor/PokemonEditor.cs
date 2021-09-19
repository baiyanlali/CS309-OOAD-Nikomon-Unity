using System.Collections.Generic;
using System.Linq;
using PokemonCore.Attack;
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
        public List<MoveData> moves;
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
            moves = SaveLoad.Load<List<MoveData>>("moves.json");
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
                PokemonID = EditorGUILayout.IntField("Pokemon ID:",PokemonID);
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

                    CurrentPokemon.innerName = EditorGUILayout.TextField("Inner Name:",CurrentPokemon.innerName);

                    if (types == null || types.Count == 0)
                    {
                        CurrentPokemon.type1 =
                            EditorGUILayout.IntField("Type1:",CurrentPokemon.type1.HasValue ? CurrentPokemon.type1.Value : 0);
                        if (CurrentPokemon.type1 == -1) CurrentPokemon.type1 = null;
                        CurrentPokemon.type2 =
                            EditorGUILayout.IntField("Type2:",CurrentPokemon.type2.HasValue ? CurrentPokemon.type2.Value : 0);
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

                    CurrentPokemon.Ability1 = EditorGUILayout.IntField("1st Ability:",CurrentPokemon.Ability1);

                    CurrentPokemon.Ability2 = EditorGUILayout.IntField("2st Ability:",CurrentPokemon.Ability2);

                    CurrentPokemon.AbilityHidden = EditorGUILayout.IntField("Hidden Ability:",CurrentPokemon.AbilityHidden);
                }
                // GUILayout.EndVertical();
                //
                // GUILayout.BeginVertical();
                GUILayout.Space(30);

                {
                    GUILayout.Label("Ratio's and Value's");

                    GUILayout.BeginHorizontal();
                    CurrentPokemon.MaleRatio = EditorGUILayout.IntSlider("Maile Ratio:",CurrentPokemon.MaleRatio, 0, 100);
                    GUILayout.EndHorizontal();

                    CurrentPokemon.CatchRate = EditorGUILayout.IntSlider("Catch Rate:",CurrentPokemon.CatchRate, 0, 255);

                    CurrentPokemon.BasicExp = EditorGUILayout.IntField("Basic Experience:", CurrentPokemon.BasicExp);

                    if (levelingRate == null || levelingRate.Count==0)
                    {
                        CurrentPokemon.GrowthRate = EditorGUILayout.IntField("Leveling Rate: ",CurrentPokemon.GrowthRate);
                    }
                    else
                    {
                        string[] levelingRateName =
                            (from levelingRate in Enumerable.Range(0, levelingRate.Count)
                                select levelingRate.ToString()).ToArray();
                        CurrentPokemon.GrowthRate = EditorGUILayout.Popup("Leveling Rate:",CurrentPokemon.GrowthRate,levelingRateName);
                    }

                    GUILayout.Space(30);
                    GUILayout.Label("Pokedex Info");

                    GUILayout.BeginHorizontal();
                    CurrentPokemon.Height = EditorGUILayout.FloatField("Height:",CurrentPokemon.Height);
                    CurrentPokemon.Weight = EditorGUILayout.FloatField("Weight:",CurrentPokemon.Weight);
                    GUILayout.EndHorizontal();


                    CurrentPokemon.Species = EditorGUILayout.IntField("Species:",CurrentPokemon.Species);

                    CurrentPokemon.BaseFriendship = EditorGUILayout.IntField("Base Friendship:",CurrentPokemon.BaseFriendship);

                    CurrentPokemon.EvoChainID = EditorGUILayout.IntField("Evolution ID:",CurrentPokemon.EvoChainID);

                    CurrentPokemon.EvolutionMethod =
                        (EvolutionMethod)EditorGUILayout.EnumPopup("Evolution Method:",CurrentPokemon.EvolutionMethod);
                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                {
                    GUILayout.Space(30);
                    GUILayout.Label("Base stats");

                    CurrentPokemon.BaseStatsHP = EditorGUILayout.IntField("HP:",CurrentPokemon.BaseStatsHP);
                    CurrentPokemon.BaseStatsATK = EditorGUILayout.IntField("Attack:",CurrentPokemon.BaseStatsATK);
                    CurrentPokemon.BaseStatsDEF = EditorGUILayout.IntField("Defense:",CurrentPokemon.BaseStatsDEF);
                    CurrentPokemon.BaseStatsSPA = EditorGUILayout.IntField("SP Atk:",CurrentPokemon.BaseStatsSPA);
                    CurrentPokemon.BaseStatsSPD = EditorGUILayout.IntField("SP def:",CurrentPokemon.BaseStatsSPD);
                    CurrentPokemon.BaseStatsSPE = EditorGUILayout.IntField("Speed:",CurrentPokemon.BaseStatsSPE);


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
                                if(moves!=null)
                                    levelMoves.Add(moves[move].MoveID);
                                else
                                {
                                    levelMoves.Add(move);
                                }
                            }
                        }
                        else
                        {
                            if(moves==null)
                                CurrentPokemon.LevelMoves.Add(level, new List<int>(new int[] { move }));
                            else
                            {
                                CurrentPokemon.LevelMoves.Add(level, new List<int>(new int[] { moves[move].MoveID }));
                            }
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
                    if (moves==null ||moves.Count==0)
                    {
                        move = EditorGUILayout.IntField("Move", move);
                    }
                    else
                    {
                        string[] strs = (from m in moves select $"{m.MoveID}||{m.innerName}").ToArray();
                        move = EditorGUILayout.Popup(move,strs);
                    }
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