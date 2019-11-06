using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace DokiVnMaker.Story
{
    [CreateNodeMenu("Music/Background music")]
    public class BackgroundMusic : StoryNodeBase
    {
        // Use this for initialization
        protected override void Init()
        {
            name = "Background music";
            base.Init();
        }

        public AudioClip audio;
        public float volume = 1;
        public bool loop;
    }
}