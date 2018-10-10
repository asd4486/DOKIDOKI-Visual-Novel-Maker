using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[Serializable]
public class CharacterOutInfos
{
    [NonSerialized]
    public int Index;
    public string CharaPath;

    public bool IsWait = true;
}
