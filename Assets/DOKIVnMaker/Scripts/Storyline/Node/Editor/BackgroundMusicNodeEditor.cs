using UnityEngine;
using UnityEditor;
using XNodeEditor;

namespace DokiVnMaker.Story
{
    [CustomNodeEditor(typeof(BackgroundMusic))]
    public class BackgroundMusicNodeEditor : StoryNodeEditorBase
    {
        public override void OnBodyGUI()
        {
            serializedObject.Update();

            var node = target as BackgroundMusic;

            DrawPorts();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("audio"), GUIContent.none);

            GUILayout.BeginHorizontal();
            EditorGUILayout.Slider(serializedObject.FindProperty("volume"), 0, 1);
            GUILayout.EndHorizontal();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("loop"));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("fadeIn"));
            if (node.fadeIn)
            {
                DrawDurationField();
            }
            serializedObject.ApplyModifiedProperties();
        }
    }
}