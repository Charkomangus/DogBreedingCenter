using System;
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
            if (Math.Abs(slider.value) < 0.01f)
                text.text = "MUTED";
            else
                text.text = (int)(slider.value * 100) + "%";
        }
	
        // Update is called once per frame
        private void Update ()
        {
            if (oldvalue == slider.value) return;
            oldvalue = slider.value;
            if (Math.Abs(slider.value) < 0.01f)
                text.text = "MUTED";
            else
                text.text = (int) (slider.value*100) + "%";
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
        public void ExitToMenu()
        {
            SceneManager.LoadScene("Menu");
        }

    }
}
