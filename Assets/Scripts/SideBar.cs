using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class SideBar : MonoBehaviour
{


    [SerializeField] private Text[] _allText;
	// Use this for initialization
	void Start ()
	{
	    _allText = GetComponentsInChildren<Text>();
	    for (int i = 0; i < 3; i++)
	    {
            _allText[i].text = LoadText(GameManager.Instance.CurrentLevel + "/MissionObjective" + i);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //Load asset in resources folder using the filepath given
    public string LoadText(string filepath)
    {
        if (Resources.Load(filepath) != null)
            return Resources.Load(filepath).ToString();
        return null;

    }
}
