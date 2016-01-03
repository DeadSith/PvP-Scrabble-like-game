using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class Letter : MonoBehaviour
{

    public Text LetterText;
    
    // Use this for initialization
    void Start () {
	
	}

    

    public void ChangeLetter(string input)
    {
        LetterText.text = input;
    }
    
    
}
