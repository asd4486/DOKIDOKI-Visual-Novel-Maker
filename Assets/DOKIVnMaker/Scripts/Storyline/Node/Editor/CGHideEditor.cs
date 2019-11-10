using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace DokiVnMaker.Story
{
    [CustomNodeEditor(typeof(CGHide))]
    public class CGHideEditor : StoryNodeEditorBase
    {
        public override void OnBodyGUI()
        {
            serializedObject.Update();
            DrawPorts();

            DrawDurationField();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("isWait"));
            serializedObject.ApplyModifiedProperties();
        }
    }
}