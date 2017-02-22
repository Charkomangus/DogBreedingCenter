using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using LoLSDK;


namespace Assets.Scripts
{
    public class PuppyManager : MonoBehaviour
    {
        [SerializeField]
        private Text[] _texts;
        [SerializeField]private CardSlot[] _slots;
        private CanvasGroup[] _slotCanvasGroups;
        private Animator _animator;
        public GameObject Holder;
        private Generation _futureGeneration;
        private Animator _areYouSure;
         [SerializeField]public GameObject chosenPuppy;

        // Use this for initialization
        void Start()
        {
            _animator = GetComponent<Animator>();
            _animator.SetBool("Open", false);
            _texts = GetComponentInChildren<TextHolder>().ReturnText();
          
            _slots = GetComponentsInChildren<CardSlot>();
            _slotCanvasGroups = new CanvasGroup[_slots.Length];
            for (int i = 0; i < _slots.Length; i++)
            {
                _slotCanvasGroups[i] = _slots[i].GetComponent<CanvasGroup>();
                _slotCanvasGroups[i].interactable = false;
                _slotCanvasGroups[i].blocksRaycasts = false;
            }
            _areYouSure = GameObject.FindGameObjectWithTag("AreYouSure").GetComponent<Animator>();
        }

        //Display the Puppies
        public void OpenPuppyManager(GameObject[] puppies)
        {   
            _texts = GetComponentInChildren<TextHolder>().ReturnText();
            for (int i = 0; i < puppies.Length; i++)
            {
                _slotCanvasGroups[i].blocksRaycasts = true;
                _slotCanvasGroups[i].interactable = true;
                puppies[i].transform.SetParent(_slots[i].transform);
                puppies[i].transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
                _texts[i].text = SetStatText(_slots[i].Item.GetComponent<Dog>());
                _animator.SetBool("Open", true);
            }

            for (int i = puppies.Length; i < _slots.Length; i++)
            {
                _slots[i].transform.SetParent(Holder.transform);
                _texts[i].transform.SetParent(Holder.transform);
            }

        }

        //Display the Puppies
        public void ClosePuppyManager()
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                _slotCanvasGroups[i].blocksRaycasts = false;
                _slotCanvasGroups[i].interactable = false;
                _slots[i].transform.SetParent(GameObject.FindGameObjectWithTag("puppyGeneration").transform);
                _texts[i].transform.SetParent(GetComponentInChildren<TextHolder>().transform);
            }
          
            _animator.SetBool("Open", false);
            AreYouSure(false);

            GameManager.Instance.SideBar.SetBool("Open", true);
        }

        //Take chosen dog and assign them as the new dog
        public void ChooseDog()
        {
            _futureGeneration = GameManager.Instance.GenerationManager.ReturnFutureGeneration(); 
            if (_futureGeneration.ReturnAvailableSlot() != null)
            {
                
                _futureGeneration.AddCard(chosenPuppy);
                _futureGeneration.FindSiblings();
            }
            else
            {
                GameManager.Instance.GenerationManager.UpdateGeneration();
                _futureGeneration = GameManager.Instance.GenerationManager.ReturnFutureGeneration();
                _futureGeneration.AddCard(chosenPuppy);
                _futureGeneration.FindSiblings();
            }
            for (int i = 0; i < _slots.Length; i++)
            {
                if(_slots[i].Item)
                    Destroy(_slots[i].Item);
            }
            ClosePuppyManager();
            GameManager.Instance.BreedingManager.BreedingType = 0;

        }


        //Take chosen dog and assign them as the new dog
        public void NoDog()
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i].Item)
                    Destroy(_slots[i].Item);
            }
            switch (GameManager.Instance.BreedingManager.BreedingType)
            {
                case 1:
                    GameManager.Instance.GeneticVarience.Value += 15;
                    GameManager.Instance.BreedingManager.BreedingType = 0;
                    break;
                case 2:
                    GameManager.Instance.GeneticVarience.Value += 5;
                    GameManager.Instance.BreedingManager.BreedingType = 0;
                    break;
            }
            ClosePuppyManager();

        }




        //Set dog text
        private string SetStatText(Dog _dog)
        {
            System.Text.StringBuilder statsBuilder = new System.Text.StringBuilder();
            statsBuilder.Append(_dog.ReturnName() + " is a " + _dog.ReturnSex().ToLower() + ", " +_dog.ReturnSizeDescription().ToLower() + " sized dog with a " +_dog.ReturnHairLengthDescription().ToLower() + " length " + "coat." +Environment.NewLine + Environment.NewLine);
            statsBuilder.Append("Intelligence : " + Mathf.FloorToInt(_dog.ReturnIntelligence() / 10) + "/10" + Environment.NewLine);
            statsBuilder.Append("Endurance: " + Mathf.FloorToInt(_dog.ReturnEndurance() / 10) + "/10" + Environment.NewLine);
            statsBuilder.Append("Demeanor: "  + Mathf.FloorToInt(_dog.ReturnDemeanor() / 10) + "/10" + Environment.NewLine);
            statsBuilder.Append("Strength: " + Mathf.FloorToInt(_dog.ReturnStrength() / 10) + "/10" + Environment.NewLine);
            statsBuilder.Append("Hearing: " + Mathf.FloorToInt(_dog.ReturnHearing() / 10) + "/10" + Environment.NewLine);
            statsBuilder.Append("Scent: " + Mathf.FloorToInt(_dog.ReturnScent() / 10) + "/10" + Environment.NewLine);
            statsBuilder.Append("Sight: " + Mathf.FloorToInt(_dog.ReturnSight() / 10) + "/10" + Environment.NewLine);
            statsBuilder.Append("Bark: " + Mathf.FloorToInt(_dog.ReturnBark() / 10) + "/10" + Environment.NewLine);


            return statsBuilder.ToString();
        }

        public void AreYouSure(bool status)
        {
           _areYouSure.SetBool("Open", status);
        }



     


    }
}
