using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace DokiVnMaker.Story
{
    [CreateNodeMenu("Image/CG fade in")]
    [NodeTint("#ff99ff")]
    public class CG : StoryNodeBase
    {
        // Use this for initialization
        protected override void Init()
        {
            name = "CG in";
            base.Init();
        }

        public Sprite image;
        public bool isWait;
        public float duration = 0.5f;
    }
}
