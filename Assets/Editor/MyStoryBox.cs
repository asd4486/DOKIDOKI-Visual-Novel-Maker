using Editor.Items;
using Editor.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class MyStoryBox:EditorWindow
{
    [MenuItem("DokiDoki Maker/Story box")]
    static void Init()
    {
        MyStoryBox window = (MyStoryBox)EditorWindow.GetWindow(typeof(MyStoryBox), false, "Story box");
    }

    private Vector2 DialogueScroller { get; set; }
    private int ActionCount { get; set; }
    public List<object> ActionList = new List<object>();

    //get all namelist


    private void OnGUI()
    {
        //dialogue box
        GUILayout.BeginHorizontal();
        DialogueScroller = GUILayout.BeginScrollView(DialogueScroller);
        GUILayout.BeginVertical();
        //add differents actions
        for (int i = 0; i < ActionCount; i++)
        {
            var action = ActionList[i];
            GUILayout.BeginHorizontal();
            //number
            GUILayout.Label(i + ".", GUILayout.Width(15));

            GUILayout.BeginHorizontal("box");
            if (action is DialogueBox) OnDialogueBox(action as DialogueBox);
            if (action is StoryBoxCharacterItem) OnCharacterSprite(action as StoryBoxCharacterItem);
            if (action is Delayer) OnDelayer(action as Delayer);

            if (GUILayout.Button("x", GUILayout.Width(20)))
            {
                ActionList.RemoveAt(i);
                ActionCount -= 1;
            }
            GUILayout.EndHorizontal();
            GUILayout.Space(10);
            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
        GUILayout.EndScrollView();

        //action buttons
        GUILayout.BeginVertical(GUILayout.Width(100));
        if (GUILayout.Button("dialogue"))
        {
            ActionCount += 1;
            ActionList.Add(new DialogueBox());
        }
        if (GUILayout.Button("Character Sprite"))
        {
            ActionCount += 1;
            ActionList.Add(new StoryBoxCharacterItem());
        }
        if (GUILayout.Button("Delayer"))
        {
            ActionCount += 1;
            ActionList.Add(new Delayer());
        }

        GUILayout.EndVertical();

        GUILayout.EndHorizontal();
    }

    void OnDialogueBox(DialogueBox dialogue)
    {
        GUILayout.BeginVertical();
        //character name
        dialogue.Name = EditorGUILayout.TextField("Character Name:", dialogue.Name);
        GUILayout.Space(10);
        dialogue.FontSize = EditorGUILayout.IntField("Font size:", dialogue.FontSize);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Text speed");
        dialogue.Speed = EditorGUILayout.Slider(dialogue.Speed, 1, 10);
        GUILayout.EndHorizontal();

        GUILayout.Label("Dialogue");
        dialogue.Dialogue = EditorGUILayout.TextArea(dialogue.Dialogue, GUILayout.Height(50));
        GUILayout.EndVertical();
    }

    void OnCharacterSprite(StoryBoxCharacterItem chara)
    {
        var list = CharacterInfoHelper.GetCharacterNames();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Character");
        chara.Index = EditorGUILayout.Popup(chara.Index, list);
        GUILayout.EndHorizontal();
    }

    void OnDelayer(Delayer delayer)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Delay");
        delayer.Delay = EditorGUILayout.FloatField(delayer.Delay);
        GUILayout.Label("S");
        GUILayout.EndHorizontal();
    }
}

