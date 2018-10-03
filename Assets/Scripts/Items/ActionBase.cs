using System;

[Serializable]
public class ActionBase
{
    public ActionTypes ActionType;
}

public enum ActionTypes
{
    CharcterSpriteInfos = 0,
    DialogBox = 1,
    BrancheBox = 2,
    CGInfoItem = 3,
    Delayer = 4,
    Audio = 5,
    Sound = 6,
}

