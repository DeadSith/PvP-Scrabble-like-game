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
    private DateTime _lastTap;

    private void Start()
    {
        _startPosition = transform.position;
        parent = gameObject.transform.parent.GetComponent<LetterBoxH>();
        _lastTap = new DateTime(0);
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
        if (DateTime.Now - _lastTap < new TimeSpan((int)(TimeSpan.TicksPerSecond*parent.DoubleTapDuration))&& parent.CanChangeLetters)
        {
            gameObject.GetComponent<Image>().material = isChecked ? StandardMaterial : CheckedMaterial;
            isChecked = !isChecked;
            _lastTap = new DateTime(0);
        }
        _lastTap = DateTime.Now;

    }

    //Removes stuck letter from field
    public void Fix()
    {
        throw new NotImplementedException();
    }
}