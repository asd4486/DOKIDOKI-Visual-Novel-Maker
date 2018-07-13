using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Editor.Tools
{
    public static class CreateObjHelper
    {
        public static void SaveGameobj(GameObject o, string localPath)
        {
            if (AssetDatabase.LoadAssetAtPath(localPath, typeof(GameObject)))
            {
                //Create dialog to ask if User is sure they want to overwrite existing prefab
                if (EditorUtility.DisplayDialog("Are you sure?",
                        "The character already exists. Do you want to overwrite it?",
                        "Yes",
                        "No"))
                //If the user presses the yes button, create the Prefab
                {
                    CreateNewObj(o, localPath);
                }
            }
            //If the name doesn't exist, create the new Prefab
            else
            {
                CreateNewObj(o, localPath);
            }
        }

        private static void CreateNewObj(GameObject obj, string localPath)
        {
            //Create a new prefab at the path given
            Object prefab = PrefabUtility.CreatePrefab(localPath, obj);
            PrefabUtility.ReplacePrefab(obj, prefab, ReplacePrefabOptions.ConnectToPrefab);
        }
    }
}
