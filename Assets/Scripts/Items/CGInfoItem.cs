using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class CGInfoItem
{
    public ActionTypes ActionType = ActionTypes.CGInfoItem;

    [NonSerialized]
    public bool Initialize;

    //index for story box selector
    [NonSerialized]
    public int Index;
    public string Path;
}

