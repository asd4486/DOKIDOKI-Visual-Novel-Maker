using DokiVnMaker.Character;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNode;

namespace DokiVnMaker.Story
{
    [CreateNodeMenu("Text/Text style")]
    [NodeTint("#99ffcc")]
    public class TextStyle : StoryNodeBase
    {
        // Use this for initialization
        protected override void Init()
        {
            name = "Text style";
            base.Init();
        }

        public TextStyleObject style;
    }
}