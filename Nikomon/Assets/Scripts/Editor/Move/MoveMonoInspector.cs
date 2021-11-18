using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(MoveMonoEdition))]
    public class MoveMonoInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Open Editor"))
            {
                MoveEditorNew.Open(target as MoveMonoEdition);
            }
        }
    }
}