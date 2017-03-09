using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using LoLSDK;

namespace Assets.Scripts
{
    public class BreedingManager : MonoBehaviour, IHasChanged
    {
        [SerializeField] private Transform _cardSlots;
        [SerializeField] private Text[] _cardStatsText;
        [SerializeField] private Text[] _cardStatsNumberText;
        [SerializeField] private GameObject[] _item;
        [SerializeField] public GameObject WarningPanel, WarningPanel1;
        [SerializeField] private GameObject _cardPrefab;
        public int BreedingType;
        public bool _firstBirthDone, _secondBirthDone;
        private Generation _futureGeneration;
        private PuppyManager _puppyManager;
        private GameObject _generationManager;
        private GameObject _geneticVariance;
        private GameObject _winningDog;
        public GameObject Holder;
        [SerializeField] private int _decrease;
        [SerializeField] private int _increase;
        private bool _finalDogFound;

        // Use this for initialization
        void Start()
        {
            _finalDogFound = false;
            _item = new GameObject[_cardStatsText.Length];
            _generationManager = GameObject.FindWithTag("Main");
            _geneticVariance = GameObject.FindWithTag("GeneticVariance");
            _puppyManager = GameObject.FindWithTag("PuppyManager").GetComponent<PuppyManager>();
            _cardPrefab = GameManager.Instance.ChosenCardPrefab;
            HasChanged();
            if (_decrease == 0)
                _decrease = -5;
            if (_increase == 0)
                _decrease = 20;

            if (GameManager.Instance.CurrentLevel == "Level1")
            {
                _firstBirthDone = false;
                _secondBirthDone = false;
            }
            else if(GameManager.Instance.CurrentLevel == "Level0")
            {
                _firstBirthDone = true;
                _secondBirthDone = true;
            }
            else
            {
                _firstBirthDone = true;
                _secondBirthDone = false;
            }
        }

        #region IHasChanged implementation

        //Populates Text description
        public void HasChanged()
        {
            System.Text.StringBuilder statsBuilder = new System.Text.StringBuilder();
            System.Text.StringBuilder statsNumbersBuilder = new System.Text.StringBuilder();
            System.Text.StringBuilder nameBuilder = new System.Text.StringBuilder();
            for (int i = 0; i < _cardStatsText.Length; i++)
            {
                _item[i] = _cardSlots.GetComponentsInChildren<CardSlot>()[i].Item;
            }

            for (int i = 0; i < _item.Length; i++)
            {
                if (_item[i])
                {
                    nameBuilder.Append(_item[i].GetComponent<Dog>().ReturnName());



                    statsBuilder.Append("Intelligence : " + Environment.NewLine);
                    statsBuilder.Append("Endurance: " + Environment.NewLine);
                    statsBuilder.Append("Strength: " + Environment.NewLine);
                    statsBuilder.Append("Demeanor: " + Environment.NewLine);
                    statsBuilder.Append("Hearing: " + Environment.NewLine);
                    statsBuilder.Append("Scent: " + Environment.NewLine);
                    statsBuilder.Append("Sight: " + Environment.NewLine);
                    statsBuilder.Append("Bark: " + Environment.NewLine);


                    statsNumbersBuilder.Append(Mathf.FloorToInt(_item[i].GetComponent<Dog>().ReturnIntelligence() / 10) + "/10" + Environment.NewLine);
                    statsNumbersBuilder.Append(Mathf.FloorToInt(_item[i].GetComponent<Dog>().ReturnEndurance() / 10) + "/10" + Environment.NewLine);
                    statsNumbersBuilder.Append(Mathf.FloorToInt(_item[i].GetComponent<Dog>().ReturnStrength() / 10) + "/10" + Environment.NewLine);
                    statsNumbersBuilder.Append(Mathf.FloorToInt(_item[i].GetComponent<Dog>().ReturnDemeanor() / 10) + "/10" + Environment.NewLine);
                    statsNumbersBuilder.Append(Mathf.FloorToInt(_item[i].GetComponent<Dog>().ReturnHearing() / 10) + "/10" + Environment.NewLine);
                    statsNumbersBuilder.Append(Mathf.FloorToInt(_item[i].GetComponent<Dog>().ReturnScent() / 10) + "/10" + Environment.NewLine);
                    statsNumbersBuilder.Append(Mathf.FloorToInt(_item[i].GetComponent<Dog>().ReturnSight() / 10) + "/10" + Environment.NewLine);
                    statsNumbersBuilder.Append(Mathf.FloorToInt(_item[i].GetComponent<Dog>().ReturnBark() / 10) + "/10" + Environment.NewLine);



                    
                



                }
                _cardStatsNumberText[i].text = statsNumbersBuilder.ToString();
                _cardStatsText[i].text = statsBuilder.ToString();
                statsBuilder.Remove(0, statsBuilder.Length);
                statsNumbersBuilder.Remove(0, statsNumbersBuilder.Length);
                nameBuilder.Remove(0, nameBuilder.Length);
            }
        }

