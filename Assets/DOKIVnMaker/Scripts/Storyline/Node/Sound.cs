using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace DokiVnMaker.StoryNode
{
    public class Sound : StoryNodeBase
    {
        // Use this for initialization
        protected override void Init()
        {
            name = "Sound";
            base.Init();
        }

        public AudioClip audio;
        public float volume = 1;
        public int track;
        public bool loop;
    }
}