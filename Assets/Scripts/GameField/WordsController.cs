using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class WordsController : MonoBehaviour
{

    public GameObject WordButton;
    private GameObject parent;
	void Start ()
    {
        parent = GameObject.FindWithTag("Words");
        for (int i = 0; i < 10; i++)
        {
            AddButton(i.ToString());
        }
	}
	
	
	void AddButton (string word)
	{
	    var button = Instantiate(WordButton, parent.transform) as GameObject;
	    button.GetComponentInChildren<Text>().text = word;
	}
}
