using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using LoLSDK;

namespace Assets.Scripts
{
    public class CardWarning : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler,
        IDragHandler
    {
        [SerializeField] private string _warningName;
        private Card _card;
        private bool _itemBeingDragged;
        
        //>>>TOUCH<<<<<
        private float _startTime, _endTime, _holdTime;
        private const float MaxTime = 0.30f;
        //>>>TOUCH<<<<<


        void Start()
        {
         _card = GetComponent<Card>();
        }


        public void OnPointerDown(PointerEventData eventData)
        {
           
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                _startTime = Time.time;
                _itemBeingDragged = false;
            }
            else
            {

                if (_card != null)
                {
                    gameObject.transform.localScale = Vector3.one;
                    _card.OpenCardReview();
                }
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_itemBeingDragged) return;
            _endTime = Time.time;
            _holdTime = _endTime - _startTime;
            Debug.Log("Hold:" + _holdTime);
            if (_holdTime > MaxTime)
            {
                if (_card != null)
                {
                    gameObject.transform.localScale = Vector3.one;
                    _card.OpenCardReview();
                }
            }
            else
            {
                GameManager.Instance.DialogueManager.OpenDialogue(GameManager.Instance.CurrentLevel + "/" + _warningName);
                if (_warningName == "Cat")
                    GameManager.Instance.SoundManager.PlaySoundEffect("Sound/meow.wav");
            }
   
        }


        public void OnBeginDrag(PointerEventData eventData)
        {
          
            _itemBeingDragged = true;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            GameManager.Instance.DialogueManager.OpenDialogue(GameManager.Instance.CurrentLevel + "/" + _warningName);
            _itemBeingDragged = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            
            _itemBeingDragged = true;
        }
    }
}
