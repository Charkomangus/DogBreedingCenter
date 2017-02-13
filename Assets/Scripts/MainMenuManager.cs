using UnityEngine;
using UnityEngine.SceneManagement;
using LoLSDK;

namespace Assets.Scripts
{
   
    public class MainMenuManager : MonoBehaviour {
       

        public void LoadLevel(string level)
        {
            SceneManager.LoadScene(level);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
