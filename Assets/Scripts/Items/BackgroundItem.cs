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
    public bool IsWait;

    // override object.Equals
    public override bool Equals(object obj)
    {
        var item = obj as BackgroundItem;
        if (obj == null) return false;

        return this.Path == item.Path && this.IsWait == item.IsWait;
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

