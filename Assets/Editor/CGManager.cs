using Editor.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CGManager : EditorWindow {

    [MenuItem("DokiDoki Maker/CG manager")]
    static void Init()
    {
        CGManager window = (CGManager)EditorWindow.GetWindow(typeof(CGManager), false, "CG manager");
    }

    Vector2 Scroller { get; set; }

    public int CGCount = 0;
    public List<CGItem> CGList = new List<CGItem>(); 

    private void OnGUI()
    {
        Scroller = GUILayout.BeginScrollView(Scroller);
        for (int i=0; i < CGCount; i++)
        {
            var myCG = CGList[i];
            GUILayout.BeginHorizontal("box");

            GUILayout.BeginVertical();
            //auto name if string is empty
            if (string.IsNullOrEmpty(myCG.Name)) myCG.Name = "cg_" + i;

            myCG.Name = EditorGUILayout.TextField("CG name:", myCG.Name);
            //body sprite
            myCG.CG = EditorGUILayout.ObjectField("CG", myCG.CG, typeof(Sprite), true) as Sprite;
            GUILayout.EndVertical();

            GUILayout.Space(10);
            if (GUILayout.Button("x", GUILayout.Width(20)))
            {
                CGList.RemoveAt(i);
                CGCount -= 1;
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndScrollView();

        //add new cg
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("+"))
        {
            CGList.Add(new CGItem());
            CGCount += 1;
        }
        if (GUILayout.Button("-") && CGCount > 0)
        {
            CGCount -= 1;
            CGList.RemoveAt(CGCount);
        }
        GUILayout.EndHorizontal();
        //auto rename cgs
        if (GUILayout.Button("Auto rename"))
        {
            RenameCGs();
        }

        GUILayout.Space(10);
        GUILayout.BeginHorizontal(GUILayout.Height(30));
        if (GUILayout.Button("Save", GUILayout.Height(25)))
        {

        }
        if (GUILayout.Button("Discard", GUILayout.Height(25)))
        {

        }
        GUILayout.EndHorizontal();
    }

    void RenameCGs()
    {
        for (int i = 0; i < CGCount; i++)
        {
            var myCG = CGList[i];
            var cgNames = myCG.Name.Split('_');
            int result = 0;
            if (myCG.Name.Contains("cg_") && cgNames.Length == 2 && Int32.TryParse(cgNames[1], out result))
                myCG.Name = "cg_" + i;
        }
    }
}
