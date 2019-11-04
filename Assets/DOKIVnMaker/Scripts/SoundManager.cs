using System.Collections.Generic;
using UnityEngine;

namespace DokiVnMaker.Game
{
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] AudioSource backgroundMusicSources;

        [SerializeField] List<AudioSource> soundSources;
        [SerializeField, Range(0, 1)] float globalVolume = 1;

        //public void PlaySound(Sound action)
        //{
        //    if(soundSources.Count -1 < action.trackIndex)
        //    {
        //        for(int i=0; i < action.trackIndex + 1 - soundSources.Count; i++)
        //        {
        //            soundSources.Add(null);
        //        }

        //        var o = new GameObject("sound");
        //        o.transform.SetParent(transform, false);
        //        soundSources[action.trackIndex] = o.AddComponent<AudioSource>();
        //        SetupAudio(soundSources[action.trackIndex], action);
        //    }
            
        //}

        //public void PlayBackgroundMusic(Music action)
        //{
        //    if(backgroundMusicSources == null)
        //    {
        //        backgroundMusicSources = new GameObject("background_music").AddComponent<AudioSource>();
        //    }
        //    SetupAudio(backgroundMusicSources, action);
        //}

        //void SetupAudio(AudioSource audioSource, AudioBase action)
        //{
        //    var audio = AssetDatabase.LoadAssetAtPath(action.path, typeof(AudioClip)) as AudioClip;
        //    if (audio != null) audioSource.clip = audio;

        //    audioSource.volume = action.volume;
        //    audioSource.loop = action.loop;
        //}
    }
}