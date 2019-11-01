using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace DokiVnMaker.MyEditor
{
    public class DokidokiTools : MonoBehaviour
    {
        [MenuItem("DokiDoki VN Maker/Game/New Game")]
        static void CreateNewGame()
        {
            //find game gameobject
            var path = ValueManager.GameSourcesPath + "Game/DokiDoki_VN_Game.prefab";
            var game = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;

            //check if game existe in this scene
            var oldgame = GameObject.FindGameObjectsWithTag("dokidoki_vn_game");
            if (oldgame.Length > 0)
            {
                //return if existe
                EditorUtility.DisplayDialog("OOPS!!", "Game is already created !!", "OK");
                return;
            }
            else
            {
                //create new game
                var newGame = Instantiate(game, Vector3.zero, Quaternion.identity);
                //rename new game
                newGame.name = game.name;
            }
        }
    }
}