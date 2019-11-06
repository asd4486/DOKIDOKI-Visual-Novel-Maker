using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

namespace DokiVnMaker.Story
{
    [CreateNodeMenu("Game/Change story")]
    public class ChangeStory : StoryNodeBase
    {
        public StoryGraph nextStoryGraph;
        public bool autoPlay = true;
    }
}