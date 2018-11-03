using NodeEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

[Serializable]
public class BrancheBox : NodeBase
{
    public string Dialogue;

    public List<BrancheItem> Branches;

    public Color Color;
    public int FontSize = ValueManager.DefaultFontSize;

    //branche out point style
    [NonSerialized]
    private GUIStyle BranchePointStyle;
    [NonSerialized]
    private Action<ConnectionPoint> BracheClickConnectionPoint;

    public BrancheBox() { }

    public BrancheBox(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle,
        GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> onClickInPoint, Action<ConnectionPoint> onClickOutPoint,
        Action<NodeBase> onClickCopyNode, Action<NodeBase> onClickRemoveNode, int id)
    {
        ActionType = ActionTypes.BrancheBox;
        Init(position, width, height, nodeStyle, selectedStyle, inPointStyle, null, onClickInPoint, null, onClickCopyNode, onClickRemoveNode, id);
        SetOutPointStyle(outPointStyle, onClickOutPoint);
    }

    public void SetOutPointStyle(GUIStyle outPointStyle, Action<ConnectionPoint> onClickOutPoint)
    {
        BranchePointStyle = outPointStyle;
        BracheClickConnectionPoint = onClickOutPoint;
    }

    public override void Draw()
    {
        InPoint.Draw();

        GUILayout.BeginArea(Rect, Title, Style);
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        GUILayout.Space(SpacePixel);
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical();
        GUILayout.Space(SpacePixel);

        //init branches if null
        if (Branches == null)
        {
            Branches = new List<BrancheItem>();
            Branches.Add(CreateNewBranche());
            Branches.Add(CreateNewBranche());
        }

        GUILayout.Label("Branches", WhiteTxtStyle);

        for (int i = 0; i < Branches.Count; i++)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label(i + 1 + ".", WhiteTxtStyle, GUILayout.Width(30));
            Branches[i].Draw();
            GUILayout.EndHorizontal();

            Rect.height = DefaultRectHeight + 20 * i;
        }

        //add new branche(6 maximun)
        if (GUILayout.Button("+"))
        {
            if (Branches.Count < 6)
            {
                Branches.Add(CreateNewBranche());
            }
        }

        //FontSize = EditorGUILayout.IntField("Font size:", FontSize);
        ////dialogue text box
        //GUILayout.Label("Dialogue");
        //Dialogue = EditorGUILayout.TextArea(Dialogue, GUILayout.Height(50));

        GUILayout.EndVertical();
        GUILayout.Space(SpacePixel);
        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        base.Draw();
    }

    private BrancheItem CreateNewBranche()
    {
        var branche = new BrancheItem(OnDeleteBranche, Id, SetBracheId());
        branche.OutPoint = new ConnectionPoint(branche, ConnectionPointType.Out, BranchePointStyle, BracheClickConnectionPoint);
        return branche;
    }

    private void OnDeleteBranche(BrancheItem branche)
    {
        if (Branches.Contains(branche))
        {
            Branches.Remove(branche);
        }
    }

    private int SetBracheId()
    {
        if (Branches == null) return 0;
        var id = 0;
        foreach (var n in Branches)
        {
            if (id <= n.Id) id = n.Id + 1;
        }
        return id;
    }

    public override NodeBase Clone(Vector2 pos, int newId)
    {
        var clone = new BrancheBox(pos, Rect.width, Rect.height, Style, SelectedNodeStyle, InPoint.Style,
            OutPoint.Style, InPoint.OnClickConnectionPoint, OutPoint.OnClickConnectionPoint,
            OnCopyNode, OnRemoveNode, newId)
        {
            Dialogue = Dialogue,
            Branches = Branches,
            Color = Color,
            FontSize = FontSize,
        };

        return clone;
    }
}

