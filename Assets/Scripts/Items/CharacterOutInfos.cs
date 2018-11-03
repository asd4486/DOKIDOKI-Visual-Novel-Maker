using NodeEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

[Serializable]
public class CharacterOutInfos : NodeBase
{
    [NonSerialized]
    public bool Initialize;
    [NonSerialized]
    public int Index;
    public string Path;

    public bool IsWait = true;

    public CharacterOutInfos() { }

    public CharacterOutInfos(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle,
        GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> onClickInPoint, Action<ConnectionPoint> onClickOutPoint,
        Action<NodeBase> onClickCopyNode, Action<NodeBase> onClickRemoveNode, int id)
    {
        ActionType = ActionTypes.CharacterOutInfos;
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

        //get all character
        var list = ObjectInfoHelper.GetCharacterNames();

        //set character index if initialize request
        if (Initialize)
        {
            //find origin object
            var origin = AssetDatabase.LoadAssetAtPath(Path, typeof(GameObject)) as GameObject;

            if (origin != null)
            {
                //set index
                Index = list.IndexOf(list.Where(c => c == origin.name).FirstOrDefault());
            }
            Initialize = false;
        }

        //choose character
        GUILayout.BeginHorizontal();
        GUILayout.Label("Character", WhiteTxtStyle, GUILayout.Width(LabelWidth));
        Index = EditorGUILayout.Popup(Index, list.ToArray());
        GUILayout.EndHorizontal();

        //find selected character
        string path = ValueManager.CharaPath + list[Index] + ".prefab";
        var selected = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
        if (selected != null)
        {
            //set character path
            Path = path;
        }

        //is wait for character disappear
        GUILayout.BeginHorizontal();
        GUILayout.Label("Is wait", WhiteTxtStyle, GUILayout.Width(LabelWidth));
        IsWait = EditorGUILayout.Toggle(IsWait);
        GUILayout.EndHorizontal();

        GUILayout.EndVertical();
        GUILayout.Space(SpacePixel);
        GUILayout.EndHorizontal();
        GUILayout.EndArea();

        base.Draw();
    }

    public override NodeBase Clone(Vector2 pos, int newId)
    {
        var clone = new CharcterSpriteInfos(pos, Rect.width, Rect.height, Style, SelectedNodeStyle, InPoint.Style,
            OutPoint.Style, InPoint.OnClickConnectionPoint, OutPoint.OnClickConnectionPoint,
            OnCopyNode, OnRemoveNode, newId)
        {
            ActionType = ActionTypes.CharcterSpriteInfos,
            Initialize = true,
            Path = Path,
            IsWait = IsWait
        };

        return clone;
    }
}
