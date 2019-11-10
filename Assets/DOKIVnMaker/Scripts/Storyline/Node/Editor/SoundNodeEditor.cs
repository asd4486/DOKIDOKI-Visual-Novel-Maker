using UnityEngine;
using UnityEditor;
using XNodeEditor;

namespace DokiVnMaker.Story
{
    [CustomNodeEditor(typeof(Sound))]
    public class SoundNodeEditor : StoryNodeEditorBase
    {
        public override void OnBodyGUI()
        {
            serializedObject.Update();

            var node = target as Sound;

            DrawPorts();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("audio"), GUIContent.none);

            EditorGUILayout.Slider(serializedObject.FindProperty("volume"), 0, 1);

            EditorGUILayout.IntSlider(serializedObject.FindProperty("track"), 0, 9);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("loop"));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("fadeIn"));
            if (node.fadeIn)      
                DrawDurationField();
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}