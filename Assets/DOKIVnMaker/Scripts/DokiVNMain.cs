using DG.Tweening;
using DokiVnMaker.Character;
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
        [SerializeField] DokiDialogueMain dialogueMain;

        List<DokiCharacterBase> characterList = new List<DokiCharacterBase>();
        [SerializeField] Transform characterParent;

        //images
        [SerializeField] Image imageBG;
        [SerializeField] Image imageCG;

        StoryLauncher storyLauncher;
        Node nowNode;

        //delay croroutine
        Coroutine delayCoroutine;

        bool nextStepClickable;

        private void Awake()
        {
            storyLauncher = GetComponent<StoryLauncher>();
            //soundManager = FindObjectOfType<SoundManager>();

            ResetGameUI();
        }

        void ResetGameUI()
        {
            dialogueMain.Init();
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
                if (dialogueMain.isDisplayingText)
                    dialogueMain.DisplayAllText();
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
                Debug.Log("FIN");
                return;
            }

            nowNode = connection.node;

            if (nowNode is Dialogue) ShowDialog();
            else if (nowNode is CharacterShow) FadeInOutCharacter();
            else if (nowNode is CharacterHide) FadeInOutCharacter();
            else if (nowNode is BackgroundImage) ShowBackground();
            else if (nowNode is CG) ShowCG();
            else if (nowNode is CGFadeOut) FadeOutCG();
            else if (nowNode is Delayer) StartDelayer((nowNode as Delayer).delay);
            else if (nowNode is BackgroundMusic) PlayBackgroundMusic();
            else if (nowNode is Sound) PlaySound();
            else if (nowNode is ChangeStory) SetNextStory();
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
                var currentChar = characterList.Where(c => c.CurrentCharacterObject == node.character).FirstOrDefault();
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
            var node = nowNode as CG;

            //set cg if can find image
            var img = node.image;
            if (img != null)
            {
                imageCG.sprite = img;
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
            var node = nowNode as CGFadeOut;

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
            //soundManager.PlayBackgroundMusic(action);
            AutoPlayNextNode();
        }

        //set sound to current track and play
        private void PlaySound()
        {
            var node = nowNode as Sound;
            //soundManager.PlaySound(action);
            AutoPlayNextNode();
        }

        #region can't parallel
        private void ShowDialog()
        {
            var node = nowNode as Dialogue;

            if(node.answers.Count == 0)
                dialogueMain.OnDisplayFinishedEvent += delegate { StartCoroutine(SetClickableCoroutine(0.1f)); };

            dialogueMain.ShowDialogue(node);

            //start next step
            //if (!dialog.NoWait)
            //    StartNextStepDelay(1);
            //else
            //    StartNextStep();
        }

        void SetNextStory()
        {
            var node = nowNode as ChangeStory;
            if (node.nextStoryGraph != null)            
                storyLauncher.ChangeCurrentStory(node.nextStoryGraph, node.autoPlay);            
        }
        #endregion


        //    #region character funcs
        //    private void ShowCharacter(CharacterSpriteInfos action, bool parallel = false)
        //    {
        //        //find character object in scene
        //        var chara = GameObject.Find(action.Name);

        //        //if null instantiate new chara object in scene
        //        if (chara == null)
        //        {
        //            chara = AssetDatabase.LoadAssetAtPath(action.Path, typeof(GameObject)) as GameObject;

        //            //new chara
        //            var newChara = Instantiate(chara);
        //            newChara.name = chara.name;
        //            chara = newChara;
        //            chara.transform.SetParent(characterParent, false);

        //        }

        //        chara.SetActive(true);
        //        var sprites = chara.GetComponentsInChildren<CharaSpriteSetting>().Select(o => o.gameObject).ToList();

        //        //deactive all sprites and active sprite selected
        //        foreach (var s in sprites) s.SetActive(false);
        //        sprites[action.SpriteIndex].SetActive(true);

        //        if (action.FaceIndex > -1)
        //        {
        //            //active face
        //            var faces = sprites[action.SpriteIndex].GetComponentInChildren<CharaFaceSetting>().GetComponentsInChildren<Image>();

        //            //inactive all
        //            foreach (var f in faces) f.gameObject.SetActive(false);
        //            //active current face
        //            faces[action.FaceIndex].gameObject.SetActive(true);
        //        }

        //        var charaRect = chara.GetComponent<RectTransform>();
        //        //set position
        //        switch (action.CharaPos)
        //        {
        //            case CharacterPosition.Left:
        //                charaRect.anchoredPosition = new Vector3(-350, 0);
        //                break;
        //            case CharacterPosition.Right:
        //                charaRect.anchoredPosition = new Vector3(355, 0);
        //                break;
        //            case CharacterPosition.Center:
        //                charaRect.anchoredPosition = Vector2.zero;
        //                break;
        //            case CharacterPosition.Custom:
        //                charaRect.anchoredPosition = action.CustomPos;
        //                break;
        //        }

        //        //start next step
        //        if (action.IsWait)
        //            StartCoroutine(StartNextStepDelay(1));
        //        else
        //            PlayNextAction();
        //    }

        //    void HideCharacter(CharacterOutInfos action, bool parallel = false)
        //    {
        //        //find character object in scene
        //        var chara = GameObject.Find(action.Name);

        //        //return if can't find character
        //        if (chara == null) return;

        //        chara.SetActive(false);

        //        if (action.IsWait)
        //            StartCoroutine(StartNextStepDelay(1));
        //        else
        //            PlayNextAction();
        //    }




        //    void PlayChangeScene(ChangeScene action)
        //    {
        //        SceneManager.LoadScene(action.sceneName);
        //    }
        //    #endregion

        //    #endregion
    }
}