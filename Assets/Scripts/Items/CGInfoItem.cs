using DokiVnMaker.Tools;
using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DokiVnMaker.MyEditor.Items
{
    [Serializable]
    public class CGInfoItem : NodeBase
    {
        //index for story box selector
        [NonSerialized]
        public int Index;
        public string Path;

        public bool IsWait = true;

        public CGInfoItem()
        {
            ActionType = ActionTypes.CGInfoItem;
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

            //get all cg
            var list = ObjectInfoHelper.GetCGsName();

            if (Initialize)
            {
                //find origin object
                var origin = AssetDatabase.LoadAssetAtPath(Path, typeof(Sprite)) as Sprite;
                if (origin != null)
                {
                    //set index
                    Index = list.IndexOf(list.Where(c => c == origin.name).FirstOrDefault());

                }
                Initialize = false;
            }

            //selector for cg
            GUILayout.BeginHorizontal();
            GUILayout.Label("CG", WhiteTxtStyle, GUILayout.Width(LabelWidth));
            Index = EditorGUILayout.Popup(Index, list.ToArray());
            GUILayout.EndHorizontal();
            //set cg path
            Path = ValueManager.CGPath + list[Index] + ".jpg";

            //is wait for CG appear
            GUILayout.BeginHorizontal();
            GUILayout.Label("Is wait", WhiteTxtStyle, GUILayout.Width(LabelWidth));
            IsWait = EditorGUILayout.Toggle(IsWait);
            GUILayout.EndHorizontal();

            //GUILayout.BeginHorizontal();
            //GUILayout.FlexibleSpace();
            //load preview cg
            string path = ValueManager.CGPath + list[Index] + ".jpg";
            var imgPriveiw = AssetDatabase.LoadAssetAtPath(path, typeof(Sprite)) as Sprite;
            if (imgPriveiw != null) GUILayout.Label(imgPriveiw.texture, GUILayout.Width(200), GUILayout.Height(113));
            //GUILayout.EndHorizontal();

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
            var clone = new CGInfoItem()
            {
                Initialize = true,
                Path = Path,
                IsWait = IsWait,
            };

            clone.Init(pos, Rect.width, Rect.height, Style, SelectedNodeStyle, InPoint.Style,
                OutPoint.Style, InPoint.OnClickConnectionPoint, OutPoint.OnClickConnectionPoint,
                OnCopyNode, OnRemoveNode, newId);

            return clone;
        }
    }
}