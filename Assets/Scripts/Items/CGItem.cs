using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CGItem
{
    public string OldName { get; set; }
    public string Name { get; set; }
    public Sprite CG { get; set; }

    public bool IsOut { get; set; }
    public List<string> Animations { get; set; }

    //index for story box selector
    public int Index { get; set; }
}

