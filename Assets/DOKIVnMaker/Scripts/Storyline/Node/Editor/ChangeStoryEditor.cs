using UnityEngine;
using UnityEditor;
using XNodeEditor;

namespace DokiVnMaker.Story
{
    [CustomNodeEditor(typeof(ChangeStory))]
    public class ChangeStoryEditor : StoryNodeEditorBase
    {
        public override void OnBodyGUI()
        {
            serializedObject.Update();

            //var node = target as ChangeStory;
            DrawPorts();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("nextStoryGraph"), GUIContent.none);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("autoPlay"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}