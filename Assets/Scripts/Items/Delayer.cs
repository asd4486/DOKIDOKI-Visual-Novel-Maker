using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class Delayer
{
    public ActionTypes ActionType = ActionTypes.Delayer;

    public float Delay;

    // override object.Equals
    public override bool Equals(object obj)
    {
        var item = obj as Delayer;
        if (obj == null) return false;

        return this.Delay == item.Delay;
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

