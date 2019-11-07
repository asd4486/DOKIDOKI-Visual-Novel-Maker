using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace DokiVnMaker.Story
{
    [CreateNodeMenu("Audio/Stop background music")]
    public class BackgroundMusicStop : StoryNodeBase
    {
        // Use this for initialization
        protected override void Init()
        {
            name = "Background music";
            base.Init();
        }
    }
}