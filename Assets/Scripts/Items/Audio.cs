using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class Audio:AudioBase
{
    //set action type
    public ActionTypes ActionType = ActionTypes.Audio;

    // override object.Equals
    public override bool Equals(object obj)
    {
        var item = obj as Audio;
        if (obj == null) return false;

        return this.MyAudio == item.MyAudio && this.Volume == item.Volume;
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

