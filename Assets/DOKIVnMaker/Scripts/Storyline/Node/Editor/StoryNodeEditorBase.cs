using UnityEngine;
using UnityEditor;
using XNodeEditor;

namespace DokiVnMaker.Story
{
    [CustomNodeEditor(typeof(StoryNodeBase))]
    public class StoryNodeEditorBase : NodeEditor
    {
        public void DrawPorts()
        {
            GUILayout.BeginHorizontal();
            NodeEditorGUILayout.PortField(GUIContent.none, target.GetInputPort("input"), GUILayout.MinWidth(0));
            NodeEditorGUILayout.PortField(GUIContent.none, target.GetOutputPort("output"), GUILayout.MinWidth(0));
            GUILayout.EndHorizontal();
            GUILayout.Space(-15);
        }

        public void DrawDurationField()
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("duration"));
            GUILayout.Label("s", GUILayout.Width(15));
            GUILayout.EndHorizontal();
        }
    }
}