using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using LoLSDK;

namespace Assets.Scripts
{
    public class Generation : MonoBehaviour
    {
       [SerializeField]private CardSlot[] _cardSlots;
      private GameObject _cardPrefab;
        private Dog[] _generationDogs;

        public bool StartingGeneration;
        // Use this for initialization

        void Start()
        {
            _cardPrefab = GameManager.Instance.ChosenCardPrefab;
            _cardSlots = new CardSlot[transform.childCount];
            _cardSlots = transform.GetComponentsInChildren<CardSlot>();
            

            //Initial the starting generation
            if (!StartingGeneration) return;
            InitialiseGeneration();


            }
       
        //Fill a generation with premade dogs
        private void InitialiseGeneration()
        {
            for (int i = 0; i < _cardSlots.Length; i++)
            {
                if (_cardSlots[i].Item) return;
                GameObject temp = Instantiate(_cardPrefab);
                //Make sure to balance genders
                if (ReturnMales() > ReturnFemales())
                {
                    temp.GetComponent<Dog>().InitialiseDogs("Female");
                }
                else if (ReturnMales() < ReturnFemales())
                {
                    temp.GetComponent<Dog>().InitialiseDogs("Male");
                }
                else
                {
                    temp.GetComponent<Dog>().InitialiseDogs("Female");
                }
                temp.transform.SetParent(_cardSlots[i].transform);
                temp.transform.localScale = new Vector3(temp.transform.GetComponentInParent<CardSlot>().Scale, temp.transform.GetComponentInParent<CardSlot>().Scale, 1);
            }
        }

        //Add a card to this generation and update the generation
        public void AddCard(GameObject newCard)
        {
            for (int i = 0; i < _cardSlots.Length; i++)
            {
                if (!_cardSlots[i].Item)
                {
                    newCard.transform.SetParent(_cardSlots[i].transform);
                    newCard.transform.localScale = new Vector3(newCard.transform.GetComponentInParent<CardSlot>().Scale,newCard.transform.GetComponentInParent<CardSlot>().Scale, 1);
                    break;
                }
            }
            UpdateGeneration();
        }

        //Return an array containing all cardslots
        public CardSlot[] ReturnCardSlots()
        {
            return _cardSlots;
        }

        //Return available cardslot or null if none available
        public Transform ReturnAvailableSlot()
        {
            return _cardSlots.Where(cardSlot => !cardSlot.Item).Select(cardSlot => cardSlot.transform).FirstOrDefault();
        }

        //Change status of cardSlots
        public void Disabled(bool status)
        {
            for (int i = 0; i < _cardSlots.Length; i++)
            {
                _cardSlots[i].Disabled = status;
            }
            GetComponent<GridLayoutGroup>().spacing = status ? new Vector2(10,0) : new Vector2(40, 0);
        }

        //Return number of male dogs currently in the generation
        public int ReturnMales()
        {
            return _cardSlots.Where(cardSlot => cardSlot.Item != null).Count(cardSlot => cardSlot.Item.GetComponent<Dog>().ReturnSex() == "Male");
        }

        //Return number of female dogs currently in the generation
        public int ReturnFemales()
        {
            return _cardSlots.Where(cardSlot => cardSlot.Item != null).Count(cardSlot => cardSlot.Item.GetComponent<Dog>().ReturnSex() == "Female");
        }


        public void UpdateGeneration()
        {
            for (int i = 0; i < _cardSlots.Length; i++)
            {
                if (_cardSlots[i].Item)
                    _cardSlots[i].Item.GetComponent<Dog>().FindSiblings();
            }
        }
    }
}
