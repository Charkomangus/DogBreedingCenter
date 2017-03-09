using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using LoLSDK;

namespace Assets.Scripts
{
  
    public class CardSlot : MonoBehaviour, IDropHandler
    {
        [SerializeField]public float Scale;
        [SerializeField]public bool Disabled;
        private bool _oldDisabled;
        private Image _image;
        private Transform _transform;
        private string _tag;
        public GameObject Item
        {
            get { return transform.childCount > 0 ? transform.GetChild(0).gameObject : null; }
        }

        void Start()
        {
            _oldDisabled = Disabled;
            _tag = tag;
            _image = GetComponent<Image>();
        }

        #region IDropHandler implementation

        public void OnDrop(PointerEventData eventData)
        {

            if(Disabled || Item)return;
         
            if (Math.Abs(Scale) < 0.1f)
                Scale = 1;
            if (DragHandler.ItemBeingDragged == null) return;
            DragHandler.ItemBeingDragged.transform.SetParent(transform);
            DragHandler.ItemBeingDragged.transform.localScale = new Vector3(Scale, Scale, 1);
            DragHandler.ItemBeingDragged = null;
            ExecuteEvents.ExecuteHierarchy<IHasChanged>(gameObject, null, (x, y) => x.HasChanged());
        }
        #endregion


        private void Update()
        {
            if (_oldDisabled == Disabled) return;
            if (_tag == "cardReviewSlot" || _tag == "cardHolder") return;

         
            _oldDisabled = Disabled;
            SetStatus();
        }

        private void SetStatus()
        {
            if (Disabled)
            {
                _image.color = new Color(0.7f, 0.7f, 0.7f, 0.5f);
                transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
            }
            else
            {
                _image.color = new Color(1, 1, 1, 1);
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
      
      
    }
}