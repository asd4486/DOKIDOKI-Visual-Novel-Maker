using System;
using System.Collections.Generic;
using UnityEngine;

namespace DokiVnMaker.MyEditor.Items
{
    [Serializable]
    public class BrancheBox : NodeBase
    {
        public string Dialogue;

        public List<BrancheItem> Branches;

        [NonSerialized]
        public Color Color;
        public int FontSize = ValueManager.DefaultFontSize;

        //branche out point style
        [NonSerialized]
        private GUIStyle OutPointStyle;
        [NonSerialized]
        private Action<ConnectionPoint> BracheClickConnectionPoint;

        public BrancheBox() { }

        public BrancheBox(Vector2 position, float width, float height, GUIStyle nodeStyle, GUIStyle selectedStyle,
            GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> onClickInPoint, Action<ConnectionPoint> onClickOutPoint,
            Action<NodeBase> onClickCopyNode, Action<NodeBase> onClickRemoveNode, int id)
        {
            ActionType = ActionTypes.BrancheBox;

            Init(position, width, height, nodeStyle, selectedStyle, inPointStyle, outPointStyle, onClickInPoint, null, onClickCopyNode, onClickRemoveNode, id);
            SetOutPointStyle(OutPointStyle, onClickOutPoint);

            //init branches if null
            if (Branches == null)
            {
                Branches = new List<BrancheItem>();
                Branches.Add(CreateNewBranche());
                Branches.Add(CreateNewBranche());
            }
        }

        public void SetOutPointStyle(GUIStyle outPointStyle, Action<ConnectionPoint> onClickOutPoint)
        {
            OutPointStyle = outPointStyle;
            BracheClickConnectionPoint = onClickOutPoint;
        }

        public override void Draw()
        {
            //add functions for braches if initialize
            if (Initialize)
            {
                var list = new List<BrancheItem>();
                foreach (var b in Branches)
                {
                    var bText = b.Text;
                    var bId = b.Id;
                    var newBrache = CreateNewBranche();
                    newBrache.Text = bText;
                    newBrache.Id = bId;
                    list.Add(newBrache);
                }
                Branches = list;

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

            for (int i = 0; i < Branches.Count; i++)
            {
                Rect.height = DefaultRectHeight + 20 * i;

                GUILayout.BeginHorizontal();
                GUILayout.Label(i + 1 + ".", WhiteTxtStyle, GUILayout.Width(30));
                Branches[i].Draw();
                GUILayout.EndHorizontal();
            }

            //add new branche(6 maximun)
            if (Branches.Count < 6)
            {
                if (GUILayout.Button("+")) Branches.Add(CreateNewBranche());
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
            for (int i = 0; i < Branches.Count; i++) Branches[i].OutPoint.Draw(-5, 32 + 21.5f * i);

            base.Draw();
        }

        private BrancheItem CreateNewBranche()
        {
            var branche = new BrancheItem(OnDeleteBranche, Id, SetBracheId());


            branche.OutPoint = new ConnectionPoint(this, ConnectionPointType.Out, OutPointStyle, BracheClickConnectionPoint);
            return branche;
        }

        private void OnDeleteBranche(BrancheItem branche)
        {
            if (Branches.Contains(branche))
            {
                Branches.Remove(branche);
            }
        }

        private int SetBracheId()
        {
            if (Branches == null) return 0;
            var id = 0;
            foreach (var n in Branches)
            {
                if (id <= n.Id) id = n.Id + 1;
            }
            return id;
        }

        public override NodeBase Clone(Vector2 pos, int newId)
        {
            var clone = new BrancheBox(pos, Rect.width, Rect.height, Style, SelectedNodeStyle, InPoint.Style,
                OutPoint.Style, InPoint.OnClickConnectionPoint, OutPoint.OnClickConnectionPoint,
                OnCopyNode, OnRemoveNode, newId)
            {
                Dialogue = Dialogue,
                Branches = Branches,
                Color = Color,
                FontSize = FontSize,
            };

            return clone;
        }
    }
}