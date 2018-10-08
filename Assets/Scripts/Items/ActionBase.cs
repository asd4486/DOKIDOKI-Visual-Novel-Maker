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
    BackgroundItem = 3,
    CGInfoItem = 4,
    Delayer = 5,
    Audio = 6,
    Sound = 7,
}

