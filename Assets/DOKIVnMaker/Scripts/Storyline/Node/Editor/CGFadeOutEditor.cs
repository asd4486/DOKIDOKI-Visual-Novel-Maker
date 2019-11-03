using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace DokiVnMaker.Story
{
    [CustomNodeEditor(typeof(CGFadeOut))]
    public class CGFadeOutEditor : NodeEditor
    {
        public override void OnBodyGUI()
        {
            serializedObject.Update();

            GUILayout.BeginHorizontal();
            NodeEditorGUILayout.PortField(GUIContent.none, target.GetInputPort("input"), GUILayout.MinWidth(0));
            NodeEditorGUILayout.PortField(GUIContent.none, target.GetOutputPort("output"), GUILayout.MinWidth(0));
            GUILayout.EndHorizontal();

            GUILayout.Space(-15);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Duration", GUILayout.Width(70));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("duration"), GUIContent.none);
            GUILayout.Label("s", GUILayout.Width(15));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Is wait", GUILayout.Width(70));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("isWait"), GUIContent.none);
            GUILayout.EndHorizontal();
        }

        public override int GetWidth()
        {
            return 200;
        }
    }
}