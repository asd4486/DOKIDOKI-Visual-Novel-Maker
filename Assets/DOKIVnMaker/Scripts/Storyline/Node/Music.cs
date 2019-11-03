using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace DokiVnMaker.Story
{
    public class Music : StoryNodeBase
    {
        // Use this for initialization
        protected override void Init()
        {
            name = "Music";
            base.Init();
        }

        public AudioClip audio;
        public float volume = 1;
        public bool loop;
    }
}