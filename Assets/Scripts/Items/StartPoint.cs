using System;
using UnityEngine;

namespace DokiVnMaker.MyEditor.Items
{
    [Serializable]
    public class EditorStartPoint : NodeBase
    {
        public EditorStartPoint()
        {
            ActionType = ActionTypes.Start;
        }

        public override void Draw()
        {
            GUILayout.BeginArea(Rect, "", Style);
            GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.Label("START", WhiteTxtStyle);
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.EndArea();

            OutPoint.Draw();

            base.Draw();
        }
    }
}