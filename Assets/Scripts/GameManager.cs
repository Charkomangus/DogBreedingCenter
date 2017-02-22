using System;
using System.Collections;
using Boo.Lang;
using LoLSDK;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Allows us to use Lists. 

namespace Assets.Scripts
{
    public class GameManager : MonoBehaviour
    {
        //>>>>FONT<<<<<
        [SerializeField]
        public bool TutorialEnabled;
        //>>>>FONT<<<<<


        //>>>BUTTONS<<<
        private Button[] _allButtonsinScene;
        //>>>BUTTONS<<<


        //>>>>MANAGERS<<<<
        private static GameManager _instance;
        //Static instance of GameManager which allows it to be accessed by any other script.

        public static GameManager Instance
        {
            get { return _instance; }
        }

        public DialogueManager DialogueManager;
        public PuppyManager PuppyManager;
        public GenerationManager GenerationManager;
        public BreedingManager BreedingManager;
        public FinalDog FinalDogManager;
        public CardReview CardReview;
        public SoundManager SoundManager;
        public Percentage GeneticVarience;
        public Animator SideBar;
        public CanvasGroup OptionsPanel;
        public Quiz Quiz;
        public QuizManager QuizManager;
        //>>>>MANAGERS<<<<



        //>>>>WARNINGS<<<<
        private bool _first, _second, _third;
        private bool _failed;
        //>>>>WARNINGS<<<<



        //>>>VARIABLES<<<<
        public string CurrentLevel, NextLevel;
        public bool Victory;
        public float MaxX, MinX, MaxY, MinY;
        //>>>VARIABLES<<<<

        //>>>PREFABS<<<<
        private GameObject _shepardCard, _mastiffCard, _pointerCard;
        public GameObject ChosenCardPrefab;
        //>>>PREFABS<<<<

        //>>>INTRO<<<<
        public Animator Fade;
        //>>>INTRO<<<<


        //>>>>PAUSE<<<<<
        public enum GameState
        {
            Paused,
            Resumed
        }

        //>>>>PAUSE<<<<<
        //Awake is always called before any Start functions
        private void Awake()
        {
            Application.targetFrameRate = -1;
            _shepardCard = (GameObject)Resources.Load("Prefabs/ShepardCard");
            _mastiffCard = (GameObject)Resources.Load("Prefabs/MastiffCard");
            _pointerCard = (GameObject)Resources.Load("Prefabs/PointerCard");


            // if the singleton hasn't been initialized yet
            if (_instance != null && _instance != this || LOLSDK.Instance.IsInitialized)
            {
                Destroy(gameObject);
                DialogueManager.Awake();
            }

            _instance = this;

            //Sets this to not be destroyed when reloading scene
            DontDestroyOnLoad(gameObject);

            if (!LOLSDK.Instance.IsInitialized)
                LOLSDK.Init("Dog Breeding Center");
            LOLSDK.Instance.GameStateChanged += InstanceOnGameStateChanged;



            SoundManager = GetComponent<SoundManager>();

            //Call the InitGame function to initialize the first level 
            InitGame(SceneManager.GetActiveScene());

            SetBounds();
        }


        //Pause Game if state is chaged
        private void InstanceOnGameStateChanged(LoLSDK.GameState gameState)
        {
            switch (gameState)
            {
                case LoLSDK.GameState.Paused:
                    {
                        Time.timeScale = 0;
                    }
                    break;

                case LoLSDK.GameState.Resumed:
                    {
                        Time.timeScale = 1;
                    }
                    break;
            }
        }



        //Start level when level has changed
        void OnLevelWasLoaded()
        {
            InitGame(SceneManager.GetActiveScene());
        }



