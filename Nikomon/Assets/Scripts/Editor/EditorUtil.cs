using System.Collections.Generic;
using System.Linq;
using PokemonCore.Combat.Interface;
using PokemonCore.Utility;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    public static class EditorUtil
    {
        public static void EditConditions(List<PokemonCore.Utility.Condition> conditions)
        {
            
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
                EditCondition(con);
                
                GUILayout.EndHorizontal();
            }
        }

        private static int ConditionIndex = 0;
        public static void EditCondition(Condition con)
        {
            con.TargetType = (EffectTargetType)EditorGUILayout.EnumPopup(con.TargetType);
            if (con.type!= null)
            {
                string[] strs = (from property in con.type.GetProperties() select property.Name).ToArray();
                ConditionIndex = EditorGUILayout.Popup( ConditionIndex, strs);
                con.property = strs[ConditionIndex];
            }
            else
            {
                con.property = EditorGUILayout.TextField(con.property);

            }
            con.mode = (ConditionMode)EditorGUILayout.EnumPopup(con.mode);
            con.treshhold = EditorGUILayout.FloatField(con.treshhold);
            con.effectResultType = (EffectResultType)EditorGUILayout.EnumPopup(con.effectResultType);
        }
        
    }
}