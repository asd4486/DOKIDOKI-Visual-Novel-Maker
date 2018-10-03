using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class BrancheBox
{
    public ActionTypes ActionType = ActionTypes.BrancheBox;

    public string Dialogue;

    public List<string> Branches;

    public Color Color;
    public int FontSize = ValueManager.DefaultFontSize;
}

