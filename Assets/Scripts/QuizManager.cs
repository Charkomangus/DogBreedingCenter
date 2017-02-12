using UnityEngine;

namespace Assets.Scripts
{
    public class QuizManager : MonoBehaviour
    {
        private Animator _polaroids, _quiz;
        private QuizCardSlot[] _cardSlots;
        private int _count;
        // Use this for initialization
        void Start ()
        {
            if (GameObject.FindGameObjectWithTag("Polaroid") != null)
                _polaroids = GameObject.FindGameObjectWithTag("Polaroid").GetComponentInParent<Animator>();
            _quiz = GameObject.FindGameObjectWithTag("Quiz").GetComponent<Animator>();
            _cardSlots = FindObjectsOfType<QuizCardSlot>();
        }
	
        // Update is called once per frame
        void Update ()
        {
            _count = 0;
            for (int i = 0; i < _cardSlots.Length; i++)
            {
                if (_cardSlots[i].Item != null)
                    _count++;
            }

            if (_count != _cardSlots.Length) return;
            _quiz.SetTrigger("Close");
            if (GameManager.Instance.Victory) return;
            GameManager.Instance.DialogueManager.OpenDialogue(GameManager.Instance.CurrentLevel+ "/Victory");
            GameManager.Instance.Victory = true;
        }


        public void ShutDownThePolaroids()
        {
            if (_polaroids != null)
            {
                _polaroids.SetTrigger("Open");
            }
        }

        public void SwitchOnTheQuiz()
        {
            _quiz.SetTrigger("Open");
        }
    }
}
