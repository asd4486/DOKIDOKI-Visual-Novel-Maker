using System;
using System.Collections.Generic;
using UnityEngine;

namespace DokiVnMaker.MyEditor.Items
{
    [Serializable]
    public class BrancheBox : NodeBase
    {
        public string dialogue;

        public List<BrancheItem> brancheList;

        [NonSerialized]
        public Color color;
        public int fontSize = ValueManager.DefaultFontSize;

        //branche out point style
        [NonSerialized]
        private GUIStyle brancheOutPointStyle;
        [NonSerialized]
        private Action<ConnectionPoint> BracheClickConnectionPoint;

        public BrancheBox()
        {
            ActionType = ActionTypes.BrancheBox;
        }

        public void SetOutPointStyle(GUIStyle _outPointStyle, Action<ConnectionPoint> _onClickOutPoint)
        {
            brancheOutPointStyle = _outPointStyle;
            BracheClickConnectionPoint = _onClickOutPoint;

            //init branches if null
            if (brancheList == null)
            {
                brancheList = new List<BrancheItem>();
                brancheList.Add(CreateNewBranche());
                brancheList.Add(CreateNewBranche());
            }
        }

        public override void Draw()
        {
            //add functions for braches if initialize
            if (Initialize)
            {
                var list = new List<BrancheItem>();
                foreach (var b in brancheList)
                {
                    var bText = b.Text;
                    var bId = b.Id;
                    var newBrache = CreateNewBranche();
                    newBrache.Text = bText;
                    newBrache.Id = bId;
                    list.Add(newBrache);
                }
                brancheList = list;

                Initialize = false;
            }

            GUILayout.BeginArea(Rect, Title, Style);
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Space(SpacePixel);
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical();
            GUILayout.Space(SpacePixel);

            GUILayout.Label("Branches", WhiteTxtStyle);

            for (int i = 0; i < brancheList.Count; i++)
            {
                Rect.height = DefaultRectHeight + 20 * i;

                GUILayout.BeginHorizontal();
                GUILayout.Label(i + 1 + ".", WhiteTxtStyle, GUILayout.Width(30));
                brancheList[i].Draw();
                GUILayout.EndHorizontal();
            }

            //add new branche(6 maximun)
            if (brancheList.Count < 6)
            {
                if (GUILayout.Button("+")) brancheList.Add(CreateNewBranche());
            }

            //FontSize = EditorGUILayout.IntField("Font size:", FontSize);
            ////dialogue text box
            //GUILayout.Label("Dialogue");
            //Dialogue = EditorGUILayout.TextArea(Dialogue, GUILayout.Height(50));

            GUILayout.EndVertical();
            GUILayout.Space(SpacePixel);
            GUILayout.EndHorizontal();
            GUILayout.EndArea();

            InPoint.Draw();
            for (int i = 0; i < brancheList.Count; i++)
            {
                brancheList[i].OutPoint.Draw(-5, 32 + 21.5f * i);
            }

            base.Draw();
        }

        private BrancheItem CreateNewBranche()
        {
            var branche = new BrancheItem(OnDeleteBranche, Id, SetBracheId());

            branche.OutPoint = new ConnectionPoint(this, ConnectionPointType.Out, brancheOutPointStyle, BracheClickConnectionPoint);
            return branche;
        }

        private void OnDeleteBranche(BrancheItem branche)
        {
            if (brancheList.Contains(branche))
            {
                brancheList.Remove(branche);
            }
        }

        private int SetBracheId()
        {
            if (brancheList == null) return 0;
            var id = 0;
            foreach (var n in brancheList)
            {
                if (id <= n.Id) id = n.Id + 1;
            }
            return id;
        }

        public override NodeBase Clone(Vector2 pos, int newId)
        {
            var clone = new BrancheBox()
            {
                dialogue = dialogue,
                brancheList = brancheList,
                color = color,
                fontSize = fontSize,
            };

            clone.Init(pos, Rect.width, Rect.height, Style, SelectedNodeStyle, InPoint.Style, brancheOutPointStyle,
                InPoint.OnClickConnectionPoint, BracheClickConnectionPoint,
                OnCopyNode, OnRemoveNode, newId);

            return clone;
        }
    }
}