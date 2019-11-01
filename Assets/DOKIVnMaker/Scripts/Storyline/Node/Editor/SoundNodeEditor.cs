using UnityEngine;
using UnityEditor;
using XNodeEditor;

namespace DokiVnMaker.StoryNode
{
    [CustomNodeEditor(typeof(Sound))]
    public class SoundNodeEditor : NodeEditor
    {
        public override void OnBodyGUI()
        {
            serializedObject.Update();

            var node = target as Sound;

            GUILayout.BeginHorizontal();
            NodeEditorGUILayout.PortField(GUIContent.none, target.GetInputPort("input"), GUILayout.MinWidth(0));
            NodeEditorGUILayout.PortField(GUIContent.none, target.GetOutputPort("output"), GUILayout.MinWidth(0));
            GUILayout.EndHorizontal();

            GUILayout.Space(-15);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("audio"), GUIContent.none);

            GUILayout.BeginHorizontal();
            GUILayout.Label("Volume", GUILayout.Width(50));
            EditorGUILayout.Slider(serializedObject.FindProperty("volume"), 0, 1, GUIContent.none, GUILayout.MaxWidth(165));
            GUILayout.EndHorizontal();

            EditorGUILayout.IntSlider(serializedObject.FindProperty("track"), 0, 10);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("loop"));
        }

        public override int GetWidth()
        {
            return 250;
        }
    }
}