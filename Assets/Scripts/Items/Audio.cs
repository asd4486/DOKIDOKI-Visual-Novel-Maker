using NodeEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

[Serializable]
public class Audio:AudioBase
{
    //set action type
    public ActionTypes ActionType = ActionTypes.Audio;

    public Audio() { }

    public Audio(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle,
        GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> onClickInPoint, Action<ConnectionPoint> onClickOutPoint,
        Action<NodeBase> onClickRemoveNode)
    {
        Init(position, width, height, nodeStyle, selectedStyle, inPointStyle, outPointStyle, onClickInPoint, onClickOutPoint, onClickRemoveNode);
        Title = "Background music";
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

        //Choose audio
        GUILayout.BeginHorizontal();
        GUILayout.Label("Audio source", WhiteTxtStyle, GUILayout.Width(LabelWidth));
        MyAudio = EditorGUILayout.ObjectField( MyAudio, typeof(AudioClip), false) as AudioClip;
        GUILayout.EndHorizontal();
        
        //get audio path
        if (MyAudio != null) AudioPath = AssetDatabase.GetAssetPath(MyAudio);

        //audio volume
        GUILayout.BeginHorizontal();
        GUILayout.Label("Volume", WhiteTxtStyle, GUILayout.Width(LabelWidth));
        Volume = EditorGUILayout.Slider(Volume, 0, 1);
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
        GUILayout.Space(SpacePixel);
        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        base.Draw();
    }

    // override object.Equals
    public override bool Equals(object obj)
    {
        var item = obj as Audio;
        if (item == null) return false;

        return MyAudio == item.MyAudio && Volume == item.Volume && Position == item.Position;
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

