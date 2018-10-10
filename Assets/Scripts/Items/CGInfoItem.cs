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

    public bool IsWait = true;

    // override object.Equals
    public override bool Equals(object obj)
    {
        var item = obj as CGInfoItem;
        if (obj == null) return false;

        return this.Path == item.Path && this.IsWait == item.IsWait;
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

