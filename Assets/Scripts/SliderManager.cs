﻿using System;
using UnityEngine;
using UnityEngine.UI;
using LoLSDK;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class SliderManager : MonoBehaviour
    {
        private Slider slider;
        private float oldvalue;
        [SerializeField] private Text text;
        // Use this for initialization
        void Start ()
        {

            GameManager temp = FindObjectOfType<GameManager>();
            slider = GetComponent<Slider>();
            slider.value = tag == "Audio" ? temp.SoundManager.AudioVolume : temp.SoundManager.MusicVolume;
            oldvalue = slider.value;
            if (slider.value < 0.1f)
                text.text = "MUTED";
            else
                text.text = (int)(slider.value * 100) + "%";
        }
	
        // Update is called once per frame
        void Update ()
        {
            if (Math.Abs(oldvalue - slider.value) > 0.1f)
            {
                if (slider.value < 0.1f)
                    text.text = "MUTED";
                else
                    text.text = (int) (slider.value*100) + "%";
            }
        }


        public void SetMusicVolume()
        {
            GameManager temp = FindObjectOfType<GameManager>();
            temp.SoundManager.SetMusicVolume(slider.value);
        }

        public void SetAudioVolume()
        {
            GameManager temp = FindObjectOfType<GameManager>();
            temp.SoundManager.SetAudioVolume(slider.value);
        }
        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    }
}
