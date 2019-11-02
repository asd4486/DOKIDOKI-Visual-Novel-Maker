﻿using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace DokiVnMaker.StoryNode
{
    [CustomNodeEditor(typeof(CG))]
    public class CGNodeEditor : NodeEditor
    {
        public override void OnBodyGUI()
        {
            serializedObject.Update();

            var node = target as CG;

            GUILayout.BeginHorizontal();
            NodeEditorGUILayout.PortField(GUIContent.none, target.GetInputPort("input"), GUILayout.MinWidth(0));
            NodeEditorGUILayout.PortField(GUIContent.none, target.GetOutputPort("output"), GUILayout.MinWidth(0));
            GUILayout.EndHorizontal();

            GUILayout.Space(-15);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("image"), GUIContent.none);
            var imgPriveiw = node.image;
            if (imgPriveiw != null) GUILayout.Label(imgPriveiw.texture, GUILayout.Width(200), GUILayout.Height(113));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("isWait"));
            if (node.isWait)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.Slider(serializedObject.FindProperty("waitTime"), 0, 99);
                GUILayout.Label("s", GUILayout.Width(20));
                GUILayout.EndHorizontal();
            }
        }

        public override int GetWidth()
        {
            return 250;
        }
    }
}