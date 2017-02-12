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
        private CardSlot _parentCardSlot;
        private CanvasGroup _canvasGroup;
        [SerializeField]private Text _name;
        [SerializeField]private Text _stats;
        [SerializeField]private Text _numbers;
        [SerializeField]private Sprite[] _giantDogs;
        [SerializeField]private Sprite[] _largeDogs;
        [SerializeField]private Sprite[] _mediumDogs;
        [SerializeField]private Sprite[] _smallDogs;
        [SerializeField]private Sprite _finalDog;
        [SerializeField]private Image _dogImage;
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
            _parentCardSlot = _parent.GetComponent<CardSlot>();
            _name.text = GetComponent<Dog>().ReturnName();
            _stats.text = SetStats();
            _numbers.text = SetStatNumberText();
            _canvasGroup = GetComponent<CanvasGroup>();
            SetImage();
           
        }
	
        // Update is called once per frame
        private void Update()
        {
            if (transform.parent.tag == "breedingSlotHolder" || transform.parent.tag == "Holder" ||
                transform.parent.tag == "cardHolder" || transform.parent.tag == "cardReviewSlot" ||
                transform.parent.tag == "FinalDogSlot")
            {
                _reviewButton.interactable = false;
                _reviewButton.blocksRaycasts = false;
                _reviewButton.alpha = 0;
               
            }
            else
            {
                _reviewButton.interactable = true;
                _reviewButton.blocksRaycasts = true;
                _reviewButton.alpha = 1;
                _parent = transform.parent;
                _canvasGroup.blocksRaycasts = !_parentCardSlot.Disabled;
            }
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, 0);
            if (_parentCardSlot == null) return;
            if (_parentCardSlot.Disabled)
                _canvasGroup.alpha = 0.5f;
            else
                _canvasGroup.alpha = 1;
        }

        //Create string with all stats
        private string SetStatNumberText()
        {
            System.Text.StringBuilder statsNumbersBuilder = new System.Text.StringBuilder();
           
            if (_dog.ReturnDemeanor() >= 60)
                statsNumbersBuilder.Append(_dog.ReturnDemeanor() / 10 + "/10" + Environment.NewLine);
            if (_dog.ReturnTrainability() >= 60)
                statsNumbersBuilder.Append(_dog.ReturnTrainability() / 10 + "/10" + Environment.NewLine);
            if (_dog.ReturnEndurance() >= 60)
                statsNumbersBuilder.Append(_dog.ReturnEndurance() / 10 + "/10" + Environment.NewLine);
            if (_dog.ReturnScent() >= 60)
                statsNumbersBuilder.Append(_dog.ReturnScent() / 10 + "/10" + Environment.NewLine);
            if (_dog.ReturnSight() >= 60)
                statsNumbersBuilder.Append(_dog.ReturnSight() / 10 + "/10" + Environment.NewLine);
            if (_dog.ReturnHearing() >= 60)
                statsNumbersBuilder.Append(_dog.ReturnHearing() / 10 + "/10" + Environment.NewLine);
            if (_dog.ReturnBark() >= 60)
                statsNumbersBuilder.Append(_dog.ReturnBark() / 10 + "/10" + Environment.NewLine);
            return statsNumbersBuilder.ToString();
        }
        //Use dog componment to populate the text describing the card
        public string SetStats()
        {
            System.Text.StringBuilder statsBuilder = new System.Text.StringBuilder();
           
            if (_dog.ReturnDemeanor() >= 60)
                statsBuilder.Append("Demeanor:" + Environment.NewLine);
            if (_dog.ReturnTrainability() >= 60)
                statsBuilder.Append("Trainability:" + Environment.NewLine);
            if (_dog.ReturnEndurance() >= 60)
                statsBuilder.Append("Endurance:" + Environment.NewLine);
            if (_dog.ReturnScent() >= 60)
                statsBuilder.Append("Scent:" + Environment.NewLine);
            if (_dog.ReturnSight() >= 60)
                statsBuilder.Append("Sight:" + Environment.NewLine);
            if (_dog.ReturnHearing() >= 60)
                statsBuilder.Append("Hearing:" + Environment.NewLine);
            if (_dog.ReturnBark() >= 60)
                statsBuilder.Append("Bark:" + Environment.NewLine);


          
            return statsBuilder.ToString();
        }

        //Return this card to its original parent. If the original parent is occupied set the first unnocupied one as it's new parent
        public void ReturnToParent()
        {

            if (_parentCardSlot.Item != null)
            {
                CardSlot[] cardSlots =GameManager.Instance.GenerationManager.ReturnCurrentGeneration().ReturnCardSlots();
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
