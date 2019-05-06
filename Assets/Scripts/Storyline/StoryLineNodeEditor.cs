using DokiVnMaker.MyEditor.Items;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DokiVnMaker.MyEditor
{
    public class StoryLineNodeEditor : EditorWindow
    {
        static StoryLine CurrentStoryLine { get; set; }

        private EditorStartPoint StartNode;
        private NodeBaseList _NodeBaseList;

        private float NodeWidth = 250;

        private GUIStyle NodeStyle;
        private GUIStyle SelectedNodeStyle;
        private GUIStyle InPointStyle;
        private GUIStyle OutPointStyle;

        private List<Connection> Connections = new List<Connection>();

        private ConnectionPoint SelectedInPoint;
        private ConnectionPoint SelectedOutPoint;

        private Vector2 Offset;
        private Vector2 Drag;

        private Texture2D BackgroundColor;

        float ZoomScale = 1.0f;
        Vector2 VanishingPoint = new Vector2(0, 21);

        private Vector2 MousePos;

        private NodeBase NodeCopied;

        public static void InitWindow(StoryLine story)
        {
            //load saved data
            //ActionList = StoryLine.OnLoadData();
            //set curretn editor
            CurrentStoryLine = story;
            //open window
            Init();
        }


        //[MenuItem("TEST/Node Based Editor")]
        static void Init()
        {
            var window = (StoryLineNodeEditor)GetWindow(typeof(StoryLineNodeEditor), false, "DokiDoki Story line");
            window.minSize = new Vector2(800, 500);
            window.Show();
        }

        private void OnEnable()
        {
            //EditorPrefs.DeleteAll();
            //if (EditorPrefs.HasKey(CurrentStoryLine.DataFileName))
            //{
            //    string objectPath = EditorPrefs.GetString(CurrentStoryLine.DataFileName);
            //    NodeList = AssetDatabase.LoadAssetAtPath(objectPath, typeof(NodebaseList.Nodes)) as NodebaseList.Nodes;
            //    return;
            //}

            //CreateNodeList();
            InitEditor();
        }

        private void OnDisable()
        {
            var actions = StoryLineActions.InitActions(_NodeBaseList.Nodes);
            CurrentStoryLine.OnSaveData(actions, Connections);
        }

        private void InitEditor()
        {
            _NodeBaseList = new NodeBaseList();
            _NodeBaseList.Nodes = CurrentStoryLine.OnLoadNodes();

            //editor background color
            BackgroundColor = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            BackgroundColor.SetPixel(0, 0, new Color(0.2f, 0.2f, 0.2f));
            BackgroundColor.Apply();

            //node radius
            var radius = 10;

            InPointStyle = new GUIStyle();
            InPointStyle.normal.background = AssetDatabase.LoadAssetAtPath("Assets/ImgSources/T_circle.png", typeof(Texture2D)) as Texture2D;
            InPointStyle.hover.background = AssetDatabase.LoadAssetAtPath("Assets/ImgSources/T_circle_active.png", typeof(Texture2D)) as Texture2D;
            //InPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
            //InPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
            //InPointStyle.border = new RectOffset(4, 4, radius, radius);

            OutPointStyle = new GUIStyle();
            OutPointStyle.normal.background = AssetDatabase.LoadAssetAtPath("Assets/ImgSources/T_circle.png", typeof(Texture2D)) as Texture2D;
            OutPointStyle.hover.background = AssetDatabase.LoadAssetAtPath("Assets/ImgSources/T_circle_active.png", typeof(Texture2D)) as Texture2D;
            //OutPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D;
            //OutPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
            //OutPointStyle.border = new RectOffset(4, 4, radius, radius);

            //draw start node
            var starttex = new Texture2D(12, 12);
            var fillColorArray = starttex.GetPixels();
            for (var i = 0; i < fillColorArray.Length; ++i)
            {
                fillColorArray[i] = new Color(0.2f, 0.8f, 0.2f, 0.7f);
            }
            starttex.SetPixels(fillColorArray);
            starttex.Apply();

            var startNodeStyle = new GUIStyle();
            startNodeStyle.normal.background = starttex;

            var starttex2 = new Texture2D(12, 12);
            var fillColorArray2 = starttex2.GetPixels();
            for (var i = 0; i < fillColorArray2.Length; ++i)
            {
                fillColorArray2[i] = new Color(0.2f, 0.8f, 0.2f, 0.9f);
            }
            starttex2.SetPixels(fillColorArray2);
            starttex2.Apply();

            var startSelectedNodeStyle = new GUIStyle();
            startSelectedNodeStyle.normal.background = starttex2;

            var tex = new Texture2D(12, 12);
            fillColorArray = tex.GetPixels();
            for (var i = 0; i < fillColorArray.Length; ++i)
            {
                fillColorArray[i] = new Color(0.4f, 0.4f, 0.4f, 0.7f);
            }
            tex.SetPixels(fillColorArray);
            tex.Apply();

            NodeStyle = new GUIStyle();
            NodeStyle.normal.background = tex;
            NodeStyle.normal.textColor = Color.gray;
            NodeStyle.border = new RectOffset(radius, radius, radius, radius);

            var tex2 = new Texture2D(12, 12);
            fillColorArray2 = tex2.GetPixels();
            for (var i = 0; i < fillColorArray2.Length; ++i)
            {
                fillColorArray2[i] = new Color(0.4f, 0.4f, 0.4f, 0.9f);
            }
            tex2.SetPixels(fillColorArray2);
            tex2.Apply();

            SelectedNodeStyle = new GUIStyle();
            SelectedNodeStyle.normal.background = tex2;
            SelectedNodeStyle.normal.textColor = Color.yellow;
            SelectedNodeStyle.border = new RectOffset(radius, radius, radius, radius);

            //import existed node 
            if (_NodeBaseList.Nodes.Count > 0)
            {
                foreach (var n in _NodeBaseList.Nodes)
                {
                    switch (n.ActionType)
                    {
                        case ActionTypes.Start:
                            n.SetNodeStyle(NodeWidth / 2, 30, 
                                startNodeStyle, startSelectedNodeStyle, InPointStyle, OutPointStyle, OnClickInPoint, OnClickOutPoint, OnClickCopyNode, OnClickRemoveNode, false);
                            break;
                        case ActionTypes.CharcterSpriteInfos:
                            n.SetNodeStyle(NodeWidth, 110, 
                                NodeStyle, SelectedNodeStyle, InPointStyle, OutPointStyle, OnClickInPoint, OnClickOutPoint, OnClickCopyNode, OnClickRemoveNode);
                            break;
                        case ActionTypes.CharacterOutInfos:
                            n.SetNodeStyle(NodeWidth, 60, 
                                NodeStyle, SelectedNodeStyle, InPointStyle, OutPointStyle, OnClickInPoint, OnClickOutPoint, OnClickCopyNode, OnClickRemoveNode);
                            break;
                        case ActionTypes.DialogBox:
                            n.SetNodeStyle(NodeWidth, 180, 
                                NodeStyle, SelectedNodeStyle, InPointStyle, OutPointStyle, OnClickInPoint, OnClickOutPoint, OnClickCopyNode, OnClickRemoveNode);
                            break;
                        case ActionTypes.BrancheBox:
                            n.SetNodeStyle(NodeWidth, 80, 
                                NodeStyle, SelectedNodeStyle, InPointStyle, OutPointStyle, OnClickInPoint, OnClickOutPoint, OnClickCopyNode, OnClickRemoveNode);
                            (n as BrancheBox).SetOutPointStyle(OutPointStyle, OnClickInPoint);
                            break;
                        case ActionTypes.BackgroundItem:
                            n.SetNodeStyle(NodeWidth, 60, 
                                NodeStyle, SelectedNodeStyle, InPointStyle, OutPointStyle, OnClickInPoint, OnClickOutPoint, OnClickCopyNode, OnClickRemoveNode);
                            break;
                        case ActionTypes.CGInfoItem:
                            n.SetNodeStyle(NodeWidth, 180, 
                                NodeStyle, SelectedNodeStyle, InPointStyle, OutPointStyle, OnClickInPoint, OnClickOutPoint, OnClickCopyNode, OnClickRemoveNode);
                            break;
                        case ActionTypes.Delayer:
                            n.SetNodeStyle(NodeWidth, 40,
                                NodeStyle, SelectedNodeStyle, InPointStyle, OutPointStyle, OnClickInPoint, OnClickOutPoint, OnClickCopyNode, OnClickRemoveNode);
                            break;
                        case ActionTypes.Audio:
                            n.SetNodeStyle(NodeWidth, 60, 
                                NodeStyle, SelectedNodeStyle, InPointStyle, OutPointStyle, OnClickInPoint, OnClickOutPoint, OnClickCopyNode, OnClickRemoveNode);
                            break;
                        case ActionTypes.Sound:
                            n.SetNodeStyle(NodeWidth, 80, 
                                NodeStyle, SelectedNodeStyle, InPointStyle, OutPointStyle, OnClickInPoint, OnClickOutPoint, OnClickCopyNode, OnClickRemoveNode);
                            break;
                        case ActionTypes.ChangeStoryLine:
                            n.SetNodeStyle(NodeWidth, 40, 
                                NodeStyle, SelectedNodeStyle, InPointStyle, OutPointStyle, OnClickInPoint, OnClickOutPoint, OnClickCopyNode, OnClickRemoveNode);
                            break;
                        case ActionTypes.ChangeScene:
                            n.SetNodeStyle(NodeWidth, 40, 
                                NodeStyle, SelectedNodeStyle, InPointStyle, OutPointStyle, OnClickInPoint, OnClickOutPoint, OnClickCopyNode, OnClickRemoveNode);
                            break;
                    }
                }

                //import connections
                Connections = CurrentStoryLine.OnLoadConnections();
                foreach (var c in Connections)
                {
                    c.OnClickRemoveConnection = OnClickRemoveConnection;
                    c.InPoint = _NodeBaseList.Nodes.Where(n => n.Id == c.InPoint.Node.Id).FirstOrDefault().InPoint;

                    var outNode = _NodeBaseList.Nodes.Where(n => n.Id == c.OutPoint.Node.Id && n.ParentId == c.OutPoint.Node.ParentId).FirstOrDefault();
                    //set out connection(each node only have one out connection)
                    outNode.OutConnection = c;
                    c.OutPoint = outNode.OutPoint;
                }
            }
            //or create new
            else
            {
                var y = position.height / 5;
                StartNode = new EditorStartPoint(new Vector2(20, y), NodeWidth / 2, 30, startNodeStyle, startSelectedNodeStyle, InPointStyle, OutPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode);
                //add start node to action nodes
                if (!_NodeBaseList.Nodes.Contains(StartNode)) _NodeBaseList.Nodes.Insert(0, StartNode);
            }
        }

        private void OnGUI()
        {
            Zoom();

            GUI.DrawTexture(new Rect(0, 0, maxSize.x, maxSize.y), BackgroundColor, ScaleMode.StretchToFill);

            DrawGrid(20, 0.2f, Color.gray);
            DrawGrid(100, 0.4f, Color.gray);

            DrawNodes();
            DrawConnections();

            DrawConnectionLine(Event.current);
            //get mouse position if is in window
            if (Event.current.mousePosition.x > 0 && Event.current.mousePosition.y > 0)
                MousePos = Event.current.mousePosition;
            ProcessNodeEvents(Event.current);

            ProcessEvents(Event.current);

            if (GUI.changed) Repaint();
        }

        void OnInspectorUpdate()
        {
            //repaint for get mouse position when cursor is in window
            if (focusedWindow == this && mouseOverWindow == this)
            {
                Repaint();
                //Debug.Log(MousePos);
            }
        }

        private void Zoom()
        {
            //window Zoom
            Matrix4x4 translation = Matrix4x4.TRS(VanishingPoint, Quaternion.identity, Vector3.one);
            Matrix4x4 scale = Matrix4x4.Scale(new Vector3(ZoomScale, ZoomScale, 1.0f));
            GUI.matrix = translation * scale * translation.inverse;
        }


        #region draw funcs
        private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
        {
            int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
            int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

            Handles.BeginGUI();
            Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

            Offset += Drag * 0.5f;
            Vector3 newOffset = new Vector3(Offset.x % gridSpacing, Offset.y % gridSpacing, 0);

            for (int i = 0; i < widthDivs; i++)
            {
                Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
            }

            for (int j = 0; j < heightDivs; j++)
            {
                Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
            }

            Handles.color = Color.white;
            Handles.EndGUI();
        }

        private void DrawNodes()
        {
            if (_NodeBaseList.Nodes != null)
            {
                for (int i = 0; i < _NodeBaseList.Nodes.Count; i++)
                {
                    _NodeBaseList.Nodes[i].Draw();
                }
            }
        }

        private void DrawConnections()
        {
            if (Connections != null)
            {
                for (int i = 0; i < Connections.Count; i++)
                {
                    Connections[i].Draw();
                }
            }
        }


        private void DrawConnectionLine(Event e)
        {
            if (SelectedInPoint != null && SelectedOutPoint == null)
            {
                Handles.DrawBezier(
                    SelectedInPoint.Rect.center,
                    e.mousePosition,
                    SelectedInPoint.Rect.center + Vector2.left * 50f,
                    e.mousePosition - Vector2.left * 50f,
                    Color.white,
                    null,
                    2f
                );

                GUI.changed = true;
            }

            if (SelectedOutPoint != null && SelectedInPoint == null)
            {
                Handles.DrawBezier(
                    SelectedOutPoint.Rect.center,
                    e.mousePosition,
                    SelectedOutPoint.Rect.center - Vector2.left * 50f,
                    e.mousePosition + Vector2.left * 50f,
                    Color.white,
                    null,
                    2f
                );

                GUI.changed = true;
            }
        }
        #endregion


        #region user controls
        private void ProcessEvents(Event e)
        {
            Drag = Vector2.zero;
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        if (SelectedInPoint != null || SelectedOutPoint != null)
                        {
                            ClearConnectionSelection();
                        }
                    }
                    if (e.button == 1)
                    {
                        ProcessContextMenu(MousePos);
                    }
                    break;
                //drag
                case EventType.MouseDrag:
                    if (e.button == 0)
                    {
                        OnDrag(e.delta);
                    }
                    break;
                //zoom
                case EventType.ScrollWheel:
                    var zoomDelta = 0.1f;
                    zoomDelta = e.delta.y < 0 ? zoomDelta : -zoomDelta;
                    ZoomScale += zoomDelta;
                    ZoomScale = Mathf.Clamp(ZoomScale, 0.25f, 1.25f);

                    e.Use();
                    break;
                case EventType.ValidateCommand:
                    // without this line we won't get ExecuteCommand
                    switch (e.commandName)
                    {
                        case "Copy":
                        case "Paste":
                            e.Use();
                            break;
                    }
                    break;
                case EventType.ExecuteCommand:
                    switch (e.commandName)
                    {
                        //copy selected node
                        case "Copy":
                            CopyAction();
                            break;
                        //past new node
                        case "Paste":
                            PasteAction(MousePos);
                            break;
                    }
                    break;
            }
        }

        private void OnDrag(Vector2 delta)
        {
            Drag = delta;

            if (_NodeBaseList.Nodes != null)
            {
                for (int i = 0; i < _NodeBaseList.Nodes.Count; i++)
                {
                    _NodeBaseList.Nodes[i].Drag(delta);
                }
            }

            GUI.changed = true;
        }

        private void ProcessNodeEvents(Event e)
        {
            if (_NodeBaseList.Nodes != null)
            {
                for (int i = _NodeBaseList.Nodes.Count - 1; i >= 0; i--)
                {
                    bool guiChanged = _NodeBaseList.Nodes[i].ProcessEvents(e);

                    if (guiChanged)
                    {
                        GUI.changed = true;
                    }
                }
            }
        }

        private void ProcessContextMenu(Vector2 mousePosition)
        {
            GenericMenu genericMenu = new GenericMenu();
            if (NodeCopied == null)
                genericMenu.AddDisabledItem(new GUIContent("Paste"));
            else
                genericMenu.AddItem(new GUIContent("Paste"), false, () => PasteAction(mousePosition));

            genericMenu.AddItem(new GUIContent("Character/Character sprite"), false, () => AddNewAction(ActionTypes.CharcterSpriteInfos, mousePosition));
            genericMenu.AddItem(new GUIContent("Character/Character out"), false, () => AddNewAction(ActionTypes.CharacterOutInfos, mousePosition));

            genericMenu.AddItem(new GUIContent("Dialog/Dialog box"), false, () => AddNewAction(ActionTypes.DialogBox, mousePosition));
            genericMenu.AddItem(new GUIContent("Dialog/Brahche"), false, () => AddNewAction(ActionTypes.BrancheBox, mousePosition));

            genericMenu.AddItem(new GUIContent("Image/Background"), false, () => AddNewAction(ActionTypes.BackgroundItem, mousePosition));
            genericMenu.AddItem(new GUIContent("Image/CG"), false, () => AddNewAction(ActionTypes.CGInfoItem, mousePosition));

            genericMenu.AddItem(new GUIContent("Audio/Background music"), false, () => AddNewAction(ActionTypes.Audio, mousePosition));
            genericMenu.AddItem(new GUIContent("Audio/Sound"), false, () => AddNewAction(ActionTypes.Sound, mousePosition));

            genericMenu.AddItem(new GUIContent("Time/Delayer"), false, () => AddNewAction(ActionTypes.Delayer, mousePosition));

            genericMenu.AddItem(new GUIContent("Story/Play storyline"), false, () => AddNewAction(ActionTypes.ChangeStoryLine, mousePosition));
            genericMenu.AddItem(new GUIContent("Story/Change scene"), false, () => AddNewAction(ActionTypes.ChangeScene, mousePosition));

            genericMenu.ShowAsContext();
        }

        //copy node
        private void CopyAction()
        {
            var selected = _NodeBaseList.Nodes.Where(n => n.IsSelected == true).FirstOrDefault();
            if (selected != null) NodeCopied = selected;

        }

        private void PasteAction(Vector2 pos)
        {
            //return if node is null
            if (NodeCopied == null) return;

            //clone node
            var newNode = NodeCopied.Clone(pos, _NodeBaseList.SetNodeId());
            _NodeBaseList.Nodes.Add(newNode);
        }

        public void AddNewAction(ActionTypes type, Vector2 position)
        {
            var id = _NodeBaseList.Nodes.Count;
            switch (type)
            {
                case ActionTypes.CharcterSpriteInfos:
                    _NodeBaseList.Nodes.Add(new CharcterSpriteInfos(position, NodeWidth, 105, NodeStyle, SelectedNodeStyle, InPointStyle, OutPointStyle,
                                                            OnClickInPoint, OnClickOutPoint, OnClickCopyNode, OnClickRemoveNode, _NodeBaseList.SetNodeId()));
                    break;
                case ActionTypes.CharacterOutInfos:
                    _NodeBaseList.Nodes.Add(new CharacterOutInfos(position, NodeWidth, 60, NodeStyle, SelectedNodeStyle, InPointStyle, OutPointStyle,
                                                            OnClickInPoint, OnClickOutPoint, OnClickCopyNode, OnClickRemoveNode, _NodeBaseList.SetNodeId()));
                    break;
                case ActionTypes.DialogBox:
                    _NodeBaseList.Nodes.Add(new DialogBox(position, NodeWidth, 180, NodeStyle, SelectedNodeStyle, InPointStyle, OutPointStyle,
                                                    OnClickInPoint, OnClickOutPoint, OnClickCopyNode, OnClickRemoveNode, _NodeBaseList.SetNodeId()));
                    break;
                case ActionTypes.BrancheBox:
                    _NodeBaseList.Nodes.Add(new BrancheBox(position, NodeWidth, 80, NodeStyle, SelectedNodeStyle, InPointStyle, OutPointStyle, 
                                                    OnClickInPoint, OnClickOutPoint, OnClickCopyNode, OnClickRemoveNode, _NodeBaseList.SetNodeId()));
                    break;
                case ActionTypes.BackgroundItem:
                    _NodeBaseList.Nodes.Add(new BackgroundItem(position, NodeWidth, 60, NodeStyle, SelectedNodeStyle, InPointStyle,
                                                    OutPointStyle, OnClickInPoint, OnClickOutPoint, OnClickCopyNode, OnClickRemoveNode, _NodeBaseList.SetNodeId()));
                    break;
                case ActionTypes.CGInfoItem:
                    _NodeBaseList.Nodes.Add(new CGInfoItem(position, NodeWidth, 180, NodeStyle, SelectedNodeStyle, InPointStyle,
                                                    OutPointStyle, OnClickInPoint, OnClickOutPoint, OnClickCopyNode, OnClickRemoveNode, _NodeBaseList.SetNodeId()));
                    break;
                case ActionTypes.Delayer:
                    _NodeBaseList.Nodes.Add(new Delayer(position, NodeWidth, 40, NodeStyle, SelectedNodeStyle, InPointStyle, OutPointStyle,
                                                    OnClickInPoint, OnClickOutPoint, OnClickCopyNode, OnClickRemoveNode, _NodeBaseList.SetNodeId()));
                    break;
                case ActionTypes.Audio:
                    _NodeBaseList.Nodes.Add(new Audio(position, NodeWidth, 60, NodeStyle, SelectedNodeStyle, InPointStyle, OutPointStyle,
                                                    OnClickInPoint, OnClickOutPoint, OnClickCopyNode, OnClickRemoveNode, _NodeBaseList.SetNodeId()));
                    break;
                case ActionTypes.Sound:
                    _NodeBaseList.Nodes.Add(new Sound(position, NodeWidth, 80, NodeStyle, SelectedNodeStyle, InPointStyle, OutPointStyle,
                                                OnClickInPoint, OnClickOutPoint, OnClickCopyNode, OnClickRemoveNode, _NodeBaseList.SetNodeId()));
                    break;
                case ActionTypes.ChangeStoryLine:
                    _NodeBaseList.Nodes.Add(new ChangeStoryLine(position, NodeWidth, 40, NodeStyle, SelectedNodeStyle, InPointStyle, OutPointStyle,
                                                OnClickInPoint, OnClickOutPoint, OnClickCopyNode, OnClickRemoveNode, _NodeBaseList.SetNodeId()));
                    break;
                case ActionTypes.ChangeScene:
                    _NodeBaseList.Nodes.Add(new ChangeScene(position, NodeWidth, 40, NodeStyle, SelectedNodeStyle, InPointStyle, OutPointStyle,
                                                    OnClickInPoint, OnClickOutPoint, OnClickCopyNode, OnClickRemoveNode, _NodeBaseList.SetNodeId()));
                    break;
            }
        }


        #endregion

        #region node editor func

        private void OnClickInPoint(ConnectionPoint inPoint)
        {
            SelectedInPoint = inPoint;

            if (SelectedOutPoint != null)
            {
                if (SelectedOutPoint.Node != SelectedInPoint.Node)
                {
                    CreateConnection();
                    ClearConnectionSelection();
                }
                else
                {
                    ClearConnectionSelection();
                }
            }
        }

        private void OnClickOutPoint(ConnectionPoint outPoint)
        {
            SelectedOutPoint = outPoint;

            if (SelectedInPoint != null)
            {
                if (SelectedOutPoint.Node != SelectedInPoint.Node)
                {
                    CreateConnection();
                    ClearConnectionSelection();
                }
                else
                {
                    ClearConnectionSelection();
                }
            }
        }

        //copie node
        private void OnClickCopyNode(NodeBase node)
        {
            NodeCopied = node;
        }

        private void OnClickRemoveNode(NodeBase node)
        {
            if (Connections != null)
            {
                List<Connection> connectionsToRemove = new List<Connection>();

                for (int i = 0; i < Connections.Count; i++)
                {
                    if (Connections[i].InPoint == node.InPoint || Connections[i].OutPoint == node.OutPoint)
                    {
                        connectionsToRemove.Add(Connections[i]);
                    }
                }

                for (int i = 0; i < connectionsToRemove.Count; i++)
                {
                    Connections.Remove(connectionsToRemove[i]);
                }

                connectionsToRemove = null;
            }

            var removeNode = _NodeBaseList.Nodes.Where(n => n.Equals(node)).FirstOrDefault();
            if (removeNode != null) _NodeBaseList.Nodes.Remove(removeNode);

            GUI.changed = true;
        }

        private void OnClickRemoveConnection(Connection conn)
        {
            if (Connections.Contains(conn))
            {
                conn.OutPoint.Node.OutConnection = null;
                Connections.Remove(conn);
            }
        }

        private void CreateConnection()
        {
            //return if in out point are null
            if (SelectedInPoint == null && SelectedOutPoint == null) return;

            var connection = new Connection(SelectedInPoint, SelectedOutPoint, OnClickRemoveConnection);

            //remove old out connection(out connection has only one)
            if (SelectedOutPoint.Node.OutConnection != null)
            {
                var oldConn = SelectedOutPoint.Node.OutConnection;
                if (Connections.Contains(oldConn)) Connections.Remove(oldConn);
            }

            SelectedOutPoint.Node.OutConnection = connection;
            if (!Connections.Contains(connection))
            {
                Connections.Add(connection);
            }
        }

        private void ClearConnectionSelection()
        {
            SelectedInPoint = null;
            SelectedOutPoint = null;
        }
        #endregion
    }
}