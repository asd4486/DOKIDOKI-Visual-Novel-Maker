using DokiVnMaker.Story;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DokiVnMaker.Game
{
    public class DokiDialogueMain : MonoBehaviour
    {
        DokiVNMain main;

        //dialog box
        [SerializeField] Text textDialog;
        [SerializeField] GameObject charaNameContent;
        [SerializeField] Text textCharaName;
        [SerializeField] GameObject iconContinue;

        //branches
        [SerializeField] Button[] answerBtns;

        Dialogue nowNode;

        public bool isDisplayingText { get; private set; }
        public event System.Action OnDisplayFinishedEvent;

        float dispalyTextSpeed;

        private void Awake()
        {
            main = FindObjectOfType<DokiVNMain>();
        }

        public void Init()
        {
            charaNameContent.SetActive(false);

            foreach (var btn in answerBtns)
            {
                btn.gameObject.SetActive(false);
                btn.onClick.RemoveAllListeners();
            }
            iconContinue.SetActive(false);
        }

        // Update is called once per frame
        void LateUpdate()
        {
            DisplayingText();
        }

        public void ShowDialogue(Dialogue _node)
        {
            Init();
            nowNode = _node;

            //show character name and
            if (nowNode.character != null || !string.IsNullOrEmpty(nowNode.charaName))
            {
                charaNameContent.SetActive(true);
                textCharaName.text = nowNode.character != null ? nowNode.character.charaName : nowNode.charaName;
            }

            var dialogueStyle = DokiGameManager.DialogueTextStyle;
            //set general text style or custom style
            textDialog.font = nowNode.font != null ? nowNode.font : dialogueStyle.font;
            textDialog.fontSize = nowNode.fontSize > 0 ? nowNode.fontSize : dialogueStyle.fontSize;
            textDialog.color = nowNode.textColor.a > 0 ? nowNode.textColor : dialogueStyle.textColor;
            dispalyTextSpeed = nowNode.displaySpeed > 0 ? nowNode.displaySpeed : dialogueStyle.displaySpeed;

            if (nowNode.displayAll)
            {
                DisplayAllText();
            }
            else
            {
                textDialog.text = "";
                isDisplayingText = true;
            }
        }

        float showTextTimer;
        void DisplayingText()
        {
            if (nowNode == null || !isDisplayingText)
                return;

            if (dispalyTextSpeed <= 0)
            {
                DisplayAllText();
                return;
            }

            var speed = 0.1f / dispalyTextSpeed;
            showTextTimer += Time.deltaTime;
            if (showTextTimer >= speed)
            {
                textDialog.text += nowNode.dialogue[textDialog.text.Length];
                showTextTimer = 0;
                if (textDialog.text.Length == nowNode.dialogue.Length)
                    DisplayAllText();
            }
        }

        public void DisplayAllText()
        {
            if (nowNode == null)
                return;

            isDisplayingText = false;
            showTextTimer = 0;
            textDialog.text = nowNode.dialogue;
            iconContinue.SetActive(true);

            if (OnDisplayFinishedEvent != null)
            {
                OnDisplayFinishedEvent();
                OnDisplayFinishedEvent = null;
            }

            if (nowNode.answers.Count > 0)
                ShowAnswers();
            else
                nowNode = null;
        }

        void ShowAnswers()
        {
            for (int i = 0; i < nowNode.answers.Count; i++)
            {
                var btn = answerBtns[i];
                btn.gameObject.SetActive(true);
                btn.GetComponentInChildren<Text>().text = nowNode.answers[i];
                var connection = nowNode.GetAnswerNextConnection(i);
                btn.onClick.AddListener(() =>
                    {
                        Init();
                        main.PlayNextNode(connection);
                    });
            }
        }
    }
}