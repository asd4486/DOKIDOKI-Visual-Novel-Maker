using DG.Tweening;
using DokiVnMaker.Story;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DokiVnMaker.Game
{
    public class DokiPlayerUI : MonoBehaviour
    {
        DokiVNMain main;

        //dialog box
        [SerializeField] Image dialogueBg;
        [SerializeField] Text textDialog;

        [SerializeField] Image charaNameBg;
        [SerializeField] Text textCharaName;
        [SerializeField] GameObject iconContinue;

        //branches
        [SerializeField] Button[] answerBtns;

        Dialogue nowDialogue;

        public bool isDisplayingText { get; private set; }
        public event System.Action OnDisplayFinishedEvent;

        float dispalyTextSpeed;

        private void Awake()
        {
            main = FindObjectOfType<DokiVNMain>();
        }

        public void Init()
        {
            ShowHideUI(false);
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
            ShowDialogue(true);
            nowDialogue = _node;

            //show character name and
            if (nowDialogue.character != null || !string.IsNullOrEmpty(nowDialogue.charaName))
            {
                ShowCharaName(true);
                textCharaName.text = nowDialogue.character != null ? nowDialogue.character.charaName : nowDialogue.charaName;
            }

            var dialogueStyle = DokiGameManager.DialogueTextStyle;
            //set general text style or custom style
            textDialog.font = nowDialogue.font != null ? nowDialogue.font : dialogueStyle.font;
            textDialog.fontSize = nowDialogue.fontSize > 0 ? nowDialogue.fontSize : dialogueStyle.fontSize;
            textDialog.color = nowDialogue.textColor.a > 0 ? nowDialogue.textColor : dialogueStyle.textColor;
            dispalyTextSpeed = nowDialogue.displaySpeed > 0 ? nowDialogue.displaySpeed : dialogueStyle.displaySpeed;

            if (nowDialogue.displayAll)
            {
                DisplayAllText();
            }
            else
            {
                textDialog.text = "";
                isDisplayingText = true;
            }
        }

        public void ShowHideUI(bool show, float duration = 0)
        {
            ShowCharaName(show, duration);
            ShowDialogue(show, duration);
            ShowAnswers(show, duration);
        }

        void ShowCharaName(bool show, float duration = 0)
        {
            textCharaName.gameObject.SetActive(show);
            var targetColor = show ? new Color(0, 0, 0, 0.8f) : Color.clear;
            charaNameBg.DOColor(targetColor, duration);
        }

        void ShowDialogue(bool show, float duration = 0)
        {
            textDialog.gameObject.SetActive(show);
            var targetColor = show ? new Color(0, 0, 0, 0.8f) : Color.clear;
            dialogueBg.DOColor(targetColor, duration);
        }

        void ShowAnswers(bool show, float duration = 0)
        {
            if (show == false)
                foreach (var btn in answerBtns)
                {
                    btn.gameObject.SetActive(show);
                    btn.onClick.RemoveAllListeners();
                }
            else
            {
                if (nowDialogue == null) return;
                //only show current answer of node
                for (int i = 0; i < nowDialogue.answers.Count; i++)
                {
                    var btn = answerBtns[i];
                    btn.gameObject.SetActive(true);
                }
            }
        }

        float showTextTimer;
        void DisplayingText()
        {
            if (nowDialogue == null || !isDisplayingText)
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
                textDialog.text += nowDialogue.dialogue[textDialog.text.Length];
                showTextTimer = 0;
                if (textDialog.text.Length == nowDialogue.dialogue.Length)
                    DisplayAllText();
            }
        }

        public void DisplayAllText()
        {
            if (nowDialogue == null)
                return;

            isDisplayingText = false;
            showTextTimer = 0;
            textDialog.text = nowDialogue.dialogue;
            iconContinue.SetActive(true);

            if (OnDisplayFinishedEvent != null)
            {
                OnDisplayFinishedEvent();
                OnDisplayFinishedEvent = null;
            }

            if (nowDialogue.answers.Count > 0)
                SetupAnswers();
            else
                nowDialogue = null;
        }

        void SetupAnswers()
        {
            for (int i = 0; i < nowDialogue.answers.Count; i++)
            {
                var btn = answerBtns[i];
                btn.gameObject.SetActive(true);
                btn.GetComponentInChildren<Text>().text = nowDialogue.answers[i];
                var connection = nowDialogue.GetAnswerNextConnection(i);
                btn.onClick.AddListener(() =>
                    {
                        Init();
                        main.PlayNextNode(connection);
                    });
            }
        }
    }
}