using UnityEngine;
using UnityEditor;

namespace DokiVnMaker.Story
{
    [NodeWidth(200)]
    [CreateNodeMenu("Screen/Fade in")]
    public class ScreenFadeIn : StoryNodeBase
    {
        // Use this for initialization
        protected override void Init()
        {
            base.Init();
        }

        public float duration = 0.5f;
        public bool isWait;
    }
}