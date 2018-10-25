using NodeEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

[Serializable]
public class CharcterSpriteInfos : NodeBase
{
    [NonSerialized]
    public bool Initialize;

    public string Path;
    //get chara name
    public string Name { get { return Path.Split('/')[Path.Split('/').Length - 1].Replace(".prefab", ""); } }

    [NonSerialized]
    public int Index;

    public int SpriteIndex;
    public int FaceIndex;

    //character position
    public CharacterPosition CharaPos;
    public Vector2 CustomPos;
    public bool IsWait = true;

    public CharcterSpriteInfos() { }

    public CharcterSpriteInfos(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle,
        GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> onClickInPoint, Action<ConnectionPoint> onClickOutPoint,
        Action<NodeBase> onClickCopyNode, Action<NodeBase> onClickRemoveNode, int id)
    {
        ActionType = ActionTypes.CharcterSpriteInfos;
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
            //get all sprites name
            var spriteList = selected.GetComponentsInChildren<Transform>().Where(s => s.GetComponent<CharaSpriteSetting>() != null).Select(s => s.name).ToArray();

            //character sprite
            GUILayout.BeginHorizontal();
            GUILayout.Label("Sprite", WhiteTxtStyle, GUILayout.Width(LabelWidth));
            SpriteIndex = EditorGUILayout.Popup(SpriteIndex, spriteList);
            GUILayout.EndHorizontal();

            //select character face if existe
            var faceList = selected.transform.GetChild(SpriteIndex).GetComponentsInChildren<Transform>().
                            Where(f => f.GetComponent<CharaFaceSetting>() != null).
                            Select(f => f.name).ToArray();

            if (faceList.Length > 0)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label("Face", WhiteTxtStyle, GUILayout.Width(LabelWidth));
                FaceIndex = EditorGUILayout.Popup(FaceIndex, faceList);
                GUILayout.EndHorizontal();
            }
        }

        //character postion
        var charaPosList = Enum.GetValues(typeof(CharacterPosition))
            .Cast<int>()
            .Select(x =>  Enum.GetName(typeof(CharacterPosition), x))
            .ToArray();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Position", WhiteTxtStyle, GUILayout.Width(LabelWidth));
        CharaPos = (CharacterPosition)EditorGUILayout.Popup((int)CharaPos, charaPosList);
        GUILayout.EndHorizontal();

        //custom position
        if (CharaPos == CharacterPosition.Custom)
        {
            if(Rect.height == DefaultRectHeight) Rect.height = DefaultRectHeight+ 20;
            CustomPos = EditorGUILayout.Vector2Field( "", CustomPos);
        }
        else { if (Rect.height != DefaultRectHeight) Rect.height = DefaultRectHeight; }

        //is wait for character appear
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
            SpriteIndex = SpriteIndex,
            FaceIndex = FaceIndex,
            CharaPos = CharaPos,
            IsWait = IsWait
        };

        return clone;
    }

    // override object.Equals
    //public override bool Equals(object obj)
    //{
    //    var item = obj as CharcterSpriteInfos;
    //    if (item == null) return false;

    //    return Path == item.Path && SpriteIndex == item.SpriteIndex
    //        && FaceIndex == item.FaceIndex && Position == item.Position && IsWait == item.IsWait
    //        && Position == item.Position && Id == item.Id;
    //}

    //// override object.GetHashCode
    //public override int GetHashCode()
    //{
    //    // TODO: write your implementation of GetHashCode() here
    //    //throw new NotImplementedException();
    //    return base.GetHashCode();
    //}
}

//position of character
public enum CharacterPosition
{
    Left,
    Center,
    Right,
    Custom
}

