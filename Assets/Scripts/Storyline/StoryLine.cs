using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryLine : MonoBehaviour {

    [HideInInspector]
    //story json data file name
    public string DataFileName { get; set; }

    private void Awake()
    {
        //game data file name
        DataFileName = SceneManager.GetActiveScene().name + '-' + gameObject.name + ".json";
    }

    #region save load data
    public List<object> OnLoadData()
    {
        var actionList = new List<object>();

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
                if (d[0] != '{') { d = "{" + d; }

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
                    case ActionTypes.BackgroundItem:
                        newAction = JsonUtility.FromJson<BackgroundItem>(d);
                        //initialize setting
                        (newAction as BackgroundItem).Initialize = true;
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
                actionList.Add(newAction);
            }
            // Pass the json to JsonUtility, and tell it to create a GameData object from it
            //var loadedData = JsonHelper.FromJson<List<dynamic>>(dataAsJson);
            //Debug.Log(loadedData.Count);
        }

        return actionList;
        //else
        //{
        //    Debug.Log("No data for this story line!");
        //}
    }

    public void OnSaveData(List<object> actionList)
    {
        var json = JsonHelper.ToJson(actionList);
        //Debug.Log(json);

        File.WriteAllText(ValueManager.GameDataPath + "/" + DataFileName, json);
    }
    #endregion
    //   // Use this for initialization
    //void Start()
    //{
    //}

    //// Update is called once per frame
    //void Update () {

    //}
}
