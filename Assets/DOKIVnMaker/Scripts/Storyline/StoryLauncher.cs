using DokiVnMaker.Game;
using UnityEngine;

namespace DokiVnMaker
{
    [RequireComponent(typeof(DokiVNMain))]
    public class StoryLauncher : MonoBehaviour
    {
        [SerializeField] StoryGraph currentStory;
        public StoryGraph CurrentStory { get { return currentStory; } }
        [SerializeField] bool autoLaunch = true;

        DokiVNMain vnLauncher;

        private void Awake()
        {
            vnLauncher = GetComponent<DokiVNMain>();
        }

        // Start is called before the first frame update
        void Start()
        {
            if (autoLaunch)
                vnLauncher.LaunchNewStory();
        }

        public void ChangeCurrentStory(StoryGraph story, bool auto)
        {
            currentStory = story;
            autoLaunch = auto;
            if (autoLaunch) vnLauncher.LaunchNewStory();
        }
    }
}