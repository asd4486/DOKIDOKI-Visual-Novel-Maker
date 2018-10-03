using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class Delayer
{
    public ActionTypes ActionType = ActionTypes.Delayer;

    public float Delay;
}

