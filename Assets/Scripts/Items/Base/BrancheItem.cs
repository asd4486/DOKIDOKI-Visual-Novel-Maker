using System;
using UnityEditor;
using UnityEngine;

namespace DokiVnMaker.MyEditor.Items
{
    [Serializable]
    public class BrancheItem : SimpleNodeBase
    {
        public string Text;

        [NonSerialized]
        public ConnectionPoint OutPoint;
        [NonSerialized]
        public Action<BrancheItem> OnDeleteBranche;

        public BrancheItem() { }

        public BrancheItem(Action<BrancheItem> onDeleteBranche, int parentId, int id)
        {
            OnDeleteBranche = onDeleteBranche;
            ParentId = parentId;
            Id = id;
        }

        public void Draw()
        {
            GUILayout.BeginVertical();
            GUILayout.BeginHorizontal();
            Text = EditorGUILayout.TextField(Text);
            //delete branch
            if (Id > 1)
            {
                if (GUILayout.Button("x", GUILayout.Width(20)))
                {
                    if (OnDeleteBranche != null) OnDeleteBranche(this);
                }
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(4);
            GUILayout.EndVertical();
        }
    }
}