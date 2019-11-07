﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace DokiVnMaker.Story
{
    [NodeWidth(250)]
    [NodeTint("#CCCCFF")]
    public abstract class StoryNodeBase : Node
    {
        [Input(backingValue = ShowBackingValue.Never, typeConstraint = TypeConstraint.Inherited)] public StoryNodeBase input;
        [outputlist(backingValue = ShowBackingValue.Never, connectionType = ConnectionType.Override)] public StoryNodeBase output;
    }
}