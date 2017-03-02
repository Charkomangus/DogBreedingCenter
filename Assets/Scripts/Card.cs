using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using LoLSDK;
using Random = UnityEngine.Random;

namespace Assets.Scripts
{
    public class Card : MonoBehaviour
    {
       
        [SerializeField]private Transform _parent;
        [SerializeField]private CanvasGroup _canvasGroup;
        [SerializeField]private Text _name;
        [SerializeField]private Text _stats;
        [SerializeField]private Text _numbers;
        [SerializeField]private Sprite[] _giantDogs;
        [SerializeField]private Sprite[] _largeDogs;
        [SerializeField]private Sprite[] _mediumDogs;
        [SerializeField]private Sprite[] _smallDogs;
        [SerializeField]private Sprite _finalDog;
        [SerializeField]private Image _dogImage;
        private string _parentTag;
        private Sprite[] _currentSize;
        private Dog _dog;
        [SerializeField]
        private CanvasGroup _reviewButton;


        // Use this for initialization
        private void Start ()
        {
            _reviewButton = GetComponentInChildren<Button>().GetComponent<CanvasGroup>();
            _dog = GetComponent<Dog>();
            _parent = transform.parent;
            _parentTag = _parent.tag;
            _name.text = GetComponent<Dog>().ReturnName();
            _stats.text = SetStats();
            _numbers.text = SetStatNumberText();
            _canvasGroup = GetComponent<CanvasGroup>();
            SetImage();
           
        }
	
        // Update is called once per frame
        private void Update()
        {
          
            if (_parentTag == "breedingSlotHolder" || _parentTag == "Holder" || _parentTag == "cardHolder" || _parentTag == "cardReviewSlot" || _parentTag == "FinalDogSlot" || _parentTag == "PuppySlots" )
            {
                _reviewButton.interactable = false;
                _reviewButton.blocksRaycasts = false;
                _reviewButton.alpha = 0;
                _canvasGroup.alpha = 1;
            }
            else if (_parent.GetComponent<CardSlot>().Disabled)
            {
                _canvasGroup.alpha = 0.75f;
                _reviewButton.interactable = true;
                _reviewButton.blocksRaycasts = true;
                _reviewButton.alpha = 1;
                _parent = transform.parent;
                _parentTag = _parent.tag;
            }
            else
            {
                _canvasGroup.alpha = 1;
                _reviewButton.interactable = true;
                _reviewButton.blocksRaycasts = true;
                _reviewButton.alpha = 1;
                _parent = transform.parent;
                _parentTag = _parent.tag;
            }  
        }

        //Create string with all stats
        private string SetStatNumberText()
        {
            System.Text.StringBuilder statsNumbersBuilder = new System.Text.StringBuilder();

            if (_dog.ReturnIntelligence() >= 60)statsNumbersBuilder.Append(Mathf.FloorToInt(_dog.ReturnIntelligence() / 10) + "/10" + Environment.NewLine);
            if (_dog.ReturnEndurance() >= 60)statsNumbersBuilder.Append(Mathf.FloorToInt(_dog.ReturnEndurance() / 10) + "/10" + Environment.NewLine);
            if (_dog.ReturnDemeanor() >= 60)statsNumbersBuilder.Append(Mathf.FloorToInt(_dog.ReturnDemeanor() / 10) + "/10" + Environment.NewLine);
            if (_dog.ReturnStrength() >= 60)statsNumbersBuilder.Append(Mathf.FloorToInt(_dog.ReturnStrength() / 10) + "/10" + Environment.NewLine);
            if (_dog.ReturnHearing() >= 60) statsNumbersBuilder.Append(Mathf.FloorToInt(_dog.ReturnHearing() / 10) + "/10" + Environment.NewLine);
            if (_dog.ReturnScent() >= 60)statsNumbersBuilder.Append(Mathf.FloorToInt(_dog.ReturnScent() / 10) + "/10" + Environment.NewLine);
            if (_dog.ReturnSight() >= 60)statsNumbersBuilder.Append(Mathf.FloorToInt(_dog.ReturnSight() / 10) + "/10" + Environment.NewLine);
            if (_dog.ReturnBark() >= 60)statsNumbersBuilder.Append(Mathf.FloorToInt(_dog.ReturnBark() / 10) + "/10" + Environment.NewLine);

            return statsNumbersBuilder.ToString();
        }
     
     
        //Use dog componment to populate the text describing the card
        public string SetStats()
        {
            System.Text.StringBuilder statsBuilder = new System.Text.StringBuilder();

            if (_dog.ReturnIntelligence() >= 60)
                statsBuilder.Append("Intelligence :" + Environment.NewLine);
            if (_dog.ReturnEndurance() >= 60)
                statsBuilder.Append("Endurance:" + Environment.NewLine);
            if (_dog.ReturnDemeanor() >= 60)
                statsBuilder.Append("Demeanor:" + Environment.NewLine);
            if (_dog.ReturnStrength() >= 60)
                statsBuilder.Append("Strength:" + Environment.NewLine);
            if (_dog.ReturnHearing() >= 60)
                statsBuilder.Append("Hearing:" + Environment.NewLine);
            if (_dog.ReturnScent() >= 60)
                statsBuilder.Append("Scent:" + Environment.NewLine);
            if (_dog.ReturnSight() >= 60)
                statsBuilder.Append("Sight:" + Environment.NewLine);
            if (_dog.ReturnBark() >= 60)
                statsBuilder.Append("Bark:" + Environment.NewLine);
            
          
            return statsBuilder.ToString();
        }

