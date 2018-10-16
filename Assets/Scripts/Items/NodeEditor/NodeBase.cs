using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace NodeEditor
{
    [Serializable]
    public class NodeBase
    {
        public ConnectionPoint InPoint;
        public ConnectionPoint OutPoint;

        public Vector2 Position;
        public List<Connection> InConnections;
        public Connection OutConnection;

        [NonSerialized]
        public bool IsDragged;
        [NonSerialized]
        public bool IsSelected;

        [NonSerialized]
        public Rect Rect;
        [NonSerialized]
        public float SpacePixel = 10;
        [NonSerialized]
        public float LabelWidth = 90;
        [NonSerialized]
        public string Title;
        [NonSerialized]
        public GUIStyle Style;

        [NonSerialized]
        public GUIStyle DefaultNodeStyle;
        [NonSerialized]
        public GUIStyle SelectedNodeStyle;
        [NonSerialized]
        public GUIStyle WhiteTxtStyle;
        [NonSerialized]
        public Action<NodeBase> OnRemoveNode;
        [NonSerialized]
        public bool CanEdit;

        //public NodeBase() { }

        //public NodeBase(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle,
        //    GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> onClickInPoint, Action<ConnectionPoint> onClickOutPoint,
        //    Action<NodeBase> onClickRemoveNode)
        //{
        //    Init(position, width, height, nodeStyle, selectedStyle, inPointStyle, outPointStyle, onClickInPoint, onClickOutPoint, onClickRemoveNode);
        //}

        public void Init(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle,
            GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> onClickInPoint, Action<ConnectionPoint> onClickOutPoint,
            Action<NodeBase> onClickRemoveNode, bool canEdit = true)
        {
            Position = position;
            Rect = new Rect(position.x, position.y, width, height+5);
            Style = nodeStyle;
            InPoint = new ConnectionPoint(this, ConnectionPointType.In, inPointStyle, onClickInPoint);
            OutPoint = new ConnectionPoint(this, ConnectionPointType.Out, outPointStyle, onClickOutPoint);
            DefaultNodeStyle = nodeStyle;
            SelectedNodeStyle = selectedStyle;
            OnRemoveNode = onClickRemoveNode;

            InConnections = new List<Connection>();

            WhiteTxtStyle = new GUIStyle();
            WhiteTxtStyle.normal.textColor = Color.white;
            WhiteTxtStyle.focused.textColor = Color.yellow;
            CanEdit = canEdit;

        }

        public void Drag(Vector2 delta)
        {
            Rect.position += delta;
            Position = Rect.position;
        }

        public virtual void Draw()
        {}

        public bool ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        if (Rect.Contains(e.mousePosition))
                        {
                            IsDragged = true;
                            GUI.changed = true;

                            IsSelected = true;
                            Style = SelectedNodeStyle;
                        }
                        else
                        {
                            GUI.changed = true;

                            IsSelected = false;
                            Style = DefaultNodeStyle;
                        }
                    }

                    if (e.button == 1 && IsSelected && Rect.Contains(e.mousePosition))
                    {
                        ProcessContextMenu();
                        e.Use();
                    }
                    break;

                case EventType.MouseUp:
                    IsDragged = false;
                    break;

                case EventType.MouseDrag:
                    if (e.button == 0 && IsDragged)
                    {
                        Drag(e.delta);
                        e.Use();
                        return true;
                    }
                    break;
            }

            return false;
        }

        private void ProcessContextMenu()
        {
            if (!CanEdit) return;

            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Delete node"), false, OnClickRemoveNode);
            genericMenu.ShowAsContext();
        }

        private void OnClickRemoveNode()
        {
            if (OnRemoveNode != null)
            {
                OnRemoveNode(this);
            }
        }

        // override object.Equals
        public override bool Equals(object obj)
        {
            var item = obj as NodeBase;
            if (item == null) return false;

            return item.Position == Position;
        }

        // override object.GetHashCode
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
