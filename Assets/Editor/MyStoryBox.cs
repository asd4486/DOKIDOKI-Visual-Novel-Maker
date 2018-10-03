//using Editor.Items;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using UnityEditor;
//using UnityEngine;

//public class MyStoryBox : EditorWindow
//{
//    [MenuItem("DokiDoki VN Maker/Story box")]
//    static void Init()
//    {
//        MyStoryBox window = (MyStoryBox)EditorWindow.GetWindow(typeof(MyStoryBox), false, "Story box");
//    }

//    private Vector2 DialogueScroller { get; set; }
//    private int ActionCount { get; set; }
//    public List<object> ActionList = new List<object>();

//    //get all namelist


//    private void OnGUI()
//    {
//        //dialogue box
//        GUILayout.BeginHorizontal();
//        DialogueScroller = GUILayout.BeginScrollView(DialogueScroller);
//        GUILayout.BeginVertical();
//        //add differents actions
//        for (int i = 0; i < ActionCount; i++)
//        {
//            var action = ActionList[i];
//            GUILayout.BeginHorizontal();
//            //number
//            GUILayout.Label(i + ".", GUILayout.Width(15));

//            GUILayout.BeginHorizontal("box");
//            //check all actions type and set current UI
//            if (action is DialogueBox) OnDialogueBox(action as DialogueBox);
//            if (action is BrancheBox) OnBranches(action as BrancheBox);
//            if (action is CharcterSpriteInfos) OnCharacterInfos(action as CharcterSpriteInfos);
//            if (action is CGItem) OnCG(action as CGItem);
//            if (action is Delayer) OnDelayer(action as Delayer);
//            if (action is Audio) OnAudio(action as Audio);
//            if (action is Sound) OnSound(action as Sound);

//            if (GUILayout.Button("x", GUILayout.Width(20)))
//            {
//                ActionList.RemoveAt(i);
//                ActionCount -= 1;
//            }
//            GUILayout.EndHorizontal();
//            GUILayout.Space(10);
//            GUILayout.EndHorizontal();
//            GUILayout.Space(3);
//        }
//        GUILayout.EndVertical();
//        GUILayout.Space(5);
//        GUILayout.EndScrollView();

//        //action buttons
//        GUILayout.BeginVertical(GUILayout.Width(120));
//        if (GUILayout.Button("Character Sprite"))
//        {
//            ActionCount += 1;
//            ActionList.Add(new CharcterSpriteInfos());
//        }
//        if (GUILayout.Button("dialogue"))
//        {
//            ActionCount += 1;
//            ActionList.Add(new DialogueBox());
//        }
//        if (GUILayout.Button("Brahche"))
//        {
//            ActionCount += 1;
//            ActionList.Add(new BrancheBox());
//        }
//        GUILayout.BeginHorizontal();
//        if (GUILayout.Button("Background"))
//        {

//        }
//        if (GUILayout.Button("CG"))
//        {
//            ActionCount += 1;
//            ActionList.Add(new CGItem());
//        }
//        GUILayout.EndHorizontal();

//        GUILayout.Space(10);
//        if (GUILayout.Button("Delayer"))
//        {
//            ActionCount += 1;
//            ActionList.Add(new Delayer());
//        }

//        GUILayout.BeginHorizontal();
//        if (GUILayout.Button("Audio"))
//        {
//            ActionCount += 1;
//            ActionList.Add(new Audio());
//        }
//        if (GUILayout.Button("Sound"))
//        {
//            ActionCount += 1;
//            ActionList.Add(new Sound());
//        }
//        GUILayout.EndHorizontal();

//        GUILayout.EndVertical();

//        GUILayout.EndHorizontal();
//    }

//    void OnDialogueBox(DialogueBox dialogue)
//    {
//        GUILayout.BeginVertical();
//        //character name
//        dialogue.Name = EditorGUILayout.TextField("Character Name:", dialogue.Name);
//        GUILayout.Space(10);

//        GUILayout.BeginHorizontal();
//        dialogue.FontSize = EditorGUILayout.IntField("Font size:", dialogue.FontSize);
//        GUILayout.Space(15);
//        //dialogue text speed
//        dialogue.Speed = EditorGUILayout.IntSlider("Text speed", dialogue.Speed, 1, 5);
//        GUILayout.EndHorizontal();

//        //dialogue text box
//        GUILayout.Label("Dialogue");
//        dialogue.Dialogue = EditorGUILayout.TextArea(dialogue.Dialogue, GUILayout.Height(50));
//        GUILayout.EndVertical();
//    }

//    void OnBranches(BrancheBox branceBox)
//    {
//        //init branches
//        if (branceBox.Branches == null)
//        { branceBox.Branches = new List<string>() { "", "" }; }

