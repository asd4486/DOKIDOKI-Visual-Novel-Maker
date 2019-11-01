using UnityEngine;
using UnityEditor;
using XNodeEditor;

namespace DokiVnMaker.StoryNode
{
    [CustomNodeEditor(typeof(Delayer))]
    public class DelayerNodeEditor : NodeEditor
    {
        public override void OnBodyGUI()
        {
            serializedObject.Update();

            var node = target as Delayer;

            GUILayout.BeginHorizontal();
            NodeEditorGUILayout.PortField(GUIContent.none, target.GetInputPort("input"), GUILayout.MinWidth(0));
            NodeEditorGUILayout.PortField(GUIContent.none, target.GetOutputPort("output"), GUILayout.MinWidth(0));
            GUILayout.EndHorizontal();

            GUILayout.Space(-15);

            GUILayout.BeginHorizontal();
            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("delay"), GUIContent.none);
            GUILayout.Label("s", GUILayout.Width(30));
            GUILayout.EndHorizontal();
        }

        public override int GetWidth()
        {
            return 180;
        }
    }
}