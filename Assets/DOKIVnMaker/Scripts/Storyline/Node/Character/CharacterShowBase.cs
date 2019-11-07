using UnityEngine;

namespace DokiVnMaker.Story
{
    public enum DisplayPositions
    {
        Left,
        Center,
        Right,
        Custom
    }

    [NodeWidth(210)]
    public abstract class CharacterShowBase : StoryNodeBase
    {
        public bool isFadeIn;
        public CharacterObject character;
        public int faceIndex = -1;


        public DisplayPositions displayPos;
        public string[] DisplayList { get { return System.Enum.GetNames(typeof(DisplayPositions)); } }

        public Vector2 customPos;

        public float duration = 0.5f;
        public bool isWait;

        protected void SetFadeIn(bool fadeIn)
        {
            isFadeIn = fadeIn;
            name = isFadeIn ? "Show character" : "Hide character";
        }
    }
}