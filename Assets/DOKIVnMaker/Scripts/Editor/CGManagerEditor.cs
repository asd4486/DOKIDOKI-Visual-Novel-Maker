using UnityEngine;
using UnityEditor;

namespace DokiVnMaker
{
    [CustomEditor(typeof(CGManagerObject))]
    public class CGManagerEditor : Editor
    {
        CGManagerObject cgManager;
        Vector2 scrollPosition;


        private void OnEnable()
        {
            cgManager = target as CGManagerObject;
        }

        public override void OnInspectorGUI()
        {
            GUILayout.Label("CG list");
            GUILayout.BeginVertical("box");
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(600));

            for (int i = 0; i < cgManager.CGList.Count; i++)
            {
                var cg = cgManager.CGList[i];
                GUILayout.BeginVertical("box");

                GUILayout.BeginHorizontal();
                if (GUILayout.Button("▲", GUILayout.Width(25))) cgManager.MoveUp(cg);
                if (GUILayout.Button("▼", GUILayout.Width(25))) cgManager.MoveDown(cg);
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("x", GUILayout.Width(20)))    cgManager.RemoveCG(cg);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                //auto name if string is empty
                if (string.IsNullOrEmpty(cg.name)) cg.name = "cg_" + i;
                cg.name = EditorGUILayout.TextField(cg.name);
                cg.image = EditorGUILayout.ObjectField(cg.image, typeof(Sprite), false, GUILayout.Width(65f), GUILayout.Height(65f)) as Sprite;
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();

            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("+", GUILayout.Width(50)))
            {
                cgManager.AddCG();
                scrollPosition = new Vector2(0, 65 * cgManager.CGList.Count);
            }
            if (GUILayout.Button("-", GUILayout.Width(50))) cgManager.RemoveCG();
            GUILayout.EndVertical();

            EditorUtility.SetDirty(cgManager);
        }
    }
}