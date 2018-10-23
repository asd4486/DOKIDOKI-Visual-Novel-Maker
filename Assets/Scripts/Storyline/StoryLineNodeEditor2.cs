//using NodeEditor;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using UnityEditor;
//using UnityEngine;

//public class StoryLineNodeEditor2 : EditorWindow
//{
//    private static StoryLineNodeEditor2 Window;

//    private EditorStartPoint StartNode;
//    private List<NodeBase> NodeList = new List<NodeBase>();

//    private List<Connection> Connections = new List<Connection>();

//    private GUIStyle NodeStyle;
//    private GUIStyle SelectedNodeStyle;
//    private GUIStyle InPointStyle;
//    private GUIStyle OutPointStyle;

//    private ConnectionPoint SelectedInPoint;
//    private ConnectionPoint SelectedOutPoint;

//    private Vector2 Offset;
//    private Vector2 Drag;

//    private static Texture2D BackgroundColor;

//    //private List<object> ActionList = new List<object>();

//    static StoryLine CurrentStoryLine { get; set; }
//    public static void InitWindow(StoryLine story)
//    {
//        //load saved data
//        //ActionList = StoryLine.OnLoadData();
//        //set curretn editor
//        CurrentStoryLine = story;
//        //open window
//        Init();
//    }

//    //[MenuItem("TEST/Node Based Editor")]
//    static void Init()
//    {
//        Window = (StoryLineNodeEditor2)GetWindow(typeof(StoryLineNodeEditor2), false, "DokiDoki Story line");
//        Window.Show();
//        Debug.Log(Window);

//        BackgroundColor = new Texture2D(1, 1, TextureFormat.RGBA32, false);
//        BackgroundColor.SetPixel(0, 0, new Color(0.2f, 0.2f, 0.2f));
//        BackgroundColor.Apply();
//    }


//    private void OnGUI()
//    {
//        //GUILayout.BeginVertical("box");
//        ////add new action
//        //if (GUILayout.Button("Add new action"))
//        //{
//        //    StoryLineMenu.InitWindow(this);
//        //}
//        //GUILayout.EndVertical();


//        //GUI.DrawTexture(new Rect(0, 0, maxSize.x, maxSize.y), BackgroundColor, ScaleMode.StretchToFill);

//        DrawGrid(20, 0.2f, Color.gray);
//        DrawGrid(100, 0.4f, Color.gray);

//        DrawNodes();
//        DrawConnections();

//        DrawConnectionLine(Event.current);

//        ProcessNodeEvents(Event.current);
//        ProcessEvents(Event.current);

//        if (GUI.changed) Repaint();
//    }

//    #region Node editor
//    private void OnEnable()
//    {
//        EditorPrefs.DeleteAll();
//        //if (EditorPrefs.HasKey(CurrentStoryLine.DataFileName))
//        //{
//        //    string objectPath = EditorPrefs.GetString(CurrentStoryLine.DataFileName);
//        //    NodeList = AssetDatabase.LoadAssetAtPath(objectPath, typeof(NodeBaseList)) as NodeBaseList;
//        //    return;
//        //}

//        //CreateNodeList();
//        InitEditor();
//    }

//    void CreateNodeList()
//    {
//        // There is no overwrite protection here!
//        // There is No "Are you sure you want to overwrite your existing object?" if it exists.
//        // This should probably get a string from the user to create a new name and pass it ...
//        //viewIndex = 1;
//        //NodeList = CreateNodeBaseList.Create(CurrentStoryLine.DataFileName);
//        //if (NodeList)
//        //{
//        //    NodeList = new List<NodeBase>();
//        //    string relPath = AssetDatabase.GetAssetPath(NodeList);
//        //    EditorPrefs.SetString(CurrentStoryLine.DataFileName, relPath);
//        //}
//    }

//    //set node unique id
//    private int SetNodeId()
//    {
//        if (NodeList == null) return 0;
//        var id = 0;
//        foreach (var n in NodeList)
//        {
//            if (id <= n.Id) id = n.Id + 1;
//        }
//        return id;
//    }

//    private void InitEditor()
//    {
//        //draw start node
//        var tex = new Texture2D(12, 12);
//        var fillColorArray = tex.GetPixels();
//        for (var i = 0; i < fillColorArray.Length; ++i)
//        {
//            fillColorArray[i] = new Color(0.2f, 0.8f, 0.2f, 0.7f);
//        }
//        tex.SetPixels(fillColorArray);
//        tex.Apply();

//        var startNodeStyle = new GUIStyle();
//        startNodeStyle.normal.background = tex;

