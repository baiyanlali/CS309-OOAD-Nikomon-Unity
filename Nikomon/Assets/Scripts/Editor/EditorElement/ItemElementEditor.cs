using System;
using PokemonCore.Inventory;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Editor.EditorElement
{
    [CustomPropertyDrawer(typeof(Item))]
    public class ItemElementEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            using (new EditorGUI.PropertyScope(position,label,property))
            {
                var nameProperty = property.FindPropertyRelative("name");
                var idProperty = property.FindPropertyRelative("ID");
                UnityEditor.EditorGUILayout.LabelField("ID", idProperty.intValue.ToString());
                nameProperty.stringValue = UnityEditor.EditorGUILayout.TextField("Name", nameProperty.stringValue);
            }
        }
    }
}