using UnityEngine;
using UnityEditor;

namespace DokiVnMaker.MyEditor
{
    [CustomEditor(typeof(StoryLine))]
    public class StoryLineEditor : Editor
    {
        StoryLine StoryLine { get; set; }
        private Vector2 DialogueScroller { get; set; }

        private void OnEnable()
        {
            //get editor target
            StoryLine = (StoryLine)target;
        }

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Edit"))
            {
                StoryLineNodeEditor.InitWindow(StoryLine);
            }
        }
    }
}