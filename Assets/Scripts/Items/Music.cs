using System;
using UnityEditor;
using UnityEngine;

namespace DokiVnMaker.MyEditor.Items
{
    [Serializable]
    public class Music : AudioBase
    {
        public Music()
        {
            //set action type
            ActionType = ActionTypes.Music;
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
                var origin = AssetDatabase.LoadAssetAtPath(path, typeof(AudioClip)) as AudioClip;

                if (origin != null)
                {
                    //set background image
                    myAudio = origin;
                }
                Initialize = false;
            }

            //Choose audio
            GUILayout.BeginHorizontal();
            GUILayout.Label("Audio source", WhiteTxtStyle, GUILayout.Width(LabelWidth));
            myAudio = EditorGUILayout.ObjectField(myAudio, typeof(AudioClip), false) as AudioClip;
            GUILayout.EndHorizontal();

            //get audio path
            if (myAudio != null) path = AssetDatabase.GetAssetPath(myAudio);

            //audio volume
            GUILayout.BeginHorizontal();
            GUILayout.Label("Volume", WhiteTxtStyle, GUILayout.Width(LabelWidth));
            volume = EditorGUILayout.Slider(volume, 0, 1);
            GUILayout.EndHorizontal();

            //music loop
            GUILayout.BeginHorizontal();
            GUILayout.Label("Loop", WhiteTxtStyle, GUILayout.Width(LabelWidth));
            loop = EditorGUILayout.Toggle(loop);
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
            var clone = new Music()
            {
                Initialize = true,
                path = path,
                volume = volume,
            };

            clone.Init(pos, myRect.size, Style, SelectedNodeStyle, InPoint.Style,
                OutPoint.Style, InPoint.OnClickConnectionPoint, OutPoint.OnClickConnectionPoint,
                OnCopyNode, OnRemoveNode, newId);

            return clone;
        }
    }
}