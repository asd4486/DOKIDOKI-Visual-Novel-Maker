using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class BackgroundItem
{
    public ActionTypes ActionType = ActionTypes.BackgroundItem;
    [NonSerialized]
    public bool Initialize;
    [NonSerialized]
    public Sprite Image;

    public string Path;
}

