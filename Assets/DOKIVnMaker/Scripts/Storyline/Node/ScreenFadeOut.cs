using UnityEngine;
using UnityEditor;

namespace DokiVnMaker.Story
{
    [NodeWidth(200)]
    [CreateNodeMenu("Screen/Fade out")]
    public class ScreenFadeOut : StoryNodeBase
    {
        // Use this for initialization
        protected override void Init()
        {
            base.Init();
        }

        public Color color = Color.black;
        public float duration = 0.5f;
        public bool isWait;
    }
}