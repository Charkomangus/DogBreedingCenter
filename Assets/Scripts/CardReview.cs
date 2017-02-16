using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using LoLSDK;

namespace Assets.Scripts
{
    public class CardReview : MonoBehaviour, IPointerUpHandler
    {
        [SerializeField]private Text _stats;
        [SerializeField]private Text _statsNumber;
        [SerializeField]private Text _description;
        [SerializeField]private Text _family;
        [SerializeField]private CardSlot _slot;
        [SerializeField]private GameObject _chosenCard;
        [SerializeField]private Dog _dog;
        [SerializeField]private CardSlot _cardSlot;
        private bool _set;
        // Use this for initialization
        void Awake() {
            _cardSlot = GetComponentInChildren<CardSlot>();
        }
	
        // Update is called once per frame
        void Update ()
        {
            if (!_slot.Item) return;
            if(!_set)
                SetDog();
        }


        private void SetDog()
        {
            _chosenCard = _slot.Item;
            _dog = _chosenCard.GetComponent<Dog>();
            _stats.text = SetStatText();
            _family.text = SetFamily();
            _statsNumber.text = SetStatNumberText();
            _description.text = SetDescriptionText();
            _set = true;
        }
        private string SetFamily()
        {
            System.Text.StringBuilder familyBuilder = new System.Text.StringBuilder();
            //If its a starting dog fake its parents
            if (_dog.StartingGeneration())
            {
                familyBuilder.Append("Parents: " + _dog.ReturnParentNames() + Environment.NewLine);
                familyBuilder.Append("Siblings: None" + Environment.NewLine);
                familyBuilder.Append("Half-Siblings: None" + _dog.ReturnHalfSiblingNames());
            }
            else
            {
                familyBuilder.Append("Parents: " + _dog.ReturnParentNames() + Environment.NewLine);
                familyBuilder.Append("Siblings: " + _dog.ReturnSiblingNames() + Environment.NewLine);
                familyBuilder.Append("Half-Siblings: " + _dog.ReturnHalfSiblingNames());
            }
            return familyBuilder.ToString();
        }
        //Create string with all stats
        private string SetStatText()
        {
            System.Text.StringBuilder statsBuilder = new System.Text.StringBuilder();
            statsBuilder.Append("Intelligence : " + Environment.NewLine);
            statsBuilder.Append("Endurance: " + Environment.NewLine);
            statsBuilder.Append("Demeanor: "  + Environment.NewLine);
            statsBuilder.Append("Strength: " + Environment.NewLine);
            statsBuilder.Append("Hearing: " + Environment.NewLine);
            statsBuilder.Append("Scent: " + Environment.NewLine);
            statsBuilder.Append("Sight: " + Environment.NewLine);
            statsBuilder.Append("Bark: " +  Environment.NewLine);
            return statsBuilder.ToString();
        }
        //Create string with all stats
        private string SetDescriptionText()
        {
            System.Text.StringBuilder descriptionBuilder = new System.Text.StringBuilder();
            descriptionBuilder.Append(_dog.ReturnName() + " is a " + _dog.ReturnSex().ToLower() + ", " +_dog.ReturnSizeDescription().ToLower() + " sized dog with a " +
                                      _dog.ReturnHairLengthDescription().ToLower() + " length " + "coat." +Environment.NewLine + Environment.NewLine);
            return descriptionBuilder.ToString();

        }

        //Create string with all stats
        private string SetStatNumberText()
        {
            System.Text.StringBuilder statsNumbersBuilder = new System.Text.StringBuilder();

            statsNumbersBuilder.Append(Mathf.FloorToInt(_dog.ReturnIntelligence() / 10) + "/10 (" + _dog.ReturnIntelligenceDescription() + ")  " + Environment.NewLine);
            statsNumbersBuilder.Append(Mathf.FloorToInt(_dog.ReturnEndurance() / 10) + "/10 (" + _dog.ReturnEnduranceDescription() + ")  " + Environment.NewLine);
            statsNumbersBuilder.Append(Mathf.FloorToInt(_dog.ReturnDemeanor() / 10) + "/10 (" + _dog.ReturnDemeanorDescription() + ")  " + Environment.NewLine);
            statsNumbersBuilder.Append(Mathf.FloorToInt(_dog.ReturnStrength() / 10) + "/10 (" + _dog.ReturnStrengthDescription() + ")  " + Environment.NewLine);
            statsNumbersBuilder.Append(Mathf.FloorToInt(_dog.ReturnHearing() / 10) + "/10 (" + _dog.ReturnHearingDescription() + ")  " + Environment.NewLine);
            statsNumbersBuilder.Append(Mathf.FloorToInt(_dog.ReturnScent() / 10) + "/10 (" + _dog.ReturnScentDescription() + ")  " + Environment.NewLine);
            statsNumbersBuilder.Append(Mathf.FloorToInt(_dog.ReturnSight() / 10) + "/10 (" + _dog.ReturnSightDescription() + ")  " + Environment.NewLine);
            statsNumbersBuilder.Append(Mathf.FloorToInt(_dog.ReturnBark() / 10) + "/10 (" + _dog.ReturnBarkDescription() + ")  " + Environment.NewLine);
            return statsNumbersBuilder.ToString();
        }

        public CardSlot ReturnCardSlot()
        {
            return _cardSlot;
        }

        public void Exit()
        { 
            _chosenCard.GetComponent<Card>().ReturnToParent();
            GetComponent<Animator>().SetBool("Open", false);
            _set = false;
            if(!GameManager.Instance.PuppyManager.GetComponent<Animator>().GetBool("Open"))
                GameManager.Instance.SideBar.SetBool("Open", true);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (transform.childCount <= 0) return;
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<Card>().ReturnToParent();
            }
        }
    }
}
