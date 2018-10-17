using NodeEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class StartPoint:NodeBase
{
    public ActionTypes ActionType = ActionTypes.Start;

    public StartPoint() { }

    public StartPoint(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle,
        GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> onClickInPoint, Action<ConnectionPoint> onClickOutPoint,
        Action<NodeBase> onClickRemoveNode)
    {
        Init(position, width, height, nodeStyle, selectedStyle, inPointStyle, outPointStyle, onClickInPoint, onClickOutPoint, null, canEdit:false);

        WhiteTxtStyle = new GUIStyle();
        WhiteTxtStyle.normal.textColor = Color.white;
        WhiteTxtStyle.fontSize = 20;
        WhiteTxtStyle.fontStyle = FontStyle.Bold;
    }

    public override void Draw()
    {
        OutPoint.Draw();
        GUILayout.BeginArea(Rect, "", Style);
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("START",WhiteTxtStyle);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.EndArea();

        base.Draw();
    }
}

