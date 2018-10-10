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

    // override object.Equals
    public override bool Equals(object obj)
    {
        var item = obj as BrancheBox;
        if (obj == null) return false;

        return this.Dialogue == item.Dialogue && this.Branches.SequenceEqual(item.Branches) && this.Color == item.Color && this.FontSize == item.FontSize;
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}

