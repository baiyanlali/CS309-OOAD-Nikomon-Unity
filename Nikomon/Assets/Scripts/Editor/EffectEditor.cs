using System;
using System.Collections.Generic;
using System.Linq;
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

            if (CurrentEffect != null)
                EditEffect();

            GUILayout.EndHorizontal();
        }

        private void CreateSaveLoadAndSelectPanel()
        {
            GUILayout.BeginVertical(GUILayout.MaxWidth(100));

            GUILayout.BeginHorizontal();
            EffectID = EditorGUILayout.IntField("EffectID:", EffectID);
            if (GUILayout.Button("Add"))
            {
                Effects.Add(new Effect(EffectID));
            }

            GUILayout.EndHorizontal();

            EffectScrollBar = GUILayout.BeginScrollView(EffectScrollBar);

            if (Effects.Count > 0)
            {
                string[] movesName =
                    (from effect in Effects select effect.EffectID + "||" + effect.innerName).ToArray();
                if (movesName.Length != 0)
                    EffectIndex = GUILayout.SelectionGrid(EffectIndex, movesName, 1);
            }


            GUILayout.EndScrollView();


            GUILayout.FlexibleSpace();

            GUILayout.BeginHorizontal();
            GUILayout.Label("File Name:");
            fileName = EditorGUILayout.TextField(fileName);
            if (GUILayout.Button("Save"))
            {
                SaveLoad.Save(fileName, Effects);
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
                if (p.PropertyType.Equals(typeof(int)))
                {
                    p.SetValue(CurrentEffect, EditorGUILayout.IntField(p.Name, (int)p.GetValue(CurrentEffect)));
                }
                else if (p.PropertyType.Equals(typeof(string)))
                {
                    p.SetValue(CurrentEffect, EditorGUILayout.TextField(p.Name, (string)p.GetValue(CurrentEffect)));
                }
                else if (p.PropertyType.Equals(typeof(EffectLastType)))
                {
                    p.SetValue(CurrentEffect,
                        EditorGUILayout.EnumPopup(p.Name, (EffectLastType)p.GetValue(CurrentEffect)));
                }
                else if (p.PropertyType.Equals(typeof(List<EffectElement>)))
                {
                    EditEffectElements((List<EffectElement>)p.GetValue(CurrentEffect));
                }
                else if (p.PropertyType.Equals(typeof(List<Condition>)))
                {
                    EditEffectCondition((List<Condition>)p.GetValue(CurrentEffect));
                }
            }

            GUILayout.EndVertical();
        }

        private void EditEffectCondition(List<Condition> conditions)
        {
            GUILayout.BeginVertical();
            
            GUILayout.BeginHorizontal();
            GUILayout.Label($"Effect Trigger Conditions: {conditions.Count}");
            if (GUILayout.Button("+",GUILayout.Width(50)))
            {
                conditions.Add(new Condition());
            }
            GUILayout.Space(50);
            if (GUILayout.Button("-",GUILayout.Width(50)))
            {
                conditions.RemoveAt(conditions.Count - 1);
            }
            
            GUILayout.EndHorizontal();
            
            
            for (int i = 0; i < conditions.Count; i++)
            {
                GUILayout.BeginHorizontal();
                Condition con = conditions[i];
                con.TargetType = (EffectTargetType)EditorGUILayout.EnumPopup(con.TargetType);
                con.property = EditorGUILayout.TextField(con.property);
                con.mode = (ConditionMode)EditorGUILayout.EnumPopup(con.mode);
                con.treshhold = EditorGUILayout.FloatField(con.treshhold);
                con.effectResultType = (EffectResultType)EditorGUILayout.EnumPopup(con.effectResultType);
                GUILayout.EndHorizontal();
            }

            GUILayout.EndVertical();
        }

        private Vector2 EffectElementScroll;
        private void EditEffectElements(List<EffectElement> eff)
        {
            GUILayout.BeginVertical();
            
            GUILayout.BeginHorizontal();
            
            GUILayout.Label($"Effect Element: {eff.Count}");
            
            if (GUILayout.Button("+",GUILayout.Width(50)))
            {
                eff.Add(new EffectElement("innerName"));
            }
            GUILayout.Space(50);
            if (GUILayout.Button("-",GUILayout.Width(50)))
            {
                eff.RemoveAt(eff.Count-1);
            }
            
            GUILayout.EndHorizontal();

            EffectElementScroll=GUILayout.BeginScrollView(EffectElementScroll);
            for (int i = 0; i < eff.Count; i++)
            {
                GUILayout.BeginHorizontal();
                eff[i].innerName = EditorGUILayout.TextField("inner name:", eff[i].innerName);
                eff[i].EffectElementChance=EditorGUILayout.IntField("Chance:",eff[i].EffectElementChance);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                eff[i].EffectType=(EffectType)EditorGUILayout.EnumPopup("Effect Type:",eff[i].EffectType);
                eff[i].WhenToAct=(EffectActTime)EditorGUILayout.EnumPopup("WhenToAct",eff[i].WhenToAct);
                
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                eff[i].TargetType=(EffectTargetType)EditorGUILayout.EnumPopup("Target Type:", eff[i].TargetType);
                eff[i].Property=EditorGUILayout.TextField("Property:", eff[i].Property);
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                eff[i].DependType=(EffectTargetType)EditorGUILayout.EnumPopup("Depend Type:", eff[i].DependType);
                eff[i].ValueDependProperty=EditorGUILayout.TextField("Property:", eff[i].ValueDependProperty);
                GUILayout.EndHorizontal();
                
                GUILayout.BeginHorizontal();
                eff[i].value = EditorGUILayout.IntField("value:", eff[i].value);
                if (eff[i].DependType != EffectTargetType.NULL)
                    eff[i].ResultType = (EffectResultType)EditorGUILayout.EnumPopup("Result Type:", eff[i].ResultType);
                else
                {
                    eff[i].ResultType = EffectResultType.RatioValue;
                    GUILayout.Label($"Result Type: {eff[i].ResultType}");
                }
                GUILayout.EndHorizontal();
                GUILayout.Space(10);
            }
            GUILayout.EndScrollView();
            GUILayout.Space(50);
            // EditorGUILayout.Foldout();
            GUILayout.EndVertical();
        }
    }
}