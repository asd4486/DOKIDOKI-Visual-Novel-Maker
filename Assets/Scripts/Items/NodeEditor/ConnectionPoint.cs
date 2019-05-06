﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DokiVnMaker.MyEditor.Items
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

        public ConnectionPoint(SimpleNodeBase _node, ConnectionPointType _type, GUIStyle _style, Action<ConnectionPoint> _onClickConnectionPoint)
        {
            Node = _node;
            Type = _type;
            Style = _style;
            OnClickConnectionPoint = _onClickConnectionPoint;
            Rect = new Rect(0, 0, 13f, 13f);
        }

        public void Draw()
        {
            Rect.y = Node.Rect.y + (Node.Rect.height * 0.5f) - Rect.height * 0.5f;

            switch (Type)
            {
                case ConnectionPointType.In:
                    Rect.x = Node.Rect.x - Rect.width + 6;
                    break;
                case ConnectionPointType.Out:
                    Rect.x = Node.Rect.x + Node.Rect.width - 6;
                    break;
            }

            if (GUI.Button(Rect, "", Style))
            {
                OnClickConnectionPoint?.Invoke(this);
            }
        }

        public void Draw(float x, float y)
        {
            Rect.y = Node.Rect.y + y;

            switch (Type)
            {
                case ConnectionPointType.In:
                    Rect.x = Node.Rect.x - Rect.width + 6 + x;
                    break;
                case ConnectionPointType.Out:
                    Rect.x = Node.Rect.x + Node.Rect.width - 6 + x;
                    break;
            }

            if (GUI.Button(Rect, "", Style))
            {
                OnClickConnectionPoint?.Invoke(this);
            }
        }
    }
}
