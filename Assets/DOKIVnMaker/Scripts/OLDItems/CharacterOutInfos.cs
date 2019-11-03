//using DokiVnMaker.Tools;
//using System;
//using System.Linq;
//using UnityEditor;
//using UnityEngine;

//namespace DokiVnMaker.MyEditor.Items
//{
//    [Serializable]
//    public class CharacterOutInfos : NodeBase
//    {
//        [NonSerialized]
//        public int Index;
//        public string Path;

//        //get chara name
//        public string Name { get { return Path.Split('/')[Path.Split('/').Length - 1].Replace(".prefab", ""); } }

//        public bool IsWait = true;

//        public CharacterOutInfos()
//        {
//            ActionType = ActionTypes.CharacterOutInfos;
//        }

//        public override void Draw()
//        {
//            GUILayout.BeginArea(myRect, Title, Style);
//            GUILayout.Space(5);
//            GUILayout.BeginHorizontal();
//            GUILayout.Space(SpacePixel);
//            GUILayout.FlexibleSpace();
//            GUILayout.BeginVertical();
//            GUILayout.Space(SpacePixel);

//            //get all character
//            var list = ObjectInfoHelper.GetCharacterNames();

//            //set character index if initialize request
//            if (Initialize)
//            {
//                //find origin object
//                var origin = AssetDatabase.LoadAssetAtPath(Path, typeof(GameObject)) as GameObject;

//                if (origin != null)
//                {
//                    //set index
//                    Index = list.IndexOf(list.Where(c => c == origin.name).FirstOrDefault());
//                }
//                Initialize = false;
//            }

//            //choose character
//            GUILayout.BeginHorizontal();
//            GUILayout.Label("Character", WhiteTxtStyle, GUILayout.Width(LabelWidth));
//            Index = EditorGUILayout.Popup(Index, list.ToArray());
//            GUILayout.EndHorizontal();

//            //find selected character
//            string path = ValueManager.CharaPath + list[Index] + ".prefab";
//            var selected = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject)) as GameObject;
//            if (selected != null)
//            {
//                //set character path
//                Path = path;
//            }

//            //is wait for character disappear
//            GUILayout.BeginHorizontal();
//            GUILayout.Label("Is wait", WhiteTxtStyle, GUILayout.Width(LabelWidth));
//            IsWait = EditorGUILayout.Toggle(IsWait);
//            GUILayout.EndHorizontal();

//            GUILayout.EndVertical();
//            GUILayout.Space(SpacePixel);
//            GUILayout.EndHorizontal();
//            GUILayout.EndArea();

//            InPoint.Draw();
//            OutPoint.Draw();

//            base.Draw();
//        }

//        public override NodeBase Clone(Vector2 pos, int newId)
//        {
//            var clone = new CharacterSpriteInfos()
//            {
//                ActionType = ActionTypes.CharacterSpriteInfos,
//                Initialize = true,
//                Path = Path,
//                IsWait = IsWait
//            };

//            clone.Init(pos, myRect.size, Style, SelectedNodeStyle, InPoint.Style,
//                OutPoint.Style, InPoint.OnClickConnectionPoint, OutPoint.OnClickConnectionPoint,
//                OnCopyNode, OnRemoveNode, newId);

//            return clone;
//        }
//    }
//}