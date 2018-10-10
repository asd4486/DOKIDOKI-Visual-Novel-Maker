using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class DialogBox
{
    public ActionTypes ActionType = ActionTypes.DialogBox;

    public string CharaName;
    public string Dialog;

    [NonSerialized]
    public bool ShowCharParam;
    public Color Color;
    public int FontSize = ValueManager.DefaultFontSize;
    public int Speed = 3;

    public bool NoWait;

    // override object.Equals
    public override bool Equals(object obj)
    {
        var item = obj as DialogBox;
        if (item == null) return false;

        return this.CharaName == item.CharaName && this.Dialog == item.Dialog
            && this.Color == item.Color && this.FontSize == item.FontSize && this.Speed == item.Speed
            && this.NoWait == item.NoWait;
    }

    // override object.GetHashCode
    public override int GetHashCode()
    {
        // TODO: write your implementation of GetHashCode() here
        //throw new NotImplementedException();
        return base.GetHashCode();
    }
}

