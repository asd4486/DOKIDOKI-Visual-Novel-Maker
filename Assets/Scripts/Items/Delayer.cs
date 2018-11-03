using NodeEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

[Serializable]
public class Delayer : NodeBase
{
    public float Delay;

    public Delayer() { }

    public Delayer(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle,
        GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> onClickInPoint, Action<ConnectionPoint> onClickOutPoint,
        Action<NodeBase> onClickCopyNode, Action<NodeBase> onClickRemoveNode, int id)
    {
        ActionType = ActionTypes.Delayer;
        Init(position, width, height, nodeStyle, selectedStyle, inPointStyle, outPointStyle, onClickInPoint, onClickOutPoint, onClickCopyNode, onClickRemoveNode, id);
    }

    public override void Draw()
    {
        InPoint.Draw();
        OutPoint.Draw();

        GUILayout.BeginArea(Rect, Title, Style);
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        GUILayout.Space(SpacePixel);
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical();
        GUILayout.Space(SpacePixel);

        GUILayout.BeginHorizontal();
        GUILayout.Label("Delay", WhiteTxtStyle, GUILayout.Width(LabelWidth));
        Delay = EditorGUILayout.FloatField(Delay);
        GUILayout.Label("S", WhiteTxtStyle);
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
        GUILayout.Space(SpacePixel);
        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        base.Draw();
    }

    public override NodeBase Clone(Vector2 pos, int newId)
    {
        var clone = new Delayer(pos, Rect.width, Rect.height, Style, SelectedNodeStyle, InPoint.Style,
            OutPoint.Style, InPoint.OnClickConnectionPoint, OutPoint.OnClickConnectionPoint,
            OnCopyNode, OnRemoveNode, newId)
        {
            Delay = Delay
        };

        return clone;
    }
}