//        var tex2 = new Texture2D(12, 12);
//        var fillColorArray2 = tex2.GetPixels();
//        for (var i = 0; i < fillColorArray2.Length; ++i)
//        {
//            fillColorArray2[i] = new Color(0.2f, 0.8f, 0.2f, 0.9f);
//        }
//        tex2.SetPixels(fillColorArray2);
//        tex2.Apply();

//        var startSelectedNodeStyle = new GUIStyle();
//        startSelectedNodeStyle.normal.background = tex2;

//        var y = 30;

//        StartNode = new EditorStartPoint(new Vector2(20, y), NodeWidth / 2, 30, startNodeStyle, startSelectedNodeStyle, InPointStyle, OutPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode);
//        //add start node to action nodes
//        if (!NodeList.Contains(StartNode)) NodeList.Insert(0, StartNode);
//        Debug.Log(NodeList.Count);
//        //node radius
//        var radius = 10;

//        tex = new Texture2D(12, 12);
//        fillColorArray = tex.GetPixels();
//        for (var i = 0; i < fillColorArray.Length; ++i)
//        {
//            fillColorArray[i] = new Color(0.4f, 0.4f, 0.4f, 0.7f);
//        }
//        tex.SetPixels(fillColorArray);
//        tex.Apply();

//        NodeStyle = new GUIStyle();
//        NodeStyle.normal.background = tex;
//        NodeStyle.normal.textColor = Color.gray;
//        NodeStyle.border = new RectOffset(radius, radius, radius, radius);

//        tex2 = new Texture2D(12, 12);
//        fillColorArray2 = tex2.GetPixels();
//        for (var i = 0; i < fillColorArray2.Length; ++i)
//        {
//            fillColorArray2[i] = new Color(0.4f, 0.4f, 0.4f, 0.9f);
//        }
//        tex2.SetPixels(fillColorArray2);
//        tex2.Apply();

//        SelectedNodeStyle = new GUIStyle();
//        SelectedNodeStyle.normal.background = tex2;
//        SelectedNodeStyle.normal.textColor = Color.yellow;
//        SelectedNodeStyle.border = new RectOffset(radius, radius, radius, radius);

//        InPointStyle = new GUIStyle();
//        InPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
//        InPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
//        InPointStyle.border = new RectOffset(4, 4, radius, radius);

//        OutPointStyle = new GUIStyle();
//        OutPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D;
//        OutPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
//        OutPointStyle.border = new RectOffset(4, 4, radius, radius);

//    }

//    private void OnDisable()
//    {
//        //CurrentStoryLine.OnSaveData(NodeList);
//    }

//    private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
//    {
//        int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
//        int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

//        Handles.BeginGUI();
//        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

//        Offset += Drag * 0.5f;
//        Vector3 newOffset = new Vector3(Offset.x % gridSpacing, Offset.y % gridSpacing, 0);

//        for (int i = 0; i < widthDivs; i++)
//        {
//            Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
//        }

//        for (int j = 0; j < heightDivs; j++)
//        {
//            Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
//        }

//        Handles.color = Color.white;
//        Handles.EndGUI();
//    }

//    private void DrawNodes()
//    {
//        //StartNode.Draw();
//        if (NodeList != null)
//        {
//            for (int i = 0; i < NodeList.Count; i++)
//            {
//                NodeList[i].Draw();
//            }
//        }
//    }

//    private void DrawConnections()
//    {
//        if (Connections != null)
//        {
//            for (int i = 0; i < Connections.Count; i++)
//            {
//                Connections[i].Draw();
//            }
//        }
//    }

//    private void ProcessEvents(Event e)
//    {
//        Drag = Vector2.zero;
//        switch (e.type)
//        {
//            case EventType.MouseDown:
//                if(e.button == 0)
//                {
//                    if (SelectedInPoint != null || SelectedOutPoint != null)
//                    {
//                        ClearConnectionSelection();
//                    }
//                }
//                if (e.button == 1)
//                {
//                     ProcessContextMenu(e.mousePosition);                    
//                }
//                break;
//            case EventType.MouseDrag:
//                if (e.button == 0)
//                {
//                    OnDrag(e.delta);
//                }
//                break;
//        }
//    }

//    private void OnDrag(Vector2 delta)
//    {
//        Drag = delta;

//        if (NodeList != null)
//        {
//            for (int i = 0; i < NodeList.Count; i++)
//            {
//                NodeList[i].Drag(delta);
//            }
//        }

//        GUI.changed = true;
//    }

//    private void ProcessNodeEvents(Event e)
//    {
//        if (NodeList != null)
//        {
//            for (int i = NodeList.Count - 1; i >= 0; i--)
//            {
//                bool guiChanged = NodeList[i].ProcessEvents(e);