        //Initializes the game for each level.
        public void InitGame(Scene activeScene)
        {

            _failed = false;
            Victory = false;
            CalculateNextLevel();

            if (GameObject.FindGameObjectWithTag("Dialogue") != null)
            {
                DialogueManager = GameObject.FindGameObjectWithTag("Dialogue").GetComponent<DialogueManager>();
                DialogueManager.Awake();
            }

            _allButtonsinScene = FindObjectsOfType<Button>(); //All Buttons

            if (_allButtonsinScene.Length > 0)
                SetUpButtons();
            SoundManager.StopAllPreviousMusic();
            SetBounds();
            switch (CurrentLevel)
            {
                case "_Init":
                    SceneManager.LoadScene("Menu");
                    break;
                case "Menu":
                    //SOUND
                    SoundManager.StopPreviousMusic("Music/menuAmbient.mp3");
                    SoundManager.StopPreviousMusic("Music/Thinking Music.ogg");
                    SoundManager.StopPreviousMusic("Music/Thinking Music(short).ogg");
                    SoundManager.StopPreviousMusic("Music/menuAmbient.mp3");
                    SoundManager.StopPreviousMusic("Music/Thinking Music.ogg");
                    SoundManager.StopPreviousMusic("Music/Thinking Music(short).ogg");
                    SoundManager.PlayBackgroundMusic("Music/menuAmbient.mp3");
                    break;

                case "Intro":
                    //>>>>>>SDK UPDATE<<<<<<<<<<<<
                    LOLSDK.Instance.SubmitProgress(0, 1, 12);
                    //>>>>>>SDK UPDATE<<<<<<<<<<<<

                    Fade = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
                    if (!DialogueManager.IsOpen())
                        DialogueManager.OpenDialogue("Introduction/Introduction");
                    //SOUND
                    SoundManager.StopPreviousMusic("Music/menuAmbient.mp3");
                    SoundManager.StopPreviousMusic("Music/Thinking Music(short).ogg");
                    SoundManager.PlayBackgroundMusic("Music/Thinking Music(short).ogg");
                    break;

                case "Level0":

                    //>>>>>>SDK UPDATE<<<<<<<<<<<<
                    LOLSDK.Instance.SubmitProgress(0, 2, 12);
                    //>>>>>>SDK UPDATE<<<<<<<<<<<<

                    Fade = GameObject.FindGameObjectWithTag("FinalFade").GetComponent<Animator>();
                    if (!DialogueManager.IsOpen() && TutorialEnabled)
                        DialogueManager.OpenDialogue(CurrentLevel + "/Introduction");
                    ChosenCardPrefab = _pointerCard;
                    LoadGameManagers();
                    SoundManager.StopPreviousMusic("Music/Thinking Music(short).ogg");
                    SoundManager.StopPreviousMusic("Music/menuAmbient.mp3");
                    SoundManager.StopPreviousMusic("Music/Thinking Music.ogg");
                    SoundManager.PlayBackgroundMusic("Music/Thinking Music.ogg");

                    break;
                case "Level1":

                    //>>>>>>SDK UPDATE<<<<<<<<<<<<
                    LOLSDK.Instance.SubmitProgress(0, 4, 12);
                    //>>>>>>SDK UPDATE<<<<<<<<<<<<
                    Fade = GameObject.FindGameObjectWithTag("FinalFade").GetComponent<Animator>();
                    if (!DialogueManager.IsOpen() && TutorialEnabled)
                        DialogueManager.OpenDialogue(CurrentLevel + "/Introduction");
                    ChosenCardPrefab = _pointerCard;
                    LoadGameManagers();
                    SoundManager.StopPreviousMusic("Music/menuAmbient.mp3");
                    SoundManager.StopPreviousMusic("Music/Thinking Music.ogg");
                    SoundManager.PlayBackgroundMusic("Music/Thinking Music.ogg");
                    break;
                case "Level2":

                    //>>>>>>SDK UPDATE<<<<<<<<<<<<
                    LOLSDK.Instance.SubmitProgress(0, 6, 12);
                    //>>>>>>SDK UPDATE<<<<<<<<<<<<
                    Fade = GameObject.FindGameObjectWithTag("FinalFade").GetComponent<Animator>();
                    if (!DialogueManager.IsOpen() && TutorialEnabled)
                        DialogueManager.OpenDialogue(CurrentLevel + "/Introduction");
                    ChosenCardPrefab = _mastiffCard;
                    SoundManager.StopPreviousMusic("Music/menuAmbient.mp3");
                    SoundManager.StopPreviousMusic("Music/Thinking Music.ogg");
                    SoundManager.PlayBackgroundMusic("Music/Thinking Music.ogg");
                    LoadGameManagers();
                    break;
                case "Level3":
                    //>>>>>>SDK UPDATE<<<<<<<<<<<<
                    LOLSDK.Instance.SubmitProgress(0, 8, 12);
                    //>>>>>>SDK UPDATE<<<<<<<<<<<<
                    Fade = GameObject.FindGameObjectWithTag("FinalFade").GetComponent<Animator>();
                    if (!DialogueManager.IsOpen() && TutorialEnabled)
                        DialogueManager.OpenDialogue(CurrentLevel + "/Introduction");
                    ChosenCardPrefab = _shepardCard;
                    LoadGameManagers();
                    Quiz = GameObject.FindGameObjectWithTag("Quiz").GetComponent<Quiz>();
                    SoundManager.StopPreviousMusic("Music/menuAmbient.mp3");
                    SoundManager.StopPreviousMusic("Music/Thinking Music.ogg");
                    SoundManager.PlayBackgroundMusic("Music/Thinking Music.ogg");
                    break;
                case "Quiz1":
                    //>>>>>>SDK UPDATE<<<<<<<<<<<<
                    LOLSDK.Instance.SubmitProgress(0, 10, 12);
                    //>>>>>>SDK UPDATE<<<<<<<<<<<<
                    Fade = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
                    if (!DialogueManager.IsOpen() && TutorialEnabled)
                        DialogueManager.OpenDialogue("Quiz1/Introduction");
                    //SOUND

                    QuizManager = GameObject.FindGameObjectWithTag("QuizManager").GetComponent<QuizManager>();
                    SoundManager.StopPreviousMusic("Music/menuAmbient.mp3");
                    SoundManager.StopPreviousMusic("Music/Thinking Music.ogg");
                    SoundManager.StopPreviousMusic("Music/Thinking Music(short).ogg");
                    SoundManager.PlayBackgroundMusic("Music/Thinking Music(short).ogg");
                    break;
                case "Quiz2":
                    //>>>>>>SDK UPDATE<<<<<<<<<<<<
                    LOLSDK.Instance.SubmitProgress(0, 12, 12);
                    //>>>>>>SDK UPDATE<<<<<<<<<<<<
                    Fade = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
                    if (!DialogueManager.IsOpen() && TutorialEnabled)
                        DialogueManager.OpenDialogue("Quiz2/Introduction");
                    //SOUND
                    QuizManager = GameObject.FindGameObjectWithTag("QuizManager").GetComponent<QuizManager>();
                 
                    SoundManager.StopPreviousMusic("Music/menuAmbient.mp3");
                    SoundManager.StopPreviousMusic("Music/Thinking Music(short).ogg");
                    SoundManager.StopPreviousMusic("Music/Thinking Music.ogg");
                    SoundManager.PlayBackgroundMusic("Music/Thinking Music(short).ogg");

                    break;

            }
        }


