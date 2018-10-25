using NodeEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

[Serializable]
public class BackgroundItem:NodeBase
{
    [NonSerialized]
    public bool Initialize;
    [NonSerialized]
    public Sprite Image;

    public string Path;
    public bool IsWait;

    public BackgroundItem() { }

    public BackgroundItem(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle,
        GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> onClickInPoint, Action<ConnectionPoint> onClickOutPoint,
        Action<NodeBase> onClickCopyNode, Action<NodeBase> onClickRemoveNode, int id)
    {

        ActionType = ActionTypes.BackgroundItem;
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

        //initialize
        if (Initialize)
        {
            //find origin object
            var origin = AssetDatabase.LoadAssetAtPath(Path, typeof(Sprite)) as Sprite;

            if (origin != null)
            {
                //set background image
                Image = origin;
            }
            Initialize = false;
        }

        //Choose image
        GUILayout.BeginHorizontal();
        GUILayout.Label("Image", WhiteTxtStyle, GUILayout.Width(LabelWidth));
        Image = EditorGUILayout.ObjectField(Image, typeof(Sprite), false) as Sprite;
        GUILayout.EndHorizontal();
            
        if (Image != null)
        {
            //get path
            Path = AssetDatabase.GetAssetPath(Image);
            //show preview
            GUILayout.Label(Image.texture, GUILayout.Width(200), GUILayout.Height(113));
        }

        //is wait for background appear
        GUILayout.BeginHorizontal();
        GUILayout.Label("Is wait", WhiteTxtStyle, GUILayout.Width(LabelWidth));
        IsWait = EditorGUILayout.Toggle( IsWait);
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
        GUILayout.Space(SpacePixel);
        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        base.Draw();
    }

    public override NodeBase Clone(Vector2 pos, int newId)
    {
        var clone = new BackgroundItem(pos, Rect.width, Rect.height, Style, SelectedNodeStyle, InPoint.Style,
            OutPoint.Style, InPoint.OnClickConnectionPoint, OutPoint.OnClickConnectionPoint,
            OnCopyNode, OnRemoveNode, newId)
        {
            Initialize = true,
            Path = Path,
            IsWait = IsWait,
        };

        return clone;
    }

    // override object.Equals
    //public override bool Equals(object obj)
    //{
    //    var item = obj as BackgroundItem;
    //    if (obj == null) return false;

    //    return Path == item.Path && IsWait == item.IsWait && Position == item.Position && Id == item.Id;
    //}

    //// override object.GetHashCode
    //public override int GetHashCode()
    //{
    //    return base.GetHashCode();
    //}
}

