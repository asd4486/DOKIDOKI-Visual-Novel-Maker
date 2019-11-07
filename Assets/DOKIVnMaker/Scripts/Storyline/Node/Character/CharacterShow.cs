using UnityEngine;
using UnityEditor;

namespace DokiVnMaker.Story
{
    [CreateNodeMenu("Character/Show")]
    public class CharacterShow : CharacterShowBase
    {
        protected override void Init()
        {
            SetFadeIn(true);
            base.Init();
        }
    }
}
