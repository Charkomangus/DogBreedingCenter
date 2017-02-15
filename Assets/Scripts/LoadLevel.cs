using UnityEngine;
using UnityEngine.SceneManagement;
using LoLSDK;


namespace Assets.Scripts
{
    public class LoadLevel : MonoBehaviour {

        [SerializeField]private string _levelName;
        
        // Update is called once per frame
        void Update ()
        {
            SceneManager.LoadScene(GameManager.Instance.ReturnFailStatus() ? SceneManager.GetActiveScene().name : _levelName);
        }
    }
}

