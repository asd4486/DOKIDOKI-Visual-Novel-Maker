using DokiVnMaker.Game;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNodeEditor;

namespace DokiVnMaker.Story
{
    [CustomNodeEditor(typeof(CGShow))]
    public class CGShowEditor : StoryNodeEditorBase
    {
        CGManagerObject cgManager = DokiGameManager.CgManager;
        int cgIndex = -1;
        CGShow node;

        public override void OnCreate()
        {
            node = target as CGShow;
            if (cgManager != null && node.cg != null && cgManager.CGList.Contains(node.cg))
            {
                cgIndex = node.cg.Index;
            }

            base.OnCreate();
        }

        public override void OnBodyGUI()
        {
            serializedObject.Update();

            DrawPorts();

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
            if (imgPriveiw != null)
                GUILayout.Label(imgPriveiw.texture, GUILayout.Width(200), GUILayout.Height(113));

            DrawDurationField();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("isWait"));

            serializedObject.ApplyModifiedProperties();
        }
    }
}