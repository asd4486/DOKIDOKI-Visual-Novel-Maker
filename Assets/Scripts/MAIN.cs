using NodeEditor;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MAIN : MonoBehaviour {

    public GameObject StartStoryLine;

    //step of storyline
    //private int Step = 0;
    private bool NextStepClicable;
    //actions of story line
    private List<NodeBase> StoryActionList;
    private List<Connection> Connections;
    private NodeBase NowAction;

    //dialog box
    private Text DialogBox;
    private Text DialogName;

    //branches
    private List<GameObject> Branches;

    //images
    private Image BG;
    private Image CG;

    //audios
    private AudioSource BackgroundMusic;
    private AudioSource[] Sounds;

    //delay croroutine
    private IEnumerator DelayCoroutine;

    // Use this for initialization
    void Start () {
        //find all component
        DialogBox = GameObject.Find("dialogbox-dialog").GetComponent<Text>();
        DialogName = GameObject.Find("dialogbox-name-text").GetComponent<Text>();

        Branches = GameObject.FindGameObjectsWithTag("doki-branche").ToList();
        //set branche active image if is seted
        foreach(var b in Branches)
        {
            var img = b.transform.GetChild(0).gameObject;
            img.gameObject.SetActive(false);//hide
            b.SetActive(false);
        }

        BG = GameObject.Find("dialog-BG").GetComponent<Image>();
        CG = GameObject.Find("dialog-CG").GetComponent<Image>();
        CG.gameObject.SetActive(false);

        BackgroundMusic = GameObject.Find("Background_music").GetComponent<AudioSource>();
        Sounds = GameObject.Find("Sound_manager").GetComponent<SoundManager>().AudioTracks;

        //auto set start story line if no story line is not set 
        if (StartStoryLine == null)
        {
            var storyLine = GameObject.FindObjectsOfType<StoryLine>();
            if (storyLine.Length < 0) return;
            StartStoryLine = storyLine[0].gameObject;
        }

        //load story line datas
        if(StartStoryLine != null)
        {
            OnLoadData();
        }
        StartAction();
    }

    private void OnLoadData()
    {
        var storyLine = StartStoryLine.GetComponent<StoryLine>();

        StoryActionList = storyLine.OnLoadNodes();
        Connections = storyLine.OnLoadConnections();
    }

    // Update is called once per frame
    void Update()
    {
        //click for next step
        if (Input.GetButtonDown("Fire1"))
        {
            //return if isn't clicable 
            if (!NextStepClicable) return;
            StartAction();//start next step when click
            NextStepClicable = false;
        }
    }

    private void StartAction()
    {
        var conn = new Connection();
        //first action
        if (NowAction == null)
        {
            conn = Connections.Where(c => c.OutPoint.Node.Id == 0).FirstOrDefault();
        }
        //find next action
        else
        {
            var old = NowAction;
            conn = Connections.Where(c => c.OutPoint.Node.Id == old.Id).FirstOrDefault();
        }

        //return if story line is finished
        if (conn == null)
        {
            Debug.Log("finished");
            return;
        }

        NowAction = StoryActionList.Where(a => a.Id == conn.InPoint.Node.Id).FirstOrDefault();
        PlayStoryLine();
    }

    //parallel actions
    private void ParallellActions()
    {
        var conns = Connections.Where(c => c.InPoint.Equals(NowAction.InPoint)).ToList();
        var actionList = new List<NodeBase>();
       
        //find all parallel actions
        foreach (var c in conns)
        {
            foreach(var a in StoryActionList)
            {
                if(a.Id == c.InPoint.Node.Id)
                {
                    actionList.Add(a);
                }
            }
        }

        //run parallel actions
        foreach(var a in actionList)
        {
            switch (a.ActionType)
            {
                case ActionTypes.CharcterSpriteInfos:
                    ShowCharacter(NowAction as CharcterSpriteInfos, true);
                    break;
                //case ActionTypes.DialogBox:
                //    ShowDialog(NowAction as DialogBox);
                //    break;
                //case ActionTypes.BrancheBox:
                //    ShowBranche(NowAction as BrancheBox);
                //    break;
                case ActionTypes.BackgroundItem:
                    ShowBackground(NowAction as BackgroundItem, true);
                    break;
                case ActionTypes.CGInfoItem:
                    ShowCG(NowAction as CGInfoItem, true);
                    break;
                //case ActionTypes.Delayer:
                //    SetDelay(NowAction as Delayer, true);
                //    break;
                case ActionTypes.Audio:
                    PlayBackgroundMusic(NowAction as Audio, true);
                    break;
                case ActionTypes.Sound:
                    PlaySound(NowAction as Sound, true);
                    break;
            }
        }
        
    }

    private void PlayStoryLine()
    {
        switch (NowAction.ActionType)
        {
            case ActionTypes.CharcterSpriteInfos:
                ShowCharacter(NowAction as CharcterSpriteInfos);
                break;
            case ActionTypes.DialogBox:
                ShowDialog(NowAction as DialogBox);
                break;
            case ActionTypes.BrancheBox:
                ShowBranche(NowAction as BrancheBox);
                break;
            case ActionTypes.BackgroundItem:
                ShowBackground(NowAction as BackgroundItem);
                break;
            case ActionTypes.CGInfoItem:
                ShowCG(NowAction as CGInfoItem);
                break;
            case ActionTypes.Delayer:
                SetDelay(NowAction as Delayer);
                break;
            case ActionTypes.Audio:
                PlayBackgroundMusic(NowAction as Audio);
                break;
            case ActionTypes.Sound:
                PlaySound(NowAction as Sound);
                break;
        }

        //var action = StoryActionList[Step];

        //if (action is CharcterSpriteInfos) ShowCharacter(action as CharcterSpriteInfos);
        //if (action is DialogBox) ShowDialog(action as DialogBox);
        //if (action is BrancheBox) ShowBranche(action as BrancheBox);
        //if (action is BackgroundItem) ShowBackground(action as BackgroundItem);
        //if (action is CGInfoItem) ShowCG(action as CGInfoItem);
        //if (action is Delayer) SetDelay(action as Delayer);
        //if (action is Audio) PlayBackgroundMusic(action as Audio);
        //if (action is Sound) PlaySound(action as Sound);
    }

    private IEnumerator SetNextStepClickable(float time)
    {
        yield return new WaitForSeconds(time);
        NextStepClicable = true;
    }

    //start next step with delay
    private IEnumerator StartNextStepDelay(float time)
    {
        yield return new WaitForSeconds(time);
        StartAction();
    }
    #region actions

    #region can't parallel
    private void ShowDialog(DialogBox dialog)
    {
        //set dialog name and text
        DialogName.text = dialog.CharaName;
        DialogBox.text = dialog.Dialog;

        StartCoroutine(SetNextStepClickable(0.1f));
        //start next step
        //if (!dialog.NoWait)
        //    StartNextStepDelay(1);
        //else
        //    StartNextStep();
    }
    private void ShowBranche(BrancheBox branche)
    {
        //set all branche
        for (int i = 0; i < branche.Branches.Count; i++)
        {
            //add branche item if is over the exist list
            if (i > Branches.Count - 1)
            {
                var newBranche = Instantiate(Branches[0]);
                newBranche.transform.parent = GameObject.Find("dialog-branches").transform;
                //set new branche position
                newBranche.transform.localPosition = new Vector2(0, 150 - 60 * 1);
                Branches.Add(newBranche);
            }

            //set item text and show
            Branches[i].transform.GetChild(1).GetComponent<Text>().text = branche.Branches[i].Text;
            Branches[i].SetActive(true);
        }
    }

    private void SetDelay(Delayer delay, bool parallel = false)
    {
        //stop old delay coroutine
        if (DelayCoroutine != null) StopCoroutine(DelayCoroutine);

        //start new delay
        DelayCoroutine = StartDelay(delay.Delay);
        StartCoroutine(DelayCoroutine);
    }

    private IEnumerator StartDelay(float time)
    {
        yield return new WaitForSeconds(time);
        StartAction(); //continue action
    }
    #endregion

    private void ShowCharacter(CharcterSpriteInfos chara, bool parallel = false)
    {
        //find character object in scene
        var o = GameObject.Find(chara.Name);

        //if null instantiate new chara object in scene
        if (o == null)
        {
            o = AssetDatabase.LoadAssetAtPath(chara.Path, typeof(GameObject)) as GameObject;

            //new chara
            var newChara = Instantiate(o);   
            newChara.name = o.name;
            o = newChara;
            o.transform.parent = GameObject.Find("Character_group").transform;

        }
        o.SetActive(true);

        //active face
        var faces = o.transform.GetChild(0).GetChild(0).GetComponentsInChildren<SpriteRenderer>();
        
        if(faces.Length > 0)
        {
            //inactive all
            foreach (var f in faces)
            {
                f.gameObject.SetActive(false);
            }
            //active current face
            faces[chara.FaceIndex].gameObject.SetActive(true);
        }



        //set position
        switch (chara.CharaPos)
        {
            case CharacterPosition.Left:
               o.transform.position =  new Vector3(-5, 0);
                break;
            case CharacterPosition.Right:
                o.transform.position = new Vector3(5, 0);
                break;
            case CharacterPosition.Center:
                o.transform.position = Vector2.zero;
                break;
            case CharacterPosition.Custom:
                o.transform.position = chara.CustomPos;
                break;
        }

        //start next step
        if (chara.IsWait)
           StartCoroutine(StartNextStepDelay(1));
        else
            StartAction();
    }

    private void ShowBackground(BackgroundItem background, bool parallel = false)
    {
        //load image by path
        var img = AssetDatabase.LoadAssetAtPath(background.Path, typeof(Sprite)) as Sprite;
        //set background if can find image
        if (img != null) BG.sprite = img;

        StartAction();
    }

    private void ShowCG(CGInfoItem cg, bool parallel = false)
    {
        //load image by path
        var img = AssetDatabase.LoadAssetAtPath(cg.Path, typeof(Sprite)) as Sprite;
        //set cg if can find image
        if (img != null)
        {
            CG.sprite = img;
            CG.gameObject.SetActive(true);
        }

        //start next step
        if (cg.IsWait)
          StartCoroutine(StartNextStepDelay(1));
        else
            StartAction();
    }

    //set background music and play
    private void PlayBackgroundMusic(Audio audio, bool parallel = false)
    {
        BackgroundMusic.clip = audio.MyAudio;
        BackgroundMusic.Play();
        StartAction();
    }

    //set sound to current track and play
    private void PlaySound(Sound sound, bool parallel = false)
    {
        Sounds[sound.TrackIndex].clip = sound.MyAudio;
        StartAction();
    }
    #endregion
}