        //Update is called every frame.
        private void Update()
        {
            if (MinX == 0 || MinY == 0)
                SetBounds();
            if (_failed && !DialogueManager.IsOpen())
            {
                _failed = false;
                StartCoroutine(LoadLevel());
                Fade.SetBool("Open", true);
            }

            switch (CurrentLevel)
            {

                case "Intro":
                    if (DialogueManager.IsOpen() == false)
                    {
                        Victory = false;
                        StartCoroutine(LoadNextLevel());
                        Fade.SetBool("Open", true);
                    }

                    break;
                case "Level0":
                    if (DialogueManager.IsOpen() == false && Victory)
                    {
                        //>>>>>>SDK UPDATE<<<<<<<<<<<<
                        LOLSDK.Instance.SubmitProgress(0, 3, 12);
                        //>>>>>>SDK UPDATE<<<<<<<<<<<<
                        Victory = false;
                        StartCoroutine(LoadNextLevel());
                        Fade.SetBool("Open", true);
                    }

                    break;

                case "Level1":
                    if (DialogueManager.IsOpen() == false && Victory)
                    {
                        //>>>>>>SDK UPDATE<<<<<<<<<<<<
                        LOLSDK.Instance.SubmitProgress(0, 5, 12);
                        //>>>>>>SDK UPDATE<<<<<<<<<<<<
                        Victory = false;
                        StartCoroutine(LoadNextLevel());
                        Fade.SetBool("Open", true);
                    }

                    break;
                case "Level2":
                    if (DialogueManager.IsOpen() == false && Victory)
                    {
                        //>>>>>>SDK UPDATE<<<<<<<<<<<<
                        LOLSDK.Instance.SubmitProgress(0, 7, 12);
                        //>>>>>>SDK UPDATE<<<<<<<<<<<<
                        Victory = false;
                        StartCoroutine(LoadNextLevel());
                        Fade.SetBool("Open", true);
                    }

                    break;
                case "Level3":
                    if (DialogueManager.IsOpen() == false && Victory)
                    {
                       
                        //>>>>>>SDK UPDATE<<<<<<<<<<<<
                        LOLSDK.Instance.SubmitProgress(0, 9, 12);
                        //>>>>>>SDK UPDATE<<<<<<<<<<<<
                        Victory = false;
                        StartCoroutine(LoadNextLevel());
                        Fade.SetBool("Open", true);
                    }

                    break;
                case "Quiz1":
                    if (DialogueManager.IsOpen() == false && Victory)
                    {
                        Victory = false;
                        StartCoroutine(LoadNextLevel());
                        Fade.SetBool("Open", true);
                    }

                    break;
                case "Quiz2":
                    if (DialogueManager.IsOpen() == false && Victory)
                    {
                        Victory = false;
                        Fade.SetBool("Open", true);
                        StartCoroutine(EndGame());

                    }

                    break;
            }


        }

