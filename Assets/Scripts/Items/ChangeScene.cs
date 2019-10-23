using System;
using UnityEditor;
using UnityEngine;

namespace DokiVnMaker.MyEditor.Items
{
    [Serializable]
    public class ChangeScene : NodeBase
    {
        public string path;
        public string sceneName { get { return path.Split('/')[path.Split('/').Length - 1].Replace(".unity", ""); } }

        [NonSerialized]
        public SceneAsset Scene;

        public ChangeScene()
        {
            ActionType = ActionTypes.ChangeScene;
        }

        public override void Draw()
        {
            GUILayout.BeginArea(myRect, Title, Style);
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(SpacePixel);
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical();
            GUILayout.Space(SpacePixel);

            //initialize
            if (Initialize)
            {
                //find origin object
                var origin = AssetDatabase.LoadAssetAtPath(path, typeof(SceneAsset)) as SceneAsset;

                if (origin != null)
                {
                    //set Scene
                    Scene = origin;
                }
                Initialize = false;
            }

            //Choose image
            GUILayout.BeginHorizontal();
            GUILayout.Label("Scene", WhiteTxtStyle, GUILayout.Width(LabelWidth));
            Scene = EditorGUILayout.ObjectField(Scene, typeof(SceneAsset), false) as SceneAsset;
            GUILayout.EndHorizontal();

            if (Scene != null)
            {
                //get path
                path = AssetDatabase.GetAssetPath(Scene);
            }

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
            var clone = new ChangeScene()
            {
                Initialize = true,
                path = path
            };

            clone.Init(pos, myRect.size, Style, SelectedNodeStyle, InPoint.Style,
                OutPoint.Style, InPoint.OnClickConnectionPoint, OutPoint.OnClickConnectionPoint,
                OnCopyNode, OnRemoveNode, newId);

            return clone;
        }
    }
}