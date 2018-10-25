using NodeEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

[Serializable]
public class DialogBox : NodeBase
{
    public string CharaName;
    public string Dialog;

    [NonSerialized]
    public bool ShowCharParam;
    public Color Color;
    public int FontSize = ValueManager.DefaultFontSize;
    public int Speed = 3;

    public bool NoWait;

    public DialogBox() { }

    public DialogBox(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle,
        GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> onClickInPoint, Action<ConnectionPoint> onClickOutPoint,
        Action<NodeBase> onClickCopyNode, Action<NodeBase> onClickRemoveNode, int id)
    {
        ActionType = ActionTypes.DialogBox;
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

        //character name
        GUILayout.BeginHorizontal();
        GUILayout.Label("Name", WhiteTxtStyle, GUILayout.Width(LabelWidth));
        CharaName = EditorGUILayout.TextField(CharaName);
        GUILayout.EndHorizontal();

        //dialogue text box
        GUILayout.Label("Dialogue", WhiteTxtStyle);
        Dialog = EditorGUILayout.TextArea(Dialog, GUILayout.Height(50));

        //show character parameter
        ShowCharParam = EditorGUILayout.Foldout(ShowCharParam, "Character +", true, WhiteTxtStyle);

        if (ShowCharParam)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Font size", WhiteTxtStyle, GUILayout.Width(LabelWidth));
            FontSize = EditorGUILayout.IntField(FontSize);
            GUILayout.EndHorizontal();
            //dialogue text speed
            GUILayout.BeginHorizontal();
            GUILayout.Label("Text speed", WhiteTxtStyle, GUILayout.Width(LabelWidth));
            Speed = EditorGUILayout.IntSlider(Speed, 1, 5);
            GUILayout.EndHorizontal();
        }
        //dialog show in one shot
        GUILayout.BeginHorizontal();
        GUILayout.Label("No wait", WhiteTxtStyle, GUILayout.Width(LabelWidth));
        NoWait = EditorGUILayout.Toggle(NoWait);
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
        GUILayout.Space(SpacePixel);
        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        base.Draw();
    }

    public override NodeBase Clone(Vector2 pos, int newId)
    {
        var clone = new DialogBox(pos, Rect.width, Rect.height, Style, SelectedNodeStyle, InPoint.Style,
            OutPoint.Style, InPoint.OnClickConnectionPoint, OutPoint.OnClickConnectionPoint,
            OnCopyNode, OnRemoveNode, newId)
        {
            CharaName = CharaName,
            Dialog = Dialog,
            Color = Color,
            FontSize = FontSize,
            Speed = Speed,
            NoWait = NoWait
        };

        return clone;
    }

    // override object.Equals
    //public override bool Equals(object obj)
    //{
    //    var item = obj as DialogBox;
    //    if (item == null) return false;

    //    return CharaName == item.CharaName && Dialog == item.Dialog
    //        && Color == item.Color && FontSize == item.FontSize && Speed == item.Speed
    //        && NoWait == item.NoWait && Position == item.Position && Id == item.Id;
    //}

    //// override object.GetHashCode
    //public override int GetHashCode()
    //{
    //    // TODO: write your implementation of GetHashCode() here
    //    //throw new NotImplementedException();
    //    return base.GetHashCode();
    //}
}

