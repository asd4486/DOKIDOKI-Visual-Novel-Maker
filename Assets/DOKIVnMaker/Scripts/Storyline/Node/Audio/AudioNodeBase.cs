using UnityEngine;
using UnityEditor;
using XNode;

namespace DokiVnMaker.Story
{
    [NodeWidth(250)]
    //[NodeTint("#CCCCFF")]
    public abstract class AudioNodeBase : StoryNodeBase
    {
        public AudioClip audio;
        public float volume = 1;
        public bool loop;

        public bool fadeIn = true;
        public float fadeTime = 2f;
    }
}