using UnityEngine;
using UnityEditor;
using XNodeEditor;
using System.Collections.Generic;

namespace DokiVnMaker.Story
{
    [CustomNodeEditor(typeof(CharacterShowBase))]
    public class CharacterFadeBaseEditor : NodeEditor
    {
        public override void OnBodyGUI()
        {
            serializedObject.Update();

            var node = target as CharacterShowBase;

            GUILayout.BeginHorizontal();
            NodeEditorGUILayout.PortField(GUIContent.none, target.GetInputPort("input"), GUILayout.MinWidth(0));
            NodeEditorGUILayout.PortField(GUIContent.none, target.GetOutputPort("output"), GUILayout.MinWidth(0));
            GUILayout.EndHorizontal();

            GUILayout.Space(-15);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("character"), GUIContent.none);

            //select face 
            if (node.isFadeIn)
            {
                if (node.character != null && node.character.faces.Count > 0)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Face", GUILayout.Width(70));
                    List<string> faceNames = new List<string>();
                    foreach (var f in node.character.faces) faceNames.Add(f.faceName);
                    node.faceIndex = EditorGUILayout.Popup(node.faceIndex, faceNames.ToArray());
                    GUILayout.EndHorizontal();
                }

                GUILayout.BeginHorizontal();
                GUILayout.Label("Display", GUILayout.Width(70));
                node.displayPos = (DisplayPositions) EditorGUILayout.Popup((int)(node.displayPos), node.DisplayList);
                GUILayout.EndHorizontal();

                if (node.displayPos == DisplayPositions.Custom)          
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("customPos"), GUIContent.none);       
            }

            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Duration", GUILayout.Width(70));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("duration"), GUIContent.none);
            GUILayout.Label("s", GUILayout.Width(15));
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();
            GUILayout.Label("Is wait", GUILayout.Width(70));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("isWait"), GUIContent.none);
            GUILayout.EndHorizontal();

            serializedObject.ApplyModifiedProperties();
        }

        public override int GetWidth()
        {
            return 210;
        }
    }
}