using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace NodeEditor
{
    [Serializable]
    public class SimpleNodeBase:ScriptableObject
    {
        public Vector2 Position;
        public Rect Rect;
        public int Id;

        public List<Connection> InConnections;
        public Connection OutConnection;

        // override object.Equals
        public override bool Equals(object obj)
        {
            var item = obj as SimpleNodeBase;
            if (item == null) return false;

            return item.Position == Position && item.Id == Id;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
