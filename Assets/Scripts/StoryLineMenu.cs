using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

class StoryLineMenu:EditorWindow
{
    static StoryLineMenu MyWindow { get; set; }
    //current story line editor who call this window
    static StoryLineEditor CurrentEditor { get; set; }

    public static void InitWindow(StoryLineEditor editor)
    {
        //set curretn editor
        CurrentEditor = editor;
        //open window
        Init();
    }

    static void Init()
    {
        MyWindow = (StoryLineMenu)EditorWindow.GetWindow(typeof(StoryLineMenu), true, "Story actions");
        //MyWindow = window;
        MyWindow.Show();
    }

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        //action buttons
        if (GUILayout.Button("Character Sprite"))
        {
            CurrentEditor.AddNewAction(new CharcterSpriteInfos());
            MyWindow.Close();
        }
        if(GUILayout.Button("Character out"))
        {
            MyWindow.Close();
        }
        GUILayout.EndHorizontal();

        if (GUILayout.Button("dialogue"))
        {
            CurrentEditor.AddNewAction(new DialogBox());
            MyWindow.Close();
        }
        if (GUILayout.Button("Brahche"))
        {
            CurrentEditor.AddNewAction(new BrancheBox());
            MyWindow.Close();
        }
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Background"))
        {
            MyWindow.Close();
        }
        if (GUILayout.Button("CG"))
        {
            CurrentEditor.AddNewAction(new CGInfoItem());
            MyWindow.Close();
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(10);
        if (GUILayout.Button("Delayer"))
        {
            CurrentEditor.AddNewAction(new Delayer());
            MyWindow.Close();
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Audio"))
        {
            CurrentEditor.AddNewAction(new Audio());
            MyWindow.Close();
        }
        if (GUILayout.Button("Sound"))
        {
            CurrentEditor.AddNewAction(new Sound());
            MyWindow.Close();
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(5);
    }
}
