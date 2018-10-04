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
    [NonSerialized]
    public int Index;

    public int SpriteIndex;
    public int FaceIndex;

    public CharacterPosition Position;
}

//position of character
public enum CharacterPosition
{
    Left,
    Center,
    Right,
    Custom
}

