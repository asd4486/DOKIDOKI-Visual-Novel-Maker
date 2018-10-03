using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class Sound : AudioBase
{
    public ActionTypes ActionType = ActionTypes.Sound;

    public int Track;
}

