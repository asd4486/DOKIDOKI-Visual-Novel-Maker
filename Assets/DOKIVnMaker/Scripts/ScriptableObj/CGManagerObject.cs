using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace DokiVnMaker
{
    [CreateAssetMenu(order = 0, fileName = "NewCGManager", menuName = "Doki VN Maker/CG Manager")]
    public class CGManagerObject : ScriptableObject
    {
        public List<CG> CGList = new List<CG>();

        [System.Serializable]
        public class CG
        {
            public string name;
            public Sprite image;
            public int index;
        }

        public void AddCG()
        {
            CGList.Add(new CG() { index = CGList.Count });
        }

        public void RemoveCG(CG cg)
        {
            CGList.Remove(cg);
            for (int i = 0; i < CGList.Count; i++)
                CGList[i].index = i;
        }

        public void RemoveCG()
        {
            if (CGList.Count < 0) return;
            CGList.RemoveAt(CGList.Count - 1);
        }

        public void MoveUp(CG cg)
        {
            var index = cg.index;
            if (index < 1) return;

            CGList.Remove(cg);
            CGList.Insert(index - 1, cg);
            CGList[index - 1].index = index - 1;
            CGList[index].index = index;
        }

        public void MoveDown(CG cg)
        {
            var index = cg.index;
            if (index >= CGList.Count - 1) return;

            CGList.Remove(cg);
            CGList.Insert(index + 1, cg);
            CGList[index].index = index;
            CGList[index + 1].index = index + 1;
        }
    }
}