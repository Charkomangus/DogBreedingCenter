using UnityEngine;
using LoLSDK;

namespace Assets.Scripts
{
    public class MenuCamera : MonoBehaviour
    {
        public float Limit;
        private Animator menuButtons;
        // Use this for initialization
        void Start()
        {
            menuButtons = GameObject.FindWithTag("menuButtons").GetComponent<Animator>();
        }

        // Update is called once per frame
        void Update()
        {


            if (Input.anyKey && !menuButtons.GetBool("Open") || Input.GetMouseButtonDown(0) && !menuButtons.GetBool("Open") || Input.touches.Length > 0 && !menuButtons.GetBool("Open"))
            {
                menuButtons.SetBool("Open", true);
            }
            if (transform.position.x < Limit)
                transform.position += new Vector3(0.05f, 0, 0);
            else
            {
                menuButtons.SetBool("Open", true);
            }
        }
    }
}

