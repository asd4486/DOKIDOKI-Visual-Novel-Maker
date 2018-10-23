using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class MAIN : MonoBehaviour {

    public GameObject StartStoryLine;

    //step of storyline
    private int Step = 0;
    private bool NextStepClicable;
    //actions of story line
    private List<object> StoryActionList;

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

        if(StartStoryLine != null)
        {
            //StoryActionList = StartStoryLine.GetComponent<StoryLine>().OnLoadData();
        }
        PlayStoryLine();
    }

    // Update is called once per frame
    void Update()
    {
        //click for next step
        if (Input.GetButtonDown("Fire1"))
        {
            //return if isn't clicable 
            if (!NextStepClicable) return;
            StartNextStep();//start next step when click
            NextStepClicable = false;
        }
    }

    private void PlayStoryLine()
    {
        var action = StoryActionList[Step];
        if (action is CharcterSpriteInfos) ShowCharacter(action as CharcterSpriteInfos);
        if (action is DialogBox) ShowDialog(action as DialogBox);
        if (action is BrancheBox) ShowBranche(action as BrancheBox);
        if (action is BackgroundItem) ShowBackground(action as BackgroundItem);
        if (action is CGInfoItem) ShowCG(action as CGInfoItem);
        if (action is Delayer) SetDelay(action as Delayer);
        if (action is Audio) PlayBackgroundMusic(action as Audio);
        if (action is Sound) PlaySound(action as Sound);
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
        StartNextStep();
    }

    private void StartNextStep()
    {
        //return if story line is finished
        if (Step >= StoryActionList.Count - 1) return;
        Step += 1;
        PlayStoryLine();
    }

    #region actions
    private void ShowCharacter(CharcterSpriteInfos chara)
    {
        //find character object in scene
        var o = GameObject.Find(chara.Name);
        //if null instantiate new chara object in scene
        if (o == null)
        {
            o = AssetDatabase.LoadAssetAtPath(chara.Path, typeof(GameObject)) as GameObject;
            //default left
            var newChara = Instantiate(o, new Vector3(-5, 0, 0), Quaternion.identity);
            newChara.name = o.name;
            newChara.transform.parent = GameObject.Find("Character_group").transform;
        }
        o.SetActive(true);
        
        //start next step
        if (chara.IsWait)
           StartCoroutine(StartNextStepDelay(1));
        else
            StartNextStep();
    }

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
        for(int i =0; i < branche.Branches.Count; i++)
        {
            //add branche item if is over the exist list
            if( i > Branches.Count - 1)
            {
                var newBranche = Instantiate(Branches[0]);
                newBranche.transform.parent = GameObject.Find("dialog-branches").transform;
                //set new branche position
                newBranche.transform.localPosition = new Vector2(0, 150 - 60*1);
                Branches.Add(newBranche);
            }

            //set item text and show
            Branches[i].transform.GetChild(1).GetComponent<Text>().text = branche.Branches[i];
            Branches[i].SetActive(true);
        }
    }

    private void ShowBackground(BackgroundItem background)
    {
        //load image by path
        var img = AssetDatabase.LoadAssetAtPath(background.Path, typeof(Sprite)) as Sprite;
        //set background if can find image
        if (img != null) BG.sprite = img;

        StartNextStep();
    }

    private void ShowCG(CGInfoItem cg)
    {
        //load image by path
        var img = AssetDatabase.LoadAssetAtPath(cg.Path, typeof(Sprite)) as Sprite;
        //set cg if can find image
        if (img != null) CG.sprite = img;

        //start next step
        if (cg.IsWait)
          StartCoroutine(StartNextStepDelay(1));
        else
            StartNextStep();
    }

    private void SetDelay(Delayer delay)
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
        StartNextStep(); //continue action
    }

    //set background music and play
    private void PlayBackgroundMusic(Audio audio)
    {
        BackgroundMusic.clip = audio.MyAudio;
        BackgroundMusic.Play();
        StartNextStep();
    }

    //set sound to current track and play
    private void PlaySound(Sound sound)
    {
        Sounds[sound.TrackIndex].clip = sound.MyAudio;
        StartNextStep();
    }
    #endregion
}
