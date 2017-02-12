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
        // Use this for initialization
        void Start ()
        {
            _percentageText = GetComponentInChildren<Text>();

        }
	
        // Update is called once per frame
        void Update ()
        {
           
            value = (int)(GetComponent<Slider>().value*100);
            if (value != oldValue)
                GameManager.Instance.GeneticVarienceWarnings();
            oldValue = value;
            _percentageText.text = value.ToString(CultureInfo.CurrentCulture) + "%";
        }
    }
}
