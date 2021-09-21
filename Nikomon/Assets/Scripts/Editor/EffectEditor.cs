using System;
using System.Collections.Generic;
using System.Linq;
using Codice.CM.SEIDInfo;
using PokemonCore.Attack.Data;
using PokemonCore.Combat;
using PokemonCore.Combat.Interface;
using PokemonCore.Utility;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class EffectEditor : EditorWindow
    {

        private int EffectID;
        private int EffectIndex;
        private List<Effect> Effects;
        private string fileName;

        private Effect CurrentEffect
        {
            get => EffectID >= Effects.Count ? null : EffectID < 0 ? null : Effects[EffectID];
        }

        private Vector2 EffectScrollBar;
        

        [MenuItem("PokemonTools/Edit Effect")]
        private static void ShowWindow()
        {
            var window = GetWindow<EffectEditor>();
            window.titleContent = new GUIContent("Effect Editor");
            window.Show();
        }

        private void CreateGUI()
        {
            Effects = new List<Effect>();
            fileName = "effects";
        }

        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            
            CreateSaveLoadAndSelectPanel();
            
            if(CurrentEffect!=null)
                EditEffect();
            
            GUILayout.EndHorizontal();
        }

        private void CreateSaveLoadAndSelectPanel()
        {
            GUILayout.BeginVertical(GUILayout.MaxWidth(100));
            
            GUILayout.BeginHorizontal();
            EffectID = EditorGUILayout.IntField("EffectID:",EffectID);
            if (GUILayout.Button("Add"))
            {
                Effects.Add(new Effect(EffectID));
            }
            GUILayout.EndHorizontal();

            EffectScrollBar = GUILayout.BeginScrollView(EffectScrollBar);
            
                string[] movesName = (from effect in Effects select effect.EffectID+"||"+effect.innerName).ToArray();
                if(movesName.Length!=0)
                    EffectIndex = GUILayout.SelectionGrid(EffectIndex,movesName,1);
            
            GUILayout.EndScrollView();
            
            
            GUILayout.FlexibleSpace();
            
            GUILayout.BeginHorizontal();
                GUILayout.Label("File Name:");
                fileName = EditorGUILayout.TextField(fileName);
                if (GUILayout.Button("Save"))
                {
                    SaveLoad.Save(fileName,Effects);
                }
                GUILayout.Space(20);
                if (GUILayout.Button("Load"))
                {
                    Effects = SaveLoad.Load<List<Effect>>(fileName);
                    // Effects.Sort((o1, o2) => {return o1.MoveID - o2.MoveID;});
                }
            GUILayout.EndHorizontal();
            
            GUILayout.EndVertical();
        }

        private void EditEffect()
        {
            GUILayout.BeginVertical();

            Type type = typeof(Effect);

            var ps = type.GetProperties();

            foreach (var p in ps)
            {
                // if (p.GetType() is )
                // {
                //     EditorGUILayout.IntField(p.Name, (int)p.GetValue(int));
                // }
            }
            
            GUILayout.EndVertical();
        }
        
    }
}