        //Checks the current generations genetic variety
        public void GeneticVarienceWarnings()
        {
            if (GeneticVarience == null || Victory || DialogueManager.IsOpen()) return;


            if (GeneticVarience.Value <= 10 && !_failed)
            {
                DialogueManager.OpenDialogue("GeneticWarning10");
                _failed = true;
            }
            else if (GeneticVarience.Value <= 25 && !_third)
            {
                _third = true;
                DialogueManager.OpenDialogue("GeneticWarning25");
            }
            else if (GeneticVarience.Value <= 50 && !_second)
            {
                _second = true;
                DialogueManager.OpenDialogue("GeneticWarning50");
            }
            else if (GeneticVarience.Value <= 75 && !_first)
            {
                _first = true;
                DialogueManager.OpenDialogue("GeneticWarning75");
            }




        }

        //Load all in game object managers
        private void LoadGameManagers()
        {
            PuppyManager = GameObject.FindGameObjectWithTag("PuppyManager").GetComponent<PuppyManager>();
            GenerationManager = GameObject.FindWithTag("Main").GetComponent<GenerationManager>();
            FinalDogManager = GameObject.FindGameObjectWithTag("finalDog").GetComponent<FinalDog>();
            CardReview = GameObject.FindGameObjectWithTag("cardReview").GetComponent<CardReview>();
            BreedingManager = GameObject.FindGameObjectWithTag("breedingPanel").GetComponent<BreedingManager>();
            GeneticVarience = GameObject.FindGameObjectWithTag("GeneticVariance").GetComponentInChildren<Percentage>();
            SideBar = GameObject.FindGameObjectWithTag("SideBar").GetComponentInChildren<Animator>();
            OptionsPanel = GameObject.FindGameObjectWithTag("Options").GetComponent<CanvasGroup>();
        }

