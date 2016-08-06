using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LetterH : MonoBehaviour, IPointerClickHandler
{
    public Text LetterText;
    public Text PointsText;
    public Material StandardMaterial;
    public Material CheckedMaterial;
    public bool isChecked = false;
    private LetterBoxH parent;
    private Vector3 _startPosition;

    private void Start()
    {
        _startPosition = transform.position;
        parent = gameObject.transform.parent.GetComponent<LetterBoxH>();
    }

    public void ChangeLetter(string letter)
    {
        LetterText.text = letter;
        PointsText.enabled = true;
        PointsText.text = LetterBoxH.PointsDictionary[LetterText.text].ToString();
    }

    //Mark LetterH as checked
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && parent.CanChangeLetters)
        {
            gameObject.GetComponent<Image>().material = isChecked ? StandardMaterial : CheckedMaterial;
            isChecked = !isChecked;
        }
    }

    //Removes stuck letter from field
    public void Fix()
    {
        throw new NotImplementedException();
    }
}