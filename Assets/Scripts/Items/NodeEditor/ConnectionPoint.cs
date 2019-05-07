using System;
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
        public SimpleNodeBase targetNode;
        public int targetNodeId;

        //specifique item id for multiple points node
        public int targetItemId = -1;

        [NonSerialized]
        public Rect myRect;
        [NonSerialized]
        public ConnectionPointType Type;

        [NonSerialized]
        public GUIStyle Style;
        [NonSerialized]
        public Action<ConnectionPoint> OnClickConnectionPoint;

        [NonSerialized]
        public Connection connectedConnection;

        public ConnectionPoint() { }

        public ConnectionPoint(SimpleNodeBase _targetNode, ConnectionPointType _type, GUIStyle _style, Action<ConnectionPoint> _onClickConnectionPoint,
            int _targetItemId = -1)
        {
            targetNode = _targetNode;
            targetNodeId = targetNode.Id;

            targetItemId = _targetItemId;

            Type = _type;

            Style = _style;
            OnClickConnectionPoint = _onClickConnectionPoint;
            myRect = new Rect(0, 0, 13f, 13f);
        }

        public void Draw()
        {
            myRect.y = targetNode.myRect.y + (targetNode.myRect.height * 0.5f) - myRect.height * 0.5f;

            switch (Type)
            {
                case ConnectionPointType.In:
                    myRect.x = targetNode.myRect.x - myRect.width + 6;
                    break;
                case ConnectionPointType.Out:
                    myRect.x = targetNode.myRect.x + targetNode.myRect.width - 6;
                    break;
            }

            if (GUI.Button(myRect, "", Style))
            {
                OnClickConnectionPoint?.Invoke(this);
            }

        }

        public void Draw(float x, float y)
        {
            myRect.y = targetNode.myRect.y + y;

            switch (Type)
            {
                case ConnectionPointType.In:
                    myRect.x = targetNode.myRect.x - myRect.width + 6 + x;
                    break;
                case ConnectionPointType.Out:
                    myRect.x = targetNode.myRect.x + targetNode.myRect.width - 6 + x;
                    break;
            }

            if (GUI.Button(myRect, "", Style))
            {
                OnClickConnectionPoint?.Invoke(this);
            }
        }

        public void SetConnection(Connection conn)
        {
            connectedConnection = conn;
        }

        public bool HasSameNode(ConnectionPoint point)
        {
            var otherId = point.targetItemId > -1 ? point.targetItemId : point.targetNodeId;
            if (targetItemId > -1)
            {
                if (targetItemId == otherId) return true;
            }
            else
            {
                if (targetNodeId == otherId) return true;
            }

            return false;
        }
    }
}
