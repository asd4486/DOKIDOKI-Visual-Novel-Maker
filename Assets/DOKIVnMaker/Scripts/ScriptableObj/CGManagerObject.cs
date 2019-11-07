using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace DokiVnMaker
{
    [CreateAssetMenu(order = 0, fileName = "NewCGManager", menuName = "Doki VN Maker/CG Manager")]
    public class CGManagerObject : ScriptableObject
    {
        public List<CG> CGList = new List<CG>();

        public class CG
        {
            public string name;
            public Sprite image;
        }

        public void AddCG()
        {
            CGList.Add(new CG());
        }

        public void RemoveCG(CG cg)
        {
            CGList.Remove(cg);
        }

        public void RemoveCG()
        {
            if (CGList.Count < 0) return;
            CGList.RemoveAt(CGList.Count - 1);
        }
    }
}