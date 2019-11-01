using UnityEngine;
using UnityEditor;
using XNode;

namespace DokiVnMaker.StoryNode
{
    [NodeTint("#ffff99")]
    public class StartPoint : Node
    {
        [Output(backingValue = ShowBackingValue.Never, connectionType = ConnectionType.Override)] public StoryNodeBase output;

        protected override void Init()
        {   
            name = "START";
            base.Init();
        }
    }
}