using UnityEditor;
using UnityEngine;

namespace Editor
{
    public class NatureEditor : EditorWindow
    {
        [MenuItem("MENUITEM/MENUITEMCOMMAND")]
        private static void ShowWindow()
        {
            var window = GetWindow<NatureEditor>();
            window.titleContent = new GUIContent("TITLE");
            window.Show();
        }

        private void OnGUI()
        {
            
        }
    }
}