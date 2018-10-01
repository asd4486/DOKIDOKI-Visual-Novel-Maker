using Editor.Items;
using Editor.Tools;
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

    public int CGCount = 0;
    public List<CGItem> CGList = new List<CGItem>();
    private bool RefreshCGList = true;
    Vector2 Scroller { get; set; }

    private void OnGUI()
    {
        //import cg list
        if (RefreshCGList){
            //refresh list
            CGCount = 0;
            CGList = new List<CGItem>();

            //find all cg names
            var list = ObjectInfoHelper.GetCGsName();
            foreach (var name in list)
            {
                CGCount += 1;
                //get cg sprite
                string path = "Assets/GameSources/CGs/" + name + ".jpg";
                var sprite = AssetDatabase.LoadAssetAtPath(path, typeof(Sprite)) as Sprite;

                CGList.Add(new CGItem { OldName = name, Name = name, CG = sprite });
            }
            RefreshCGList = false;
        }
        //refresh cg list
        if (GUILayout.Button("Refresh CG list"))
        {
            RefreshCGList = true;
        }

        //cg count
        GUILayout.Label("CG count: " + CGCount, GUILayout.Height(15));
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

        GUILayout.Space(5);
        GUILayout.BeginHorizontal(GUILayout.Height(30));
        GUILayout.Space(5);
        //save cgs
        if (GUILayout.Button("Save", GUILayout.Height(25)))
        {
            SaveCGList();
        }
        //reload cgs
        if (GUILayout.Button("Discard", GUILayout.Height(25)))
        {
            RefreshCGList = true;
        }
        GUILayout.Space(5);
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

    void SaveCGList()
    {
        //check if cg list is empty
        if (CGCount <= 0)
        {
            EditorUtility.DisplayDialog("OOOOOps", "No CG Create :(", "OK");
            return;
        }

        for (int i = 0; i < CGCount; i++)
        {
            var CGInfo = CGList[i];
            //create CG
            if (CGInfo.CG != null)
            {
                //get asset path
                string directory = AssetDatabase.GetAssetPath(CGInfo.CG);
                //save path
                string path = "Assets/GameSources/CGs/" + CGInfo.Name + ".jpg";

                //if CG already existe, replace the cg
                if (AssetDatabase.LoadAssetAtPath(path, typeof(Sprite)) != null)
                {
                    //replace if cg has changed
                    if(directory != path) FileUtil.ReplaceFile(directory, path);
                }
                else
                {
                    //get old cg
                    string oldPath = "Assets/GameSources/CGs/" + CGInfo.OldName + ".jpg";
                    var oldCg = AssetDatabase.LoadAssetAtPath(oldPath, typeof(Sprite)) as Sprite;
                    //change oldCg name if cg already existed
                    if (oldCg != null)
                    {
                        var fullPath = Application.dataPath + "/GameSources/CGs/";
                        System.IO.File.Move(fullPath + CGInfo.OldName+".jpg", fullPath + CGInfo.Name + ".jpg");
                    }
                    else
                    {
                        //save new cg
                        FileUtil.CopyFileOrDirectory(directory, path);
                    }
                    
                }
                
            }
        }

        //Delete removed cgs from list
        //get all cgs name
        var existeCGList = ObjectInfoHelper.GetCGsName();
        //delete list
        var deleteCGList = ObjectInfoHelper.GetCGsName();
        foreach (var name in existeCGList)
        {
            //if cg exite in cg manager ,remove it from delete list
            foreach(var cg in CGList)
            {
                if (name == cg.Name) deleteCGList.Remove(name);
            }
        }

        //delete removed cgs
        foreach(var name in deleteCGList)
        {
            string path = "Assets/GameSources/CGs/" + name + ".jpg";
            var sprite = AssetDatabase.LoadAssetAtPath(path, typeof(Sprite)) as Sprite;
            if (sprite != null) FileUtil.DeleteFileOrDirectory(path);
        }

        EditorUtility.DisplayDialog("WOAH!!", "CGs saved!!", "OK");
    }
}
