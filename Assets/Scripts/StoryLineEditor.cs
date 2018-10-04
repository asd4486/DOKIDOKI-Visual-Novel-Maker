using UnityEngine;
using System.Collections;
using UnityEditor;
using TreeEditor;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine.SceneManagement;
using System;

[CustomEditor(typeof(StoryLine))]
public class StoryLineEditor : Editor
{
    private string DataFileName { get; set; }

    StoryLine StoryLine { get; set; }

    private Vector2 DialogueScroller { get; set; }

    private void OnEnable()
    {
        //get editor target
        StoryLine = (StoryLine)target;
        //game data file name
        DataFileName = SceneManager.GetActiveScene().name + '-' + StoryLine.gameObject.name + ".json";

        //load saved data
        OnLoadData();
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        DialogueScroller = GUILayout.BeginScrollView(DialogueScroller, GUILayout.MinHeight(0), GUILayout.MaxHeight(500));

        //add differents actions
        for (int i = 0; i < StoryLine.ActionList.Count; i++)
        {
            var action = StoryLine.ActionList[i];
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
            if (action is CGInfoItem) title = "CG";
            if (action is Delayer) title = "Delayer";
            if (action is Audio) title = "Audio";
            if (action is Sound) title = "Sound";
            GUILayout.Label(title, EditorStyles.boldLabel);

            GUILayout.FlexibleSpace();
            //delete button
            if (GUILayout.Button("x", GUILayout.Width(20)))
            {
                StoryLine.ActionList.RemoveAt(i);
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(3);

            //check all actions type and set current UI
            if (action is DialogBox) OnDialogueBox(action as DialogBox);
            if (action is BrancheBox) OnBranches(action as BrancheBox);
            if (action is CharcterSpriteInfos) OnCharacterInfos(action as CharcterSpriteInfos);
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
            OnSaveData();
        }
        //discard change
        if (GUILayout.Button("Discard"))
        {
            OnLoadData();
        }
        GUILayout.EndHorizontal();
    }

    //add new action to story lines
    public void AddNewAction(object action)
    {
        if (action is DialogBox) StoryLine.ActionList.Add(action as DialogBox);
        if (action is BrancheBox) StoryLine.ActionList.Add(action as BrancheBox);
        if (action is CharcterSpriteInfos) StoryLine.ActionList.Add(action as CharcterSpriteInfos);
        if (action is CGInfoItem) StoryLine.ActionList.Add(action as CGInfoItem);
        if (action is Delayer) StoryLine.ActionList.Add(action as Delayer);
        if (action is Audio) StoryLine.ActionList.Add(action as Audio);
        if (action is Sound) StoryLine.ActionList.Add(action as Sound);
    }

    void OnDialogueBox(DialogBox dialogue)
    {
        GUILayout.BeginVertical();
        //character name
        dialogue.Name = EditorGUILayout.TextField("Character Name:", dialogue.Name);
        dialogue.FontSize = EditorGUILayout.IntField("Font size:", dialogue.FontSize);
        //dialogue text speed
        dialogue.Speed = EditorGUILayout.IntSlider("Text speed", dialogue.Speed, 1, 5);

        //dialogue text box
        GUILayout.Label("Dialogue");
        dialogue.Dialogue = EditorGUILayout.TextArea(dialogue.Dialogue, GUILayout.Height(50));
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

    void OnCharacterInfos(CharcterSpriteInfos chara)
    {
        GUILayout.BeginVertical();
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
        GUILayout.EndVertical();
    }

    void OnCG(CGInfoItem cg)
    {
        //get all cg
        var list = ObjectInfoHelper.GetCGsName();

        if (cg.Initialize)
        {
            //find origin object
            var origin = AssetDatabase.LoadAssetAtPath(cg.Path, typeof(GameObject)) as GameObject;

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
        cg.Path = ValueManager.CGPath + list[cg.Index] + ".prefab";

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        //load preview cg
        string path = "Assets/GameSources/CGs/" + list[cg.Index] + ".jpg";
        var imgPriveiw = AssetDatabase.LoadAssetAtPath(path, typeof(Sprite)) as Sprite;
        if (imgPriveiw != null) GUILayout.Label(imgPriveiw.texture, GUILayout.Width(128), GUILayout.Height(72));
        GUILayout.EndHorizontal();
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

        //sound tracks(6 tracks)
        var list = new List<string>();
        for (int i = 0; i < 6; i++)
        {
            list.Add((i + 1).ToString());
        }
        //audio volume
        sound.Volume = EditorGUILayout.Slider("Volume", sound.Volume, 0, 1);
        GUILayout.Space(10);
        sound.Track = EditorGUILayout.Popup("Track", sound.Track, list.ToArray());

        GUILayout.EndVertical();
    }

    void OnLoadData()
    {
        //refresh action list
        StoryLine.ActionList.Clear();
        // Path.Combine combines strings into a file path
        // Application.StreamingAssets points to Assets/StreamingAssets in the Editor, and the StreamingAssets folder in a build
        string filePath = Path.Combine(ValueManager.GameDataPath, DataFileName);

        if (File.Exists(filePath))
        {
            //find all json data
            string jsons = File.ReadAllText(filePath);
            string[] datas = jsons.Remove(jsons.Length - 1).Remove(0, 1).Split(new string[] { ",{" }, StringSplitOptions.RemoveEmptyEntries);

            for (var i = 0; i < datas.Length; i++)
            {
                var d = datas[i];
                if(d[0] != '{') { d = "{" + d; }

                var type = JsonUtility.FromJson<ActionBase>(d);
                //check action type and load current class
                var newAction = new object();
                switch (type.ActionType)
                {
                    case ActionTypes.Audio:
                        newAction = JsonUtility.FromJson<Audio>(d);
                        break;
                    case ActionTypes.BrancheBox:
                        newAction = JsonUtility.FromJson<BrancheBox>(d);
                        break;
                    case ActionTypes.CGInfoItem:
                        newAction = JsonUtility.FromJson<CGInfoItem>(d);
                        //initialize setting
                        (newAction as CGInfoItem).Initialize = true;
                        break;
                    case ActionTypes.CharcterSpriteInfos:
                        newAction = JsonUtility.FromJson<CharcterSpriteInfos>(d);
                        //initialize setting
                        (newAction as CharcterSpriteInfos).Initialize = true;
                        break;
                    case ActionTypes.Delayer:
                        newAction = JsonUtility.FromJson<Delayer>(d);
                        break;
                    case ActionTypes.DialogBox:
                        newAction = JsonUtility.FromJson<DialogBox>(d);
                        break;
                    case ActionTypes.Sound:
                        newAction = JsonUtility.FromJson<Sound>(d);
                        break;
                }
                StoryLine.ActionList.Add(newAction);
            }
            // Pass the json to JsonUtility, and tell it to create a GameData object from it
            //var loadedData = JsonHelper.FromJson<List<dynamic>>(dataAsJson);
            //Debug.Log(loadedData.Count);
        }
        //else
        //{
        //    Debug.Log("No data for this story line!");
        //}
    }

    void OnSaveData()
    {
        var json = JsonHelper.ToJson(StoryLine.ActionList);
        Debug.Log(json);

        File.WriteAllText(ValueManager.GameDataPath + "/" + DataFileName, json);
    }
}
