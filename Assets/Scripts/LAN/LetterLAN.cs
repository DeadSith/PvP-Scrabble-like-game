using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LetterLAN : MonoBehaviour, IPointerClickHandler
{
    public Text LetterText;
    public Material StandardMaterial;
    public Material CheckedMaterial;
    public bool isChecked = false;
    private LetterBoxLAN parent;
    private Vector3 _startPosition;

    // Use this for initialization
    private void Start()
    {
        _startPosition = transform.position;
        parent = gameObject.transform.parent.GetComponent<LetterBoxLAN>();
    }

    public void ChangeLetter(string input)
    {
        LetterText.text = input;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right && parent.CanChangeLetters)
        {
            gameObject.GetComponent<Image>().material = isChecked ? StandardMaterial : CheckedMaterial;
            isChecked = !isChecked;
        }
    }

    public void Fix()
    {
        parent.FreeCoordinates.Add(_startPosition);
        parent.ChangeBox(1, LetterText.text);
        var index = parent.FindIndex(this);
        parent.CurrentLetters[index] = parent.CurrentLetters[parent.CurrentLetters.Count - 1];
        parent.CurrentLetters.RemoveAt(parent.CurrentLetters.Count - 1);
        transform.position = new Vector3(-500, -500);
    }
}