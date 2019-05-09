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
        private List<NodeBase> nodeActionList;

        private float NodeWidth = 250;

        private GUIStyle NodeStyle;
        private GUIStyle SelectedNodeStyle;
        private GUIStyle InPointStyle;
        private GUIStyle outPointStyle;

        private List<Connection> Connections = new List<Connection>();

        private ConnectionPoint selectedInPoint;
        private ConnectionPoint selectedOutPoint;

        private Vector2 Offset;
        private Vector2 dragPos;

        private Texture2D backgroundColor;

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
            var story = StoryLineActions.Create(nodeActionList);
            CurrentStoryLine.OnSaveData(story, Connections);
        }

        private void InitEditor()
        {
            nodeActionList = CurrentStoryLine.OnLoadNodes();

            //editor background color
            backgroundColor = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            backgroundColor.SetPixel(0, 0, new Color(0.2f, 0.2f, 0.2f));
            backgroundColor.Apply();

            //node radius
            var radius = 10;

            InPointStyle = new GUIStyle();
            InPointStyle.normal.background = AssetDatabase.LoadAssetAtPath("Assets/ImgSources/T_circle.png", typeof(Texture2D)) as Texture2D;
            InPointStyle.hover.background = AssetDatabase.LoadAssetAtPath("Assets/ImgSources/T_circle_active.png", typeof(Texture2D)) as Texture2D;
            //InPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
            //InPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
            //InPointStyle.border = new RectOffset(4, 4, radius, radius);

            outPointStyle = new GUIStyle();
            outPointStyle.normal.background = AssetDatabase.LoadAssetAtPath("Assets/ImgSources/T_circle.png", typeof(Texture2D)) as Texture2D;
            outPointStyle.hover.background = AssetDatabase.LoadAssetAtPath("Assets/ImgSources/T_circle_active.png", typeof(Texture2D)) as Texture2D;
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
            if (nodeActionList.Count > 0)
            {
                foreach (var n in nodeActionList)
                {
                    switch (n.ActionType)
                    {
                        case ActionTypes.StartPoint:
                            n.SetRectInfo(NodeWidth / 2, 30);
                            n.CanEdit = false;
                            break;
                        case ActionTypes.CharcterSpriteInfos:
                            n.SetRectInfo(NodeWidth, 110);
                            break;
                        case ActionTypes.CharacterOutInfos:
                            n.SetRectInfo(NodeWidth, 60);
                            break;
                        case ActionTypes.DialogBox:
                            n.SetRectInfo(NodeWidth, 180);
                            break;
                        case ActionTypes.BrancheBox:
                            n.SetRectInfo(NodeWidth, 80);
                            (n as BrancheBox).SetOutPointStyle(outPointStyle, onClickOutPoint);
                            break;
                        case ActionTypes.BackgroundImage:
                            n.SetRectInfo(NodeWidth, 60);
                            break;
                        case ActionTypes.CGImage:
                            n.SetRectInfo(NodeWidth, 180);
                            break;
                        case ActionTypes.Delayer:
                            n.SetRectInfo(NodeWidth, 40);
                            break;
                        case ActionTypes.Audio:
                            n.SetRectInfo(NodeWidth, 60);
                            break;
                        case ActionTypes.Sound:
                            n.SetRectInfo(NodeWidth, 80);
                            break;
                        case ActionTypes.ChangeStoryLine:
                            n.SetRectInfo(NodeWidth, 40);
                            break;
                        case ActionTypes.ChangeScene:
                            n.SetRectInfo(NodeWidth, 40);
                            break;
                    }

                    if (n.ActionType == ActionTypes.StartPoint)
                    {
                        n.SetNodeStyle(startNodeStyle, startSelectedNodeStyle, InPointStyle, outPointStyle, onClickInPoint, onClickOutPoint, OnClickCopyNode, OnClickRemoveNode);
                    }
                    else
                    {
                        n.SetNodeStyle(NodeStyle, SelectedNodeStyle, InPointStyle, outPointStyle, onClickInPoint, onClickOutPoint, OnClickCopyNode, OnClickRemoveNode);
                    }
                }

                //import connections
                Connections = CurrentStoryLine.OnLoadConnections();
                foreach (var c in Connections)
                {
                    c.OnClickRemoveConnection = OnClickRemoveConnection;

                    if (c.InPoint.targetItemId > -1)
                    {
                    }
                    else
                        c.InPoint = nodeActionList.Where(n => n.Id == c.InPoint.targetNodeId).FirstOrDefault().InPoint;

                    ConnectionPoint outPoint = new ConnectionPoint();
                    if (c.OutPoint.targetItemId > -1)
                    {
                        var node = nodeActionList.Where(n => n.Id == c.OutPoint.targetNodeId).FirstOrDefault();
                        if (node is BrancheBox)
                        {
                            outPoint = (node as BrancheBox).outPointList[c.OutPoint.targetItemId];
                        }
                    }
                    else
                    {
                        outPoint = nodeActionList.Where(n => n.Id == c.OutPoint.targetNodeId).FirstOrDefault().OutPoint;
                    }

                    //set out connection(each node only have one out connection)
                    outPoint.SetConnection(c);
                    c.OutPoint = outPoint;
                }
            }
            //or create new
            else
            {
                var y = position.height / 5;
                StartNode = new EditorStartPoint();
                //add start node to action nodes
                StartNode.Init(
                    new Vector2(20, y), NodeWidth / 2, 30,
                    startNodeStyle, startSelectedNodeStyle, null, outPointStyle, null, onClickOutPoint, null, null,
                    0, _canEdit: false);

                if (!nodeActionList.Contains(StartNode)) nodeActionList.Insert(0, StartNode);
            }
        }

        private void OnGUI()
        {
            Zoom();

            GUI.DrawTexture(new Rect(0, 0, maxSize.x, maxSize.y), backgroundColor, ScaleMode.StretchToFill);

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

            Offset += dragPos * 0.5f;
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
            if (nodeActionList != null)
            {
                for (int i = 0; i < nodeActionList.Count; i++)
                {
                    nodeActionList[i].Draw();
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
            if (selectedInPoint != null && selectedOutPoint == null)
            {
                Handles.DrawBezier(
                    selectedInPoint.myRect.center,
                    e.mousePosition,
                    selectedInPoint.myRect.center + Vector2.left * 50f,
                    e.mousePosition - Vector2.left * 50f,
                    Color.white,
                    null,
                    2f
                );

                GUI.changed = true;
            }

            if (selectedOutPoint != null && selectedInPoint == null)
            {
                Handles.DrawBezier(
                    selectedOutPoint.myRect.center,
                    e.mousePosition,
                    selectedOutPoint.myRect.center - Vector2.left * 50f,
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
            dragPos = Vector2.zero;
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        if (selectedInPoint != null || selectedOutPoint != null)
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
            dragPos = delta;

            if (nodeActionList != null)
            {
                for (int i = 0; i < nodeActionList.Count; i++)
                {
                    nodeActionList[i].Drag(delta);
                }
            }

            GUI.changed = true;
        }

        private void ProcessNodeEvents(Event e)
        {
            if (nodeActionList != null)
            {
                for (int i = nodeActionList.Count - 1; i >= 0; i--)
                {
                    bool guiChanged = nodeActionList[i].ProcessEvents(e);

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

            genericMenu.AddItem(new GUIContent("Image/Background"), false, () => AddNewAction(ActionTypes.BackgroundImage, mousePosition));
            genericMenu.AddItem(new GUIContent("Image/CG"), false, () => AddNewAction(ActionTypes.CGImage, mousePosition));

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
            var selected = nodeActionList.Where(n => n.IsSelected == true).FirstOrDefault();
            if (selected != null) NodeCopied = selected;

        }

        private void PasteAction(Vector2 pos)
        {
            //return if node is null
            if (NodeCopied == null) return;

            //clone node
            var newNode = NodeCopied.Clone(pos, SetNodeId());
            nodeActionList.Add(newNode);
        }

        public void AddNewAction(ActionTypes type, Vector2 position)
        {
            var id = nodeActionList.Count;
            NodeBase node = new NodeBase();
            float _width = NodeWidth;
            float _height = 0;

            switch (type)
            {
                case ActionTypes.CharcterSpriteInfos:
                    node = new CharcterSpriteInfos();
                    _height = 105;
                    break;
                case ActionTypes.CharacterOutInfos:
                    node = new CharacterOutInfos();
                    _height = 60;
                    break;
                case ActionTypes.DialogBox:
                    node = new DialogBox();
                    _height = 180;
                    break;
                case ActionTypes.BrancheBox:
                    node = new BrancheBox();
                    _height = 80;
                    (node as BrancheBox).SetOutPointStyle(outPointStyle, onClickOutPoint);
                    break;
                case ActionTypes.BackgroundImage:
                    node = new BackgroundImage();
                    _height = 60;
                    break;
                case ActionTypes.CGImage:
                    node = new CGImage();
                    _height = 180;
                    break;
                case ActionTypes.Delayer:
                    node = new Delayer();
                    _height = 40;
                    break;
                case ActionTypes.Audio:
                    node = new Audio();
                    _height = 60;
                    break;
                case ActionTypes.Sound:
                    node = new Sound();
                    _height = 80;
                    break;
                case ActionTypes.ChangeStoryLine:
                    node = new ChangeStoryLine();
                    _height = 40;
                    break;
                case ActionTypes.ChangeScene:
                    node = new ChangeScene();
                    _height = 40;
                    break;
            }

            node.Init(position, _width, _height, NodeStyle, SelectedNodeStyle, InPointStyle, outPointStyle,
            onClickInPoint, onClickOutPoint, OnClickCopyNode, OnClickRemoveNode, SetNodeId());

            nodeActionList.Add(node);
        }


        #endregion

        #region node editor func

        private void onClickInPoint(ConnectionPoint inPoint)
        {
            selectedInPoint = inPoint;

            if (selectedOutPoint != null)
            {
                // if connect node isn't the same node
                if (!selectedOutPoint.HasSameNode(selectedInPoint))
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

        private void onClickOutPoint(ConnectionPoint outPoint)
        {
            selectedOutPoint = outPoint;

            if (selectedInPoint != null)
            {
                // if connect node isn't the same node
                if (!selectedOutPoint.HasSameNode(selectedInPoint))
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

            var removeNode = nodeActionList.Where(n => n.Equals(node)).FirstOrDefault();
            if (removeNode != null) nodeActionList.Remove(removeNode);

            GUI.changed = true;
        }

        private void OnClickRemoveConnection(Connection conn)
        {
            if (Connections.Contains(conn))
            {
                conn.OutPoint.SetConnection(null);
                Connections.Remove(conn);
            }
        }

        private void CreateConnection()
        {
            //return if in out point are null
            if (selectedInPoint == null && selectedOutPoint == null) return;

            var connection = new Connection(selectedInPoint, selectedOutPoint, OnClickRemoveConnection);

            //remove old out connection(out connection has only one)
            if (selectedOutPoint.connectedConnection != null)
            {
                var oldConn = selectedOutPoint.connectedConnection;
                if (Connections.Contains(oldConn)) Connections.Remove(oldConn);
            }

            selectedOutPoint.SetConnection(connection);
            if (!Connections.Contains(connection))
            {
                Connections.Add(connection);
            }
        }

        private void ClearConnectionSelection()
        {
            selectedInPoint = null;
            selectedOutPoint = null;
        }
        #endregion

        public int SetNodeId()
        {
            if (nodeActionList == null) return 0;
            var id = 0;
            foreach (var n in nodeActionList)
            {
                if (id <= n.Id) id = n.Id + 1;
            }
            return id;
        }
    }
}