using NodeEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

[Serializable]
public class Sound : AudioBase
{
    public ActionTypes ActionType = ActionTypes.Sound;

    public int TrackIndex;

    public Sound() { }

    public Sound(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle,
        GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> onClickInPoint, Action<ConnectionPoint> onClickOutPoint,
        Action<NodeBase> onClickRemoveNode)
    {
        Init(position, width, height, nodeStyle, selectedStyle, inPointStyle, outPointStyle, onClickInPoint, onClickOutPoint, onClickRemoveNode);
        Title = "Sound";
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
        GUILayout.Label("Sound source", WhiteTxtStyle, GUILayout.Width(LabelWidth));
        MyAudio = EditorGUILayout.ObjectField( MyAudio, typeof(AudioClip), false) as AudioClip;
        GUILayout.EndHorizontal();

        //get audio path
        if (MyAudio != null) AudioPath = AssetDatabase.GetAssetPath(MyAudio);

        //audio volume
        GUILayout.BeginHorizontal();
        GUILayout.Label("Volume", WhiteTxtStyle, GUILayout.Width(LabelWidth));
        Volume = EditorGUILayout.Slider(Volume, 0, 1);
        GUILayout.EndHorizontal();

        //find sound manager object
        var obj = GameObject.FindGameObjectsWithTag("doki_sound_manager").FirstOrDefault();
        if (obj != null)
        {
            var soundManager = obj.GetComponent<SoundManager>();

            //find all sound tracks
            if (soundManager.AudioTracks != null && soundManager.AudioTracks.Length > 0)
            {
                //sound track list for seletion
                var list = new List<string>();
                for (int i = 0; i < soundManager.AudioTracks.Length; i++)
                {
                    list.Add((i + 1).ToString());
                }
                GUILayout.BeginHorizontal();
                GUILayout.Label("Track", WhiteTxtStyle, GUILayout.Width(LabelWidth));
                TrackIndex = EditorGUILayout.Popup( TrackIndex, list.ToArray());
                GUILayout.EndHorizontal();
            }
        }

        GUILayout.EndVertical();
        GUILayout.Space(SpacePixel);
        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        base.Draw();
    }

    // override object.Equals
    public override bool Equals(object obj)
    {
        var item = obj as Sound;
        if (obj == null) return false;

        return MyAudio == item.MyAudio && Volume == item.Volume && TrackIndex == item.TrackIndex && Position == item.Position;
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

