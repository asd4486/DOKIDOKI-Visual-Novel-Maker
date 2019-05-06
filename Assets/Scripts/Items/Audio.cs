using System;
using UnityEditor;
using UnityEngine;

namespace DokiVnMaker.MyEditor.Items
{
    [Serializable]
    public class Audio : AudioBase
    {
        public Audio() { }

        public Audio(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle,
            GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> onClickInPoint, Action<ConnectionPoint> onClickOutPoint,
            Action<NodeBase> onClickCopyNode, Action<NodeBase> onClickRemoveNode, int id)
        {
            //set action type
            ActionType = ActionTypes.Audio;
            Init(position, width, height, nodeStyle, selectedStyle, inPointStyle, outPointStyle, onClickInPoint, onClickOutPoint, onClickCopyNode, onClickRemoveNode, id);
        }

        public override void Draw()
        {
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
                var origin = AssetDatabase.LoadAssetAtPath(AudioPath, typeof(AudioClip)) as AudioClip;

                if (origin != null)
                {
                    //set background image
                    MyAudio = origin;
                }
                Initialize = false;
            }

            //Choose audio
            GUILayout.BeginHorizontal();
            GUILayout.Label("Audio source", WhiteTxtStyle, GUILayout.Width(LabelWidth));
            MyAudio = EditorGUILayout.ObjectField(MyAudio, typeof(AudioClip), false) as AudioClip;
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

            InPoint.Draw();
            OutPoint.Draw();

            base.Draw();
        }

        public override NodeBase Clone(Vector2 pos, int newId)
        {
            var clone = new Audio(pos, Rect.width, Rect.height, Style, SelectedNodeStyle, InPoint.Style,
                OutPoint.Style, InPoint.OnClickConnectionPoint, OutPoint.OnClickConnectionPoint,
                OnCopyNode, OnRemoveNode, newId)
            {
                Initialize = true,
                AudioPath = AudioPath,
                Volume = Volume,
            };

            return clone;
        }
    }
}