using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace NodeEditor
{
    public enum ConnectionPointType { In, Out }

    [Serializable]
    public class ConnectionPoint
    {
        [NonSerialized]
        public Rect Rect;
        [NonSerialized]
        public ConnectionPointType Type;
        public SimpleNodeBase Node;

        [NonSerialized]
        public GUIStyle Style;
        [NonSerialized]
        public Action<ConnectionPoint> OnClickConnectionPoint;

        public ConnectionPoint(SimpleNodeBase node, ConnectionPointType type, GUIStyle style, Action<ConnectionPoint> onClickConnectionPoint)
        {
            this.Node = node;
            this.Type = type;
            this.Style = style;
            this.OnClickConnectionPoint = onClickConnectionPoint;
            Rect = new Rect(0, 0, 10f, 20f);
        }

        public void Draw()
        {
            Rect.y = Node.Rect.y + (Node.Rect.height * 0.5f) - Rect.height * 0.5f;

            switch (Type)
            {
                case ConnectionPointType.In:
                    Rect.x = Node.Rect.x - Rect.width;
                    break;

                case ConnectionPointType.Out:
                    Rect.x = Node.Rect.x + Node.Rect.width;
                    break;
            }

            if (GUI.Button(Rect, "", Style))
            {
                if (OnClickConnectionPoint != null)
                {
                    OnClickConnectionPoint(this);
                }
            }
        }
    }
}
