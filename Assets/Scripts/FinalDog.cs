using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;
using LoLSDK;


public class FinalDog : MonoBehaviour
{

    private GameObject _finalDog;
    private Dog _dog;
    [SerializeField]private Text _stats;
    [SerializeField]private Text _statsNumber;
    [SerializeField]private Text _description;
    private CardSlot _slot;
    private bool _finished;
    [SerializeField] private Animator _animator;
  
    // Use this for initialization
    void Start ()
	{
	    _slot = GetComponentInChildren<CardSlot>();
        _finished = false;
        _animator = GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update ()
	{
	    if (_animator.GetBool("Open") && !_finished)
	    {
	        GameManager.Instance.DialogueManager.OpenDialogue(GameManager.Instance.CurrentLevel + "/FinalDog");
	        GameManager.Instance.Victory = true;
            _finished = true;
	    }
	}

    //Set the given dog to be the final dog
    public void SetFinalDog(GameObject dog, Sprite finalDogImage)
    {
        _finalDog = Instantiate(dog);
        _finalDog.transform.SetParent(_slot.transform);
        _finalDog.transform.localScale = Vector3.one;
        _stats.text = SetStatText();
        _statsNumber.text = SetStatNumberText(_finalDog.GetComponent<Dog>());
        _description.text = SetDescriptionText(_finalDog.GetComponent<Dog>());
        _finalDog.GetComponent<Card>().SetImage(finalDogImage);
    }

    //Create string with all stats
        private string SetStatText()
        {
            System.Text.StringBuilder statsBuilder = new System.Text.StringBuilder();
          
            
            statsBuilder.Append("Intelligence : "  + Environment.NewLine);
            statsBuilder.Append("Endurance: " + Environment.NewLine);
            statsBuilder.Append("Demeanor: " + Environment.NewLine);
            statsBuilder.Append("Hearing: " + Environment.NewLine);
            statsBuilder.Append("Scent: " + Environment.NewLine);
            statsBuilder.Append("Sight: " + Environment.NewLine);
            statsBuilder.Append("Bark: " +  Environment.NewLine);
            return statsBuilder.ToString();
        }
        //Create string with all stats
        private string SetDescriptionText(Dog dog)
        {
            System.Text.StringBuilder descriptionBuilder = new System.Text.StringBuilder();
            descriptionBuilder.Append(dog.ReturnName() + " is a " + dog.ReturnSex().ToLower() + ", " +dog.ReturnSizeDescription().ToLower() + " sized dog with a " +
                                      dog.ReturnHairLengthDescription().ToLower() + " length " + "coat." +Environment.NewLine + Environment.NewLine);
            return descriptionBuilder.ToString();

        }

        //Create string with all stats
        private string SetStatNumberText(Dog _dog)
        {
            System.Text.StringBuilder statsNumbersBuilder = new System.Text.StringBuilder();

           
         
            statsNumbersBuilder.Append(_dog.ReturnIntelligence() / 10 + "/10 (" + _dog.ReturnIntelligenceDescription() + ")  " + Environment.NewLine);
            statsNumbersBuilder.Append(_dog.ReturnEndurance() / 10 + "/10 (" + _dog.ReturnEnduranceDescription() + ")  " + Environment.NewLine);
            statsNumbersBuilder.Append(_dog.ReturnDemeanor() / 10 + "/10 (" + _dog.ReturnDemeanorDescription() + ")  " + Environment.NewLine);
            statsNumbersBuilder.Append(_dog.ReturnHearing() / 10 + "/10 (" + _dog.ReturnHearingDescription() + ")  " + Environment.NewLine);
            statsNumbersBuilder.Append(_dog.ReturnScent() / 10 + "/10 (" + _dog.ReturnScentDescription() + ")  " + Environment.NewLine);
            statsNumbersBuilder.Append(_dog.ReturnSight() / 10 + "/10 (" + _dog.ReturnSightDescription() + ")  " + Environment.NewLine);
            statsNumbersBuilder.Append(_dog.ReturnBark() / 10 + "/10 (" + _dog.ReturnBarkDescription() + ")  " + Environment.NewLine);
            return statsNumbersBuilder.ToString();
        }

}
