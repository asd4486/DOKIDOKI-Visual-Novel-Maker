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
        public float DefaultRectHeight;

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
        public Action<NodeBase> OnCopyNode;
        [NonSerialized]
        public Action<NodeBase> OnRemoveNode;
        [NonSerialized]
        public bool CanEdit;

        public NodeBase() { }

        public NodeBase(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle,
            GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> onClickInPoint, Action<ConnectionPoint> onClickOutPoint,
            Action<NodeBase> onClickCopyNode, Action<NodeBase> onClickRemoveNode, int id = 0, bool canEdit = true)
        {
            Init(position, width, height, nodeStyle, selectedStyle, inPointStyle, outPointStyle, onClickInPoint, onClickOutPoint, onClickCopyNode, onClickRemoveNode, id, canEdit);
        }

        //init node(NEW)
        public void Init(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle,
            GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> onClickInPoint, Action<ConnectionPoint> onClickOutPoint,
            Action<NodeBase> onClickCopyNode, Action<NodeBase> onClickRemoveNode, int id = 0, bool canEdit = true)
        {
            Position = position;
            Id = id;

            SetNodeStyle(width, height, nodeStyle, selectedStyle, inPointStyle, outPointStyle, onClickInPoint, onClickOutPoint, onClickCopyNode, onClickRemoveNode, canEdit);
        }

        //set node basic style (Imported)
        public void SetNodeStyle(float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle,
            GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> onClickInPoint, Action<ConnectionPoint> onClickOutPoint,
           Action<NodeBase> onClickCopyNode, Action<NodeBase> onClickRemoveNode, bool canEdit = true)
        {
            DefaultRectHeight = height;
            Rect = new Rect(Position.x, Position.y, width, height);
            Style = nodeStyle;
            InPoint = new ConnectionPoint(this, ConnectionPointType.In, inPointStyle, onClickInPoint);
            OutPoint = new ConnectionPoint(this, ConnectionPointType.Out, outPointStyle, onClickOutPoint);
            DefaultNodeStyle = nodeStyle;
            SelectedNodeStyle = selectedStyle;
            OnCopyNode = onClickCopyNode;
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
                case ActionTypes.CharcterSpriteInfos:
                    Title = "Character sprite";
                    break;
                case ActionTypes.CharacterOutInfos:
                    Title = "Character out";
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
                case ActionTypes.ChangeStoryLine:
                    Title = "Play storyline";
                    break;
                case ActionTypes.ChangeScene:
                    Title = "Change scene";
                    break;
            }
        }

        public void Drag(Vector2 delta)
        {
            Rect.position += delta;
            Position = Rect.position;
        }

        public virtual NodeBase Clone(Vector2 pos ,int newId)
        {
            return new NodeBase(pos, Rect.width,  Rect.height, Style, SelectedNodeStyle, InPoint.Style,  
                OutPoint.Style, InPoint.OnClickConnectionPoint, OutPoint.OnClickConnectionPoint,
                OnCopyNode, OnRemoveNode, newId, CanEdit);
        }

        public virtual void Draw(){ }

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

                    if (e.button == 1 )
                    {
                        if(Rect.Contains(e.mousePosition))
                        {
                            ProcessContextMenu();
                            e.Use();
                        }

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
                case EventType.KeyDown:
                    switch (e.keyCode)
                    {
                        //delete node if is selected
                        case KeyCode.Delete:
                            if (IsSelected) OnClickRemoveNode();
                            break;
                    }
                    break;
            }

            return false;
        }

        private void ProcessContextMenu()
        {
            if (!CanEdit) return;

            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Copy"), false, OnClickCopy);
            genericMenu.AddItem(new GUIContent("Delete"), false, OnClickRemoveNode);
            genericMenu.ShowAsContext();
        }

        private void OnClickCopy()
        {
            if (OnCopyNode != null)
            {
                OnCopyNode(this);
            }
        }

        private void OnClickRemoveNode()
        {
            if (OnRemoveNode != null)
            {
                OnRemoveNode(this);
            }
        }
    }
}
