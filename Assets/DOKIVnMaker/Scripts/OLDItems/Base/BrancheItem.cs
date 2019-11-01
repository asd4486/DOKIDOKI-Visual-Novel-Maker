using System;
using UnityEditor;
using UnityEngine;

namespace DokiVnMaker.MyEditor.Items
{
    [Serializable]
    public class BrancheItem
    {
        public string text;
        [NonSerialized]
        public int id;

        [NonSerialized]
        public Action<BrancheItem> OnDeleteBranche;

        public BrancheItem() { }

        public BrancheItem(Action<BrancheItem> onDeleteBranche, int _id)
        {
            OnDeleteBranche = onDeleteBranche;

            id = _id;
        }

        public void Draw()
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            text = EditorGUILayout.TextField(text);
            //delete branch
            if (id > 1)
            {
                if (GUILayout.Button("x", GUILayout.Width(20)))
                {
                    OnDeleteBranche?.Invoke(this);
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(4);
            GUILayout.EndVertical();
        }
    }
}