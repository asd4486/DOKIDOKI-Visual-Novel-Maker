using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace DokiVnMaker
{
    public class DokidokiTools:Editor
    {
        [MenuItem("DokiDoki VN Maker/New Game")]
        static void CreateNewGame()
        {
            //find game gameobject
            var game = Resources.Load("DokiDoki_VN_Game") as GameObject;

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