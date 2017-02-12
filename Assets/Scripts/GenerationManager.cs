using System.Collections;
using System.Linq;
using UnityEngine;
using LoLSDK;


namespace Assets.Scripts
{
    public class GenerationManager : MonoBehaviour
    {
        [SerializeField] private Generation[] _generations;
        [SerializeField] private Generation _currentGeneration;
        [SerializeField] private Generation _nextGeneration;
        private bool failed;
        private int _generationIndex;


        // Use this for initialization
        void Start()
        {
            _generations = new Generation[transform.childCount];
            for (int i = 0; i < transform.childCount; i++)
            {
                _generations[i] = transform.GetChild(i).gameObject.GetComponent<Generation>();
            }
            // _generations = GameObject.FindGameObjectsWithTag("Generation");
            UpdateGeneration();
            StartCoroutine(Check());
        }

        // Update is called once per frame
        void Update()
        {
          
            if (_nextGeneration.GetComponent<Generation>().ReturnAvailableSlot() == null)
                UpdateGeneration();
        }

        public Generation ReturnCurrentGeneration()
        {
            return _currentGeneration;
        }

        public Generation ReturnFutureGeneration()
        {
            return _nextGeneration;
        }

        //Change the active generation
        public void UpdateGeneration()
        {
            //POTENTIAL_ERROR WHEN GENERATIONS ARE FINISHED
            _generationIndex++;
            if (_generationIndex >= _generations.Length)
            {
                if (!failed)
                {
                    GameManager.Instance.DialogueManager.OpenDialogue(GameManager.Instance.CurrentLevel + "/Fail");
                    failed = true;
                }
                GameManager.Instance.Failed = true;
            }
            else
            {
                _currentGeneration = _generations[_generationIndex-1];
                _nextGeneration = _generations[_generationIndex];
                ManageGenerations();
            }
        }

        //Keep the active generation active and disable the rest
        private void ManageGenerations()
        {
            for (int i = 0; i < _generations.Length; i++)
            {
                _generations[i].Disabled(_generations[i] != _currentGeneration);
            }
        }

        IEnumerator Check()
        {
            yield return new WaitForSeconds(0.001f);
            ManageGenerations();
              
            
        }



    }
}



