using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class DyslexiaToggle : MonoBehaviour
    {
        private Toggle _toggle;
        // Use this for initialization
        void Start ()
        {
            _toggle = GetComponent<Toggle>();
        }


        public void SetDyslexiaFont()
        {
            GameManager temp = GameObject.FindObjectOfType<GameManager>();
            temp.DyslexiaFontEnabled = _toggle.isOn;
        }
    }
}
