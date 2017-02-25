using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using LoLSDK;

namespace Assets.Scripts
{
    public class DialogueManager : MonoBehaviour, IPointerDownHandler
    {

        private float _letterPause;
        [SerializeField]private Animator[] _animators;
        [SerializeField]
        private Animator _proffessorAnimator, _catBarAnimator,_arrow,_cat, _proffessorCrash, _fade;
        private float _originalLetterPauseValue;
        private string _filepath;
   
        private bool _speedUp;
        private bool _finished;
        
        private int _index;
        private bool _open;
        private bool _catOpen;
        private Text _dialogue;
        private TextAsset _message;
        private Polaroid _polaroid;
        [SerializeField]
        private Sprite _oldProffessor;

        //>>>TOOLTIPS<<<<<
        private bool _tooltip;
        [SerializeField]private Animator[] _tooltipAnimators;
        private int _tooltipCounter = 0;
        private bool _cardReviewOpen, _breedingMenuOpen;
        private Card _randomDog;
        //>>>TOOLTIPS<<<<<
        
      
        public void Awake()
        {
            _letterPause = 0.03f;
            _dialogue = GetComponentInChildren<Text>();
            _animators = GetComponentsInChildren<Animator>();
            for (int i = 0; i < _animators.Length; i++)
            {
                switch (_animators[i].transform.name)
                {
                    case "Proffessor":
                        _proffessorAnimator = _animators[i];
                        break;
                    case "CatBar":
                        _catBarAnimator = _animators[i];
                        break;
                    case "Arrow":
                        _arrow = _animators[i];
                        break;
                    case "Cat":
                        _cat = _animators[i];
                        break;
                    case "ProffessorCrash":
                        _proffessorCrash = _animators[i];
                        break;
                    case "FadePanel":
                        _fade = _animators[i];
                        break;
                }
            }
            _originalLetterPauseValue = _letterPause;

            if (GameObject.FindGameObjectWithTag("Polaroid") != null)
                _polaroid = GameObject.FindGameObjectWithTag("Polaroid").GetComponent<Polaroid>();
          
            if (GameObject.FindGameObjectWithTag("Dog") != null)
                _randomDog = GameObject.FindGameObjectWithTag("Dog").GetComponent<Card>();
        
        }
        //Type text letter by letter
        private IEnumerator TypeText()
        {
            if (_message == null) yield break;
            for (int i = 1; i < _message.text.Length; i++)
            {
                    _dialogue.text += _message.text[i];
                    yield return new WaitForSeconds(_letterPause * Time.deltaTime);
            }
            _finished = true;
        }

        //Set all variables to starting positions and start text coroutine
        public void OpenDialogue(string filepath)
        {
            if (_open || _finished)return;


            //If its a tutorial dialogue only show the tooltips
            if (filepath == "tutorial")
            {
                _open = true;
                _tooltipAnimators = GameObject.FindGameObjectWithTag("Tooltip").GetComponent<Tooltip>().ReturnTooltips();
                _tooltipCounter = 0;
               _fade.SetBool("Open", true);    //Darken Screen
                _tooltip = true;
                _filepath = System.String.Empty;
                OpenTooltips(_tooltipCounter);
                var pointer = new PointerEventData(EventSystem.current);
                ExecuteEvents.Execute(gameObject, pointer, ExecuteEvents.pointerDownHandler);
               
            }
          //Bring the dialogue box up and start the text co routine
            else
            {
                for (int i = 0; i < _animators.Length; i++)
                {
                    if(_animators[i] != _proffessorAnimator || _animators[i] != _catBarAnimator)
                        _animators[i].SetBool("Open", true);
                }
                _message = LoadText(filepath);
                _dialogue = GetComponentInChildren<Text>();
                _dialogue.text = string.Empty;
                StartCoroutine(TypeText());
                EventCheck(_message.text[0]);
                EventCheck(_message.text[1]);
                _open = true;
                _filepath = filepath;
            }
        }




        //Move on to next piece of dialogue
        private void NextDialogue()
        {
            _letterPause = _originalLetterPauseValue;
            _finished = false;
            _speedUp = false;
            _index++;
            _message = LoadText(_filepath + _index);

            GameManager.Instance.SoundManager.PlaySoundEffect("Sound/tap.wav");
            if (_message == null)
            {
                CloseDialogue();
            }
            else
            {
                _arrow.SetBool("Open", true);
                _dialogue.text = System.String.Empty;
                StartCoroutine(TypeText());
                EventCheck(_message.text[0]);
            }
        }


