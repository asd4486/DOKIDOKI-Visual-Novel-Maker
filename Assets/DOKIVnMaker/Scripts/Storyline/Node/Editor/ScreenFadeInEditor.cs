using UnityEngine;
using UnityEditor;
using XNodeEditor;

namespace DokiVnMaker.Story
{
    [CustomNodeEditor(typeof(ScreenFadeIn))]
    public class ScreenFadeInEditor : StoryNodeEditorBase
    {
        public override void OnBodyGUI()
        {
            serializedObject.Update();

            var node = target as ScreenFadeIn;

            DrawPorts();

            DrawDurationField();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("isWait"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}