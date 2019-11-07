using UnityEngine;
using UnityEditor;

namespace DokiVnMaker.Story
{
    [CreateNodeMenu("Character/Hide")]
    public class CharacterHide : CharacterShowBase
    {
        protected override void Init()
        {
            SetFadeIn(false);
            base.Init();
        }
    }
}
