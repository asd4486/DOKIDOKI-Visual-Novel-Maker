using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace DokiVnMaker.Story
{
    [CustomNodeEditor(typeof(BackgroundImage))]
    public class BackgroundNodeEditor : StoryNodeEditorBase
    {
        public override void OnBodyGUI()
        {
            serializedObject.Update();

            var node = target as BackgroundImage;

            DrawPorts();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("image"), GUIContent.none);
            var imgPriveiw = node.image;
            if (imgPriveiw != null) GUILayout.Label(imgPriveiw.texture, GUILayout.Width(200), GUILayout.Height(113));
            serializedObject.ApplyModifiedProperties();
        }
    }
}