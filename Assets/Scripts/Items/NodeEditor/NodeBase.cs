using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace NodeEditor
{
    [Serializable]
    public class NodeBase : SimpleNodeBase
    {
        public ActionTypes ActionType;

        [NonSerialized]
        public ConnectionPoint InPoint;
        [NonSerialized]
        public ConnectionPoint OutPoint;

        [NonSerialized]
        public bool IsDragged;
        [NonSerialized]
        public bool IsSelected;

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

        //init node(NEW)
        public void Init(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle,
            GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> onClickInPoint, Action<ConnectionPoint> onClickOutPoint,
            Action<NodeBase> onClickRemoveNode, int id = 0, bool canEdit = true)
        {
            Position = position;
            Id = id;

            SetNodeStyle(width, height, nodeStyle, selectedStyle, inPointStyle, outPointStyle, onClickInPoint, onClickOutPoint, onClickRemoveNode, canEdit);
        }

        //set node basic style (Imported)
        public void SetNodeStyle(float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle,
            GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> onClickInPoint, Action<ConnectionPoint> onClickOutPoint,
            Action<NodeBase> onClickRemoveNode, bool canEdit = true)
        {
            Rect = new Rect(Position.x, Position.y, width, height + 5);
            Style = nodeStyle;
            InPoint = new ConnectionPoint(this, ConnectionPointType.In, inPointStyle, onClickInPoint);
            OutPoint = new ConnectionPoint(this, ConnectionPointType.Out, outPointStyle, onClickOutPoint);
            DefaultNodeStyle = nodeStyle;
            SelectedNodeStyle = selectedStyle;
            OnRemoveNode = onClickRemoveNode;

            WhiteTxtStyle = new GUIStyle();
            WhiteTxtStyle.normal.textColor = Color.white;
            WhiteTxtStyle.focused.textColor = Color.yellow;

            if (ActionType == ActionTypes.Start)
            {
                WhiteTxtStyle.fontSize = 20;
                WhiteTxtStyle.fontStyle = FontStyle.Bold;
            }

            CanEdit = canEdit;

            //set titles
            switch (ActionType)
            {
                //case ActionTypes.Start:
                //    Title = "START";
                //    break;
                case ActionTypes.CharcterSpriteInfos:
                    Title = "Character sprite";
                    break;
                case ActionTypes.DialogBox:
                    Title = "Dialog";
                    break;
                case ActionTypes.BrancheBox:
                    Title = "Branches";
                    break;
                case ActionTypes.BackgroundItem:
                    Title = "Background";
                    break;
                case ActionTypes.CGInfoItem:
                    Title = "CG";
                    break;
                case ActionTypes.Audio:
                    Title = "Background music";
                    break;
                case ActionTypes.Sound:
                    Title = "Sound";
                    break;
                case ActionTypes.Delayer:
                    Title = "Delayer";
                    break;
            }
        }

        public void Drag(Vector2 delta)
        {
            Rect.position += delta;
            Position = Rect.position;
        }

        public virtual void Draw()
        { }

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
        //public override bool Equals(object obj)
        //{
        //    var item = obj as NodeBase;
        //    Debug.Log(item);
        //    if (item == null) return false;

        //    return item.Position == Position && item.Id == Id;
        //}

        //// override object.GetHashCode
        //public override int GetHashCode()
        //{
        //    return base.GetHashCode();
        //}
    }
}
