using DokiVnMaker.Game;
using UnityEngine;

namespace DokiVnMaker
{
    [RequireComponent(typeof(DokiVNLauncher))]
    public class StoryLauncher : MonoBehaviour
    {
        [SerializeField] StoryGraph currentStory;
        public StoryGraph CurrentStory { get { return currentStory; } }
        [SerializeField] bool autoLaunch = true;

        DokiVNLauncher vnLauncher;

        private void Awake()
        {
            vnLauncher = GetComponent<DokiVNLauncher>();
        }

        // Start is called before the first frame update
        void Start()
        {
            if (autoLaunch)
            {
                vnLauncher.LaunchStory();
            }
        }

        public void ChangeCurrentStory(StoryGraph story)
        {
            currentStory = story;
        }
    }
}