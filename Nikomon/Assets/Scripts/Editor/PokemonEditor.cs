using UnityEditor;
using UnityEngine;
// using System.Text.Json;
// using System.Text.Json.Serialization;

namespace Editor
{
    public class PokemonEditor : EditorWindow
    {
        [MenuItem("PokemonTools/Edit Pokemon")]
        private static void ShowWindow()
        {
            var window = GetWindow<PokemonEditor>();
            window.titleContent = new GUIContent("PokemonEditor");
            window.Show();
        }

        private void OnGUI()
        {
            
        }
    }
}