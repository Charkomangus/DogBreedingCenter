using UnityEngine;
using UnityEngine.UI;
using LoLSDK;

namespace Assets.Scripts
{
    public class Gender : MonoBehaviour {


        [SerializeField]private Sprite male;
        [SerializeField]private Sprite female;
        private Dog dog;
        // Use this for initialization
        void Start()
        {
            dog = GetComponentInParent<Dog>();
            GetComponent<Image>().sprite = dog.ReturnSex() == "Male" ? male : female;
        }

        // Update is called once per frame
        void Update ()
        {
		
        }
    }
}
