using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using LoLSDK;


namespace Assets.Scripts
{
    public class Percentage : MonoBehaviour
    {

        public int value, oldValue;
     [SerializeField]private Text _percentageText;
        private int GeneticDiveristyLevel;
        // Use this for initialization
        void Start ()
        {
            _percentageText = GetComponentInChildren<Text>();
            value = 100;
            oldValue = 100;
            GetComponent<Slider>().normalizedValue = 100;
            GetComponent<Slider>().value = 100;
            _percentageText.text = value.ToString(CultureInfo.CurrentCulture) + "%";
        }
	
        // Update is called once per frame
        void Update ()
        {
            if (value != oldValue)
            {
                GameManager.Instance.GeneticVarienceWarnings();
                oldValue = value;
                GetComponent<Slider>().value = value;
                _percentageText.text = value.ToString(CultureInfo.CurrentCulture) + "%";
            }
        }
    }
}
