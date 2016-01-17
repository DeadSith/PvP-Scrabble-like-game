using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

public class Letter : MonoBehaviour, IPointerClickHandler
{

    public Text LetterText;
    public Material StandardMaterial;
    public Material CheckedMaterial;
    public bool isChecked = false;
    private LetterBox parent;
    // Use this for initialization
    void Start ()
    {
        parent = gameObject.transform.parent.GetComponent<LetterBox>();
    }

    

    public void ChangeLetter(string input)
    {
        LetterText.text = input;
    }


    public void OnPointerClick(PointerEventData eventData)
    { 
        if (eventData.button == PointerEventData.InputButton.Right&&parent.CanChangeLetters)
        {
            gameObject.GetComponent<Image>().material = isChecked ? StandardMaterial : CheckedMaterial;
            isChecked = !isChecked;
        }
    }
}
