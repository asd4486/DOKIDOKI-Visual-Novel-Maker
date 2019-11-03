using UnityEngine;
using UnityEditor;

namespace DokiVnMaker.Story
{
    public class CharacterFadeIn : CharacterFadeBase
    {
        protected override void Init()
        {
            SetFadeIn(true);
            base.Init();
        }
    }
}
