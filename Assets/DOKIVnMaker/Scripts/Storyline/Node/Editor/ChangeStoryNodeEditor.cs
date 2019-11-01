using UnityEngine;
using UnityEditor;
using XNodeEditor;

namespace DokiVnMaker.StoryNode
{
    [CustomNodeEditor(typeof(ChangeStory))]
    public class ChangeStoryNodeEditor : NodeEditor
    {
        public override void OnBodyGUI()
        {
            serializedObject.Update();

            var node = target as ChangeStory;

            GUILayout.BeginHorizontal();
            NodeEditorGUILayout.PortField(GUIContent.none, target.GetInputPort("input"), GUILayout.MinWidth(0));
            NodeEditorGUILayout.PortField(GUIContent.none, target.GetOutputPort("output"), GUILayout.MinWidth(0));
            GUILayout.EndHorizontal();

            GUILayout.Space(-15);

            //EditorGUILayout.PropertyField(serializedObject.FindProperty("character"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("nextStoryGraph"), GUIContent.none);

            serializedObject.ApplyModifiedProperties();
        }
    }
}