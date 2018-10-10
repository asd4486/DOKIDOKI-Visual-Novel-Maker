using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine.SceneManagement;
using System;

[CustomEditor(typeof(StoryLine))]
public class StoryLineEditor : Editor
{
    private List<object> ActionList = new List<object>();

    StoryLine StoryLine { get; set; }

    private Vector2 DialogueScroller { get; set; }

    private void OnEnable()
    {
        //get editor target
        StoryLine = (StoryLine)target;
        //game data file name
        StoryLine.DataFileName = SceneManager.GetActiveScene().name + '-' + StoryLine.gameObject.name + ".json";
        
        //load saved data
        ActionList = StoryLine.OnLoadData();
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DialogueScroller = GUILayout.BeginScrollView(DialogueScroller, GUILayout.MinHeight(0), GUILayout.MaxHeight(500));

        //add differents actions
        for (int i = 0; i < ActionList.Count; i++)
        {
            var action = ActionList[i];
            GUILayout.BeginHorizontal();
            //number
            GUILayout.Label(i + ".", GUILayout.Width(15));

            GUILayout.BeginVertical("box");

            GUILayout.BeginHorizontal();
            //action title
            var title = "";
            if (action is DialogBox) title = "Dialog";
            if (action is BrancheBox) title = "Branche";
            if (action is CharcterSpriteInfos) title = "Character sprite";
            if (action is BackgroundItem) title = "Background";
            if (action is CGInfoItem) title = "CG";
            if (action is Delayer) title = "Delayer";
            if (action is Audio) title = "Background music";
            if (action is Sound) title = "Sound";
            GUILayout.Label(title, EditorStyles.boldLabel);

            GUILayout.FlexibleSpace();
            //delete button
            if (GUILayout.Button("x", GUILayout.Width(20)))
            {
                ActionList.RemoveAt(i);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(3);

            //check all actions type and set current UI
            if (action is DialogBox) OnDialogueBox(action as DialogBox);
            if (action is BrancheBox) OnBranches(action as BrancheBox);
            if (action is CharcterSpriteInfos) OnCharacterInfos(action as CharcterSpriteInfos);
            if (action is BackgroundItem) OnBackground(action as BackgroundItem);
            if (action is CGInfoItem) OnCG(action as CGInfoItem);
            if (action is Delayer) OnDelayer(action as Delayer);
            if (action is Audio) OnAudio(action as Audio);
            if (action is Sound) OnSound(action as Sound);

            GUILayout.EndVertical();
            GUILayout.Space(10);
            GUILayout.EndHorizontal();
            //GUILayout.Space(3);
        }
        GUILayout.Space(5);
        GUILayout.EndScrollView();

        //add new action
        if (GUILayout.Button("Add new action"))
        {
            StoryLineMenu.InitWindow(this);
            //myScript.BuildObject();
        }

        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        //save data
        if (GUILayout.Button("Save"))
        {
            StoryLine.OnSaveData(ActionList);
        }
        //discard change
        if (GUILayout.Button("Discard"))
        {
           ActionList = StoryLine.OnLoadData();
        }
        GUILayout.EndHorizontal();
    }

    private void OnDisable()
    {     
        var oldList = StoryLine.OnLoadData();
        //check story line is changed or not
        bool listChanged = false;
        if (oldList.Count != ActionList.Count) { listChanged = true; }
        else
        {
            for(int i = 0; i< ActionList.Count; i++) {
                var a = ActionList[i];
                var old = oldList[i];
                if (!a.Equals(old)) listChanged = true;
            }
        }

        if (!listChanged) return;
        //show save alert when story line have changed but not saved
        bool IsSave = EditorUtility.DisplayDialog("Story line has not been Saved", "Do you want to save the change?", "Yes", "No");
        if (IsSave) { StoryLine.OnSaveData(ActionList); }
    }

    //add new action to story lines
    public void AddNewAction(object action)
    {
        if (action is CharcterSpriteInfos) ActionList.Add(action as CharcterSpriteInfos);
        if (action is DialogBox) ActionList.Add(action as DialogBox);
        if (action is BrancheBox) ActionList.Add(action as BrancheBox);
        if (action is BackgroundItem) ActionList.Add(action as BackgroundItem);
        if (action is CGInfoItem) ActionList.Add(action as CGInfoItem);
        if (action is Delayer) ActionList.Add(action as Delayer);
        if (action is Audio) ActionList.Add(action as Audio);
        if (action is Sound) ActionList.Add(action as Sound);
    }

    #region action UIs
    void OnCharacterInfos(CharcterSpriteInfos chara)
    {
        //get all character
        var list = ObjectInfoHelper.GetCharacterNames();

        //set character index if initialize request
        if (chara.Initialize)
        {
            //find origin object
            var origin = AssetDatabase.LoadAssetAtPath(chara.Path, typeof(GameObject)) as GameObject;

            if (origin != null)
            {
                //set index
                chara.Index = list.IndexOf(list.Where(c => c == origin.name).FirstOrDefault());
            }
            chara.Initialize = false;
        }

        //choice character
        chara.Index = EditorGUILayout.Popup("Character", chara.Index, list.ToArray());

        //find selected character
        string path = ValueManager.CharaPath + list[chara.Index] + ".prefab";
        var selected = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
        if (selected != null)
        {
            //set character path
            chara.Path = path;

            //get all sprites name
            var spriteList = selected.GetComponentsInChildren<Transform>().Where(s => s.GetComponent<CharaSpriteSetting>() != null).Select(s => s.name).ToArray();
            //character sprite
            chara.SpriteIndex = EditorGUILayout.Popup("Sprite", chara.SpriteIndex, spriteList);
            //select character face if existe
            var faceList = selected.transform.GetChild(chara.SpriteIndex).GetComponentsInChildren<Transform>().
                            Where(f => f.GetComponent<CharaFaceSetting>() != null).
                            Select(f => f.name).ToArray();
            if (faceList.Length > 0) chara.FaceIndex = EditorGUILayout.Popup("Face", chara.FaceIndex, faceList);
        }

        //is wait for character appear
        chara.IsWait = EditorGUILayout.Toggle("Is wait", chara.IsWait);
    }

    void OnDialogueBox(DialogBox dialog)
    {
        GUILayout.BeginVertical();
        //character name
        dialog.CharaName = EditorGUILayout.TextField("Character Name:", dialog.CharaName);
        //dialogue text box
        GUILayout.Label("Dialogue");
        dialog.Dialog = EditorGUILayout.TextArea(dialog.Dialog, GUILayout.Height(50));

        GUILayout.BeginHorizontal();
        GUILayout.Space(15);
        //show character parameter
        dialog.ShowCharParam = EditorGUILayout.Foldout(dialog.ShowCharParam, "Character");
        GUILayout.EndHorizontal();
        if (dialog.ShowCharParam)
        {
            dialog.FontSize = EditorGUILayout.IntField("Font size:", dialog.FontSize);
            //dialogue text speed
            dialog.Speed = EditorGUILayout.IntSlider("Text speed", dialog.Speed, 1, 5);
        }
        //dialog show in one shot
        dialog.NoWait = EditorGUILayout.Toggle("No wait", dialog.NoWait);
        GUILayout.EndVertical();
    }

    void OnBranches(BrancheBox branceBox)
    {
        //init branches
        if (branceBox.Branches == null)
        { branceBox.Branches = new List<string>() { "", "" }; }

        GUILayout.BeginVertical();

        GUILayout.Label("Branches");
        GUILayout.BeginVertical("Box");
        for (int i = 0; i < branceBox.Branches.Count; i++)
        {
            GUILayout.BeginHorizontal();
            var branche = branceBox.Branches[i];
            branceBox.Branches[i] = EditorGUILayout.TextField(i + ".", branche);
            //delete branch
            if (i >= 2)
            {
                if (GUILayout.Button("x", GUILayout.Width(20)))
                {
                    branceBox.Branches.RemoveAt(i);
                }
            }
            GUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();
        //add new branche
        if (GUILayout.Button("+"))
        {
            branceBox.Branches.Add("");
        }

        //branceBox.FontSize = EditorGUILayout.IntField("Font size:", branceBox.FontSize);
        ////dialogue text box
        //GUILayout.Label("Dialogue");
        //branceBox.Dialogue = EditorGUILayout.TextArea(branceBox.Dialogue, GUILayout.Height(50));

        GUILayout.EndVertical();
    }



    void OnBackground(BackgroundItem background)
    {
        //initialize
        if (background.Initialize)
        {
            //find origin object
            var origin = AssetDatabase.LoadAssetAtPath(background.Path, typeof(Sprite)) as Sprite;

            if (origin != null)
            {
                //set background image
                background.Image = origin;
            }
            background.Initialize = false;
        }

        //Choice image
        background.Image = EditorGUILayout.ObjectField("Image", background.Image, typeof(Sprite), false) as Sprite;
        //get path
        if (background.Image != null) background.Path = AssetDatabase.GetAssetPath(background.Image);

        //is wait for background appear
        background.IsWait = EditorGUILayout.Toggle("Is wait", background.IsWait);
    }

    void OnCG(CGInfoItem cg)
    {
        //get all cg
        var list = ObjectInfoHelper.GetCGsName();

        if (cg.Initialize)
        {
            //find origin object
            var origin = AssetDatabase.LoadAssetAtPath(cg.Path, typeof(Sprite)) as Sprite;
            if (origin != null)
            {
                //set index
                cg.Index = list.IndexOf(list.Where(c => c == origin.name).FirstOrDefault());

            }
            cg.Initialize = false;
        }

        //selector for cg
        cg.Index = EditorGUILayout.Popup("CG", cg.Index, list.ToArray());
        //set cg path
        cg.Path = ValueManager.CGPath + list[cg.Index] + ".jpg";

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        //load preview cg
        string path = "Assets/GameSources/CGs/" + list[cg.Index] + ".jpg";
        var imgPriveiw = AssetDatabase.LoadAssetAtPath(path, typeof(Sprite)) as Sprite;
        if (imgPriveiw != null) GUILayout.Label(imgPriveiw.texture, GUILayout.Width(128), GUILayout.Height(72));
        GUILayout.EndHorizontal();

        //is wait for CG appear
        cg.IsWait = EditorGUILayout.Toggle("Is wait", cg.IsWait);
    }

    void OnDelayer(Delayer delayer)
    {
        GUILayout.BeginHorizontal();
        delayer.Delay = EditorGUILayout.FloatField("Delay", delayer.Delay);
        GUILayout.Label("S");
        GUILayout.EndHorizontal();
    }

    void OnAudio(Audio audio)
    {
        //Choice audio
        audio.MyAudio = EditorGUILayout.ObjectField("Audio source", audio.MyAudio, typeof(AudioClip), false) as AudioClip;
        //get audio path
        if (audio.MyAudio != null) audio.AudioPath = AssetDatabase.GetAssetPath(audio.MyAudio);

        //audio volume
        audio.Volume = EditorGUILayout.Slider("Volume", audio.Volume, 0, 1);
    }

    void OnSound(Sound sound)
    {
        GUILayout.BeginVertical();
        //Choice audio
        sound.MyAudio = EditorGUILayout.ObjectField("Sound source", sound.MyAudio, typeof(AudioClip), false) as AudioClip;
        //get audio path
        if (sound.MyAudio != null) sound.AudioPath = AssetDatabase.GetAssetPath(sound.MyAudio);

        //audio volume
        sound.Volume = EditorGUILayout.Slider("Volume", sound.Volume, 0, 1);
        //GUILayout.Space(10);

        //find sound manager object
        var obj = GameObject.FindGameObjectsWithTag("doki_sound_manager").FirstOrDefault();
        if (obj != null)
        {
            var soundManager = obj.GetComponent<SoundManager>();

            //find all sound tracks
            if(soundManager.AudioTracks != null && soundManager.AudioTracks.Length > 0)
            {
                //sound track list for seletion
                var list = new List<string>();
                for (int i = 0; i < soundManager.AudioTracks.Length; i++)
                {
                    list.Add((i + 1).ToString());
                }
                sound.TrackIndex = EditorGUILayout.Popup("Track", sound.TrackIndex, list.ToArray());
            }
            
        }
        GUILayout.EndVertical();
    }
    #endregion
}
