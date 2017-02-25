using System;
using System.Collections.Generic;
using LoLSDK;
using UnityEngine;

namespace Assets.Scripts
{
    public class SoundManager : MonoBehaviour {
        //>>>>AUDIO<<<<<
        [SerializeField]public float AudioVolume = 0.65f;
        [SerializeField]public float MusicVolume = 0.20f;
        [SerializeField]public float FadedMusicVolume = 0.01f;
        private List<string> music = new List<string>();
        private string _currentMusic;
        //>>>>AUDIO<<<<<
        private void Start()
        {
            SetVolume();
        }
        // play soft music sound repeatedly in the background.
        public void PlayBackgroundMusic(string filePath)
        {
            _currentMusic = filePath;
            LOLSDK.Instance.PlaySound(filePath, true, true);
            if(filePath!=null)
                music.Add(filePath);
        }

        //play a sound once.
        public void PlaySoundEffect(string filePath)
        {
            LOLSDK.Instance.PlaySound(filePath);
        }

        public void SetVolume()
        {
            if(Math.Abs(AudioVolume) < 0.01f || Math.Abs(MusicVolume) < 0.01f)
                LOLSDK.Instance.ConfigureSound(AudioVolume, MusicVolume, 0);
            else if(Math.Abs(AudioVolume) >= 0.01f && Math.Abs(MusicVolume) >= 0.01f)
                LOLSDK.Instance.ConfigureSound(AudioVolume, MusicVolume, FadedMusicVolume);
            else
                LOLSDK.Instance.ConfigureSound(AudioVolume, MusicVolume, 0);
        }

        //Control Volume
        public void SetAudioVolume(float volume)
        {
            AudioVolume = volume;
            SetVolume();
        }

        //Control Volume
        public void SetMusicVolume(float volume)
        {
            MusicVolume = volume;
            SetVolume();
        }

        //Control Volume
        public void StopPreviousMusic()
        {
            foreach (var track in music)
            {
                LOLSDK.Instance.StopSound(track);
            }
        }

        public void StopAllPreviousMusic()
        {
            LOLSDK.Instance.StopSound(_currentMusic);
        }

        //Control Volume
        public void StopPreviousMusic(string filepath)
        {
            LOLSDK.Instance.StopSound(filepath);
        }
    }
}
