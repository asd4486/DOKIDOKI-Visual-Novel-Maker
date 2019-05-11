using System;
using UnityEditor;
using UnityEngine;

namespace DokiVnMaker.MyEditor.Items
{
    [Serializable]
    public class Delayer : NodeBase
    {
        public float Delay;

        public Delayer()
        {
            ActionType = ActionTypes.Delayer;
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

            GUILayout.BeginHorizontal();
            GUILayout.Label("Delay", WhiteTxtStyle, GUILayout.Width(LabelWidth));
            Delay = EditorGUILayout.FloatField(Delay);
            GUILayout.Label("S", WhiteTxtStyle);
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
            var clone = new Delayer()
            {
                Delay = Delay
            };

            clone.Init(pos, myRect.size, Style, SelectedNodeStyle, InPoint.Style,
                OutPoint.Style, InPoint.OnClickConnectionPoint, OutPoint.OnClickConnectionPoint,
                OnCopyNode, OnRemoveNode, newId);

            return clone;
        }
    }
}