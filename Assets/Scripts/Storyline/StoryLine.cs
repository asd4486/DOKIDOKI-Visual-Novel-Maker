using DokiVnMaker.MyEditor.Items;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DokiVnMaker.MyEditor
{
    public class StoryLine : MonoBehaviour
    {
        [HideInInspector]
        //story json data file name
        public string DataFileName { get { return SceneManager.GetActiveScene().name + '-' + gameObject.name + "-story" + ".json"; } }
        public string ConnectionsDataFileName { get { return SceneManager.GetActiveScene().name + '-' + gameObject.name + "-connection" + ".json"; } }

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
                var actions = JsonConvert.DeserializeObject<StoryLineActions>(json);

                nodeList.Add(actions.StartPoint);
                foreach (var n in actions.CharcterSpriteInfoList) { nodeList.Add(n); }
                foreach (var n in actions.DialogBoxList) { nodeList.Add(n); }
                foreach (var n in actions.BrancheBoxList) { nodeList.Add(n); }
                foreach (var n in actions.BackgroundItemList) { nodeList.Add(n); }
                foreach (var n in actions.CGInfoItemList) { nodeList.Add(n); }
                foreach (var n in actions.DelayerList) nodeList.Add(n);
                foreach (var n in actions.AudioList) nodeList.Add(n);
                foreach (var n in actions.SoundList) nodeList.Add(n);
                foreach (var n in actions.CharacterOutInfoList) nodeList.Add(n);
                foreach (var n in actions.ChangeStoryLineList) nodeList.Add(n);
                foreach (var n in actions.ChangeSceneList) nodeList.Add(n);

                foreach (var n in nodeList) n.Initialize = true;

                nodeList = nodeList.OrderBy(n => n.Id).ToList();
            }

            return nodeList;
        }

        public List<Connection> OnLoadConnections()
        {
            var list = new List<Connection>();
            string filePath = Path.Combine(ValueManager.GameDataPath, ConnectionsDataFileName);

            if (File.Exists(filePath))
            {
                //find all json data
                string json = File.ReadAllText(filePath);
                list = JsonConvert.DeserializeObject<List<Connection>>(json);
                //string[] datas = json.Remove(json.Length - 1).Remove(0, 1).Split(new string[] { ",{" }, StringSplitOptions.RemoveEmptyEntries);
                //Debug.Log(json);
            }

            return list;
        }

        public void OnSaveData(StoryLineActions actions, List<Connection> connections)
        {
            var json = JsonConvert.SerializeObject(actions);
            File.WriteAllText(ValueManager.GameDataPath + DataFileName, json);

            var json2 = JsonConvert.SerializeObject(connections, Formatting.Indented);
            //Debug.Log
            File.WriteAllText(ValueManager.GameDataPath + ConnectionsDataFileName, json2);

        }
        #endregion
    }
}