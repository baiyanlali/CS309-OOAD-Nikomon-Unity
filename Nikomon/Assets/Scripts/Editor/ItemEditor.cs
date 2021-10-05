using System;
using System.Collections.Generic;
using PokemonCore.Inventory;
using PokemonCore.Utility;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Editor
{
    public class ItemEditor : EditorWindow
    {
        [MenuItem("PokemonTools/Edit Item")]
        private static void ShowWindow()
        {
            var window = GetWindow<ItemEditor>();
            window.titleContent = new GUIContent("Item Editor");
            window.Show();
        }

        private int tagIndex;
        private void CreateGUI()
        {
            items = new Dictionary<Item.Tag, List<Item>>();
            for (int i = 0; i < Enum.GetNames(typeof(Item.Tag)).Length; i++)
            {
                items.Add((Item.Tag)i,new List<Item>());
            }
        }

        private Dictionary<PokemonCore.Inventory.Item.Tag, List<Item>> items;
        private ReorderableList _reorderableList;
        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            {
                int taggg = tagIndex;
                tagIndex = GUILayout.SelectionGrid(tagIndex,Enum.GetNames(typeof(Item.Tag)),1);
                if (taggg != tagIndex)
                {
                     var arr = items[(Item.Tag) tagIndex];
                     _reorderableList = new ReorderableList(arr, typeof(Item), true, true, true, true);
                     _reorderableList.drawHeaderCallback = (rect) => { GUILayout.Label($"Edit: {(Item.Tag)tagIndex}");};
                     _reorderableList.drawElementCallback = (rect, index, selected, focused) =>
                     {
                        
                         var item = _reorderableList.list[index];
                         var i = item as Item;
                         GUILayout.Label(i.ID.ToString());
                         i.name = EditorGUILayout.TextField("Name",i.name);
                     };
                     _reorderableList.onAddCallback = (list) =>
                     {
                         list.list.Add(new Item((Item.Tag) tagIndex, list.list.Count, "Check here"));
                     };
                     _reorderableList.onRemoveCallback = (list) =>
                     {
                         ReorderableList.defaultBehaviours.DoRemoveButton(list);
                     };
                }
            }
            
            GUILayout.FlexibleSpace();

            EditorUtil<Dictionary<Item.Tag, List<Item>>>.OnLoad = (o) =>
            {
                items = o;
            };
            EditorUtil<Dictionary<Item.Tag, List<Item>>>.EditSaveLoad(items,"items");
            GUILayout.EndVertical();  
            
            GUILayout.BeginVertical();
            {
                if(_reorderableList!=null)
                    _reorderableList.DoLayoutList();

            }
            GUILayout.EndVertical();  
            
            
            GUILayout.BeginHorizontal();
        }
    }
}