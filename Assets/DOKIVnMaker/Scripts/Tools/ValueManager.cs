using UnityEngine;

namespace DokiVnMaker
{
    public static class ValueManager
    {
        public static int DefaultFontSize = 20;
        public static string GameSourcesFullPath = Application.dataPath + "/DOKIVnMaker/GameSources/";
        public static string GameSourcesPath = "Assets/DOKIVnMaker/GameSources/";

        public static string CGPath = GameSourcesPath + "CGs/";

        public static string CharaPath = GameSourcesPath + "Characters/";
        public static string CharaFullPath = GameSourcesFullPath + "Characters/";
    }
}
