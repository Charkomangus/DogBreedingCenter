using UnityEngine;
using UnityEngine.EventSystems;
using LoLSDK;

namespace Assets.Scripts
{
    public class CameraController : MonoBehaviour
    {
        public float speed = 0.1F;
        private float scrollWheelDelta;
        private float zoom = 0;
        private float x, y;
        private GameObject _cardReview;
        private GameObject _dialogueBox, _warning;

        public float dragSpeed = 10;
        private Vector3 dragOrigin;

        public static GameObject DraggedInstance;

        Vector3 _startPosition;
        Vector3 _offsetToMouse, pos, move;
        float _zDistanceToCamera;

        [SerializeField] int minX = -1100, maxX = 0, minY = 0, maxY = 800;
        // Use this for initialization
        void Start()
        {
            _cardReview = GameObject.FindWithTag("cardReview");
            _dialogueBox = GameObject.FindWithTag("Dialogue");

        }


        // Update is called once per frame
        void Update()
        {
            if (DragHandler.ItemBeingDragged != null) return; if (_cardReview.GetComponentInParent<Animator>().GetBool("Open") || _dialogueBox.GetComponent<DialogueManager>().IsOpen() || GameManager.Instance.BreedingManager.WarningPanel.activeSelf || GameManager.Instance.OptionsPanel.alpha == 1) return;
            int nbTouches = Input.touchCount;
            if (nbTouches > 0)
            {
                for (int i = 0; i < nbTouches; i++)
                {
                    Touch touch = Input.GetTouch(i);

                    TouchPhase phase = touch.phase;

                    switch (phase)
                    {
                        case TouchPhase.Began:
                            break;
                        case TouchPhase.Moved:
                            break;
                        case TouchPhase.Stationary:
                            transform.localPosition = Vector3.Lerp(transform.localPosition, Camera.main.ScreenToViewportPoint(Input.GetTouch(i).position),0);
                            break;
                        case TouchPhase.Ended:
                           
                            break;
                        case TouchPhase.Canceled:
                           
                            break;
                    }
                }
            }

            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    dragOrigin = Input.mousePosition;
                    return;
                }
               if (!Input.GetMouseButton(0)) return;

                pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
                move = new Vector3(pos.x*dragSpeed, pos.y*dragSpeed, 0);
                transform.Translate(move*Time.deltaTime, Space.Self);
            }






            OutOfBounds();
        

}
        private
            void OutOfBounds()
        {
            if (transform.localPosition.x < minX)
            {
                transform.localPosition = new Vector3(minX, transform.localPosition.y);
            }
            if (transform.localPosition.x > maxX)
            {
                transform.localPosition = new Vector3(maxX, transform.localPosition.y);
            }


            if (transform.localPosition.y < minY)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, minY);
            }
            if (transform.localPosition.y > maxY)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, maxY);
            }



        }



    }

}


