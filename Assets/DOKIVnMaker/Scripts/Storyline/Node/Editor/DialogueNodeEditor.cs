using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNode;
using XNodeEditor;

namespace DokiVnMaker.Story
{
    [CustomNodeEditor(typeof(Dialogue))]
    public class DialogueNodeEditor : NodeEditor
    {
        bool showAdvance;
        Dialogue node;

        public override void OnCreate()
        {
            node = target as Dialogue;
            base.OnCreate();
        }

        public override void OnBodyGUI()
        {
            serializedObject.Update();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Character", GUILayout.Width(70));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("character"), GUIContent.none);
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Name", GUILayout.Width(70));
            //custom name
            if (node.character == null)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("charaName"), GUIContent.none);
            }
            //selected character name
            else
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.TextField(node.character.charaName);
                EditorGUI.EndDisabledGroup();
            }
            GUILayout.EndHorizontal();

            if (node.answers.Count == 0)
            {
                GUILayout.BeginHorizontal();
                NodeEditorGUILayout.PortField(GUIContent.none, target.GetInputPort("input"), GUILayout.MinWidth(0));
                NodeEditorGUILayout.PortField(GUIContent.none, target.GetOutputPort("output"), GUILayout.MinWidth(0));
                GUILayout.EndHorizontal();
            }
            else
            {
                NodeEditorGUILayout.PortField(GUIContent.none, target.GetInputPort("input"));
            }
            GUILayout.Space(-30);

            NodeEditorGUILayout.PropertyField(serializedObject.FindProperty("dialogue"), GUIContent.none);

            showAdvance = EditorGUILayout.Foldout(showAdvance, "advance");
            if (showAdvance)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("font"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("fontSize"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("textColor"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("displayAll"));
                if (!node.displayAll)
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("displaySpeed"));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("voiceClip"));
            }

            GUILayout.Space(5);
            NodeEditorGUILayout.DynamicPortList("answers", typeof(StoryNodeBase), serializedObject, NodePort.IO.Output, Node.ConnectionType.Override);
            node.CheckAnswerCount();

            serializedObject.ApplyModifiedProperties();
        }
    }
}