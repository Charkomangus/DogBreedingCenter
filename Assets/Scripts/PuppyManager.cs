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
            _areYouSure = GameObject.FindGameObjectWithTag("AreYouSure").GetComponent<Animator>();
        }

        //Display the Puppies
        public void OpenPuppyManager(GameObject[] puppies)
        {   
            _texts = GetComponentInChildren<TextHolder>().ReturnText();
            for (int i = 0; i < puppies.Length; i++)
            {
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
            }
            else
            {
                GameManager.Instance.GenerationManager.UpdateGeneration();
                _futureGeneration = GameManager.Instance.GenerationManager.ReturnFutureGeneration();
                _futureGeneration.AddCard(chosenPuppy);
                _futureGeneration.UpdateGeneration();
            }
            for (int i = 0; i < _slots.Length; i++)
            {
                if(_slots[i].Item)
                    Destroy(_slots[i].Item);
            }
            ClosePuppyManager();
        }


        //Take chosen dog and assign them as the new dog
        public void NoDog()
        {
            for (int i = 0; i < _slots.Length; i++)
            {
                if (_slots[i].Item)
                    Destroy(_slots[i].Item);
            }
            ClosePuppyManager();

        }




        //Set dog text
        private string SetStatText(Dog _dog)
        {
            System.Text.StringBuilder statsBuilder = new System.Text.StringBuilder();
            statsBuilder.Append(_dog.ReturnName() + " is a " + _dog.ReturnSex().ToLower() + ", " +_dog.ReturnSizeDescription().ToLower() + " sized dog with a " +_dog.ReturnHairLengthDescription().ToLower() + " length " + "coat." +Environment.NewLine + Environment.NewLine);
       
            statsBuilder.Append("Demeanor: "  + _dog.ReturnDemeanor() / 10 + "/10" + Environment.NewLine);
            statsBuilder.Append("Trainability: " + _dog.ReturnTrainability() / 10 + "/10" + Environment.NewLine);
            statsBuilder.Append("Endurance: "  + _dog.ReturnEndurance() / 10 + "/10" + Environment.NewLine);
            statsBuilder.Append("Scent: " + _dog.ReturnScent() / 10 + "/10" + Environment.NewLine);
            statsBuilder.Append("Sight: " + _dog.ReturnSight() / 10 + "/10" + Environment.NewLine);
            statsBuilder.Append("Hearing: " + _dog.ReturnHearing() / 10 + "/10" + Environment.NewLine);
            statsBuilder.Append("Bark: " +  _dog.ReturnBark() / 10 + "/10" + Environment.NewLine);
            return statsBuilder.ToString();
        }

        public void AreYouSure(bool status)
        {
           _areYouSure.SetBool("Open", status);
        }



     


    }
}
