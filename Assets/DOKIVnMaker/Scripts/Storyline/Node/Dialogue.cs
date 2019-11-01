using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace DokiVnMaker.StoryNode
{
    [NodeTint("#66ff66")]
    public class Dialogue : StoryNodeBase
    {
        // Use this for initialization
        protected override void Init()
        {
            name = "Dialogue";
            base.Init();
        }

        public CharacterGraph character;
        [TextArea] public string dialogue;
        [Output(instancePortList = true)] public List<Answer> answers = new List<Answer>();

        [System.Serializable]
        public class Answer
        {
            public string text;
            public AudioClip voiceClip;
        }

        //public void AnswerQuestion(int index)
        //{
        //    NodePort port = null;
        //    if (answers.Count == 0)
        //    {
        //        port = GetOutputPort("output");
        //    }
        //    else
        //    {
        //        if (answers.Count <= index) return;
        //        port = GetOutputPort("answers " + index);
        //    }

        //    if (port == null) return;
        //    for (int i = 0; i < port.ConnectionCount; i++)
        //    {
        //        NodePort connection = port.GetConnection(i);
        //        (connection.node as DialogueBaseNode).Trigger();
        //    }
        //}

        //public override void Trigger()
        //{
        //    (graph as StoryGraph).current = this;
        //}
    }
}