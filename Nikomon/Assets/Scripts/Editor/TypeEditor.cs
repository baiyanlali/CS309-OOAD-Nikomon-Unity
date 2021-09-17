using PokemonCore;
using UnityEditor;
using UnityEngine;
// using System.Text.Json;
// using System.Text.Json.Serialization;

namespace Editor
{
    public class TypeEditor : EditorWindow
    {
        public TypeRelationship[,] types;
        public int typeNum;
        
        
        
        [MenuItem("PokemonTools/Edit Type")]
        private static void ShowWindow()
        {
            
            var window = GetWindow<TypeEditor>();
            window.titleContent = new GUIContent("Type Editor");
            window.Show();
        }

        private void OnGUI()
        {
            typeNum = 2;
            types=new TypeRelationship[typeNum,typeNum];
            if (GUILayout.Button("Load"))
            {
                
            }
            if (GUILayout.Button("Save"))
            {
                
            }
            if (GUILayout.Button("Add Type"))
            {
                
            }
            
            GUILayout.BeginArea(Rect.MinMaxRect(20,20,1000,1000));
            {
                
                for (int i = 0; i < typeNum; i++)
                {
                    GUILayout.Label(i.ToString());

                }
                for (int i = 0; i < typeNum; i++)
                {
                    GUILayout.Label(i.ToString());
                    for (int j = 0; j < typeNum; j++)
                    {
                        
                    }
                }
            }
        }
    }
}