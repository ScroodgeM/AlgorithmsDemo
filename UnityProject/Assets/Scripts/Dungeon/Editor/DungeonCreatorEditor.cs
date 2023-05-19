//this empty line for UTF-8 BOM header
using UnityEditor;
using UnityEngine;

namespace AlgorithmsDemo.Dungeon.Editor
{
    [CustomEditor(typeof(DungeonCreator))]
    public class DungeonCreatorEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Clear"))
                {
                    ((DungeonCreator)target).Clear();
                }

                if (GUILayout.Button("Generate"))
                {
                    ((DungeonCreator)target).GenerateDungeon();
                }

                if (GUILayout.Button("Generate w/ new seed"))
                {
                    serializedObject.FindProperty("seed").intValue = Random.Range(int.MinValue, int.MaxValue);
                    serializedObject.ApplyModifiedProperties();
                    ((DungeonCreator)target).GenerateDungeon();
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}
