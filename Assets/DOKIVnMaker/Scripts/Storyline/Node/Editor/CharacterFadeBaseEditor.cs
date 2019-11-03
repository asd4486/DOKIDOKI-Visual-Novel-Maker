using UnityEngine;
using UnityEditor;
using XNodeEditor;

namespace DokiVnMaker.Story
{
    [CustomNodeEditor(typeof(CharacterFadeBase))]
    public class CharacterFadeBaseEditor : NodeEditor
    {
        public override void OnBodyGUI()
        {
            serializedObject.Update();

            //var node = target as CharacterFadeBase;

            GUILayout.BeginHorizontal();
            NodeEditorGUILayout.PortField(GUIContent.none, target.GetInputPort("input"), GUILayout.MinWidth(0));
            NodeEditorGUILayout.PortField(GUIContent.none, target.GetOutputPort("output"), GUILayout.MinWidth(0));
            GUILayout.EndHorizontal();

            GUILayout.Space(-15);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("character"), GUIContent.none);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Duration", GUILayout.Width(70));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("duration"), GUIContent.none);
            GUILayout.Label("s", GUILayout.Width(15));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Is wait", GUILayout.Width(70));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("isWait"), GUIContent.none);
            GUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }

        public override int GetWidth()
        {
            return 200;
        }
    }
}