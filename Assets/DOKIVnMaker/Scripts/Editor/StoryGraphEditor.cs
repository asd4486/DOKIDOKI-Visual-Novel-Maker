using UnityEngine;
using UnityEditor;
using XNodeEditor;
using System;
using System.Linq;
using DokiVnMaker.StoryNode;

namespace DokiVnMaker
{
    [CustomNodeGraphEditor(typeof(StoryGraph))]
    public class StoryGraphEditor : NodeGraphEditor
    {
        public override void OnOpen()
        {
            window.titleContent = new GUIContent(Selection.activeObject.name);

            var story = target as StoryGraph;
            if (!story.nodes.Any(n => n is StartPoint))
            {
                CreateNode(typeof(StartPoint), new Vector2(-250, -20));
            }

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

        public override void AddContextMenuItems(GenericMenu menu)
        {
            Vector2 pos = NodeEditorWindow.current.WindowToGridPosition(Event.current.mousePosition);
            for (int i = 0; i < NodeEditorWindow.nodeTypes.Length; i++)
            {
                Type type = NodeEditorWindow.nodeTypes[i];

                //Get node context menu path
                string path = GetNodeMenuName(type);
                if (string.IsNullOrEmpty(path)) continue;

                if (path != "Start Point")
                {
                    menu.AddItem(new GUIContent(path), false, () =>
                    {
                        CreateNode(type, pos);
                    });
                }
            }
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Preferences"), false, () => NodeEditorWindow.OpenPreferences());
            NodeEditorWindow.AddCustomContextMenuItems(menu, target);
        }
    }
}