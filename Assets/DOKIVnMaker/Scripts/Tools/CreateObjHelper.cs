using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DokiVnMaker.Tools
{
    public static class CreateObjHelper
    {
        public static Object SaveGameobj(Object o, string localPath)
        {
            //if (AssetDatabase.LoadAssetAtPath(localPath, typeof(Object)))
            //{
            //    //Create dialog to ask if User is sure they want to overwrite existing prefab
            //    if (EditorUtility.DisplayDialog("Are you sure?",
            //            "Object is already exists. Do you want to overwrite it?",
            //            "Yes",
            //            "No"))
            //    //If the user presses the yes button, create the Prefab
            //    {
            //        return CreateNewObj(o, localPath, true);
            //    }
            //}
            //If the name doesn't exist, create the new Prefab

               return CreateNewObj(o, localPath, false);
            
        }

        private static Object CreateNewObj(Object _object, string localPath, bool isReplace)
        {
            //if (isReplace)
            //{
            //    AssetDatabase.DeleteAsset(localPath);
            //}

            if(_object is GameObject)
            {
                PrefabUtility.SaveAsPrefabAsset(_object as GameObject, localPath);
            }
            else
            {
                AssetDatabase.CreateAsset(_object, localPath);
                AssetDatabase.SaveAssets();
            }
           
            return _object;
        }
    }
}