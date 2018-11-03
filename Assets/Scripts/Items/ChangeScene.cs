using NodeEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

[Serializable]
public class ChangeScene : NodeBase
{
    [NonSerialized]
    public bool Initialize;

    public string Path;
    [NonSerialized]
    public SceneAsset Scene;

    public ChangeScene() { }

    public ChangeScene(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle,
        GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> onClickInPoint, Action<ConnectionPoint> onClickOutPoint,
        Action<NodeBase> onClickCopyNode, Action<NodeBase> onClickRemoveNode, int id)
    {
        ActionType = ActionTypes.ChangeScene;
        Init(position, width, height, nodeStyle, selectedStyle, inPointStyle, outPointStyle, onClickInPoint, onClickOutPoint, onClickCopyNode, onClickRemoveNode, id);
    }

    public override void Draw()
    {
        InPoint.Draw();
        OutPoint.Draw();

        GUILayout.BeginArea(Rect, Title, Style);
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        GUILayout.Space(SpacePixel);
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical();
        GUILayout.Space(SpacePixel);

        //initialize
        if (Initialize)
        {
            //find origin object
            var origin = AssetDatabase.LoadAssetAtPath(Path, typeof(SceneAsset)) as SceneAsset;

            if (origin != null)
            {
                //set Scene
                Scene = origin;
            }
            Initialize = false;
        }

        //Choose image
        GUILayout.BeginHorizontal();
        GUILayout.Label("Scene", WhiteTxtStyle, GUILayout.Width(LabelWidth));
        Scene = EditorGUILayout.ObjectField(Scene, typeof(SceneAsset), false) as SceneAsset;
        GUILayout.EndHorizontal();

        if (Scene != null)
        {
            //get path
            Path = AssetDatabase.GetAssetPath(Scene);
        }

        GUILayout.EndVertical();
        GUILayout.Space(SpacePixel);
        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        base.Draw();
    }

    public override NodeBase Clone(Vector2 pos, int newId)
    {
        var clone = new ChangeScene(pos, Rect.width, Rect.height, Style, SelectedNodeStyle, InPoint.Style,
            OutPoint.Style, InPoint.OnClickConnectionPoint, OutPoint.OnClickConnectionPoint,
            OnCopyNode, OnRemoveNode, newId)
        {
            Initialize = true,
            Path = Path
        };

        return clone;
    }
}

