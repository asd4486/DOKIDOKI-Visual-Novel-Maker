using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace DokiVnMaker.StoryNode
{
    [CustomNodeEditor(typeof(StartPoint))]
    public class StartPointNodeEditor : NodeEditor
    {
        public override void OnBodyGUI()
        {
            serializedObject.Update();

            GUILayout.Space(-20);
            NodeEditorGUILayout.PortField(GUIContent.none, target.GetOutputPort("output"), GUILayout.MinWidth(0));
        }

        public override int GetWidth()
        {
            return 180;
        }
    }
}