using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class NatureEditor : EditorWindow
    {
        [MenuItem("PokemonTools/Edit Nature")]
        private static void ShowWindow()
        {
            var window = GetWindow<NatureEditor>();
            window.titleContent = new GUIContent("Nature Editor");
            window.Show();
        }

        private void OnGUI()
        {
            
        }
    }
}