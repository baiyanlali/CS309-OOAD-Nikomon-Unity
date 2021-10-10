using System;
using System.Collections.Generic;
using System.Linq;
using PokemonCore.Combat.Interface;
using PokemonCore.Utility;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

namespace Editor
{
    public static class EditorUtil<T>
    {

        public static Action<T> OnLoad;

        public static void EditSaveLoad(T data,string name="")
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Save"))
            {
                SaveLoad.Save(name, data, @"Assets/Resources/PokemonData/");
            }

            if (GUILayout.Button("Load"))
            {
                OnLoad?.Invoke(SaveLoad.Load<T>(name, @"Assets/Resources/PokemonData/"));
            }
            GUILayout.EndHorizontal();
        }
        
        // public static void EditConditions(List<PokemonCore.Utility.Condition> conditions)
        // {
        //     GUILayout.BeginHorizontal();
        //     GUILayout.Label($"Effect Trigger Conditions: {conditions.Count}");
        //     if (GUILayout.Button("+", GUILayout.Width(50)))
        //     {
        //         conditions.Add(new Condition());
        //     }
        //
        //     GUILayout.Space(50);
        //     if (GUILayout.Button("-", GUILayout.Width(50)))
        //     {
        //         conditions.RemoveAt(conditions.Count - 1);
        //     }
        //
        //     GUILayout.EndHorizontal();
        //
        //     for (int i = 0; i < conditions.Count; i++)
        //     {
        //         GUILayout.BeginHorizontal();
        //         Condition con = conditions[i];
        //         EditCondition(con);
        //
        //         GUILayout.EndHorizontal();
        //     }
        // }

        // private static int ConditionIndex = 0;
        // private static int ConditionBoolIndex = 0;
        //
        // public static void EditCondition(Condition con)
        // {
        //     con.TargetType = (EffectTargetType)EditorGUILayout.EnumPopup(con.TargetType);
        //     if (con.type != null)
        //     {
        //         string[] strss = (from property in con.type.GetProperties() select property.Name).ToArray();
        //         ConditionIndex = EditorGUILayout.Popup(ConditionIndex, strss);
        //         if (ConditionIndex >= strss.Length) ConditionIndex = 0;
        //         con.property = strss[ConditionIndex];
        //         Debug.Log($"index:{ConditionIndex},{con.property},{strss[ConditionIndex]}");
        //         GUILayout.Label($"property:{con.property} && con.PropertyType{con.PropertyType}");
        //         #region PropertyType
        //
        //         if (con.PropertyType != null)
        //         {
        //             GUILayout.Label($"Type:{con.PropertyType.DeclaringType.Name}");
        //             if (con.PropertyType.DeclaringType == typeof(int))
        //             {
        //                 // con.mode = (ConditionMode)EditorGUILayout.EnumPopup(con.mode);
        //                 con.mode = (ConditionMode)EditorGUILayout.EnumPopup(new GUIContent("int"), con.mode,
        //                     (c) => (ConditionMode)c != ConditionMode.If && (ConditionMode)c != ConditionMode.IfNot,
        //                     true);
        //
        //                 con.treshholdInt = EditorGUILayout.IntField(con.treshholdInt);
        //             }
        //             else if (con.PropertyType.DeclaringType == typeof(byte))
        //             {
        //                 // con.mode = (ConditionMode)EditorGUILayout.EnumPopup(con.mode);
        //                 con.mode = (ConditionMode)EditorGUILayout.EnumPopup(new GUIContent("byte"), con.mode,
        //                     (c) => (ConditionMode)c != ConditionMode.If && (ConditionMode)c != ConditionMode.IfNot,
        //                     true);
        //
        //                 con.treshholdInt = EditorGUILayout.IntField(con.treshholdInt);
        //             }
        //             else if (con.PropertyType.DeclaringType == typeof(float))
        //             {
        //                 // con.mode = (ConditionMode)EditorGUILayout.EnumPopup(con.mode);
        //                 con.mode = (ConditionMode)EditorGUILayout.EnumPopup(new GUIContent("float"), con.mode,
        //                     (c) => (ConditionMode)c != ConditionMode.If && (ConditionMode)c != ConditionMode.IfNot,
        //                     true);
        //
        //                 con.treshholdFloat = EditorGUILayout.FloatField(con.treshholdFloat);
        //             }
        //             else if (con.PropertyType.DeclaringType == typeof(bool))
        //             {
        //                 //(ConditionMode)EditorGUILayout.EnumPopup(con.mode);
        //                 con.mode = (ConditionMode)EditorGUILayout.EnumPopup(new GUIContent("bool"), con.mode,
        //                     (c) => (ConditionMode)c == ConditionMode.If || (ConditionMode)c == ConditionMode.IfNot,
        //                     true);
        //                 ConditionBoolIndex =
        //                     EditorGUILayout.Popup(ConditionBoolIndex, new string[] { "True", "False" });
        //                 if (ConditionBoolIndex == 0) con.treshholdBool = true;
        //                 else con.treshholdBool = false;
        //             }
        //             else if (con.PropertyType.DeclaringType == typeof(string))
        //             {
        //                 // con.mode = (ConditionMode)EditorGUILayout.EnumPopup(con.mode);
        //                 con.mode = (ConditionMode)EditorGUILayout.EnumPopup(new GUIContent("string"), con.mode,
        //                     (c) => (ConditionMode)c == ConditionMode.Equal ||
        //                            (ConditionMode)c == ConditionMode.NotEqual,
        //                     true);
        //                 con.treshholdString = EditorGUILayout.TextField(con.treshholdString);
        //             }
        //             else GUILayout.Label($"Type:{con.PropertyType.DeclaringType}");
        //         }
        //
        //         #endregion
        //     }
        //     else
        //     {
        //         con.property = EditorGUILayout.TextField(con.property);
        //
        //       
        //     }
        //     // Debug.Log(con.type);
        //
        //
        //     con.effectResultType = (EffectResultType)EditorGUILayout.EnumPopup(con.effectResultType);
        // }
    }
}