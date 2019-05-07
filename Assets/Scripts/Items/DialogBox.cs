using System;
using UnityEditor;
using UnityEngine;

namespace DokiVnMaker.MyEditor.Items
{
    [Serializable]
    public class DialogBox : NodeBase
    {
        public string CharaName;
        public string Dialog;

        [NonSerialized]
        public bool ShowCharParam;
        [NonSerialized]
        public Color Color;
        public int FontSize = ValueManager.DefaultFontSize;
        public int Speed = 3;

        public bool NoWait;

        public DialogBox()
        {
            ActionType = ActionTypes.DialogBox;
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

            InPoint.Draw();
            OutPoint.Draw();

            base.Draw();
        }

        public override NodeBase Clone(Vector2 pos, int newId)
        {
            var clone = new DialogBox()
            {
                CharaName = CharaName,
                Dialog = Dialog,
                Color = Color,
                FontSize = FontSize,
                Speed = Speed,
                NoWait = NoWait
            };

            clone.Init(pos, myRect.width, myRect.height, Style, SelectedNodeStyle, InPoint.Style,
                OutPoint.Style, InPoint.OnClickConnectionPoint, OutPoint.OnClickConnectionPoint,
                OnCopyNode, OnRemoveNode, newId);

            return clone;
        }
    }
}