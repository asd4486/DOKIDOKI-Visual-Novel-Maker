using UnityEngine;
using UnityEditor;
using DokiVnMaker.Character;

namespace DokiVnMaker.Story
{
    public abstract class CharacterFadeBase : StoryNodeBase
    {
        public bool isFadeIn;
        public CharacterObject character;
        public float duration = 1f;
        public bool isWait;

        protected void SetFadeIn(bool fadeIn)
        {
            isFadeIn = fadeIn;
            name = isFadeIn ? "Character in" : "Character out";
        }
    }
}