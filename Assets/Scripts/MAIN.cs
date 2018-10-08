using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MAIN : MonoBehaviour {

    public GameObject StartStoryLine;

    //step of storyline
    private int Step = 0;
    //actions of story line
    private List<object> StoryActionList;

    // Use this for initialization
    void Start () {
		
        //auto set start story line if no story line is not set 
        if(StartStoryLine == null)
        {
            var storyLine = GameObject.FindObjectsOfType<StoryLine>();
            if (storyLine.Length < 0) return;
            StartStoryLine = storyLine[0].gameObject;
        }

        if(StartStoryLine != null)
        {
            StoryActionList = StartStoryLine.GetComponent<StoryLine>().OnLoadData();
        }
        PlayStoryLine();
    }

    // Update is called once per frame
    //void Update () {

    //}

    private void PlayStoryLine()
    {
        var action = StoryActionList[Step];
        if (action is CharcterSpriteInfos) ShowCharacter(action as CharcterSpriteInfos);
    }

    private void ShowCharacter(CharcterSpriteInfos chara)
    {
        //find character object in scene
        var o = GameObject.Find(chara.Name);
        //if null instantiate new chara object in scene
        if (o == null)
        {
            o = AssetDatabase.LoadAssetAtPath(chara.Path, typeof(GameObject)) as GameObject;
            //default left
            var newChara = Instantiate(o, new Vector3(-5, 0, 0), Quaternion.identity);
            newChara.name = o.name;
            newChara.transform.parent = GameObject.Find("Character_group").transform;
        }

        o.SetActive(true);
    }
}
