using UnityEngine;
using UnityEditor;

namespace DokiVnMaker.Story
{
    public class CharacterFadeOut : CharacterFadeBase
    {
        protected override void Init()
        {
            SetFadeIn(false);
            base.Init();
        }
    }
}
