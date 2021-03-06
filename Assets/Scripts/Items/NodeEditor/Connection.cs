﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace DokiVnMaker.MyEditor.Items
{
    [Serializable]
    public class Connection
    {
        public ConnectionPoint InPoint;
        public ConnectionPoint OutPoint;
        [NonSerialized]
        public Action<Connection> OnClickRemoveConnection;

        public Connection() { }

        public Connection(ConnectionPoint inPoint, ConnectionPoint outPoint, Action<Connection> OnClickRemoveConnection)
        {
            this.InPoint = inPoint;
            this.OutPoint = outPoint;
            this.OnClickRemoveConnection = OnClickRemoveConnection;
        }

        public void Draw()
        {
            Handles.DrawBezier(
                InPoint.myRect.center,
                OutPoint.myRect.center,
                InPoint.myRect.center + Vector2.left * 50f,
                OutPoint.myRect.center - Vector2.left * 50f,
                Color.white,
                null,
                2f
            );

            if (Handles.Button((InPoint.myRect.center + OutPoint.myRect.center) * 0.5f, Quaternion.identity, 4, 8, Handles.RectangleCap))
            {
                OnClickRemoveConnection?.Invoke(this);
            }
        }
    }
}