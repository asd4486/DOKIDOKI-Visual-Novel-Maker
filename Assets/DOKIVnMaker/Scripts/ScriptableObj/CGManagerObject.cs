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
            int index;
            public int Index { get { return index; } }

            public void SetIndex(int i)
            {
                index = i;
            }
        }

        public void SetupCgIndex()
        {
            for (int i = 0; i < CGList.Count; i++)
                CGList[i].SetIndex(i);
        }
    }
}