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
        [SerializeField] Text textCharaName;
        [SerializeField] GameObject iconContinue;

        //branches
        [SerializeField] Button[] answerBtns;

        Dialogue currentNode;

        public bool isDisplayingText { get; private set; }
        public event System.Action OnDisplayFinishedEvent;

        private void Awake()
        {
            main = FindObjectOfType<DokiVNMain>();
        }

        public void Init()
        {
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
            currentNode = _node;

            //set dialog name and text
            if (currentNode.character != null && string.IsNullOrEmpty(currentNode.characterName))
                textCharaName.text = currentNode.character.charaName;
            else
                textCharaName.text = currentNode.characterName;

            if (currentNode.font != null) textDialog.font = currentNode.font;
            if (currentNode.fontSize > 0) textDialog.fontSize = currentNode.fontSize;
            if (currentNode.textColor.a > 0) textDialog.color = currentNode.textColor;

            if (currentNode.displayAll)
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
            if (currentNode == null || !isDisplayingText)
                return;

            if (currentNode.displaySpeed <= 0)
            {
                DisplayAllText();
                return;
            }

            var speed = 0.1f / currentNode.displaySpeed;
            showTextTimer += Time.deltaTime;
            if (showTextTimer >= speed)
            {
                textDialog.text += currentNode.dialogue[textDialog.text.Length];
                showTextTimer = 0;
                if (textDialog.text.Length == currentNode.dialogue.Length)               
                    DisplayAllText();                
            }
        }

        public void DisplayAllText()
        {
            if (currentNode == null)
                return;

            isDisplayingText = false;
            showTextTimer = 0;
            textDialog.text = currentNode.dialogue;
            iconContinue.SetActive(true);

            if (OnDisplayFinishedEvent != null)
            {
                OnDisplayFinishedEvent();
                OnDisplayFinishedEvent = null;
            }

            if (currentNode.answers.Count > 0)
                ShowAnswers();
            else   
                currentNode = null;             
        }

        void ShowAnswers()
        {
            for (int i = 0; i < currentNode.answers.Count; i++)
            {
                var btn = answerBtns[i];
                btn.gameObject.SetActive(true);
                btn.GetComponentInChildren<Text>().text = currentNode.answers[i];
                var connection = currentNode.GetAnswerNextConnection(i);
                btn.onClick.AddListener(() =>
                    {
                        main.PlayNextNode(connection);
                        Init();
                    });
            }
        }
    }
}