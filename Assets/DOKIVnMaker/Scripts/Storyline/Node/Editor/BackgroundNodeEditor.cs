using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace DokiVnMaker.Story
{
    [CustomNodeEditor(typeof(BackgroundImage))]
    public class BackgroundNodeEditor : NodeEditor
    {
        public override void OnBodyGUI()
        {
            serializedObject.Update();

            var node = target as BackgroundImage;

            GUILayout.BeginHorizontal();
            NodeEditorGUILayout.PortField(GUIContent.none, target.GetInputPort("input"), GUILayout.MinWidth(0));
            NodeEditorGUILayout.PortField(GUIContent.none, target.GetOutputPort("output"), GUILayout.MinWidth(0));
            GUILayout.EndHorizontal();

            GUILayout.Space(-15);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("image"), GUIContent.none);
            var imgPriveiw = node.image;
            if (imgPriveiw != null) GUILayout.Label(imgPriveiw.texture, GUILayout.Width(200), GUILayout.Height(113));
        }

        public override int GetWidth()
        {
            return 250;
        }
    }
}