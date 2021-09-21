using System;
using System.Collections.Generic;
using System.Linq;
using PokemonCore.Attack.Data;
using PokemonCore.Utility;
using UnityEditor;
using UnityEngine;
using Types = PokemonCore.Types;

namespace Editor
{
    public class MoveEditor : EditorWindow
    {
        public Vector2 moveScrollBar;
        public int moveIndex;
        public List<MoveData> movedatas;
        public string fileName;
        public int MoveID ;
        public List<Types> types;

        private MoveData CurrentMove
        {
            get => moveIndex<movedatas.Count && moveIndex>=0? movedatas[moveIndex]:null;
        }

        [MenuItem("PokemonTools/Edit Moves")]
        private static void ShowWindow()
        {
            var window = GetWindow<MoveEditor>();
            window.titleContent = new GUIContent("Moves Editor");
            window.Show();
            
        }

        private void CreateGUI()
        {
            movedatas = new List<MoveData>();
            moveIndex = 0;

            types = SaveLoad.Load<List<Types>>("types.json");
        }

        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
                GUILayout.BeginVertical(GUILayout.MaxWidth(100));
                {
                    GUILayout.BeginHorizontal();
                        GUILayout.Label("Move ID:");
                        MoveID = EditorGUILayout.IntField(MoveID);
                        if (GUILayout.Button("Add"))
                        {
                            //这里是计算是否有重复的moveID，如果没有才加入
                            // Debug.Log((from movedata in movedatas where this.MoveID == movedata.MoveID select movedata)
                                // .ToArray());
                            if ((from movedata in movedatas where this.MoveID == movedata.MoveID select movedata)
                                .ToArray().Length == 0)
                            {
                                MoveData md = new MoveData(MoveID);
                                movedatas.Add(md);
                                movedatas.Sort((o1, o2) => {return o1.MoveID - o2.MoveID;});
                                MoveID++;
                                this.Repaint();
                            }
                            else
                            {
                                Debug.Log("This ID has been used, select another one");
                            }

                            
                        }
                    GUILayout.EndHorizontal();

                    moveScrollBar = GUILayout.BeginScrollView(moveScrollBar);
                    
                        string[] movesName = (from movedata in movedatas select movedata.MoveID+"||"+movedata.innerName).ToArray();
                        if(movesName.Length!=0)
                            moveIndex = GUILayout.SelectionGrid(moveIndex,movesName,1);
                    
                    GUILayout.EndScrollView();
                    
                    
                    GUILayout.FlexibleSpace();
                    
                    GUILayout.BeginHorizontal();
                        GUILayout.Label("File Name:");
                        fileName = EditorGUILayout.TextField(fileName);
                        if (GUILayout.Button("Save"))
                        {
                            SaveLoad.Save(fileName,movedatas);
                        }
                        GUILayout.Space(20);
                        if (GUILayout.Button("Load"))
                        {
                            movedatas = SaveLoad.Load<List<MoveData>>(fileName);
                            movedatas.Sort((o1, o2) => {return o1.MoveID - o2.MoveID;});
                        }
                    GUILayout.EndHorizontal();
                    
                }
                GUILayout.EndVertical();

                if (CurrentMove == null || movedatas.Count == 0)
                {
                    
                }
                else
                {

                    GUILayout.BeginVertical();
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label($"Edit Moves:{CurrentMove.MoveID}");
                        GUILayout.FlexibleSpace();
                        if (GUILayout.Button("Remove"))
                        {
                            movedatas.Remove(CurrentMove);
                            return;
                        }

                        GUILayout.EndHorizontal();
                        
                        
                        CurrentMove.innerName = EditorGUILayout.TextField("inner Name:",CurrentMove.innerName);
                        
                        CurrentMove.Category = (Category) EditorGUILayout.EnumPopup("Category",CurrentMove.Category);

                        if (types == null || types.Count == 0)
                        {
                            CurrentMove.Type = EditorGUILayout.IntField("Type",CurrentMove.Type);
                        }
                        else
                        {
                            string[] strs = (from type in types select type.Name).ToArray();
                            CurrentMove.Type = EditorGUILayout.Popup("Type",CurrentMove.Type,strs);
                        }

                        CurrentMove.Target =
                            (PokemonCore.Attack.Data.Targets) EditorGUILayout.EnumPopup("Target",CurrentMove.Target);

                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Accuracy:");
                        CurrentMove.Accuracy = EditorGUILayout.IntField((int) CurrentMove.Accuracy);
                        if (CurrentMove.Accuracy == -1) CurrentMove.Accuracy = null;
                        GUILayout.EndHorizontal();
                        
                        
                        CurrentMove.CriticalLevel=EditorGUILayout.IntField("Critical Level",(int) CurrentMove.Accuracy);

                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Power:");
                        CurrentMove.Power = EditorGUILayout.IntField((int) CurrentMove.Power);
                        if (CurrentMove.Power == -1) CurrentMove.Power = null;
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        GUILayout.Label("PP:");
                        CurrentMove.PP = (byte) EditorGUILayout.IntField(CurrentMove.PP);
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        GUILayout.Label("Priority:");
                        CurrentMove.Priority = (byte) EditorGUILayout.IntSlider(CurrentMove.Priority, 0, 2);
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        GUILayout.Label("EffectID:");
                        CurrentMove.EffectID = EditorGUILayout.IntField(CurrentMove.EffectID);
                        GUILayout.EndHorizontal();

                        GUILayout.BeginHorizontal();
                        GUILayout.Label("EffectChance:");
                        CurrentMove.EffectChance = EditorGUILayout.IntField((int) CurrentMove.EffectChance);
                        if (CurrentMove.EffectChance == -1) CurrentMove.EffectChance = null;
                        GUILayout.EndHorizontal();




                    }
                    GUILayout.EndVertical();
                }

                GUILayout.EndHorizontal();
            
        }

    }
}