using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoLSDK;


public class Polaroid : MonoBehaviour
{
    [SerializeField]private float _letterPause;
    [SerializeField]
    private Animator[] _polaroids;
	// Use this for initialization
	void Start () {
        _polaroids = new Animator[transform.childCount];
	    for (int i = 0; i < transform.childCount; i++)
	    {
            _polaroids[i] = transform.GetChild(i).GetComponent<Animator>();
        }
    }

    public void ActivatePolaroids()
    {
        StartCoroutine(ShowPolaroids());
    }

    //Show polaroids
    private IEnumerator ShowPolaroids()
    {
        for (int i = 0; i < _polaroids.Length; i++)
        {
            _polaroids[i].SetBool("Open", true);
            yield return new WaitForSeconds(_letterPause);
            _letterPause -= 0.005f;

        }
    }
}
