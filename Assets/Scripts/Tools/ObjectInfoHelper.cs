using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace DokiVnMaker.Tools
{
    public class ObjectInfoHelper
    {
        public static List<string> GetCharacterNames()
        {
            //find all character
            var list = System.IO.Directory.GetFiles(Application.dataPath + "/GameSources/Characters/", "*.prefab");
            List<string> mynames = new List<string>();
            //get names
            foreach (var o in list)
            {
                string name = o.Split('/')[o.Split('/').Length - 1].Split('.')[0];
                mynames.Add(name);
            }
            return mynames;
        }

        public static List<string> GetCGsName()
        {
            //find all character
            var list = System.IO.Directory.GetFiles(Application.dataPath + "/GameSources/CGs/", "*.jpg");
            List<string> mynames = new List<string>();
            //get names
            foreach (var o in list)
            {
                string name = o.Split('/')[o.Split('/').Length - 1].Split('.')[0];
                mynames.Add(name);
            }
            return mynames;
        }
    }
}