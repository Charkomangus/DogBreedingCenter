using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using LoLSDK;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{

    public class QuizCardSlot : MonoBehaviour, IDropHandler
    {
        [SerializeField]public float Scale;
        [SerializeField]public bool Disabled;
        [SerializeField]public string acceptedReward, acceptedReward2;
        private Image image;
        public Text _name;
        public GameObject Item
        {
            get { return transform.childCount > 0 ? transform.GetChild(0).gameObject : null; }
        }

        void Start()
        {
            image = GetComponent<Image>();
        }

        #region IDropHandler implementation

        public void OnDrop(PointerEventData eventData)
        {

            if (Disabled || Item) return;
          
               

            if (Math.Abs(Scale) < 0.1f)
                Scale = 1;
            if (DragHandler.ItemBeingDragged == null) return;
            if (DragHandler.ItemBeingDragged.name != acceptedReward && DragHandler.ItemBeingDragged.name != acceptedReward2)
            {

                GameManager.Instance.DialogueManager.OpenDialogue("Woops"+Random.Range(0,2));
                return;
            }
           
            Disabled = true;
            if(_name!= null)
                _name.color = new Color(0,0,0,1);
            DragHandler.ItemBeingDragged.transform.SetParent(transform);
            DragHandler.ItemBeingDragged.transform.localScale = new Vector3(Scale, Scale, 1);
            DragHandler.ItemBeingDragged = null;
            ExecuteEvents.ExecuteHierarchy<IHasChanged>(gameObject, null, (x, y) => x.HasChanged());
        }

        private void Update()
        {
            if (tag == "cardReviewSlot" || tag == "cardHolder") return;
            if (Disabled)
            {
                image.color = new Color(0, 0, 0, 0.2f);
                transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
            }
            else
            {
                image.color = new Color(1, 1, 1, 1);
                transform.localScale = new Vector3(1, 1, 1);
            }
        }

        #endregion
    }
}