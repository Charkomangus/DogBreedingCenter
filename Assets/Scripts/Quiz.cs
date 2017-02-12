using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quiz : MonoBehaviour
{
    private Animator _animator ;
	// Use this for initialization
	void Start ()
	{
	    _animator = GetComponent<Animator>();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool ReturnStatus()
    {
        return _animator.GetBool("Open");
    }
    public void ChangeQuizStatus()
    {
        _animator.SetBool("Open", !_animator.GetBool("Open"));
    }
}
