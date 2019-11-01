using System;
using UnityEngine;

namespace DokiVnMaker.MyEditor.Items
{
    public class AudioBase : NodeBase
    {
        public string path;
        [NonSerialized]
        public AudioClip myAudio;
        public float volume = 1;
        public bool loop;
    }
}