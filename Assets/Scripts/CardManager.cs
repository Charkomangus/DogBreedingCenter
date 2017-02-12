
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class CardManager : MonoBehaviour, IHasChanged
    {
        [SerializeField] private Transform _cardSlots;
        [SerializeField] private Text _inventoryText;
        // Use this for initialization
        private void Start()
        {
            HasChanged();
        }

        #region IHasChanged implementation
        public void HasChanged()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" - ");
            foreach (GameObject item in _cardSlots.Cast<Transform>().Select(slotTransform => slotTransform.GetComponent<CardSlot>().Item).Where(item => item))
            {
                builder.Append(item.name);
                builder.Append(" - ");
            }
            _inventoryText.text = builder.ToString();
        }
        #endregion
    }


    public interface IHasChanged : IEventSystemHandler
    {
        void HasChanged();
    }
}