using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace NodeEditor
{
    [Serializable]
    public class SimpleNodeBase
    {
        [NonSerialized]
        public Rect Rect;
        [NonSerialized]
        public Connection OutConnection;

        public Vector2 Position;
        public int ParentId = -1;
        public int Id;

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (!(obj is SimpleNodeBase)) return false;
            var item = obj as SimpleNodeBase;

            return item.Position == Position && item.ParentId == ParentId && item.Id == Id;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
