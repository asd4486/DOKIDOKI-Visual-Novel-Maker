using System;
using UnityEngine;

namespace DokiVnMaker.MyEditor.Items
{
    public class AudioBase : NodeBase
    {
        public string Path;
        [NonSerialized]
        public AudioClip MyAudio;
        public float Volume = 1;
    }
}