using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

public class StorylineManager : EditorWindow
{
    [MenuItem("DokiDoki VN Maker/Storyline manager")]
    static void Init()
    {
        StorylineManager window =
            (StorylineManager)EditorWindow.GetWindow(typeof(StorylineManager), true, "Story line manager");
    }

    private void OnGUI()
    {
    }
}

