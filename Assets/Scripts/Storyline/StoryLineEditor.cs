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
    StoryLine StoryLine { get; set; }
    private Vector2 DialogueScroller { get; set; }

    private void OnEnable()
    {
        //get editor target
        StoryLine = (StoryLine)target;
        //game data file name
        StoryLine.DataFileName = SceneManager.GetActiveScene().name + '-' + StoryLine.gameObject.name + ".json";
       
    }

    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Edit"))
        {
            StoryLineNodeEditor.InitWindow(StoryLine);
        }

        //DrawDefaultInspector();

        //DialogueScroller = GUILayout.BeginScrollView(DialogueScroller, GUILayout.MinHeight(0), GUILayout.MaxHeight(500));

        ////add differents actions
        //for (int i = 0; i < ActionList.Count; i++)
        //{
        //    var action = ActionList[i];
        //    GUILayout.BeginHorizontal();
        //    //number
        //    GUILayout.Label(i + ".", GUILayout.Width(15));

        //    GUILayout.BeginVertical("box");

        //    GUILayout.BeginHorizontal();
        //    //action title
        //    var title = "";
        //    if (action is DialogBox) title = "Dialog";
        //    if (action is BrancheBox) title = "Branche";
        //    if (action is CharcterSpriteInfos) title = "Character sprite";
        //    if (action is BackgroundItem) title = "Background";
        //    if (action is CGInfoItem) title = "CG";
        //    if (action is Delayer) title = "Delayer";
        //    if (action is Audio) title = "Background music";
        //    if (action is Sound) title = "Sound";
        //    GUILayout.Label(title, EditorStyles.boldLabel);

        //    GUILayout.FlexibleSpace();
        //    //delete button
        //    if (GUILayout.Button("x", GUILayout.Width(20)))
        //    {
        //        ActionList.RemoveAt(i);
        //    }
        //    GUILayout.EndHorizontal();
        //    GUILayout.Space(3);

        //    //check all actions type and set current UI
        //    if (action is DialogBox) OnDialogueBox(action as DialogBox);
        //    if (action is BrancheBox) OnBranches(action as BrancheBox);
        //    if (action is CharcterSpriteInfos) OnCharacterInfos(action as CharcterSpriteInfos);
        //    if (action is BackgroundItem) OnBackground(action as BackgroundItem);
        //    if (action is CGInfoItem) OnCG(action as CGInfoItem);
        //    if (action is Delayer) OnDelayer(action as Delayer);
        //    if (action is Audio) OnAudio(action as Audio);
        //    if (action is Sound) OnSound(action as Sound);

        //    GUILayout.EndVertical();
        //    GUILayout.Space(10);
        //    GUILayout.EndHorizontal();
        //    //GUILayout.Space(3);
        //}
        //GUILayout.Space(5);
        //GUILayout.EndScrollView();

        ////add new action
        //if (GUILayout.Button("Add new action"))
        //{
        //    StoryLineMenu.InitWindow(this);
        //    //myScript.BuildObject();
        //}

        //GUILayout.Space(5);
        //GUILayout.BeginHorizontal();
        ////save data
        //if (GUILayout.Button("Save"))
        //{
        //    StoryLine.OnSaveData(ActionList);
        //}
        ////discard change
        //if (GUILayout.Button("Discard"))
        //{
        //   ActionList = StoryLine.OnLoadData();
        //}
        //GUILayout.EndHorizontal();
    }

    private void OnDisable()
    {     
        //var oldList = StoryLine.OnLoadData();
        ////check story line is changed or not
        //bool listChanged = false;
        //if (oldList.Count != ActionList.Count) { listChanged = true; }
        //else
        //{
        //    for(int i = 0; i< ActionList.Count; i++) {
        //        var a = ActionList[i];
        //        var old = oldList[i];
        //        if (!a.Equals(old)) listChanged = true;
        //    }
        //}

        //if (!listChanged) return;
        ////show save alert when story line have changed but not saved
        //bool IsSave = EditorUtility.DisplayDialog("Story line has not been Saved", "Do you want to save the change?", "Yes", "No");
        //if (IsSave) { StoryLine.OnSaveData(ActionList); }
    }
  
}
