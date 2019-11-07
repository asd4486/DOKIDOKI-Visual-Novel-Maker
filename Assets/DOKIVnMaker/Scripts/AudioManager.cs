using DG.Tweening;
using DokiVnMaker.Story;
using System.Collections.Generic;
using UnityEngine;

namespace DokiVnMaker.Game
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] AudioSource backgroundMusicSource;
        [SerializeField] List<AudioSource> soundSources;

        public void PlaySound(Sound node)
        {
            if (soundSources.Count - 1 < node.track)
            {
                for (int i = soundSources.Count - 1; i < node.track; i++)
                {
                    var o = new GameObject("sound");
                    o.transform.SetParent(transform, false);
                    soundSources.Add(o.AddComponent<AudioSource>());
                }
            }
            SetupAudio(soundSources[node.track], node);
        }

        public void PlayBackgroundMusic(BackgroundMusic node)
        {
            if (backgroundMusicSource == null)
            {
                backgroundMusicSource = new GameObject("background_music").AddComponent<AudioSource>();
            }
            SetupAudio(backgroundMusicSource, node);
        }

        void SetupAudio(AudioSource audioSource, AudioNodeBase node)
        {
            if (node.audio == null) return;
            audioSource.clip = node.audio;
            audioSource.volume = 0;
            audioSource.loop = node.loop;

            //audio fade in
            if (node.fadeIn)
                audioSource.DOFade(node.volume, node.fadeTime);
            else
                audioSource.volume = node.volume;

            audioSource.Play();
        }
    }
}