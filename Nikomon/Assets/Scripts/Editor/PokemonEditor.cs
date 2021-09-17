using System.Collections.Generic;
using System.Linq;
using PokemonCore.Attack.Data;
using PokemonCore.Monster.Data;
using PokemonCore.Utility;
using UnityEditor;
using UnityEngine;
using Types = PokemonCore.Types;

namespace Editor
{
    public class PokemonEditor : EditorWindow
    {
        public List<PokemonData> PokemonDatas;
        public Vector2 pokemonScrollBar;
        public List<PokemonCore.Types> types;
        public int PokemonID;
        public int PokemonIndex;
        public string fileName;

        public PokemonData CurrentPokemon
        {
            get => PokemonIndex<PokemonDatas.Count && PokemonIndex>=0? PokemonDatas[PokemonIndex]:null;
        }
        
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
             types = SaveLoad.Load<List<PokemonCore.Types>>("types.json");
             // types = null;
         }

        private void OnGUI()
        {
            Pokemon();
            
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
                            if ((from pokemonData in PokemonDatas where pokemonData.ID==PokemonID select pokemonData)
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
                    
                        string[] pokemonName = (from pokemonData in PokemonDatas select pokemonData.ID+"||"+pokemonData.innerName).ToArray();
                        if(pokemonName.Length!=0)
                            PokemonIndex = GUILayout.SelectionGrid(PokemonIndex,pokemonName,1);
                    
                    GUILayout.EndScrollView();
                    
                    
                    GUILayout.FlexibleSpace();
                    
                    GUILayout.BeginHorizontal();
                        GUILayout.Label("File Name:");
                        fileName = EditorGUILayout.TextField(fileName);
                        if (GUILayout.Button("Save"))
                        {
                            SaveLoad.Save(fileName,PokemonDatas);
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

                    GUILayout.BeginVertical();
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
                            CurrentPokemon.type1= EditorGUILayout.IntField(CurrentPokemon.type1.HasValue?CurrentPokemon.type1.Value:0);
                            if (CurrentPokemon.type1 == -1) CurrentPokemon.type1 = null;
                            GUILayout.Label("Type2:");
                            CurrentPokemon.type2= EditorGUILayout.IntField(CurrentPokemon.type2.HasValue?CurrentPokemon.type2.Value:0);
                            if (CurrentPokemon.type2 == -1) CurrentPokemon.type2 = null;
                            GUILayout.EndHorizontal();
                        }
                        else
                        {
                            GUILayout.BeginHorizontal();
                            List<string> strs = new List<string>();
                            strs.Add("NULL");
                            strs.AddRange((from type in types select type.Name).ToArray());
                            // Debug.Log($"String len: {strs.Length}");
                            GUILayout.Label("Type1:");
                            CurrentPokemon.type1 = EditorGUILayout.Popup(CurrentPokemon.type1.HasValue?CurrentPokemon.type1.Value:0,strs.Where(s => !s.Equals("NULL")).ToArray());
                            GUILayout.Label("Type2:");
                            CurrentPokemon.type2 = EditorGUILayout.Popup(CurrentPokemon.type2.HasValue?CurrentPokemon.type2.Value:0,strs.ToArray());
                            if (CurrentPokemon.type2 == 0) CurrentPokemon.type2 = null;
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
                        
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Leveling Rate:");
                        CurrentPokemon.GrowthRate = EditorGUILayout.IntField(CurrentPokemon.GrowthRate);
                        GUILayout.EndHorizontal();
                        
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
                        CurrentPokemon.EvolutionMethod = (EvolutionMethod)EditorGUILayout.EnumPopup(CurrentPokemon.EvolutionMethod);
                        GUILayout.EndHorizontal();
                        
                        
                        
                    }
                    GUILayout.EndVertical();
                    
                    GUILayout.BeginVertical();
                    {
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
                        
                        GUILayout.Label("Base stats");
                        
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
                        if (GUILayout.Button("Add Level"))
                        {
                            CurrentPokemon.LevelMoves.Add(0,0);
                        }
                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Level");
                        GUILayout.FlexibleSpace();
                        GUILayout.Label("Move");
                        GUILayout.EndHorizontal();
                        
                        foreach (var key in CurrentPokemon.LevelMoves)
                        {
                            GUILayout.BeginHorizontal();
                            GUILayout.Label($"{key.Key}");
                            GUILayout.FlexibleSpace();
                            GUILayout.Label($"{key.Value}");
                            GUILayout.EndHorizontal();
                        }
                    }
                    GUILayout.EndVertical();
                    
                     GUILayout.BeginVertical();
                    {
                        GUILayout.Label("");
                        GUILayout.Label("");
                        GUILayout.Label("Remove Moves");
                        foreach (var key in CurrentPokemon.LevelMoves)
                        {
                            if (GUILayout.Button("X"))
                            {
                                CurrentPokemon.LevelMoves.Remove(key.Key);
                            }
                        }

                        
                    }
                    GUILayout.EndVertical();
                    
                    
                    
                }

                GUILayout.EndHorizontal();
        }
    }
}