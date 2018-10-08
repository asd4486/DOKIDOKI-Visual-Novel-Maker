using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class DialogBox
{
    public ActionTypes ActionType = ActionTypes.DialogBox;

    public string Name;
    public string Dialogue;

    [NonSerialized]
    public bool ShowCharParam;
    public Color Color;
    public int FontSize = ValueManager.DefaultFontSize;
    public int Speed = 3;
}

