using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class Sound : AudioBase
{
    public ActionTypes ActionType = ActionTypes.Sound;

    public int TrackIndex;

    // override object.Equals
    public override bool Equals(object obj)
    {
        var item = obj as Sound;
        if (obj == null) return false;

        return this.MyAudio == item.MyAudio && this.Volume == item.Volume && this.TrackIndex == item.TrackIndex;
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

