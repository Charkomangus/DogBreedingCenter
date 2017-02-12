using UnityEngine;
using UnityEngine.UI;
using LoLSDK;

namespace Assets.Scripts
{
    public class BreedButton : MonoBehaviour
    {
        [SerializeField] private GameObject _breedingPanel;
        // Use this for initialization
        void Start()
        {
            _breedingPanel = GameObject.FindGameObjectWithTag("breedingPanel");
        }

        // Update is called once per frame
        void Update()
        {
            //Open and close the breeding Panel
            if (_breedingPanel.GetComponent<Animator>().GetBool("Open"))
            {
                GetComponent<Image>().enabled = false;
                GetComponent<Button>().enabled = false;
                GetComponentInChildren<Text>().enabled = false;
            }
            else
            {
                GetComponent<Image>().enabled = true;
                GetComponent<Button>().enabled = true;
                GetComponentInChildren<Text>().enabled = true;
            }
        }
    }
}
