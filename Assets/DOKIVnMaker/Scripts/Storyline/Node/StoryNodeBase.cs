using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace DokiVnMaker.StoryNode
{
    [NodeTint("#CCCCFF")]
    public abstract class StoryNodeBase : Node
    {
        [Input(backingValue = ShowBackingValue.Never, typeConstraint = TypeConstraint.Inherited)] public StoryNodeBase input;
        [Output(backingValue = ShowBackingValue.Never, connectionType = ConnectionType.Override)] public StoryNodeBase output;

        //abstract public void Trigger();
    }
}