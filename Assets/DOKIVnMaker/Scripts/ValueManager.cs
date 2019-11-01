using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class ValueManager
{
    public static int DefaultFontSize = 20;
    public static string GameSourcesFullPath = Application.dataPath + "/GameSources/";
    public static string GameSourcesPath = "Assets/GameSources/";

    public static string CGPath = GameSourcesPath + "CGs/";
    public static string CGFullPath = GameSourcesFullPath + "CGs/";

    public static string CharaPath = GameSourcesPath + "Characters/";
    public static string CharaFullPath = GameSourcesFullPath + "Characters/";

    public static string GameDataPath = "Assets/GameData/";
    public static string GameDataFullPath = Application.dataPath + "/GameData/";
}

