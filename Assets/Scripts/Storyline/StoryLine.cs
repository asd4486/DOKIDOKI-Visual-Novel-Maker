using NodeEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryLine : MonoBehaviour
{
    [HideInInspector]
    //story json data file name
    public string DataFileName { get { return SceneManager.GetActiveScene().name + '-' + gameObject.name+ "-story" + ".json"; } }
    public string ConnectionsDataFileName { get { return SceneManager.GetActiveScene().name + '-' + gameObject.name+ "-connection" + ".json"; } }

    #region save load data
    public List<NodeBase> OnLoadNodes()
    {
        var nodeList = new List<NodeBase>();

        // Path.Combine combines strings into a file path
        // Application.StreamingAssets points to Assets/StreamingAssets in the Editor, and the StreamingAssets folder in a build
        string filePath = Path.Combine(ValueManager.GameDataPath, DataFileName);

        if (File.Exists(filePath))
        {
            //find all json data
            string json = File.ReadAllText(filePath);
            string[] datas = json.Remove(json.Length - 1).Remove(0, 1).Split(new string[] { ",{" }, StringSplitOptions.RemoveEmptyEntries);
            //Debug.Log(json);
            //Debug.Log(JsonHelper.FromJson<NodeBase>(json));
            nodeList = new List<NodeBase>();

            for (var i = 0; i < datas.Length; i++)
            {
                var d = datas[i];
                if (d[0] != '{') { d = "{" + d; }

                var type = JsonUtility.FromJson<NodeBase>(d);
                //check action type and load current class
                var newAction = new NodeBase();
                switch (type.ActionType)
                {
                    case ActionTypes.Start:
                        newAction = JsonUtility.FromJson<EditorStartPoint>(d);
                        break;
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
                nodeList.Add(newAction);
            }
        }

        return nodeList;
        //else
        //{
        //    Debug.Log("No data for this story line!");
        //}
    }

    public List<Connection> OnLoadConnections()
    {
        var list = new List<Connection>();
        string filePath = Path.Combine(ValueManager.GameDataPath, ConnectionsDataFileName);

        if (File.Exists(filePath))
        {
            //find all json data
            string json = File.ReadAllText(filePath);
            list = JsonHelper.FromJson<Connection>(json).ToList();
            //string[] datas = json.Remove(json.Length - 1).Remove(0, 1).Split(new string[] { ",{" }, StringSplitOptions.RemoveEmptyEntries);
            //Debug.Log(json);
        }

        return list;
    }

    public void OnSaveData(List<NodeBase> actionList, List<Connection> connections)
    {
        var json = JsonHelper.ToJson(actionList);
        File.WriteAllText(ValueManager.GameDataPath + DataFileName, json);

        var json2 = JsonHelper.ToJson(connections.ToArray());
        File.WriteAllText(ValueManager.GameDataPath + ConnectionsDataFileName, json2);
        //BinaryFormatter formatter = new BinaryFormatter();

        //FileStream saveFile = File.Create(ValueManager.GameDataPath + DataFileName);

        //formatter.Serialize(saveFile, actionList);
        //saveFile.Close();
    }
    #endregion
}
