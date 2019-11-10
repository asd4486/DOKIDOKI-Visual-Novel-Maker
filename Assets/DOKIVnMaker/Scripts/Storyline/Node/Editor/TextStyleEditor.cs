using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace DokiVnMaker.Story
{
    [CustomNodeEditor(typeof(TextStyle))]
    public class TextStyleEditor : StoryNodeEditorBase
    {
        public override void OnBodyGUI()
        {
            serializedObject.Update();

            var node = target as Sound;
            DrawPorts();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("style"), GUIContent.none);
            serializedObject.ApplyModifiedProperties();
        }
    }
}
