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
        PointsText.enabled = false;
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

    //Hides points, shows letter
    public void OnMouseExit()
    {
        LetterText.enabled = true;
        PointsText.enabled = false;
    }

    //Shows points for current letter
    public void OnMouseEnter()
    {
        PointsText.text = LetterBoxH.PointsDictionary[LetterText.text].ToString();
        LetterText.enabled = false;
        PointsText.enabled = true;
    }

    //Removes stuck letter from field
    public void Fix()
    {
        parent.FreeCoordinates.Add(DragHandler.StartPosition);
        parent.ChangeBox(1, LetterText.text);
        var index = parent.FindIndex(this);
        parent.CurrentLetters[index] = parent.CurrentLetters[parent.CurrentLetters.Count - 1];
        parent.CurrentLetters.RemoveAt(parent.CurrentLetters.Count - 1);
        transform.position = new Vector3(-1500, -1500);
    }
}