        //Return this card to its original parent. If the original parent is occupied set the first unnocupied one as it's new parent
        public void ReturnToParent()
        {
            if (_parent == null || _parent.GetComponent<CardSlot>().Item != null)
            {
                CardSlot[] cardSlots = GameManager.Instance.GenerationManager.ReturnCurrentGeneration().ReturnCardSlots();
                for (int i = 0; i < cardSlots.Length; i++)
                {
                    if (cardSlots[i].Item == null)
                        _parent = cardSlots[i].transform;
                }
            }
            transform.SetParent(_parent);
            transform.localScale = new Vector3(_parent.GetComponent<CardSlot>().Scale, _parent.GetComponent<CardSlot>().Scale, 1);
        }

        public Transform ReturnParent()
        {
            return _parent;
        }

   
        //Set this card as the chosen card of the CardReview
        public void OpenCardReview()
        {
            GameManager.Instance.SoundManager.PlaySoundEffect(Random.Range(0, 2) == 0? "Sound/Bark.wav": "Sound/doubleBark.wav");
            GameManager.Instance.SideBar.SetBool("Open", false);
            GameManager.Instance.CardReview.GetComponent<Animator>().SetBool("Open", true);
            transform.SetParent(GameManager.Instance.CardReview.ReturnCardSlot().transform);
            transform.localScale = Vector3.one;
        }


        //Set this card as the chosen card of the CardReview
        public void OpenFinalDog()
        {
            GameManager.Instance.FinalDogManager.GetComponent<Animator>().SetBool("Open", true);
            GameManager.Instance.FinalDogManager.SetFinalDog(gameObject, _finalDog);


        }


        //Choose apropriate image to show dog
        public void SetImage(Sprite image)
        {
            _dogImage.sprite = image;
        }

        //Choose apropriate image to show dog
        private void SetImage()
        {
            if (GameManager.Instance.FinalDogManager != null)
            {
                if (GameManager.Instance.FinalDogManager.GetComponent<Animator>().GetBool("Open"))
                {
                    _dogImage.sprite = _finalDog;
                    return;
                }
            }
            //Determine what size the dog is
            if (_dog.ReturnSize() <= 30)
            {
                _currentSize = _smallDogs;
            }
            else if (_dog.ReturnSize() <= 60)
            {
                _currentSize = _mediumDogs;
            }
            else if (_dog.ReturnSize() <= 90)
            {
                _currentSize = _largeDogs;
            }
            else
            {
                _currentSize = _giantDogs;
            }


            //Determine what hair lenth it has
            if (_dog.ReturnHair() <= 33)
            {
                _dogImage.sprite = _currentSize[0];
            }
            else if (_dog.ReturnHair() <= 66)
            {
                _dogImage.sprite = _currentSize[1];
            }
            else
            {
                _dogImage.sprite = _currentSize[2];
            }

        }


      
    }
}
