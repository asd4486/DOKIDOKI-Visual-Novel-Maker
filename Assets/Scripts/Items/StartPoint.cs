using System;
using UnityEngine;

namespace DokiVnMaker.MyEditor.Items
{
    [Serializable]
    public class EditorStartPoint : NodeBase
    {
        public EditorStartPoint() { }

        public EditorStartPoint(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle,
            GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> onClickInPoint, Action<ConnectionPoint> onClickOutPoint,
            Action<NodeBase> onClickRemoveNode)
        {
            ActionType = ActionTypes.Start;
            Init(position, width, height, nodeStyle, selectedStyle, inPointStyle, outPointStyle, onClickInPoint, onClickOutPoint, null, null, canEdit: false);
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