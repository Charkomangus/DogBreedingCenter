using System;
using LoLSDK;
using UnityEngine;

namespace Assets.Scripts
{
    public class SoundManager : MonoBehaviour {
        //>>>>AUDIO<<<<<
        [SerializeField]public float AudioVolume = 0.65f;
        [SerializeField]public float MusicVolume = 0.20f;
        [SerializeField]public float FadedMusicVolume = 0.10f;
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
        }

        //play a sound once.
        public void PlaySoundEffect(string filePath)
        {
            LOLSDK.Instance.PlaySound(filePath);
        }

        public void SetVolume()
        {
            LOLSDK.Instance.ConfigureSound(AudioVolume, MusicVolume, Math.Abs(MusicVolume) < 0.1f ? 0 : FadedMusicVolume);
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
            LOLSDK.Instance.StopSound(_currentMusic);
        }

        //Control Volume
        public void StopPreviousMusic(string filepath)
        {
            LOLSDK.Instance.StopSound(filepath);
        }
    }
}
