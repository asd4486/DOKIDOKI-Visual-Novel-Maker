using UnityEngine;
using UnityEditor;

namespace DokiVnMaker
{
    [CustomEditor(typeof(TextStyleObject))]
    public class TextStyleObjectEditor : Editor
    {
        GUIStyle previewStyle = new GUIStyle();
        TextStyleObject obj;


        private void OnEnable()
        {
            obj = target as TextStyleObject;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("font"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("fontSize"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("textColor"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("displaySpeed"));
            serializedObject.ApplyModifiedProperties();

            previewStyle.alignment = TextAnchor.MiddleCenter;
            previewStyle.font = obj.font;
            previewStyle.fontSize = obj.fontSize;
            previewStyle.normal.textColor = obj.textColor;

            GUILayout.Space(10);
            GUILayout.BeginVertical("box");
            GUILayout.Label("Hello World!", previewStyle);
            GUILayout.EndVertical();
        }
    }
}