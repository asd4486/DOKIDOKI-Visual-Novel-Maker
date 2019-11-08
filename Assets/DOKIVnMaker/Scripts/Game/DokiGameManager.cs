using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DokiVnMaker.Game
{
    public class DokiGameManager : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField] TextStyleObject dialogueTextStyle;
        public static TextStyleObject DialogueTextStyle { get; private set; }

        [SerializeField] CGManagerObject cgManager;
        public static CGManagerObject CgManager { get; private set; }

        public void ChangeDialogueTextStyle(TextStyleObject style)
        {
            DialogueTextStyle = dialogueTextStyle;
        }

        public void OnAfterDeserialize()
        {
            DialogueTextStyle = dialogueTextStyle;
            CgManager = cgManager;
        }

        public void OnBeforeSerialize()
        {
        }
    }
}