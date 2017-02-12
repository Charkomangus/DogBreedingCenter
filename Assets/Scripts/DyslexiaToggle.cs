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
            _toggle.isOn = GameManager.Instance.DyslexiaFontEnabled;
        }


        public void SetDyslexiaFont()
        {
          GameManager.Instance.DyslexiaFontEnabled = _toggle.isOn;
        }
    }
}
