using DokiVnMaker.Game;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace DokiVnMaker.Story
{
    [CustomNodeEditor(typeof(CGShow))]
    public class CGShowEditor : NodeEditor
    {
        CGManagerObject cgManager = DokiGameManager.CgManager;
        int cgIndex = -1;
        CGShow node;

        public override void OnCreate()
        {
            node = target as CGShow;
            if (cgManager != null && node.cg != null && cgManager.CGList.Contains(node.cg))
            {
                cgIndex = node.cg.index;
            }

            base.OnCreate();
        }

        public override void OnBodyGUI()
        {
            serializedObject.Update();

            GUILayout.BeginHorizontal();
            NodeEditorGUILayout.PortField(GUIContent.none, target.GetInputPort("input"), GUILayout.MinWidth(0));
            NodeEditorGUILayout.PortField(GUIContent.none, target.GetOutputPort("output"), GUILayout.MinWidth(0));
            GUILayout.EndHorizontal();

            GUILayout.Space(-15);

            if (DokiGameManager.CgManager == null || DokiGameManager.CgManager.CGList.Count < 1)
            {
                GUILayout.Label("Please set up your CG Manager before:)");
                return;
            }

            List<string> cgNames = new List<string>();
            foreach (var cg in cgManager.CGList) cgNames.Add(cg.name);
            cgIndex = EditorGUILayout.Popup(cgIndex, cgNames.ToArray());
            if (cgIndex >= 0 && cgIndex < cgNames.Count) node.cg = cgManager.CGList[cgIndex];

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.ObjectField(node.cg.image, typeof(Sprite));
            EditorGUI.EndDisabledGroup();

            var imgPriveiw = node.cg.image;
            if (imgPriveiw != null) GUILayout.Label(imgPriveiw.texture, GUILayout.Width(200), GUILayout.Height(113));

            GUILayout.BeginHorizontal();
            EditorGUILayout.Slider(serializedObject.FindProperty("duration"), 0, 99);
            GUILayout.Label("s", GUILayout.Width(20));
            GUILayout.EndHorizontal();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("isWait"));
        }
    }
}