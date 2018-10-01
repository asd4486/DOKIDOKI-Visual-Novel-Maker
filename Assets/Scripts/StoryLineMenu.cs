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
        //action buttons
        if (GUILayout.Button("Character Sprite"))
        {
            CurrentEditor.addNewAction();
            MyWindow.Close();
        }
        if (GUILayout.Button("dialogue"))
        {
            MyWindow.Close();
        }
        if (GUILayout.Button("Brahche"))
        {
            MyWindow.Close();
        }
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Background"))
        {
            MyWindow.Close();
        }
        if (GUILayout.Button("CG"))
        {
            MyWindow.Close();
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(10);
        if (GUILayout.Button("Delayer"))
        {
            MyWindow.Close();
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Audio"))
        {
            MyWindow.Close();
        }
        if (GUILayout.Button("Sound"))
        {
            MyWindow.Close();
        }
        GUILayout.EndHorizontal();
    }
}