        #endregion

        // Clears Text if no dogs are present
        private void Update()
        {
            for (int i = 0; i < _cardSlots.childCount; i++)
            {
                if (_cardSlots.GetComponentsInChildren<CardSlot>()[i].Item == null)
                {
                    _cardStatsText[i].text = System.String.Empty;
                    _cardStatsNumberText[i].text = System.String.Empty;
                }
                HasChanged();

            }

        }


        // Exits menu -> Any cards left in the menu are returned to their original parent, resets text
        private void ExitBreedingMenu()
        {
            if (WarningPanel.activeSelf) return;
            for (int i = 0; i < _cardStatsText.Length; i++)
            {
                if (!_item[i]) continue;
                _item[i] = _cardSlots.GetComponentsInChildren<CardSlot>()[i].Item;
                _item[i].GetComponent<Card>().ReturnToParent();

                _cardStatsText[i].text = string.Empty;
                _cardStatsNumberText[i].text = string.Empty;
                _item[i] = null;
            }
            GetComponent<Animator>().SetBool("Open", false);
        }

        // Breeds the two Dogs
        public void Breed()
        {
            int numberOfDogs = 0;
            if (WarningPanel.activeSelf) return;

            for (int i = 0; i < _cardStatsText.Length; i++)
            {
                if (_cardSlots.GetComponentsInChildren<CardSlot>()[i].Item != null)
                    numberOfDogs++;
            }

            switch (numberOfDogs)
            {
                case 0:
                    WarningPanel.GetComponentInChildren<Text>().text = "No dogs selected!";
                    WarningPanel.SetActive(true);
                    break;
                case 1:
                    WarningPanel.GetComponentInChildren<Text>().text = "You need two dogs in order to breed them!";
                    WarningPanel.SetActive(true);
                    break;
                default:
                    if (_cardSlots.GetComponentsInChildren<CardSlot>()[0].Item.GetComponent<Dog>().ReturnSex() ==
                        _cardSlots.GetComponentsInChildren<CardSlot>()[1].Item.GetComponent<Dog>().ReturnSex())
                    {
                        WarningPanel.GetComponentInChildren<Text>().text = "The dogs need to be a different gender!";
                        WarningPanel.SetActive(true);
                    }
                    else if (
                        _cardSlots.GetComponentsInChildren<CardSlot>()[0].Item.GetComponent<Dog>()
                            .ReturnSiblings()
                            .Contains(_cardSlots.GetComponentsInChildren<CardSlot>()[1].Item))
                    {
                        WarningPanel1.GetComponentInChildren<Text>().text =
                            "These dogs are siblings! If you breed them you will lose 15% of the genetic diversity!";
                        WarningPanel1.SetActive(true);
                        BreedingType = 1;
                    }
                    else if (_cardSlots.GetComponentsInChildren<CardSlot>()[0].Item.GetComponent<Dog>().ReturnHalfSiblings().Contains(_cardSlots.GetComponentsInChildren<CardSlot>()[1].Item))
                    {
                        WarningPanel1.GetComponentInChildren<Text>().text =
                            "These dogs are half-siblings! If you breed them you will lose 5% of the genetic diversity!";
                        WarningPanel1.SetActive(true);
                        BreedingType = 2;
                    }
                    else
                    {
                        if (!_firstBirthDone)
                        {
                            _firstBirthDone = true;
                            GameObject[] puppies = new GameObject[2];
                            for (int i = 0; i < 2; i++)
                            {
                                puppies[i] = MakeNewDog(-40, -30);
                                puppies[i].transform.SetParent(Holder.transform);
                            }
                            _puppyManager.OpenPuppyManager(puppies);
                            GameManager.Instance.SoundManager.PlaySoundEffect("Sound/puppy.wav");
                            GameManager.Instance.DialogueManager.OpenDialogue(
                                GameManager.Instance.CurrentLevel +
                                "/firstBirth");

                        }
                        else
                        {
                            int temp = Random.Range(1, 5);
                            GameObject[] puppies = new GameObject[temp];
                            for (int i = 0; i < temp; i++)
                            {
                                puppies[i] = MakeNewDog();
                                puppies[i].transform.SetParent(Holder.transform);
                                if (GameManager.Instance.IsFinalDog(puppies[i].GetComponent<Dog>()))
                                {
                                    _finalDogFound = true;
                                    _winningDog = puppies[i];
                                }

                            }
                            if (_finalDogFound)
                                _winningDog.GetComponent<Card>().OpenFinalDog();
                            else
                            {
                                if (!_secondBirthDone)
                                {
                                    _secondBirthDone = true;
                                    GameManager.Instance.DialogueManager.OpenDialogue(
                                        GameManager.Instance.CurrentLevel +
                                        "/secondBirth");
                                }
                                _puppyManager.OpenPuppyManager(puppies);
                                GameManager.Instance.SoundManager.PlaySoundEffect("Sound/puppy.wav");
                            }
                        }
                        ExitBreedingMenu();
                        GameManager.Instance.SideBar.SetBool("Open", false);
                    }
                    break;
            }
        }