        private void SetUpButtons()
        {
            for (int i = 0; i < _allButtonsinScene.Length; i++)
            {
                _allButtonsinScene[i].onClick.AddListener(() => SoundManager.PlaySoundEffect("Sound/tap.wav"));
            }

        }



        //Checks to see if any dogs fit the final dog criteria
        public bool IsFinalDog(Dog puppy)
        {
            if (Victory) return false;
            switch (CurrentLevel)
            {
                case "Level0":
                  
                    return puppy.ReturnIntelligence() > 60 && puppy.ReturnScent() > 70;
                case "Level1":
                  
                    return puppy.ReturnEndurance() > 70 && puppy.ReturnBark() > 60 && puppy.ReturnScent() > 70;
                case "Level2":
                  
                    return puppy.ReturnIntelligence() > 70 && puppy.ReturnDemeanor() > 70 && puppy.ReturnSight() > 80 && puppy.ReturnBark() > 60;
                case "Level3":
                  
                    return puppy.ReturnIntelligence() > 90 && puppy.ReturnEndurance() > 70 && puppy.ReturnScent() > 60 &&
                           puppy.ReturnDemeanor() > 80;
                default:
                    return false;
            }
        }

        private void CalculateNextLevel()
        {
            CurrentLevel = SceneManager.GetActiveScene().name;
            switch (CurrentLevel)
            {
                case "_Init":
                    NextLevel = "Menu";
                    break;
                case "Menu":
                    NextLevel = "Intro";
                    break;
                case "Intro":
                    NextLevel = "Level0";
                    break;
                case "Level0":
                    NextLevel = "Level1";
                    break;
                case "Level1":
                    NextLevel = "Level2";
                    break;
                case "Level2":
                    NextLevel = "Level3";
                    break;
                case "Level3":
                    NextLevel = "Quiz1";
                    break;
                case "Quiz1":
                    NextLevel = "Quiz2";
                    break;
                case "Quiz2":
                    NextLevel = "Quiz3";
                    break;

            }
        }

        private void SetBounds()
        {
            switch (CurrentLevel)
            {

                case "Level0":
                    MaxX = 0;
                    MinX = -250;
                    MinY = 0;
                    MaxY = 200;
                    break;
                case "Level1":
                    MaxX = 0;
                    MinY = 0;
                    MinX = -300;
                    MaxY = 300;
                    break;
                case "Level2":
                    MaxX = 0;
                    MinY = 0;
                    MinX = -500;
                    MaxY = 550;
                    break;
                case "Level3":
                    MaxX = 0;
                    MinY = 0;
                    MinX = -600;
                    MaxY = 750;
                    break;
            }
        }


        //Show polaroids
        private IEnumerator LoadNextLevel()
        {
            yield return new WaitForSeconds(3);
            SceneManager.LoadScene(NextLevel);
            StopAllCoroutines();
        }


        private IEnumerator LoadLevel()
        {
            yield return new WaitForSeconds(3);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            StopAllCoroutines();
        }
        //Show polaroids
        private IEnumerator EndGame()
        {
            yield return new WaitForSeconds(3);
            LOLSDK.Instance.CompleteGame();
            LOLSDK.Instance.StopSound("Music/Thinking Music(short).ogg");
            yield return new WaitForSeconds(3);
            SceneManager.LoadScene("Menu");
            StopAllCoroutines();
        }


        public void SetFailStatus(bool status)
        {
            _failed = status;
        }

        public bool ReturnFailStatus()
        {
            return _failed;
        }


    }

}

