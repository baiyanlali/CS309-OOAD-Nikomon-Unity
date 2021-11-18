using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PokemonCore;
using PokemonCore.Attack.Data;
using PokemonCore.Utility;
using UnityEditor;
using Debug = UnityEngine.Debug;
using Types = PokemonCore.Types;

namespace Editor
{
    public class MoveEditorNew : EditorWindow
    {
        public Vector2 moveScrollBar;
        public int moveIndex;
        public List<MoveData> movedatas;
        public string fileName;
        public int MoveID;
        public List<Types> types;
        private MoveData CurrentMove
        {
            get => moveIndex<movedatas.Count && moveIndex>=0? movedatas[moveIndex]:null;
        }

        [MenuItem("PokemonTools/Edit Moves (New)")]
        private static void ShowWindow()
        {
            var window = GetWindow<MoveEditorNew>();
            window.titleContent = new GUIContent("Move Editor");
            window.Show();
        }

        public static void Open(MoveMonoEdition monoEdition)
        {
            var window = GetWindow<MoveEditorNew>();
            window.titleContent = new GUIContent("Move Editor");
            window.Show();
            monoEdition.MoveDatas=window.movedatas;
            window._monoEdition = monoEdition;
            window._serializedObject = new SerializedObject(monoEdition);
            window._currentProperty =window._serializedObject.FindProperty("MoveDatas");

        }

        private MoveMonoEdition _monoEdition;

        private void CreateGUI()
        {
            movedatas = new List<MoveData>();
            moveIndex = 0;

            types = SaveLoad.Load<List<Types>>("types.json", @"Assets\Resources\PokemonData\");
        }

        private void OnGUI()
        {
            
            #region draw select panel

            
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
                    // if ((from movedata in movedatas where this.MoveID == movedata.MoveID select movedata)
                    //     .ToArray().Length == 0)
                    // {
                        // MoveData md = new MoveData(MoveID);
                        // movedatas.Add(md);
                        // movedatas.Sort((o1, o2) => { return o1.MoveID - o2.MoveID; });
                        _currentProperty.arraySize++;
                        var prop = _currentProperty.GetArrayElementAtIndex(_currentProperty.arraySize - 1);
                        prop.FindPropertyRelative("MoveID").intValue = MoveID;
                        _serializedObject.ApplyModifiedProperties();
                        MoveID++;
                        this.Repaint();
                    // }
                    // else
                    // {
                    //     Debug.Log("This ID has been used, select another one");
                    // }
                }

                GUILayout.EndHorizontal();

                moveScrollBar = GUILayout.BeginScrollView(moveScrollBar);

                string[] movesName = (from movedata in movedatas select movedata.MoveID + "||" + movedata.innerName)
                    .ToArray();
                if (movesName.Length != 0)
                    moveIndex = GUILayout.SelectionGrid(moveIndex, movesName, 1);

                GUILayout.EndScrollView();


                GUILayout.FlexibleSpace();

                GUILayout.BeginHorizontal();
                GUILayout.Label("File Name:");
                fileName = EditorGUILayout.TextField(fileName);
                if (GUILayout.Button("Save"))
                {
                    movedatas = _monoEdition.MoveDatas;
                    SaveLoad.Save(fileName, movedatas, @"Assets\Resources\PokemonData\");
                }

                GUILayout.Space(20);
                if (GUILayout.Button("Load"))
                {
                    movedatas = SaveLoad.Load<List<MoveData>>(fileName, @"Assets\Resources\PokemonData\");
                    movedatas.Sort((o1, o2) => { return o1.MoveID - o2.MoveID; });
                    _monoEdition.MoveDatas = movedatas;
                    _serializedObject = new SerializedObject(_monoEdition);
                    _currentProperty =_serializedObject.FindProperty("MoveDatas");
                }

                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();
            #endregion

            if (CurrentMove == null || movedatas.Count == 0)
            {
                    
            }
            else
            {
                GUILayout.BeginVertical();
                // Debug.Log($"the current property is array? {_currentProperty.isArray} total size {_currentProperty.arraySize} and current index: {moveIndex}");
                var ppp = _currentProperty.GetArrayElementAtIndex(moveIndex);
                DrawProperties(ppp,true);
                GUILayout.EndVertical();
            }
            
            
            GUILayout.EndHorizontal();
            
            
        }


        protected SerializedObject _serializedObject;
        protected SerializedProperty _currentProperty;

        protected void DrawProperties(SerializedProperty prop, bool drawChildren)
        {
            string lastPropPath = string.Empty;
            foreach (SerializedProperty p in prop)
            {
                if (p.isArray && p.propertyType == SerializedPropertyType.Generic)
                {
                    // EditorGUILayout.BeginHorizontal();
                    // p.isExpanded = EditorGUILayout.Foldout(p.isExpanded, p.displayName);
                    // EditorGUILayout.EndHorizontal();
                    //
                    // if (p.isExpanded)
                    // {
                    //     EditorGUI.indentLevel++;
                    //     DrawProperties(p,drawChildren);
                    //     EditorGUI.indentLevel--;
                    // }
                    if (!string.IsNullOrEmpty(lastPropPath) && p.propertyPath.Contains(lastPropPath))
                    {
                        continue;
                    }

                    lastPropPath = p.propertyPath;
                    EditorGUILayout.PropertyField(p, drawChildren);
                }
                else
                {
                    if (!string.IsNullOrEmpty(lastPropPath) && p.propertyPath.Contains(lastPropPath))
                    {
                        continue;
                    }

                    lastPropPath = p.propertyPath;
                    EditorGUILayout.PropertyField(p, drawChildren);
                }
            }
        }
       
    }
}