using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class JsonHelper
{
    //public static T[] FromJson<T>(string json)
    //{
    //    Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
    //    return wrapper.Items;
    //}

    public static List<T> FromJson<T>(string json)
    {
        List<T> list = new List<T>();
        //var jsonobj = JsonUtility.FromJson<object>(json);
        Debug.Log(JsonUtility.FromJson<Wrapper<T>>(json).Items);
        return list;
    }

    public static string ToJson<T>(List<T> list)
    {
        string json = "";
        List<string> jsonList = new List<string>();
        //action list to string list
        foreach (var a in list)
        {
            var actionjson = JsonUtility.ToJson(a);
            jsonList.Add(actionjson);
        }
        //write json and save
        json = "["+ string.Join(",", jsonList.ToArray())+ "]";

        return json;
        //Wrapper<T> wrapper = new Wrapper<T>();
        //wrapper.Items = array;
        //return JsonUtility.ToJson(wrapper);
    }

    //public static string ToJson<T>(T[] array, bool prettyPrint)
    //{
    //    Wrapper<T> wrapper = new Wrapper<T>();
    //    wrapper.Items = array;
    //    return JsonUtility.ToJson(wrapper, prettyPrint);
    //}

    [Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}