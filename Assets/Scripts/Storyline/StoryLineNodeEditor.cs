using NodeEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class StoryLineNodeEditor : EditorWindow
{
    private static StoryLineNodeEditor Window;

    private StartPoint StartNode;
    private List<NodeBase> ActionNodes = new List<NodeBase>();
    private List<Connection> Connections;

    private GUIStyle NodeStyle;
    private GUIStyle SelectedNodeStyle;
    private GUIStyle InPointStyle;
    private GUIStyle OutPointStyle;

    private ConnectionPoint SelectedInPoint;
    private ConnectionPoint SelectedOutPoint;

    private Vector2 Offset;
    private Vector2 Drag;

    private static Texture2D BackgroundColor;

    //private List<object> ActionList = new List<object>();

    static StoryLine CurrentStoryLine { get; set; }
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
    private static void Init()
    {
        Window = (StoryLineNodeEditor)GetWindow(typeof(StoryLineNodeEditor), false, "DokiDoki Story line");
        Window.Show();

        BackgroundColor = new Texture2D(1, 1, TextureFormat.RGBA32, false);
        BackgroundColor.SetPixel(0, 0, new Color(0.2f, 0.2f, 0.2f));
        BackgroundColor.Apply();

    }


    private void OnGUI()
    {
        //GUILayout.BeginVertical("box");
        ////add new action
        //if (GUILayout.Button("Add new action"))
        //{
        //    StoryLineMenu.InitWindow(this);
        //}
        //GUILayout.EndVertical();


        GUI.DrawTexture(new Rect(0, 0, maxSize.x, maxSize.y), BackgroundColor, ScaleMode.StretchToFill);

        DrawGrid(20, 0.2f, Color.gray);
        DrawGrid(100, 0.4f, Color.gray);

        DrawNodes();
        DrawConnections();

        DrawConnectionLine(Event.current);

        ProcessNodeEvents(Event.current);
        ProcessEvents(Event.current);

        if (GUI.changed) Repaint();
    }

    #region Node editor
    private void OnEnable()
    {
        //node radius
        var radius = 10;

        var tex = new Texture2D(12, 12);
        var fillColorArray = tex.GetPixels();
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
        var fillColorArray2 = tex2.GetPixels();
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

        InPointStyle = new GUIStyle();
        InPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
        InPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
        InPointStyle.border = new RectOffset(4, 4, radius, radius);

        OutPointStyle = new GUIStyle();
        OutPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D;
        OutPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
        OutPointStyle.border = new RectOffset(4, 4, radius, radius);

    }

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
        //draw start node
        if(StartNode == null)
        {
            var tex = new Texture2D(12, 12);
            var fillColorArray = tex.GetPixels();
            for (var i = 0; i < fillColorArray.Length; ++i)
            {
                fillColorArray[i] = new Color(0.2f, 0.8f, 0.2f, 0.7f);
            }
            tex.SetPixels(fillColorArray);
            tex.Apply();

            var startNodeStyle = new GUIStyle();
            startNodeStyle.normal.background = tex;

            var tex2 = new Texture2D(12, 12);
            var fillColorArray2 = tex2.GetPixels();
            for (var i = 0; i < fillColorArray2.Length; ++i)
            {
                fillColorArray2[i] = new Color(0.2f, 0.8f, 0.2f, 0.9f);
            }
            tex2.SetPixels(fillColorArray2);
            tex2.Apply();

            var startSelectedNodeStyle = new GUIStyle();
            startSelectedNodeStyle.normal.background = tex2;

            var y = Window.position.height / 5;
            StartNode = new StartPoint(new Vector2(20, y), NodeWidth/2, 30, startNodeStyle, startSelectedNodeStyle, InPointStyle, OutPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode);
            //add start node to action nodes
            if (!ActionNodes.Contains(StartNode)) ActionNodes.Insert(0, StartNode);
        }
        StartNode.Draw();

        if (ActionNodes != null)
        {
            for (int i = 0; i < ActionNodes.Count; i++)
            {
                ActionNodes[i].Draw();
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

    private void ProcessEvents(Event e)
    {
        Drag = Vector2.zero;
        switch (e.type)
        {
            case EventType.MouseDown:
                if(e.button == 0)
                {
                    if (SelectedInPoint != null || SelectedOutPoint != null)
                    {
                        ClearConnectionSelection();
                    }
                }
                if (e.button == 1)
                {
                     ProcessContextMenu(e.mousePosition);                    
                }
                break;
            case EventType.MouseDrag:
                if (e.button == 0)
                {
                    OnDrag(e.delta);
                }
                break;
        }
    }

    private void OnDrag(Vector2 delta)
    {
        Drag = delta;

        if (ActionNodes != null)
        {
            for (int i = 0; i < ActionNodes.Count; i++)
            {
                ActionNodes[i].Drag(delta);
            }
        }

        GUI.changed = true;
    }

    private void ProcessNodeEvents(Event e)
    {
        if (ActionNodes != null)
        {
            for (int i = ActionNodes.Count - 1; i >= 0; i--)
            {
                bool guiChanged = ActionNodes[i].ProcessEvents(e);

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
        genericMenu.AddItem(new GUIContent("Character/Character sprite"), false, () => AddNewAction(ActionTypes.CharcterSpriteInfos, mousePosition));
        genericMenu.AddItem(new GUIContent("Character/Character out"), false, () => AddNewAction(ActionTypes.CharcterSpriteInfos, mousePosition));

        genericMenu.AddItem(new GUIContent("Dialog/Dialog box"), false, () => AddNewAction(ActionTypes.DialogBox, mousePosition));
        genericMenu.AddItem(new GUIContent("Dialog/Brahche"), false, () => AddNewAction(ActionTypes.BrancheBox, mousePosition));

        genericMenu.AddItem(new GUIContent("Image/Background"), false, () => AddNewAction(ActionTypes.BackgroundItem, mousePosition));
        genericMenu.AddItem(new GUIContent("Image/CG"), false, () => AddNewAction(ActionTypes.CGInfoItem, mousePosition));

        genericMenu.AddItem(new GUIContent("Audio/Background music"), false, () => AddNewAction(ActionTypes.Audio, mousePosition));
        genericMenu.AddItem(new GUIContent("Audio/Sound"), false, () => AddNewAction(ActionTypes.Sound, mousePosition));

        genericMenu.AddItem(new GUIContent("Time/Delayer"), false, () => AddNewAction(ActionTypes.Delayer, mousePosition));

        genericMenu.AddItem(new GUIContent("Story/Play storyline"), false, () => AddNewAction(ActionTypes.Delayer, mousePosition));
        genericMenu.AddItem(new GUIContent("Story/Change scene"), false, () => AddNewAction(ActionTypes.CharcterSpriteInfos, mousePosition));

        genericMenu.ShowAsContext();
    }

    //private void OnClickAddNode(Vector2 mousePosition)
    //{
    //    if (ActionNodes == null)
    //    {
    //        ActionNodes = new List<NodeBase>();
    //    }

    //    ActionNodes.Add(new NodeBase(mousePosition, 200, 100, NodeStyle, SelectedNodeStyle, InPointStyle, OutPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode));
    //}

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
        //else ClearConnectionSelection();
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
        //else ClearConnectionSelection();
    }

    private void OnClickRemoveNode(NodeBase node)
    {
        if (Connections != null)
        {
            List<Connection> connectionsToRemove = new List<Connection>();

            for (int i = 0; i < Connections.Count; i++)
            {
                if (Connections[i].inPoint == node.InPoint || Connections[i].outPoint == node.OutPoint)
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

        var removeNode = ActionNodes.Where(n => n.Equals(node)).FirstOrDefault();
        if(removeNode != null) ActionNodes.Remove(removeNode);
    }

    private void OnClickRemoveConnection(Connection connection)
    {
        Connections.Remove(connection);
    }

    private void CreateConnection()
    {
        if (Connections == null)
        {
            Connections = new List<Connection>();
        }

        Connections.Add(new Connection(SelectedInPoint, SelectedOutPoint, OnClickRemoveConnection));
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

    private void ClearConnectionSelection()
    {
        SelectedInPoint = null;
        SelectedOutPoint = null;
    }
    #endregion

    private float NodeWidth = 250;
    //add new action to story lines
    public void AddNewAction(ActionTypes type, Vector2 mousePosition)
    {
        var id = ActionNodes.Count;
        switch (type)
        {
            case ActionTypes.CharcterSpriteInfos:
                ActionNodes.Add(new CharcterSpriteInfos(mousePosition, NodeWidth, 90, NodeStyle, SelectedNodeStyle, InPointStyle, OutPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode));
                break;
            case ActionTypes.DialogBox:
                ActionNodes.Add(new DialogBox(mousePosition, NodeWidth, 180, NodeStyle, SelectedNodeStyle, InPointStyle, OutPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode));
                break;
            case ActionTypes.BrancheBox:
                ActionNodes.Add(new BrancheBox(mousePosition, NodeWidth, 180, NodeStyle, SelectedNodeStyle, InPointStyle, OutPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode));
                break;
            case ActionTypes.BackgroundItem:
                ActionNodes.Add(new BackgroundItem(mousePosition, NodeWidth, 150, NodeStyle, SelectedNodeStyle, InPointStyle, OutPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode));
                break;
            case ActionTypes.CGInfoItem:
                ActionNodes.Add(new CGInfoItem(mousePosition, NodeWidth, 170, NodeStyle, SelectedNodeStyle, InPointStyle, OutPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode));
                break;
            case ActionTypes.Delayer:
                ActionNodes.Add(new Delayer(mousePosition, NodeWidth, 40, NodeStyle, SelectedNodeStyle, InPointStyle, OutPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode));
                break;
            case ActionTypes.Audio:
                ActionNodes.Add(new Audio(mousePosition, NodeWidth, 60, NodeStyle, SelectedNodeStyle, InPointStyle, OutPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode));
                break;
            case ActionTypes.Sound:
                ActionNodes.Add(new Sound(mousePosition, NodeWidth, 70, NodeStyle, SelectedNodeStyle, InPointStyle, OutPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode));
                break;
        }
    }
}
