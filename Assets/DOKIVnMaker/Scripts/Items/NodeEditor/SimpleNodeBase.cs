using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace DokiVnMaker.MyEditor.Items
{
    [Serializable]
    public class SimpleNodeBase
    {
        [NonSerialized]
        public Rect myRect;

        [NonSerialized]
        public ConnectionPoint InPoint;
        [NonSerialized]
        public ConnectionPoint OutPoint;

        public Vector2 Position;

        public int Id;

        // override object.Equals
        public override bool Equals(object obj)
        {
            if (!(obj is SimpleNodeBase)) return false;
            var item = obj as SimpleNodeBase;

            return item.Position == Position && item.Id == Id;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