        //Open all the UI tooltips then continue dialogue
        private void OpenTooltips(int index)
        {
            //If we run out of tooltips continue the dialogue
            if (_tooltipAnimators.Length <= index)
            {
                //Reset values
                _tooltip = false;
                _tooltipCounter = 0;
             

                //Exit the card review panel
                if (_cardReviewOpen)
                {
                    GameManager.Instance.CardReview.Exit();
                    _cardReviewOpen = false;
                }

                //Exit the card review panel
                if (_breedingMenuOpen)
                {
                    GameManager.Instance.BreedingManager.OpenBreedingMenu(false);
                    _breedingMenuOpen = false;
                }

                //Switch off the last tooltip animators

                _tooltipAnimators[index-1].SetBool("Open", false);



                //Switch on all the dialogue animators
                for (int i = 0; i < _animators.Length; i++)
                {
                    if(_animators[i].name !="CatBar")
                        _animators[i].SetBool("Open", true);
                }
                   
                
                NextDialogue();
            }
            else
            {
                //If its not on the first tooltip close the previous one
                if (index > 0)
                {
                    if (_tooltipAnimators[index - 1].GetCurrentAnimatorStateInfo(0).IsName("TooltipOpened"))
                        _tooltipAnimators[index - 1].SetBool("Open", false);
                    else
                        return;
                }

                //Switch off all the dialogue animators but the fade
                for (int i = 0; i < _animators.Length; i++)
                {
                    if(_animators[i].tag != "Fade")
                        _animators[i].SetBool("Open", false);
                }

                //Switch on the tooltip
                _tooltipAnimators[index].SetBool("Open", true);

                if (_tooltipAnimators[index].name == "TooltipCardReview")
                {
                   _randomDog.OpenCardReview();
                    _cardReviewOpen = true;
                }
                else
                {
                    if (_cardReviewOpen)
                    {
                        GameManager.Instance.CardReview.Exit();
                        _cardReviewOpen = false;
                    }
                }

                if (_tooltipAnimators[index].name == "TooltipBreeding")
                {
                    GameManager.Instance.BreedingManager.OpenBreedingMenu(true);
                    _breedingMenuOpen = true;
                }

                
                //Exit the card review panel
                else
                {
                    if (_breedingMenuOpen)
                    {
                        GameManager.Instance.BreedingManager.OpenBreedingMenu(false);
                        _breedingMenuOpen = false;
                    }
                }
            }
            _tooltipCounter++;
        }


        //Speed up text or go to the next dialogue
        public void OnPointerDown(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right) return;
            if (!_speedUp)
            {
                _speedUp = true;
                _arrow.SetBool("Open", false);
                _letterPause /= 10f;
            }


            if (!_finished) return;
            if (_tooltip)
            {
                OpenTooltips(_tooltipCounter);
            }
            else
            {
                NextDialogue();
                
            }
        }

        //Reset variables and close all animators
        public void CloseDialogue()
        {
            for (int i = 0; i < _animators.Length; i++)
            {
                _animators[i].SetBool("Open", false);
            }
           
            if (_catBarAnimator != null)
            {
                _catBarAnimator.SetBool("OpenCat", false);
               
            }
            if (_cat != null)
            {
                _cat.SetBool("OpenCat", false);
            }
            _dialogue.text = System.String.Empty;
            _open = false;
            _speedUp = false;
            _finished = false;
            _index = 0;
        }

        //Returns if the program is open
        public bool IsOpen()
        {
            return _open;
        }


        //Load asset in resources folder using the filepath given
        public TextAsset LoadText(string filepath)
        {
            if(Resources.Load(filepath) != null)
                return (TextAsset)Resources.Load(filepath, typeof(TextAsset));
           CloseDialogue();
            return null;
            
        }

        private void OpenCatBar()
        {
            _catBarAnimator.SetBool("OpenCat", true);
            GameManager.Instance.SoundManager.PlaySoundEffect("Sound/meow.wav");
        }

        private void CloseCatBar()
        {
            _catBarAnimator.SetBool("OpenCat", false);
            GameManager.Instance.SoundManager.PlaySoundEffect("Sound/meow.wav");
        }

        //Determine what dialogue mode should be called
        private void EventCheck(Char check)
        {
            switch (check)
            {
                case '0':
                    _proffessorAnimator.SetBool("Open", true);
                    _dialogue.fontStyle = FontStyle.Normal;
                    break;
                case '1':
                    _proffessorAnimator.SetBool("Open", false);
                    break;
                case '2':
                    _tooltip = true;
                    _tooltipAnimators = GameObject.FindGameObjectWithTag("Tooltip").GetComponent<Tooltip>().ReturnTooltips();
                  break;
                case '3':
                    _polaroid.ActivatePolaroids();
                    break;
                case '4':
                    OpenCatBar();
                    break;
                case '5':
                    CloseCatBar();
                    break;
                case '6':
                    _proffessorAnimator.SetBool("Open", false);
                    _dialogue.fontStyle = FontStyle.BoldAndItalic;
                    _cat.SetBool("OpenCat", true);
                    break;
                case '7':
                    GameManager.Instance.SoundManager.PlaySoundEffect("Sound/Crash.wav");
                    _proffessorCrash.SetTrigger("Open!");
                    break;
                case '8':
                    _proffessorAnimator.GetComponent<Image>().sprite = _oldProffessor;
                        break;
                case '9':
                    GameManager.Instance.Quiz.ChangeQuizStatus();
                    _proffessorAnimator.SetBool("Open", !GameManager.Instance.Quiz.ReturnStatus());
                    break;
                case '#':
                    GameManager.Instance.QuizManager.ShutDownThePolaroids();
                    GameManager.Instance.QuizManager.SwitchOnTheQuiz(0);
                    break;
                case '@':
                    GameManager.Instance.QuizManager.SwitchOnTheQuiz(1);
                    break;
                case '?':
                    _tooltip = true;
                    _tooltipAnimators = GameObject.FindGameObjectWithTag("Tooltip").GetComponent<Tooltip>().ReturnTooltips();
                    _proffessorAnimator.SetBool("Open", false);
                    break;

            }
        }
   
    }
}
