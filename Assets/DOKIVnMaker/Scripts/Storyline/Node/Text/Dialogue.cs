using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using XNode;

namespace DokiVnMaker.Story
{
    [CreateNodeMenu("Text/Show dialogue")]
    [NodeWidth(300)]
    [NodeTint("#99ffcc")]
    public class Dialogue : StoryNodeBase
    {
        // Use this for initialization
        protected override void Init()
        {
            name = "Show dialogue";
            base.Init();
        }

        public CharacterObject character;
        //public CharacterObject character;
        public string charaName;
        [TextArea] public string dialogue;

        public Font font;
        public int fontSize;
        public Color textColor;

        public bool displayAll;
        public float displaySpeed;   

        public AudioClip voiceClip;

        [outputlist(dynamicPortList = true), DialogueAnswerAttribute] public List<string> answers = new List<string>();

        //remove last answer if out of range
        public void CheckAnswerCount()
        {
            if (answers.Count > 6) answers.RemoveAt(answers.Count - 1);
        }

        public NodePort GetAnswerNextConnection(int index)
        {
            NodePort port = null;
            if (answers.Count == 0)
            {
                port = GetOutputPort("output");
            }
            else
            {
                if (answers.Count <= index) return null;
                port = GetOutputPort("answers " + index);
            }

            if (port == null) return null;

            return port.Connection;
        }
    }
}