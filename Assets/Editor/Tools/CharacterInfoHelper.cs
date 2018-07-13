using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Editor.Tools
{
    public class CharacterInfoHelper
    {
        public static string[] GetCharacterNames()
        {
            var list = System.IO.Directory.GetFiles(Application.dataPath + "/GameSources/Characters/", "*.prefab");
            List<string> mynames = new List<string>();
            foreach (var o in list)
            {
                string name = o.Split('/')[o.Split('/').Length - 1].Split('.')[0];
                mynames.Add(name);
            }
            return mynames.ToArray();
        }
    }
}
