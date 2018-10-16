using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace NodeEditor
{
    public class Connection
    {
        public ConnectionPoint inPoint;
        public ConnectionPoint outPoint;
        public Action<Connection> OnClickRemoveConnection;

        public Connection(ConnectionPoint inPoint, ConnectionPoint outPoint, Action<Connection> OnClickRemoveConnection)
        {
            this.inPoint = inPoint;
            this.outPoint = outPoint;
            this.OnClickRemoveConnection = OnClickRemoveConnection;
        }

        public void Draw()
        {
            Handles.DrawBezier(
                inPoint.Rect.center,
                outPoint.Rect.center,
                inPoint.Rect.center + Vector2.left * 50f,
                outPoint.Rect.center - Vector2.left * 50f,
                Color.white,
                null,
                2f
            );

            if (Handles.Button((inPoint.Rect.center + outPoint.Rect.center) * 0.5f, Quaternion.identity, 4, 8, Handles.RectangleCap))
            {
                if (OnClickRemoveConnection != null)
                {
                    OnClickRemoveConnection(this);
                }
            }
        }
    }
}
