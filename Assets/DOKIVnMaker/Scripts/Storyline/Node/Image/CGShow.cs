using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace DokiVnMaker.Story
{
    [CreateNodeMenu("Image/Show CG")]
    [NodeTint("#ff99ff")]
    public class CGShow : StoryNodeBase
    {
        // Use this for initialization
        protected override void Init()
        {
            name = "Show CG";
            base.Init();
        }

        public Sprite image;
        public bool isWait;
        public float duration = 0.5f;
    }
}
