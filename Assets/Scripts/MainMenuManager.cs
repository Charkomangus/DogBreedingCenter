using UnityEngine;
using UnityEngine.SceneManagement;
using LoLSDK;

namespace Assets.Scripts
{
   
    public class MainMenuManager : MonoBehaviour {
        [SerializeField]int level;

        public void StartGame()
        {
            if (level == 0)
                level = 2;
            SceneManager.LoadScene(level);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
