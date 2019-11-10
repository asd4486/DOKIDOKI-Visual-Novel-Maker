using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DokiVnMaker.Story
{
    [CreateNodeMenu("Text/Hide dialogue")]
    [NodeWidth(200)]
    [NodeTint("#99ffcc")]
    public class DialogueHide : StoryNodeBase
    {
        // Use this for initialization
        protected override void Init()
        {
            name = "Hide dialogue";
            base.Init();
        }

        public float duration = 0.5f;
        public bool isWait;
    }
}