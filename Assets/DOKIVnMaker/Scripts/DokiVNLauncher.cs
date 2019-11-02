using DokiVnMaker.StoryNode;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using XNode;

namespace DokiVnMaker.Game
{
    [RequireComponent(typeof(StoryLauncher))]
    public class DokiVNLauncher : MonoBehaviour
    {
        //dialog box
        [SerializeField] Text textDialog;
        [SerializeField] Text textCharaName;

        //branches
        [SerializeField] GameObject branchesParent;
        Button[] brancheBtnList;

        [SerializeField] Transform characterParent;

        //images
        [SerializeField] Image imageBG;
        [SerializeField] Image imageCG;

        StoryLauncher storyLauncher;
        StoryGraph currentStory;
        Node nowNode;

        //delay croroutine
        Coroutine delayCoroutine;
        bool nextStepClickable;

        private void Awake()
        {
            storyLauncher = GetComponent<StoryLauncher>();


            //soundManager = FindObjectOfType<SoundManager>();
            //auto find all branche buttons
            if (branchesParent != null)
                brancheBtnList = branchesParent.GetComponentsInChildren<Button>();

            ResetGameUI();
        }

        void ResetGameUI()
        {
            branchesParent.SetActive(false);
            imageCG.gameObject.SetActive(false);
        }

        //launch new story 
        public void LaunchStory()
        {
            if (storyLauncher.CurrentStory == null) return;

            currentStory = storyLauncher.CurrentStory;

            PlayNextStoryNode();
        }


        // Update is called once per frame
        void Update()
        {
            //click for next step
            if (Input.GetButtonDown("Fire1"))
            {
                //return if isn't clicable 
                if (!nextStepClickable) return;
                //start next step when click
                PlayNextStoryNode();
                nextStepClickable = false;
            }
        }

        //play next story node in current story graph
        void PlayNextStoryNode()
        {
            NodePort connection = null;
            //find  current node
            if (nowNode == null)
                connection = currentStory.nodes.Where(n => n is StartPoint).FirstOrDefault().GetOutputPort("output").Connection;
            else
                connection = currentStory.nodes.Where(n => n == nowNode).FirstOrDefault().GetOutputPort("output").Connection;

            if (connection == null)
            {
                Debug.Log("FIN");
                return;
            }

            nowNode = connection.node;
            ResetGameUI();

            if (nowNode is Dialogue) ShowDialog();
            else if (nowNode is BackgroundImage) ShowBackground();
            else if (nowNode is CG) ShowCG();
            else if (nowNode is Delayer) StartDelayer((nowNode as Delayer).delay);
            else if (nowNode is Music) PlayBackgroundMusic();
            else if (nowNode is Sound) PlaySound();
            else if (nowNode is ChangeStory) SetNextStory();
        }

        //    //parallel actions
        //    private void PlayParallellActions()
        //    {
        //        var conns = myConnections.Where(c => c.InPoint.Equals(nowAction.InPoint)).ToList();
        //        var actionList = new List<NodeBase>();

        //        //find all parallel actions
        //        foreach (var c in conns)
        //        {
        //            foreach (var a in storyActionList)
        //            {
        //                if (a.Id == c.InPoint.targetNodeId)
        //                {
        //                    actionList.Add(a);
        //                }
        //            }
        //        }

        //        //run parallel actions
        //        foreach (var a in actionList)
        //        {
        //            switch (a.ActionType)
        //            {
        //                case ActionTypes.CharacterSpriteInfos:
        //                    ShowCharacter(nowAction as CharacterSpriteInfos, true);
        //                    break;
        //                //case ActionTypes.DialogBox:
        //                //    ShowDialog(NowAction as DialogBox);
        //                //    break;
        //                //case ActionTypes.BrancheBox:
        //                //    ShowBranche(NowAction as BrancheBox);
        //                //    break;
        //                case ActionTypes.BackgroundImage:
        //                    ShowBackground(nowAction as BackgroundImage, true);
        //                    break;
        //                case ActionTypes.CGImage:
        //                    ShowCG(nowAction as CGImage, true);
        //                    break;
        //                //case ActionTypes.Delayer:
        //                //    SetDelay(NowAction as Delayer, true);
        //                //    break;
        //                case ActionTypes.Music:
        //                    PlayBackgroundMusic(nowAction as Music, true);
        //                    break;
        //                case ActionTypes.Sound:
        //                    PlaySound(nowAction as Sound, true);
        //                    break;
        //            }
        //        }

        //    }

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
            PlayNextStoryNode();
        }

        //    #region actions

        private IEnumerator SetStepClickableCoroutine(float time)
        {
            yield return new WaitForSeconds(time);
            nextStepClickable = true;
        }

        private void ShowBackground()
        {
            var node = nowNode as BackgroundImage;
            imageBG.sprite = node.image;

            PlayNextStoryNode();
        }

        private void ShowCG()
        {
            var node = nowNode as CG;

            //set cg if can find image
            var img = node.image;
            if (img != null)
            {
                imageCG.sprite = img;
                imageCG.gameObject.SetActive(true);
            }

            //start next step
            if (node.isWait)
                StartDelayer(node.waitTime);
            else
                PlayNextStoryNode();
        }

        //set background music and play
        private void PlayBackgroundMusic()
        {
            var node = nowNode as Music;
            //soundManager.PlayBackgroundMusic(action);
            PlayNextStoryNode();
        }

        //set sound to current track and play
        private void PlaySound()
        {
            var node = nowNode as Sound;
            //soundManager.PlaySound(action);
            PlayNextStoryNode();
        }

        #region can't parallel
        private void ShowDialog()
        {
            var node = nowNode as Dialogue;

            //set dialog name and text
            if (node.character != null && string.IsNullOrEmpty(node.characterName))
                textCharaName.text = node.character.name;
            else
                textCharaName.text = node.characterName;

            textDialog.text = node.dialogue;
            if (node.fontSize > 0) textDialog.fontSize = node.fontSize;

            StartCoroutine(SetStepClickableCoroutine(0.1f));
            //start next step
            //if (!dialog.NoWait)
            //    StartNextStepDelay(1);
            //else
            //    StartNextStep();
        }

        //private void ShowBranche(BrancheBox action)
        //{
        //    branchesParent.SetActive(true);

        //    foreach (var b in brancheBtnList) b.gameObject.SetActive(false);

        //    for (int i = 0; i < action.brancheList.Count; i++)
        //    {
        //        var info = action.brancheList[i];
        //        var btn = brancheBtnList[i];
        //        btn.gameObject.SetActive(true);
        //        btn.GetComponentInChildren<Text>().text = info.text;
        //    }
        //}


        void SetNextStory()
        {
            var node = nowNode as ChangeStory;
            if (node.nextStoryGraph != null)
            {
                currentStory = node.nextStoryGraph;
                storyLauncher.ChangeCurrentStory(currentStory);
            }
        }

        #endregion

        //    #region action normals
        //    private void SetDelay(Delayer action, bool parallel = false)
        //    {
        //        //stop old delay coroutine
        //        if (DelayCoroutine != null) StopCoroutine(DelayCoroutine);

        //        //start new delay
        //        DelayCoroutine = StartCoroutine(StartDelay(action.Delay));
        //    }

        //    private IEnumerator StartDelay(float time)
        //    {
        //        yield return new WaitForSeconds(time);
        //        PlayNextAction(); //continue action
        //    }

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
        //    #endregion




        //    void PlayChangeScene(ChangeScene action)
        //    {
        //        SceneManager.LoadScene(action.sceneName);
        //    }
        //    #endregion

        //    #endregion
    }
}