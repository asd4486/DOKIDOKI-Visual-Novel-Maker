using NodeEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

[Serializable]
public class BrancheItem: SimpleNodeBase
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
        Text = EditorGUILayout.TextField(Text);
        //delete branch
        if (Id > 1)
        {
            if (GUILayout.Button("x", GUILayout.Width(20)))
            {
               if(OnDeleteBranche != null)  OnDeleteBranche(this);
            }
        }
        OutPoint.CustomDraw(50, 20);
    }
}