//                if (guiChanged)
//                {
//                    GUI.changed = true;
//                }
//            }
//        }
//    }

//    private void ProcessContextMenu(Vector2 mousePosition)
//    {
//        GenericMenu genericMenu = new GenericMenu();
//        genericMenu.AddItem(new GUIContent("Character/Character sprite"), false, () => AddNewAction(ActionTypes.CharcterSpriteInfos, mousePosition));
//        genericMenu.AddItem(new GUIContent("Character/Character out"), false, () => AddNewAction(ActionTypes.CharcterSpriteInfos, mousePosition));

//        genericMenu.AddItem(new GUIContent("Dialog/Dialog box"), false, () => AddNewAction(ActionTypes.DialogBox, mousePosition));
//        genericMenu.AddItem(new GUIContent("Dialog/Brahche"), false, () => AddNewAction(ActionTypes.BrancheBox, mousePosition));

//        genericMenu.AddItem(new GUIContent("Image/Background"), false, () => AddNewAction(ActionTypes.BackgroundItem, mousePosition));
//        genericMenu.AddItem(new GUIContent("Image/CG"), false, () => AddNewAction(ActionTypes.CGInfoItem, mousePosition));

//        genericMenu.AddItem(new GUIContent("Audio/Background music"), false, () => AddNewAction(ActionTypes.Audio, mousePosition));
//        genericMenu.AddItem(new GUIContent("Audio/Sound"), false, () => AddNewAction(ActionTypes.Sound, mousePosition));

//        genericMenu.AddItem(new GUIContent("Time/Delayer"), false, () => AddNewAction(ActionTypes.Delayer, mousePosition));

//        genericMenu.AddItem(new GUIContent("Story/Play storyline"), false, () => AddNewAction(ActionTypes.Delayer, mousePosition));
//        genericMenu.AddItem(new GUIContent("Story/Change scene"), false, () => AddNewAction(ActionTypes.CharcterSpriteInfos, mousePosition));

//        genericMenu.ShowAsContext();
//    }

//    //private void OnClickAddNode(Vector2 mousePosition)
//    //{
//    //    if (NodeList == null)
//    //    {
//    //        NodeList = new List<NodeBase>();
//    //    }

//    //    NodeList.Add(new NodeBase(mousePosition, 200, 100, NodeStyle, SelectedNodeStyle, InPointStyle, OutPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode));
//    //}

//    private void OnClickInPoint(ConnectionPoint inPoint)
//    {
//        SelectedInPoint = inPoint;

//        if (SelectedOutPoint != null)
//        {
//            if (SelectedOutPoint.Node != SelectedInPoint.Node)
//            {
//                CreateConnection();
//                ClearConnectionSelection();
//            }
//            else
//            {
//                ClearConnectionSelection();
//            }
//        }
//        //else ClearConnectionSelection();
//    }

//    private void OnClickOutPoint(ConnectionPoint outPoint)
//    {
//        SelectedOutPoint = outPoint;

//        if (SelectedInPoint != null)
//        {
//            if (SelectedOutPoint.Node != SelectedInPoint.Node)
//            {
//                CreateConnection();
//                ClearConnectionSelection();
//            }
//            else
//            {
//                ClearConnectionSelection();
//            }
//        }
//        //else ClearConnectionSelection();
//    }

//    private void OnClickRemoveNode(NodeBase node)
//    {
//        //if (Connections != null)
//        //{
//        //    List<Connection> connectionsToRemove = new List<Connection>();

//        //    for (int i = 0; i < Connections.Count; i++)
//        //    {
//        //        if (Connections[i].inPoint == node.InPoint || Connections[i].outPoint == node.OutPoint)
//        //        {
//        //            connectionsToRemove.Add(Connections[i]);
//        //        }
//        //    }

//        //    for (int i = 0; i < connectionsToRemove.Count; i++)
//        //    {
//        //        Connections.Remove(connectionsToRemove[i]);
//        //    }

//        //    connectionsToRemove = null;
//        //}

//        var removeNode = NodeList.Where(n => n.Equals(node)).FirstOrDefault();
//        if(removeNode != null) NodeList.Remove(removeNode);
//    }

//    private void OnClickRemoveConnection(Connection conn)
//    {
//        if (Connections.Contains(conn))
//        {
//            conn.OutPoint.Node.OutConnection = null;
//            Connections.Remove(conn);
//        }
//    }

//    private void CreateConnection()
//    {
//        //return if in out point are null
//        if (SelectedInPoint == null && SelectedOutPoint == null) return;

