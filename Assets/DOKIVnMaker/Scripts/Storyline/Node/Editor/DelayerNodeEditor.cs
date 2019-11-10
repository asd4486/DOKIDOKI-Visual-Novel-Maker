using UnityEngine;
using UnityEditor;
using XNodeEditor;

namespace DokiVnMaker.Story
{
    [CustomNodeEditor(typeof(Delayer))]
    public class DelayerNodeEditor : StoryNodeEditorBase
    {
        public override void OnBodyGUI()
        {
            serializedObject.Update();

            var node = target as Delayer;

            DrawPorts();

            GUILayout.BeginHorizontal();
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("delay"), GUIContent.none);
            GUILayout.Label("s", GUILayout.Width(15));
            GUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }
    }
}