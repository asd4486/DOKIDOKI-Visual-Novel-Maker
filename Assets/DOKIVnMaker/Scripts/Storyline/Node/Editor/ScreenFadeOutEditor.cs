using UnityEngine;
using UnityEditor;
using XNodeEditor;

namespace DokiVnMaker.Story
{
    [CustomNodeEditor(typeof(ScreenFadeOut))]
    public class ScreenFadeOutEditor : StoryNodeEditorBase
    {
        public override void OnBodyGUI()
        {
            serializedObject.Update();

            var node = target as ScreenFadeOut;

            DrawPorts();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("color"));
            DrawDurationField();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("isWait"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}