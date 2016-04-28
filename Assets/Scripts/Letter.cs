using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Letter : MonoBehaviour, IPointerClickHandler
{
    public Text LetterText;
    public Text PointsText;
    public Material StandardMaterial;
    public Material CheckedMaterial;
    public bool isChecked = false;

    private LetterBox parent;
    private Vector3 _startPosition;

    private void Start()
    {
        _startPosition = transform.position;
        parent = gameObject.transform.parent.GetComponent<LetterBox>();
    }

    public void ChangeLetter(string letter)
    {
        LetterText.text = letter;
        PointsText.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && parent.CanChangeLetters)
        {
            gameObject.GetComponent<Image>().material = isChecked ? StandardMaterial : CheckedMaterial;
            isChecked = !isChecked;
        }
    }

    public void OnMouseExit()
    {
        LetterText.enabled = true;
        PointsText.enabled = false;
    }

    public void OnMouseEnter()
    {
        PointsText.text = LetterBox.PointsDictionary[LetterText.text].ToString();
        LetterText.enabled = false;
        PointsText.enabled = true;
    }

    public void Fix()
    {
        parent.FreeCoordinates.Add(_startPosition);
        parent.ChangeBox(1, LetterText.text);
        var index = parent.FindIndex(this);
        parent.CurrentLetters[index] = parent.CurrentLetters[parent.CurrentLetters.Count - 1];
        parent.CurrentLetters.RemoveAt(parent.CurrentLetters.Count - 1);
        transform.position = new Vector3(-1500, -1500);
    }
}