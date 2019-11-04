using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace DokiVnMaker.Story
{
    [NodeTint("#ff99ff")]
    public class CGFadeOut : StoryNodeBase
    {
        // Use this for initialization
        protected override void Init()
        {
            name = "CG out";
            base.Init();
        }

        public bool isWait;
        public float duration = 0.8f;
    }
}
