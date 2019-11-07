using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace DokiVnMaker.Story
{
    [CreateNodeMenu("Image/Hide CG")]
    [NodeWidth(200)]
    [NodeTint("#ff99ff")]
    public class CGHide : StoryNodeBase
    {
        // Use this for initialization
        protected override void Init()
        {
            name = "Hide CG";
            base.Init();
        }

        public bool isWait;
        public float duration = 0.8f;
    }
}
