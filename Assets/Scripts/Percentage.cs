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
        private Slider _slider;
        private int GeneticDiveristyLevel;
        // Use this for initialization
        void Start ()
        {
            _slider = GetComponent<Slider>();
            _percentageText = GetComponentInChildren<Text>();
            value = 100;
            oldValue = 100;
            _slider.normalizedValue = 100;
            _slider.value = 100;
            _percentageText.text = value.ToString(CultureInfo.CurrentCulture) + "%";
        }
	
        // Update is called once per frame
        void Update ()
        {
            if (value != oldValue)
            {
                GameManager.Instance.GeneticVarienceWarnings();
                _slider.value = value;
                _slider.normalizedValue = value;
                _percentageText.text = value.ToString(CultureInfo.CurrentCulture) + "%";
                oldValue = value;
            }
        }
    }
}