        // Breed with no pesky warnings
        public void BreedWithNoWarnings()
        {
            WarningPanel1.SetActive(false);
            int temp = Random.Range(1, 5);
            GameObject[] puppies = new GameObject[temp];
            for (int i = 0; i < temp; i++)
            {
                puppies[i] = MakeNewDog();
                puppies[i].transform.SetParent(Holder.transform);
                if (GameManager.Instance.IsFinalDog(puppies[i].GetComponent<Dog>()))
                {
                    _finalDogFound = true;
                    _winningDog = puppies[i];
                }

            }
            if (_finalDogFound)
                _winningDog.GetComponent<Card>().OpenFinalDog();
            else
            {
                if (!_secondBirthDone)
                {
                    _secondBirthDone = true;
                    GameManager.Instance.DialogueManager.OpenDialogue(GameManager.Instance.CurrentLevel + "/secondBirth");
                }
                _puppyManager.OpenPuppyManager(puppies);
                GameManager.Instance.SoundManager.PlaySoundEffect("Sound/puppy.wav");

                switch (BreedingType)
                {
                    case 1:
                        GameManager.Instance.GeneticVarience.Value -= 15;
                        break;
                    case 2:
                        GameManager.Instance.GeneticVarience.Value -= 5;
                        break;
                }
            }

            ExitBreedingMenu();
            GameManager.Instance.SideBar.SetBool("Open", false);
        }

        // Open the menu
        public void OpenBreedingMenu(bool status)
        {
            GetComponent<Animator>().SetBool("Open", status);
        }

        private GameObject MakeNewDog()
        {
            return MakeNewDog(_decrease, _increase);
        }

        private GameObject MakeNewDog(int min, int max)
        {
            _futureGeneration = GameManager.Instance.GenerationManager.ReturnFutureGeneration();
            if (_futureGeneration.ReturnAvailableSlot() == null)
          {
                _generationManager.GetComponent<GenerationManager>().UpdateGeneration();
               _futureGeneration = _generationManager.GetComponent<GenerationManager>().ReturnFutureGeneration();
               
            }

            GameObject dog = Instantiate(_cardPrefab);
            Dog newDog = dog.GetComponent<Dog>();
            
            // Make sure to balance genders
            if (_futureGeneration.GetComponent<Generation>().ReturnMales() >_futureGeneration.GetComponent<Generation>().ReturnFemales())
            {
                newDog.SetSex("Female");
            }
            else if (_futureGeneration.GetComponent<Generation>().ReturnMales() <_futureGeneration.GetComponent<Generation>().ReturnFemales())
            {
                newDog.SetSex("Male");
            }
            else
            {
                newDog.SetSex();
            }
            // Set Name & Sex
            newDog.SetName();

            // Set parents
            Dog parent0 = _cardSlots.GetComponentsInChildren<CardSlot>()[0].Item.GetComponent<Dog>();
            Dog parent1 = _cardSlots.GetComponentsInChildren<CardSlot>()[1].Item.GetComponent<Dog>();


            newDog.SetParents(parent0.gameObject, parent1.gameObject);
            // Set Size
            newDog.SetSize(Constrain((parent0.ReturnSize() + parent1.ReturnSize())/2 + Random.Range(_decrease, _increase)));
            // Set Bark
            newDog.SetBark(Constrain((parent0.ReturnBark() + parent1.ReturnBark())/2 + Random.Range(min, max)));
            // Set Endurance
            newDog.SetEndurance(Constrain((parent0.ReturnEndurance() + parent1.ReturnEndurance())/2 + Random.Range(min, max)));
            // Set Scent
            newDog.SetScent(Constrain((parent0.ReturnScent() + parent1.ReturnScent())/2 + Random.Range(min, max)));
            // Set Hearing
            newDog.SetHearing(Constrain((parent0.ReturnHearing() + parent1.ReturnHearing())/2 + Random.Range(min, max)));
            // Set Demeanor
            newDog.SetDemeanor(Constrain((parent0.ReturnDemeanor() + parent1.ReturnDemeanor())/2 + Random.Range(min, max)));
            // Set Sight
            newDog.SetSight(Constrain((parent0.ReturnSight() + parent1.ReturnSight())/2 + Random.Range(min, max)));
            // Set Intelligence 
            newDog.SetIntelligence (Constrain((parent0.ReturnIntelligence () + parent1.ReturnIntelligence ())/2 +Random.Range(min, max)));
            // Set Hair
            newDog.SetHairLength(Constrain((parent0.ReturnHair() + parent1.ReturnHair())/2 + Random.Range(_decrease, _increase)));
            // Set Strenth
            newDog.SetStrength(Constrain((parent0.ReturnStrength() + parent1.ReturnStrength())/2 + Random.Range(min, max)));


            return dog;
        }

        private static int Constrain(int value)
        {
            if (value > 100)
                return 100;
            if (value < 0)
                return 0;
            return value;
        }   
    }
}



