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
        [SerializeField] public bool TutorialEnabled;
      //>>>>FONT<<<<<


        //>>>BUTTONS<<<
        [SerializeField] private Button[] _allButtonsinScene;
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
        public bool Failed;
        //>>>>WARNINGS<<<<



        //>>>VARIABLES<<<<
        public string CurrentLevel, NextLevel;
        public bool Victory;
        public float MaxX, MinX, MaxY, MinY;
        //>>>VARIABLES<<<<

        //>>>PREFABS<<<<
        public GameObject ShepardCard, MastiffCard, PointerCard, CatCard, ChosenCardPrefab;
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
            ShepardCard = (GameObject) Resources.Load("Prefabs/ShepardCard");
            MastiffCard = (GameObject) Resources.Load("Prefabs/MastiffCard");
            PointerCard = (GameObject) Resources.Load("Prefabs/PointerCard");
            CatCard = (GameObject) Resources.Load("Prefabs/BobCatCard");

            // if the singleton hasn't been initialized yet
            if (_instance != null && _instance != this || LOLSDK.Instance.IsInitialized)
            {
                Destroy(gameObject);
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
        void InitGame(Scene activeScene)
        {
            SetBounds();
            Failed = false;
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

            switch (CurrentLevel)
            {
                case "_Init":
                    SceneManager.LoadScene("Menu");
                    break;
                case "Menu":
                    //SOUND
                    SoundManager.PlayBackgroundMusic("Music/menuAmbient.mp3");
                    SoundManager.StopPreviousMusic("Music/Thinking Music.ogg");
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
                    SoundManager.PlayBackgroundMusic("Music/Thinking Music(short).ogg");
                    break;

                case "Level0":

                    //>>>>>>SDK UPDATE<<<<<<<<<<<<
                    LOLSDK.Instance.SubmitProgress(0, 2, 12);
                    //>>>>>>SDK UPDATE<<<<<<<<<<<<

                    Fade = GameObject.FindGameObjectWithTag("FinalFade").GetComponent<Animator>();
                    if (!DialogueManager.IsOpen() && TutorialEnabled)
                        DialogueManager.OpenDialogue(CurrentLevel + "/Introduction");
                    ChosenCardPrefab = PointerCard;
                    LoadGameManagers();
                    SoundManager.StopPreviousMusic("Music/Thinking Music(short).ogg");
                    SoundManager.PlayBackgroundMusic("Music/Thinking Music.ogg");

                    break;
                case "Level1":

                    //>>>>>>SDK UPDATE<<<<<<<<<<<<
                    LOLSDK.Instance.SubmitProgress(0, 4, 12);
                    //>>>>>>SDK UPDATE<<<<<<<<<<<<
                    Fade = GameObject.FindGameObjectWithTag("FinalFade").GetComponent<Animator>();
                    if (!DialogueManager.IsOpen() && TutorialEnabled)
                        DialogueManager.OpenDialogue(CurrentLevel + "/Introduction");
                    ChosenCardPrefab = PointerCard;
                    LoadGameManagers();
                    break;
                case "Level2":

                    //>>>>>>SDK UPDATE<<<<<<<<<<<<
                    LOLSDK.Instance.SubmitProgress(0, 6, 12);
                    //>>>>>>SDK UPDATE<<<<<<<<<<<<
                    Fade = GameObject.FindGameObjectWithTag("FinalFade").GetComponent<Animator>();
                    if (!DialogueManager.IsOpen() && TutorialEnabled)
                        DialogueManager.OpenDialogue(CurrentLevel + "/Introduction");
                    ChosenCardPrefab = MastiffCard;
                    LoadGameManagers();
                    break;
                case "Level3":
                    //>>>>>>SDK UPDATE<<<<<<<<<<<<
                    LOLSDK.Instance.SubmitProgress(0, 8, 12);
                    //>>>>>>SDK UPDATE<<<<<<<<<<<<
                    Fade = GameObject.FindGameObjectWithTag("FinalFade").GetComponent<Animator>();
                    if (!DialogueManager.IsOpen() && TutorialEnabled)
                        DialogueManager.OpenDialogue(CurrentLevel + "/Introduction");
                    ChosenCardPrefab = ShepardCard;
                    LoadGameManagers();
                    Quiz = GameObject.FindGameObjectWithTag("Quiz").GetComponent<Quiz>();
                    break;
                case "Quiz1":
                    //>>>>>>SDK UPDATE<<<<<<<<<<<<
                    LOLSDK.Instance.SubmitProgress(0, 10, 12);
                    //>>>>>>SDK UPDATE<<<<<<<<<<<<
                    Fade = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
                    if (!DialogueManager.IsOpen() && TutorialEnabled)
                        DialogueManager.OpenDialogue("Quiz1/Introduction");
                    //SOUND
                    SoundManager.StopPreviousMusic("Music/Thinking Music.ogg");
                    SoundManager.PlayBackgroundMusic("Music/Thinking Music(short).ogg");
                    QuizManager = GameObject.FindGameObjectWithTag("QuizManager").GetComponent<QuizManager>();
                    break;
                case "Quiz2":
                    //>>>>>>SDK UPDATE<<<<<<<<<<<<
                    LOLSDK.Instance.SubmitProgress(0, 11, 12);
                    //>>>>>>SDK UPDATE<<<<<<<<<<<<
                    Fade = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
                    if (!DialogueManager.IsOpen() && TutorialEnabled)
                        DialogueManager.OpenDialogue("Quiz2/Introduction");
                    //SOUND
                    SoundManager.StopPreviousMusic("Music/Thinking Music.ogg");
                    SoundManager.PlayBackgroundMusic("Music/Thinking Music(short).ogg");
                    QuizManager = GameObject.FindGameObjectWithTag("QuizManager").GetComponent<QuizManager>();
                    break;
                case "Quiz3":
                    //>>>>>>SDK UPDATE<<<<<<<<<<<<
                    LOLSDK.Instance.SubmitProgress(0, 12, 12);
                    //>>>>>>SDK UPDATE<<<<<<<<<<<<
                    Fade = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
                    if (!DialogueManager.IsOpen() && TutorialEnabled)
                        DialogueManager.OpenDialogue("Quiz3/Introduction");
                    //SOUND
                    SoundManager.StopPreviousMusic("Music/Thinking Music.ogg");
                    SoundManager.PlayBackgroundMusic("Music/Thinking Music(short).ogg");
                    QuizManager = GameObject.FindGameObjectWithTag("QuizManager").GetComponent<QuizManager>();
                    break;
            }
        }


        //Update is called every frame.
        private void Update()
        {

            if (Failed && !DialogueManager.IsOpen())
            {
                Failed = false;
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
                        Victory = false;
                        StartCoroutine(LoadNextLevel());
                        Fade.SetBool("Open", true);
                    }

                    break;

                case "Level1":
                    if (DialogueManager.IsOpen() == false && Victory)
                    {
                        Victory = false;
                        StartCoroutine(LoadNextLevel());
                        Fade.SetBool("Open", true);
                    }

                    break;
                case "Level2":
                    if (DialogueManager.IsOpen() == false && Victory)
                    {
                        Victory = false;
                        StartCoroutine(LoadNextLevel());
                        Fade.SetBool("Open", true);
                    }

                    break;
                case "Level3":
                    if (DialogueManager.IsOpen() == false && Victory)
                    {
                        Victory = false;
                        StartCoroutine(LoadNextLevel());
                        Fade.SetBool("Open", true);
                    }

                    break;
                case "Quiz1":
                    if (DialogueManager.IsOpen() == false && Victory)
                    {
                        Victory = false;
                        SceneManager.LoadScene("Quiz2");
                    }

                    break;
                case "Quiz2":
                    if (DialogueManager.IsOpen() == false && Victory)
                    {
                        Victory = false;
                        SceneManager.LoadScene("Quiz3");
                    }

                    break;
                case "Quiz3":
                    if (DialogueManager.IsOpen() == false && Victory)
                    {
                        Victory = false;
                        StartCoroutine(EndGame());
                        Fade.SetBool("Open", true);
                    }

                    break;
            }


        }

        //Checks the current generations genetic variety
        public void GeneticVarienceWarnings()
        {
            if (GeneticVarience == null || Victory) return;


            if (GeneticVarience.value <= 10 && !Failed)
            {
                DialogueManager.OpenDialogue("GeneticWarning10");
                Failed = true;
            }
            else if (GeneticVarience.value <= 25 && !_third)
            {
                _third = true;
                DialogueManager.OpenDialogue("GeneticWarning25");
            }
            else if (GeneticVarience.value <= 50 && !_second)
            {
                _second = true;
                DialogueManager.OpenDialogue("GeneticWarning50");
            }
            else if (GeneticVarience.value <= 75 && !_first)
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
                _allButtonsinScene[i].onClick.AddListener(() => SoundManager.PlaySoundEffect("Sound/tap.mp3"));
            }
        }

       



        //Checks to see if any dogs fit the final dog criteria
        public bool IsFinalDog(Dog puppy)
        {
            switch (CurrentLevel)
            {
                case "Level0":
                    //>>>>>>SDK UPDATE<<<<<<<<<<<<
                    LOLSDK.Instance.SubmitProgress(0, 3, 12);
                    //>>>>>>SDK UPDATE<<<<<<<<<<<<
                    return puppy.ReturnIntelligence () > 60 && puppy.ReturnScent() > 60;
                case "Level1":
                    //>>>>>>SDK UPDATE<<<<<<<<<<<<
                    LOLSDK.Instance.SubmitProgress(0, 5, 12);
                    //>>>>>>SDK UPDATE<<<<<<<<<<<<
                    return puppy.ReturnEndurance() > 70 && puppy.ReturnSize() > 60 && puppy.ReturnBark() > 60 &&
                           puppy.ReturnScent() > 60;
                case "Level2":
                    //>>>>>>SDK UPDATE<<<<<<<<<<<<
                    LOLSDK.Instance.SubmitProgress(0, 7, 12);
                    //>>>>>>SDK UPDATE<<<<<<<<<<<<
                    return puppy.ReturnIntelligence () > 70 && puppy.ReturnSight() > 70 && puppy.ReturnDemeanor() > 60 &&
                           puppy.ReturnSize() > 60;
                case "Level3":
                    //>>>>>>SDK UPDATE<<<<<<<<<<<<
                    LOLSDK.Instance.SubmitProgress(0, 9, 12);
                    //>>>>>>SDK UPDATE<<<<<<<<<<<<
                    return puppy.ReturnIntelligence () > 80 && puppy.ReturnEndurance() > 60 && puppy.ReturnScent() > 80 &&
                           puppy.ReturnDemeanor() > 60;
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
                    MaxY = 100;
                    break;
                case "Level1":
                    MaxX = 0;
                    MinY = 0;
                    MinX = -300;
                    MaxY = 150;
                    break;
                case "Level2":
                    MaxX = 0;
                    MinY = 0;
                    MinX = -500;
                    MaxY = 300;
                    break;
                case "Level3":
                    MaxX = 0;
                    MinY = 0;
                    MinX = -500;
                    MaxY = 500;
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
            StopAllCoroutines();
        }


       
    }

}
