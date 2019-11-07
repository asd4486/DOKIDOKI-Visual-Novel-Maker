using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace DokiVnMaker.Story
{
    [CreateNodeMenu("Audio/Play background music")]
    public class BackgroundMusic : AudioNodeBase
    {
        // Use this for initialization
        protected override void Init()
        {
            name = "Background music";
            base.Init();
        }
    }
}