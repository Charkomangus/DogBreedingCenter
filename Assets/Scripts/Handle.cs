using UnityEngine;
using UnityEngine.UI;
using LoLSDK;

namespace Assets.Scripts
{
    public class Handle : MonoBehaviour
    {

        private Text _percentageText;
        // Use this for initialization
        void Start ()
        {
            _percentageText = GetComponentInChildren<Text>();
        }
	
        // Update is called once per frame
        void Update () {
            if (GetComponentInParent<Slider>().value <= 0.2)
            {
                _percentageText.transform.localPosition = new Vector3(36, 0, 0);
                _percentageText.color = Color.white;
                _percentageText.fontStyle = FontStyle.Bold;
            }
            else
            {
                _percentageText.transform.localPosition = new Vector3(0, 0, 0);
                _percentageText.color = Color.black;
                _percentageText.fontStyle = FontStyle.Normal;
            }
        }
    }
}
