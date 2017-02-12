using UnityEngine;
using UnityEngine.UI;
using LoLSDK;

namespace Assets.Scripts
{
    public class TextHolder : MonoBehaviour {


        [SerializeField]private Text[] _text;
        // Use this for initialization
        void Start () {
            _text = new Text[transform.childCount];

            for (int i = 0; i < transform.childCount; i++)
            {
                _text[i] = transform.GetChild(i).GetComponent<Text>();
            }
        }
	
        // Update is called once per frame
        public Text[] ReturnText ()
        {
            return _text;
        }
    }
}
