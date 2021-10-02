using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using PokemonCore;
using UnityEditor;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Newtonsoft.Json;
using PokemonCore.Utility;
using Types = PokemonCore.Types;

namespace Editor
{
    public class TypeEditor : EditorWindow
    {
        public TypeRelationship[,] types;
        public string[] TypeName;
        public int typeNum;
        public string fileName;
        public Vector2 viewField;
        private void CreateGUI()
        {
            Debug.Log("GUI Created");
            if (types == null)
            {
                typeNum = 2;
                types=new TypeRelationship[typeNum,typeNum];
                TypeName = new string[typeNum];
                for (int i = 0; i < typeNum; i++)
                {
                    TypeName[i] = "";
                }
            }
        }

        private void OnGUI()
        {
            

            GUILayout.BeginHorizontal();
            {
                //正常操作
                GUILayout.BeginVertical(GUILayout.MinWidth(100),GUILayout.MaxWidth(300));
                {
                    GUILayout.BeginHorizontal();
                    // GUILayout.Label("File Name:");
                    fileName = EditorGUILayout.TextField("File Name:",fileName);
                    string filePath="Assets//Data//"+fileName+".json";
                    // EditorUtility.SetDirty(this);
                    this.Repaint();
                    GUILayout.EndHorizontal();
                    if (GUILayout.Button("Load"))
                    {
                        // if (!File.Exists(filePath)) return;
                        // StreamReader sr = File.OpenText(filePath);
                        // string data = sr.ReadToEnd();
                        Types[] typesArray = SaveLoad.Load<Types[]>("types.json",@"Assets\Resources\PokemonData\");
                        // JsonConvert.DeserializeObject<Types[]>(data);
                        typeNum = typesArray.Length;
                        types = new TypeRelationship[typeNum, typeNum];
                        TypeName = new string[typeNum];
                        for (int i = 0; i < typeNum; i++)
                        {
                            TypeName[i] = typesArray[i].Name;
                            foreach (int index in typesArray[i].NEType)
                            {
                                types[i, index] = TypeRelationship.NotEffective;
                            }
                            foreach (int index in typesArray[i].SEType)
                            {
                                types[i, index] = TypeRelationship.SuperEffective;
                            }
                            foreach (int index in typesArray[i].NVEType)
                            {
                                types[i, index] = TypeRelationship.NotVeryEffective;
                            }
                            
                        }
                    }

                    if (GUILayout.Button("Save"))
                    {
                        List<Types> typesList = new List<Types>();
                        for (int i = 0; i < typeNum; i++)
                        {
                            Types t = new Types(i,TypeName[i]);
                            typesList.Add(t);
                        }
                        for (int i = 0; i < typeNum; i++)
                        {

                            for (int j = 0; j < typeNum; j++)
                            {
                                switch (types[i, j])
                                {
                                    case TypeRelationship.Effective : 
                                        break;
                                    case TypeRelationship.NotEffective: typesList[i].NEType.Add(typesList[j].ID);
                                        break;
                                    case TypeRelationship.SuperEffective: typesList[i].SEType.Add(typesList[j].ID);
                                        break;
                                    case TypeRelationship.NotVeryEffective: typesList[i].NVEType.Add(typesList[j].ID);
                                        break;
                                }
                            }
                            
                        }

                        SaveLoad.Save("types", typesList, @"Assets\Resources\PokemonData\");
                        // string data = JsonConvert.SerializeObject(typesList.ToArray());
                        // FileStream fs;
                        // if (File.Exists(filePath))
                        // {
                        //      fs = File.Create(filePath);
                        // }
                        // else
                        // {
                        //     fs = File.OpenWrite(filePath);
                        // }
                        //
                        // StreamWriter sw = new StreamWriter(fs);
                        // sw.Write(data);
                        // sw.Flush();
                        // sw.Close();
                        // fs.Close();
                        // Debug.Log($"Type json write to {filePath} successfully");
                    }

                    if (GUILayout.Button("Add Type"))
                    {
                        // Debug.Log($"Add type, now type number {typeNum}");
                        typeNum++;
                        TypeRelationship[,] tmp=new TypeRelationship[typeNum,typeNum];
                        string[] tmpString = new string[typeNum];
                        for (int i = 0; i < typeNum-1; i++)
                        {
                            tmpString[i] = TypeName[i];
                            for (int j = 0; j < typeNum-1; j++)
                            {
                                
                                tmp[i, j] = types[i, j];
                                
                            }
                        }

                        types = tmp;
                        TypeName = tmpString;
                        this.Repaint();
                    }

                }
                GUILayout.EndVertical();
                
                //编辑属性
                GUILayout.BeginVertical(GUILayout.MaxWidth(40));
                {
                    GUILayout.Label("Type",GUILayout.MaxWidth(40));
                    GUILayout.Label("",GUILayout.MaxWidth(40));
                    for (int i = 0; i < typeNum; i++)
                    {
                        GUILayout.BeginHorizontal();
                        GUILayout.Label(i.ToString()+":");
                        TypeName[i] = EditorGUILayout.TextField(TypeName[i]);
                        GUILayout.EndHorizontal();
                        EditorUtility.SetDirty(this);
                        this.Repaint();
                    }
                }
                GUILayout.EndVertical();
                
                GUILayout.BeginVertical();
                {
                    
                    GUILayout.BeginHorizontal();
                    {
                        for (int i = 0; i < typeNum; i++)
                        {
                            GUILayout.Label(i.ToString());
                            // TypeName[i] = GUILayout.TextField("");
                        }
                    }
                    GUILayout.EndHorizontal();
                    
                    GUILayout.BeginHorizontal();
                    {
                        for (int i = 0; i < typeNum; i++)
                        {
                            GUILayout.Label(TypeName[i]);
                            // TypeName[i] = GUILayout.TextField("");
                        }
                    }
                    GUILayout.EndHorizontal();

                    viewField = GUILayout.BeginScrollView(viewField);
                    for (int i = 0; i < typeNum; i++)
                    {
                        GUILayout.BeginHorizontal();
                        {
                            for (int j = 0; j < typeNum; j++)
                            {
                                types[i,j] = (TypeRelationship)EditorGUILayout.EnumPopup(types[i, j],GUILayout.MinWidth(100));
                                // GUILayout.Button(tag);
                                // GUILayout.Label(tag);
                            }
                        }
                        GUILayout.EndHorizontal();
                    }
                }
                GUILayout.EndScrollView();
                GUILayout.EndVertical();

                GUILayout.BeginVertical();
                
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();
        }


        [MenuItem("PokemonTools/Edit Type")]
        private static void ShowWindow()
        {
            
            var window = GetWindow<TypeEditor>();
            window.titleContent = new GUIContent("Type Editor");
            window.Show();
        }
    }
}