using UnityEngine;
using System.Collections;
using UnityEditor;
using TreeEditor;
using System.Collections.Generic;

[CustomEditor(typeof(StoryLine))]
public class StoryLineEditor : Editor
{
    StoryLine StoryLine { get; set; }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        //get editor target
        StoryLine = (StoryLine)target;

        //add new action
        if (GUILayout.Button("Add new action"))
        {
            StoryLineMenu.InitWindow(this);
            //myScript.BuildObject();
        }
    }

    private Vector2 DialogueScroller { get; set; }
    private int ActionCount { get; set; }
    public List<object> ActionList = new List<object>();

    public void addNewAction()
    {

    }
}
