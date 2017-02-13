using Boo.Lang;
using LoLSDK;
using UnityEngine;

namespace Assets.Scripts
{
    public class QuizManager : MonoBehaviour
    {

        public Animator _polaroids;
        [SerializeField] public QuizCardSlot[] _cardSlots;
        public bool quiz1Complete, quiz2Complete;
        public GameObject[] Quiz;
        public Animator[] _quizAnimators;
        public int _count;
        // Use this for initialization
        void Start()
        {
            Quiz = GameObject.FindGameObjectsWithTag("Quiz");
            _cardSlots = Quiz[0].GetComponentsInChildren<QuizCardSlot>();
            _quizAnimators = new Animator[Quiz.Length];

            if (GameObject.FindGameObjectWithTag("Polaroid") != null)
                _polaroids = GameObject.FindGameObjectWithTag("Polaroid").GetComponentInParent<Animator>();



        }

        // Update is called once per frame
        void Update()
        {
            if (!quiz1Complete)
            {
                _count = 0;
                for (int i = 0; i < _cardSlots.Length; i++)
                {
                    if (_cardSlots[i].Item != null)
                        _count++;
                }
            

            if (_count != _cardSlots.Length) return;
            quiz1Complete = true;
            Quiz[0].GetComponent<Animator>().SetTrigger("Close");

                if (Quiz.Length == 1)
                {
                    GameManager.Instance.DialogueManager.OpenDialogue(GameManager.Instance.CurrentLevel + "/Victory");
                    GameManager.Instance.Victory = true;
                    return;
                }
                LOLSDK.Instance.SubmitProgress(0, 11, 12);
                _cardSlots = Quiz[1].GetComponentsInChildren<QuizCardSlot>();
                 GameManager.Instance.DialogueManager.OpenDialogue(GameManager.Instance.CurrentLevel + "/NextQuiz");
                
            }
            else if (!quiz2Complete && Quiz.Length > 1)
            {
                _count = 0;
                for (int i = 0; i < _cardSlots.Length; i++)
                {
                    if (_cardSlots[i].Item != null)
                        _count++;
                }

                if (_count != _cardSlots.Length) return;
                quiz2Complete = true;
                Quiz[1].GetComponent<Animator>().SetTrigger("Close");
                GameManager.Instance.DialogueManager.OpenDialogue(GameManager.Instance.CurrentLevel + "/Victory");
                GameManager.Instance.Victory = true;
            }

        }


        public void ShutDownThePolaroids()
        {
            if (_polaroids != null)
            {
                _polaroids.SetTrigger("Open");
            }
        }

        public void SwitchOnTheQuiz(int i)
        {
            Debug.Log("Hey");
            Quiz[i].GetComponent<Animator>().SetTrigger("Open");
        }
    }
}
