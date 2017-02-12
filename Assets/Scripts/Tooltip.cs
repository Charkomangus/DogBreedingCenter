using UnityEngine;
using LoLSDK;

namespace Assets.Scripts
{
    public class Tooltip : MonoBehaviour
    {
     [SerializeField]private Animator[] _tooltips;


        // Use this for initialization
        void Start ()
        {
            _tooltips = new Animator[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                _tooltips[i] = transform.GetChild(i).gameObject.GetComponent<Animator>();
            }
        }

        
        //Open all the UI tooltips then continue dialogue
        public Animator[] ReturnTooltips()
        {
            return _tooltips;
        }

    }
}
