using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace DokiVnMaker.Story
{
    public class CG : StoryNodeBase
    {
        // Use this for initialization
        protected override void Init()
        {
            name = "CG";
            base.Init();
        }

        public Sprite image;
        public bool isWait;
        public float duration = 0.5f;
    }
}
