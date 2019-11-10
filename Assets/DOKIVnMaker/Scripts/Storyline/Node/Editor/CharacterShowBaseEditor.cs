﻿using UnityEngine;
using UnityEditor;
using XNodeEditor;
using System.Collections.Generic;

namespace DokiVnMaker.Story
{
    [CustomNodeEditor(typeof(CharacterShowBase))]
    public class CharacterShowBaseEditor : StoryNodeEditorBase
    {
        public override void OnBodyGUI()
        {
            serializedObject.Update();

            var node = target as CharacterShowBase;

            DrawPorts();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("character"), GUIContent.none);

            //select face 
            if (node.isFadeIn)
            {
                if (node.character != null && node.character.faces.Count > 0)
                {
                    GUILayout.BeginHorizontal();
                    GUILayout.Label("Face", GUILayout.Width(75));
                    List<string> faceNames = new List<string>();
                    foreach (var f in node.character.faces) faceNames.Add(f.name);
                    node.faceIndex = EditorGUILayout.Popup(node.faceIndex, faceNames.ToArray());
                    GUILayout.EndHorizontal();
                }

                GUILayout.BeginHorizontal();
                GUILayout.Label("Display", GUILayout.Width(75));
                node.displayPos = (DisplayPositions) EditorGUILayout.Popup((int)(node.displayPos), node.DisplayList);
                GUILayout.EndHorizontal();

                if (node.displayPos == DisplayPositions.Custom)          
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("customPos"), GUIContent.none);       
            }

            GUILayout.Space(5);

            DrawDurationField();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("isWait"));
            serializedObject.ApplyModifiedProperties();
        }
    }
}