//        var connection = new Connection(SelectedInPoint, SelectedOutPoint, OnClickRemoveConnection);

//        //remove old out connection(out connection has only one)
//        if (SelectedOutPoint.Node.OutConnection != null)
//        {
//            var oldConn = SelectedOutPoint.Node.OutConnection;
//            if (Connections.Contains(oldConn)) Connections.Remove(oldConn);
//        }

//        SelectedOutPoint.Node.OutConnection = connection;
//        if (!Connections.Contains(connection))
//        {
//            //all connections
//            Connections.Add(connection);
//            SelectedInPoint.Node.InConnections.Add(connection);
//        }

//        //if (Connections == null)
//        //{
//        //    Connections = new List<Connection>();
//        //}

//        //Connections.Add(new Connection(SelectedInPoint, SelectedOutPoint, OnClickRemoveConnection));
//    }

//    private void DrawConnectionLine(Event e)
//    {
//        if (SelectedInPoint != null && SelectedOutPoint == null)
//        {
//            Handles.DrawBezier(
//                SelectedInPoint.Rect.center,
//                e.mousePosition,
//                SelectedInPoint.Rect.center + Vector2.left * 50f,
//                e.mousePosition - Vector2.left * 50f,
//                Color.white,
//                null,
//                2f
//            );

//            GUI.changed = true;
//        }

//        if (SelectedOutPoint != null && SelectedInPoint == null)
//        {
//            Handles.DrawBezier(
//                SelectedOutPoint.Rect.center,
//                e.mousePosition,
//                SelectedOutPoint.Rect.center - Vector2.left * 50f,
//                e.mousePosition + Vector2.left * 50f,
//                Color.white,
//                null,
//                2f
//            );

//            GUI.changed = true;
//        }
//    }

//    private void ClearConnectionSelection()
//    {
//        SelectedInPoint = null;
//        SelectedOutPoint = null;
//    }
//    #endregion

//    private float NodeWidth = 250;
//    //add new action to story lines
//    public void AddNewAction(ActionTypes type, Vector2 mousePosition)
//    {
//        var id = NodeList.Count;
//        switch (type)
//        {
//            case ActionTypes.CharcterSpriteInfos:
//                NodeList.Add(new CharcterSpriteInfos(mousePosition, NodeWidth, 90, NodeStyle, SelectedNodeStyle, InPointStyle, OutPointStyle,
//                                                        OnClickInPoint, OnClickOutPoint, OnClickRemoveNode, SetNodeId()));
//                break;
//            case ActionTypes.DialogBox:
//                NodeList.Add(new DialogBox(mousePosition, NodeWidth, 180, NodeStyle, SelectedNodeStyle, InPointStyle, OutPointStyle, 
//                                                OnClickInPoint, OnClickOutPoint, OnClickRemoveNode, SetNodeId()));
//                break;
//            case ActionTypes.BrancheBox:
//                NodeList.Add(new BrancheBox(mousePosition, NodeWidth, 180, NodeStyle, SelectedNodeStyle, InPointStyle, 
//                                                OutPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode, SetNodeId()));
//                break;
//            case ActionTypes.BackgroundItem:
//                NodeList.Add(new BackgroundItem(mousePosition, NodeWidth, 150, NodeStyle, SelectedNodeStyle, InPointStyle, 
//                                                OutPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode, SetNodeId()));
//                break;
//            case ActionTypes.CGInfoItem:
//                NodeList.Add(new CGInfoItem(mousePosition, NodeWidth, 170, NodeStyle, SelectedNodeStyle, InPointStyle, 
//                                                OutPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode, SetNodeId()));
//                break;
//            case ActionTypes.Delayer:
//                NodeList.Add(new Delayer(mousePosition, NodeWidth, 40, NodeStyle, SelectedNodeStyle, InPointStyle, OutPointStyle, 
//                                                OnClickInPoint, OnClickOutPoint, OnClickRemoveNode, SetNodeId()));
//                break;
//            case ActionTypes.Audio:
//                NodeList.Add(new Audio(mousePosition, NodeWidth, 60, NodeStyle, SelectedNodeStyle, InPointStyle, OutPointStyle,
//                                                OnClickInPoint, OnClickOutPoint, OnClickRemoveNode, SetNodeId()));
//                break;
//            case ActionTypes.Sound:
//                NodeList.Add(new Sound(mousePosition, NodeWidth, 70, NodeStyle, SelectedNodeStyle, InPointStyle, OutPointStyle,
//                                            OnClickInPoint, OnClickOutPoint, OnClickRemoveNode, SetNodeId()));
//                break;
//        }
//    }
//}
