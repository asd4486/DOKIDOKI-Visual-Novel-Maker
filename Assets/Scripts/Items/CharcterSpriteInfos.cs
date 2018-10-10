using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class CharcterSpriteInfos
{
    public ActionTypes ActionType = ActionTypes.CharcterSpriteInfos;
    [NonSerialized]
    public bool Initialize;

    public string Path;
    //get chara name
    public string Name { get { return Path.Split('/')[Path.Split('/').Length - 1].Replace(".prefab", ""); } }

    [NonSerialized]
    public int Index;

    public int SpriteIndex;
    public int FaceIndex;

    public CharacterPosition Position;
    public bool IsWait = true;

    // override object.Equals
    public override bool Equals(object obj)
    {
        var item = obj as CharcterSpriteInfos;
        if (item == null)return false;
            
        return this.Path == item.Path && this.SpriteIndex == item.SpriteIndex 
            && this.FaceIndex == item.FaceIndex && this.Position == item.Position && this.IsWait == item.IsWait;
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        // TODO: write your implementation of GetHashCode() here
        //throw new NotImplementedException();
        return base.GetHashCode();
    }
}

//position of character
public enum CharacterPosition
{
    Left,
    Center,
    Right,
    Custom
}

