using System;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.EventSystems;
using LoLSDK;

namespace Assets.Scripts
{
    public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        public static GameObject ItemBeingDragged;
        private Vector3 _startPosition;
        private Vector3 _startScale;
        private Transform _startParent;
        private float _planeDistance;
        private GameObject _cardHolder;
        private float _distX;
        private float _distY;
        private Vector3 _screenPoint;
        private CanvasGroup _canvasGroup;
        private Card _card;
        //>>>TOUCH<<<<<
        private float startTime, endTime, holdTime;
        private const float MaxTime = 0.30f;
        //>>>TOUCH<<<<<


        public void Start()
        {
          
            _cardHolder = GameObject.FindGameObjectWithTag("cardHolder"); //Temporary holding space for cards being dragged.
            _canvasGroup = GetComponent<CanvasGroup>();
            _card = GetComponent<Card>();

        }
        


        #region IBeginDragHandler implementation

                //Start dragging item
        public void OnBeginDrag(PointerEventData eventData)
        {
      
            if ((ItemBeingDragged != null)  || (eventData.button != PointerEventData.InputButton.Left) || (gameObject.transform.parent.tag == "cardReviewSlot") || (gameObject.transform.parent.tag == "PuppySlots") || GetComponentInParent<CardSlot>().Disabled) return;
           
            ItemBeingDragged = gameObject; //For readability
            _startPosition = transform.position; //Save starting position
            _startParent = transform.parent; //Save starting parent
            _canvasGroup.blocksRaycasts = false;
            _startScale = transform.localScale;
            transform.localScale = new Vector3(1, 1, 1);
            transform.SetParent(_cardHolder.transform); 
        }

        #endregion

        #region IPointerDownHandler implementation
        public void OnPointerDown(PointerEventData eventData)
        {
            bool which;
            if (GetComponentInParent<CardSlot>() == null)
            {
                if (GetComponentInParent<QuizCardSlot>().Disabled) return;
            }
            else if (GetComponentInParent<CardSlot>().Disabled) return;

            startTime = Time.time;
           
            if (gameObject.transform.parent.tag == "PuppySlots")
            {
                GameManager.Instance.PuppyManager.chosenPuppy = gameObject;
                GameManager.Instance.PuppyManager.AreYouSure(true);
            }
            else if (eventData.button == PointerEventData.InputButton.Left && gameObject.transform.parent.tag != "cardReviewSlot")
             {
                _screenPoint = Camera.main.WorldToScreenPoint(transform.position);
                _distX = eventData.position.x - _screenPoint.x;
                _distY = eventData.position.y - _screenPoint.y;
            }
            else
            {
                if (gameObject.transform.parent.tag == "breedingSlotHolder")
                   _card.ReturnToParent();

                else if(gameObject.transform.parent.tag != "cardReviewSlot")
                {
                    if (_card != null)
                    {
                        _card.OpenCardReview();
                        gameObject.transform.localScale = Vector3.one;
                    }
                }
            }
        }
        #endregion
        
        #region IDragHandler implementation
        //Move object to cursor position
        public void OnDrag(PointerEventData eventData)
        {
            if ((eventData.button != PointerEventData.InputButton.Left) || (gameObject.transform.parent.tag == "cardReviewSlot") || (gameObject.transform.parent.tag == "PuppySlots") || GetComponentInParent<CardSlot>().Disabled) return;
            Vector3 currentScreenPoint = new Vector3(eventData.position.x - _distX, eventData.position.y - _distY,_screenPoint.z);
            Vector3 currentPos = Camera.main.ScreenToWorldPoint(currentScreenPoint);

            if(currentPos != transform.position)
                transform.position = Vector3.Lerp(transform.position, currentPos, Time.deltaTime * 20); // use a float between 0 and 1 for lerp factor
          
        }

        #endregion

        #region IEndDragHandler implementation
        //Drop item. If non valid drop point take it back to starting position
        public void OnEndDrag(PointerEventData eventData)
        {
            endTime = 0;
            startTime = 0;
            if (GetComponentInParent<CardSlot>() == null)
            {
                if ((gameObject.transform.parent.tag == "cardReviewSlot") || (gameObject.transform.parent.tag == "PuppySlots") || GetComponentInParent<QuizCardSlot>().Disabled) return;
            }
            else
            {
                if ((gameObject.transform.parent.tag == "cardReviewSlot") || (gameObject.transform.parent.tag == "PuppySlots") || GetComponentInParent<CardSlot>().Disabled) return;
            }
            
            ItemBeingDragged = null;
            if (eventData.button != PointerEventData.InputButton.Left) return;
            _canvasGroup.blocksRaycasts = true;

            if (transform.parent == _cardHolder.transform)
            {
                transform.SetParent(_startParent);
            }
            if (transform.parent != _startParent) return;
            transform.localScale = _startScale;
            transform.position = _startPosition;
            

        }

        #endregion

        public void OnPointerUp(PointerEventData eventData)
        {
            endTime = Time.time;
            holdTime = endTime - startTime;
            if (holdTime > MaxTime && ItemBeingDragged == null)
            {
                if(_card!= null)
                _card.OpenCardReview();
            }
            else
            {
                holdTime = 0;
                    startTime = 0;
                    endTime = 0;
            }

        }
    }
}