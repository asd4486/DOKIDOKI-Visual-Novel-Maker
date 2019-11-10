using DG.Tweening;
using DokiVnMaker.Story;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using XNode;

namespace DokiVnMaker.Game
{
    [RequireComponent(typeof(StoryLauncher))]
    public class DokiVNMain : MonoBehaviour
    {
        bool nextStepClickable;
        [SerializeField] DokiPlayerUI playerUI;

        List<DokiCharacterBase> characterList = new List<DokiCharacterBase>();
        [SerializeField] Transform characterParent;

        //images
        [SerializeField] Image imageBG;
        [SerializeField] Image imageCG;
        [SerializeField] Image imageFadeOut;

        StoryLauncher storyLauncher;
        Node nowNode;

        //delay croroutine
        Coroutine delayCoroutine;

        AudioManager audioMgr;

        public event System.Action onStoryFinishedAction;

        private void Awake()
        {
            storyLauncher = GetComponent<StoryLauncher>();
            audioMgr = FindObjectOfType<AudioManager>();

            ResetGameUI();
        }

        void ResetGameUI()
        {
            playerUI.Init();
            imageCG.color = new Color(1, 1, 1, 0);
        }

        //launch new story 
        public void LaunchNewStory()
        {
            if (storyLauncher.CurrentStory == null) return;
            nowNode = null;
            AutoPlayNextNode();
        }


        // Update is called once per frame
        void Update()
        {
            //click for next step
            if (Input.GetButtonDown("Fire1"))
            {
                //show all text
                if (playerUI.isDisplayingText)
                    playerUI.DisplayAllText();
                //return if isn't clicable 
                else if (nextStepClickable)
                {
                    //start next step when click
                    AutoPlayNextNode();
                    nextStepClickable = false;
                }
            }
        }

        //play next story node in current story graph
        void AutoPlayNextNode()
        {
            NodePort connection = null;
            //find  current node
            if (nowNode == null)
                connection = storyLauncher.CurrentStory.nodes.Where(n => n is StartPoint).FirstOrDefault().GetOutputPort("output").Connection;
            else
                connection = storyLauncher.CurrentStory.nodes.Where(n => n == nowNode).FirstOrDefault().GetOutputPort("output").Connection;

            PlayNextNode(connection);
        }

        public void PlayNextNode(NodePort connection)
        {
            //finish story
            if(connection == null)
            {
                if(onStoryFinishedAction != null) onStoryFinishedAction();
                Debug.Log("FIN");
                return;
            }

            nowNode = connection.node;

            if (nowNode is Dialogue) ShowDialogue();
            else if (nowNode is DialogueHide) HideDialogue();
            else if (nowNode is CharacterShow) FadeInOutCharacter();
            else if (nowNode is CharacterHide) FadeInOutCharacter();
            else if (nowNode is BackgroundImage) ShowBackground();
            else if (nowNode is CGShow) ShowCG();
            else if (nowNode is CGHide) FadeOutCG();
            else if (nowNode is Delayer) StartDelayer((nowNode as Delayer).delay);
            else if (nowNode is BackgroundMusic) PlayBackgroundMusic();
            else if (nowNode is Sound) PlaySound();
            else if (nowNode is ChangeStory) SetNextStory();
            else if (nowNode is ScreenFadeIn) FadeinScreen();
            else if (nowNode is ScreenFadeOut) FadeoutScreen();
        }

        //start next step with delay
        void StartDelayer(float delay)
        {
            if (delayCoroutine != null)
                StopCoroutine(delayCoroutine);

            delayCoroutine = StartCoroutine(StartDelayCoroutine(delay));
        }

        private IEnumerator StartDelayCoroutine(float delay)
        {
            yield return new WaitForSeconds(delay);
            AutoPlayNextNode();
        }

        //a little delay for next step clickable
        private IEnumerator SetClickableCoroutine(float time)
        {
            yield return new WaitForSeconds(time);
            nextStepClickable = true;
        }

        private void ShowBackground()
        {
            var node = nowNode as BackgroundImage;
            imageBG.sprite = node.image;

            AutoPlayNextNode();
        }

        private void FadeInOutCharacter()
        {
            var node = nowNode as CharacterShowBase;

            if (node.character != null)
            {
                var currentChar = characterList.Where(c => c.currentCharacter == node.character).FirstOrDefault();
                if (node.isFadeIn)
                {
                    if (currentChar == null) currentChar = CreateNewCharacter(node.character);
                    currentChar.FadeIn(node);
                }
                else
                {
                    if (currentChar != null) currentChar.FadeOut(node.duration);
                }
            }

            if (node.isWait)
                StartDelayer(node.duration);
            else
                AutoPlayNextNode();
        }

        DokiCharacterBase CreateNewCharacter(CharacterObject character)
        {
            var newChar = Instantiate(Resources.Load("doki_characterBase") as GameObject);
            newChar.transform.SetParent(characterParent, false);
            var charaBase = newChar.GetComponent<DokiCharacterBase>();
            charaBase.Init(character);
            characterList.Add(charaBase);

            return charaBase;
        }

        private void ShowCG()
        {
            var node = nowNode as CGShow;

            //set cg if can find image
            var cg = node.cg;
            if (cg != null)
            {
                imageCG.sprite = cg.image;
                imageCG.DOColor(Color.white, node.duration);
            }

            //start next step
            if (node.isWait)
                StartDelayer(node.duration);
            else
                AutoPlayNextNode();
        }

        void FadeOutCG()
        {
            var node = nowNode as CGHide;

            imageCG.DOColor(new Color(1, 1, 1, 0), node.duration);

            //start next step
            if (node.isWait)
                StartDelayer(node.duration);
            else
                AutoPlayNextNode();
        }

        //set background music and play
        private void PlayBackgroundMusic()
        {
            var node = nowNode as BackgroundMusic;
            audioMgr.PlayBackgroundMusic(node);
            AutoPlayNextNode();
        }

        //set sound to current track and play
        private void PlaySound()
        {
            var node = nowNode as Sound;
            audioMgr.PlaySound(node);
            AutoPlayNextNode();
        }

        private void FadeoutScreen()
        {
            var node = nowNode as ScreenFadeOut;         
            imageFadeOut.DOColor(node.color, node.duration);

            //start next step
            if (node.isWait)
                StartDelayer(node.duration);
            else
                AutoPlayNextNode();
        }

        private void FadeinScreen()
        {
            var node = nowNode as ScreenFadeIn;
            imageFadeOut.DOFade(0, node.duration);
            
            //start next step
            if (node.isWait)
                StartDelayer(node.duration);
            else
                AutoPlayNextNode();
        }

        #region can't parallel
        private void ShowDialogue()
        {
            var node = nowNode as Dialogue;
            if(node.answers.Count == 0)
                playerUI.OnDisplayFinishedEvent += delegate { StartCoroutine(SetClickableCoroutine(0.1f)); };

            playerUI.ShowDialogue(node);
        }

        private void HideDialogue()
        {
            var node = nowNode as DialogueHide;
            playerUI.ShowHideUI(false, node.duration);
            if (node.isWait)
                StartDelayer(node.duration);
            else
                AutoPlayNextNode();
        }

        void SetNextStory()
        {
            var node = nowNode as ChangeStory;
            if (node.nextStoryGraph != null)            
                storyLauncher.ChangeCurrentStory(node.nextStoryGraph, node.autoPlay);            
        }
        #endregion
    }
}