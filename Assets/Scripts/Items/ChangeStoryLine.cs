using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DokiVnMaker.MyEditor.Items
{
    [Serializable]
    public class ChangeStoryLine : NodeBase
    {
        public string Name;
        [NonSerialized]
        public int Index;

        public ChangeStoryLine() { }

        public ChangeStoryLine(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle,
            GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> onClickInPoint, Action<ConnectionPoint> onClickOutPoint,
            Action<NodeBase> onClickCopyNode, Action<NodeBase> onClickRemoveNode, int id)
        {
            ActionType = ActionTypes.ChangeStoryLine;
            Init(position, width, height, nodeStyle, selectedStyle, inPointStyle, outPointStyle, onClickInPoint, onClickOutPoint, onClickCopyNode, onClickRemoveNode, id);
        }

        public override void Draw()
        {
            GUILayout.BeginArea(Rect, Title, Style);
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(SpacePixel);
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical();
            GUILayout.Space(SpacePixel);

            //find all story line in main game
            GameObject game = GameObject.FindGameObjectWithTag("dokidoki_vn_game");
            var list = game.GetComponentsInChildren<StoryLine>().ToList();

            //return if can't find any story line
            if (list.Count < 1)
            {
                GUILayout.Label("OOOps!Can't find any story line!", WhiteTxtStyle, GUILayout.Width(LabelWidth));
                GUILayout.Label("Please add a new one :)", WhiteTxtStyle, GUILayout.Width(LabelWidth));
                return;
            }

            //set story line index if initialize request
            if (Initialize)
            {
                //find origin object
                var origin = list.Where(s => s.name == Name && s.tag == "doki-storyline").FirstOrDefault();

                if (origin != null)
                {
                    //set index
                    Index = list.IndexOf(origin);
                }
                Initialize = false;
            }

            //get all story lines names
            var nameList = list.Select(s => s.gameObject.name).ToArray();

            GUILayout.BeginHorizontal();
            GUILayout.Label("Story line", WhiteTxtStyle, GUILayout.Width(LabelWidth));
            Index = EditorGUILayout.Popup(Index, nameList);
            GUILayout.EndHorizontal();

            GUILayout.EndVertical();
            GUILayout.Space(SpacePixel);
            GUILayout.EndHorizontal();
            GUILayout.EndArea();

            InPoint.Draw();
            OutPoint.Draw();

            base.Draw();
        }

        public override NodeBase Clone(Vector2 pos, int newId)
        {
            var clone = new ChangeStoryLine(pos, Rect.width, Rect.height, Style, SelectedNodeStyle, InPoint.Style,
                OutPoint.Style, InPoint.OnClickConnectionPoint, OutPoint.OnClickConnectionPoint,
                OnCopyNode, OnRemoveNode, newId)
            {
                Initialize = true,
                Name = Name,
            };

            return clone;
        }
    }
}