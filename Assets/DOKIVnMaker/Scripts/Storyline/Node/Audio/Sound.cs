using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace DokiVnMaker.Story
{
    [CreateNodeMenu("Audio/Play sound")]
    public class Sound : AudioNodeBase
    {
        // Use this for initialization
        protected override void Init()
        {
            name = "Sound";
            base.Init();
        }

        public int track;
    }
}