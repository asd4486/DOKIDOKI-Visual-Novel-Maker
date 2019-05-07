using DokiVnMaker.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DokiVnMaker.MyEditor.Items
{
    [Serializable]
    public class Sound : AudioBase
    {
        public int trackIndex;

        public Sound()
        {
            ActionType = ActionTypes.Sound;
        }

        public override void Draw()
        {
            GUILayout.BeginArea(myRect, Title, Style);
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
                var origin = AssetDatabase.LoadAssetAtPath(Path, typeof(AudioClip)) as AudioClip;

                if (origin != null)
                {
                    //set background image
                    MyAudio = origin;
                }
                Initialize = false;
            }

            //Choose audio
            GUILayout.BeginHorizontal();
            GUILayout.Label("Sound source", WhiteTxtStyle, GUILayout.Width(LabelWidth));
            MyAudio = EditorGUILayout.ObjectField(MyAudio, typeof(AudioClip), false) as AudioClip;
            GUILayout.EndHorizontal();

            //get audio path
            if (MyAudio != null) Path = AssetDatabase.GetAssetPath(MyAudio);

            //audio volume
            GUILayout.BeginHorizontal();
            GUILayout.Label("Volume", WhiteTxtStyle, GUILayout.Width(LabelWidth));
            Volume = EditorGUILayout.Slider(Volume, 0, 1);
            GUILayout.EndHorizontal();

            GUILayout.Space(5);
            //find sound manager object
            GUILayout.BeginHorizontal();
            GUILayout.Label("Track Index", WhiteTxtStyle, GUILayout.Width(LabelWidth));
            trackIndex = EditorGUILayout.IntField(trackIndex);
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
            GUILayout.Space(SpacePixel);
            GUILayout.EndHorizontal();
            GUILayout.EndArea();

            InPoint.Draw();
            OutPoint.Draw();

            base.Draw();
        }

        public override NodeBase Clone(Vector2 pos, int newId)
        {
            var clone = new Sound()
            {
                Initialize = true,
                Path = Path,
                Volume = Volume,
                trackIndex = trackIndex
            };
            clone.Init(pos, myRect.width, myRect.height, Style, SelectedNodeStyle, InPoint.Style,
                OutPoint.Style, InPoint.OnClickConnectionPoint, OutPoint.OnClickConnectionPoint,
                OnCopyNode, OnRemoveNode, newId);

            return clone;
        }
    }
}