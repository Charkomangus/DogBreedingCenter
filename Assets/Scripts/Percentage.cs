using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using LoLSDK;


namespace Assets.Scripts
{
    public class Percentage : MonoBehaviour
    {

        public int Value = 100, OldValue = 100;
     [SerializeField]private Text _percentageText;
        private Slider _slider;
        private int GeneticDiveristyLevel;
        // Use this for initialization
        void Start ()
        {
            _slider = GetComponent<Slider>();
            _slider.maxValue = 100;
            _slider.value = Value;
            _percentageText = GetComponentInChildren<Text>();
            _percentageText.text = Value.ToString(CultureInfo.CurrentCulture) + "%";
        }
	
        // Update is called once per frame
        void Update ()
        {
            if (Value != OldValue)
            {
                GameManager.Instance.GeneticVarienceWarnings();
                _slider.value = Value;
                _percentageText.text = Value.ToString(CultureInfo.CurrentCulture) + "%";
                OldValue = Value;
            }
        }
    }
}