//        GUILayout.BeginVertical();

//        branceBox.FontSize = EditorGUILayout.IntField("Font size:", branceBox.FontSize);
//        //dialogue text box
//        GUILayout.Label("Dialogue");
//        branceBox.Dialogue = EditorGUILayout.TextArea(branceBox.Dialogue, GUILayout.Height(50));

//        GUILayout.Space(5);
//        GUILayout.Label("Branches");
//        GUILayout.BeginVertical("Box");
//        for (int i = 0; i < branceBox.Branches.Count; i++)
//        {
//            GUILayout.BeginHorizontal();
//            var branche = branceBox.Branches[i];
//            branche = EditorGUILayout.TextField(i + ".", branche);
//            //delete branch
//            if (i >= 2)
//            {
//                if (GUILayout.Button("x", GUILayout.Width(20)))
//                {
//                    branceBox.Branches.RemoveAt(i);
//                }
//            }
//            GUILayout.EndHorizontal();
//        }

//        GUILayout.EndVertical();
//        //add new branche
//        if (GUILayout.Button("+"))
//        {
//            branceBox.Branches.Add("");
//        }
//        GUILayout.EndVertical();
//    }

//    void OnCharacterInfos(CharcterSpriteInfos chara)
//    {
//        GUILayout.BeginVertical();
//        //get all character
//        var list = ObjectInfoHelper.GetCharacterNames().ToArray();
//        //choice character
//        chara.Index = EditorGUILayout.Popup("Character", chara.Index, list);

//        //find selected character
//        string path = "Assets/GameSources/Characters/" + list[chara.Index] + ".prefab";
//        var selected = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
//        if (selected != null)
//        {
//            GUILayout.BeginHorizontal();
//            //get all sprites name
//            var spriteList = selected.GetComponentsInChildren<Transform>().Where(s=>s.GetComponent<CharaSpriteSetting>() != null).Select(s => s.name).ToArray();
//            //character sprite
//            chara.SpriteIndex = EditorGUILayout.Popup("Sprite", chara.SpriteIndex, spriteList);
//            GUILayout.Space(10);
//            //select character face if existe
//            var faceList = selected.transform.GetChild(chara.SpriteIndex).GetComponentsInChildren<Transform>().
//                            Where(f => f.GetComponent<CharaFaceSetting>() != null).
//                            Select(f => f.name).ToArray();
//            if (faceList.Length > 0) chara.FaceIndex = EditorGUILayout.Popup("Face", chara.FaceIndex, faceList);
            

//            GUILayout.EndHorizontal();
//        }
//        GUILayout.EndVertical();
//    }

//    void OnCG(CGItem cg)
//    {
//        //get all cg
//        var list = ObjectInfoHelper.GetCGsName().ToArray();

//        //selector for cg
//        GUILayout.BeginHorizontal();

//        cg.Index = EditorGUILayout.Popup("CG", cg.Index, list);

//        //load preview cg
//        string path = "Assets/GameSources/CGs/" + list[cg.Index] + ".jpg";
//        var imgPriveiw = AssetDatabase.LoadAssetAtPath(path, typeof(Sprite)) as Sprite;
//        if (imgPriveiw != null) GUILayout.Label(imgPriveiw.texture, GUILayout.Width(128), GUILayout.Height(72));
//        GUILayout.EndHorizontal();
//    }

//    void OnDelayer(Delayer delayer)
//    {
//        delayer.Delay = EditorGUILayout.FloatField("Delay",delayer.Delay);
//        GUILayout.Label("S");
//    }

//    void OnAudio(Audio audio)
//    {
//        //Choice audio
//        audio.MyAudio = EditorGUILayout.ObjectField("Audio source", audio.MyAudio, typeof(AudioClip), false) as AudioClip;
//        GUILayout.Space(10);
//        //audio volume
//        audio.Volume = EditorGUILayout.Slider("Volume", audio.Volume, 0, 1);
//    }

//    void OnSound(Sound sound)
//    {
//        GUILayout.BeginVertical();
//        //Choice audio
//        sound.MyAudio = EditorGUILayout.ObjectField("Sound source", sound.MyAudio, typeof(AudioClip), false) as AudioClip;

//        //sound tracks(6 tracks)
//        var list = new List<string>();
//        for(int i =0; i < 6; i++)
//        {
//            list.Add((i+1).ToString());
//        }

//        GUILayout.BeginHorizontal();
//        //audio volume
//        sound.Volume = EditorGUILayout.Slider("Volume", sound.Volume, 0, 1);
//        GUILayout.Space(10);
//        sound.Track = EditorGUILayout.Popup("Track",sound.Track, list.ToArray());
//        GUILayout.EndHorizontal();

//        GUILayout.EndVertical();
//    }
//}

