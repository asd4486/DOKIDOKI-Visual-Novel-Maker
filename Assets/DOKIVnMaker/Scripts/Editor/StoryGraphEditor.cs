using UnityEngine;
using UnityEditor;
using XNodeEditor;
using System;

namespace DokiVnMaker
{
    [CustomNodeGraphEditor(typeof(StoryGraph))]
    public class StoryGraphEditor : NodeGraphEditor
    {
        public override void OnOpen()
        {
            window.titleContent = new GUIContent(Selection.activeObject.name);
            base.OnOpen();
        }

        public override string GetNodeMenuName(Type type)
        {
            if (type.Namespace == "DokiVnMaker.StoryNode")
            {
                return base.GetNodeMenuName(type).Replace("Doki Vn Maker/Story Node/", "");
            }

            return null;
        }
    }
}