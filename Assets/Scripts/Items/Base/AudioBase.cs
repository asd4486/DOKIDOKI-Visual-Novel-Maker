using NodeEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class AudioBase:NodeBase
{
    public bool Initialize;
    public string AudioPath;
    [NonSerialized]
    public AudioClip MyAudio;
    public float Volume = 1;
}

