﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LetterBoxH : MonoBehaviour
{
    #region Letters and scores

    public static Dictionary<string, int> PointsDictionary =
        new Dictionary<string, int>
        {
            {"а", 1},
            {"с", 2},
            {"и", 1},
            {"р", 1},
            {"о", 1},
            {"д", 2},
            {"ш", 6},
            {"ц", 6},
            {"е", 1},
            {"н", 1},
            {"т", 1},
            {"щ", 8},
            {"к", 2},
            {"ж", 6},
            {"п", 2},
            {"в", 1},
            {"у", 3},
            {"м", 2},
            {"г", 4},
            {"і", 1},
            {"х", 5},
            {"л", 2},
            {"ю", 7},
            {"б", 4},
            {"я", 4},
            {"ф", 8},
            {"ь", 5},
            {"ґ", 10},
            {"з", 4},
            {"є", 8},
            {"й", 5},
            {"ї", 6},
            {"ч", 5},
            {"*", 0},
            {"'", 10}
        };

    private static List<string> _allLetters;

    #endregion Letters and scores

    public List<LetterH> CurrentLetters;
    public int Score = 0;
    public Button ChangeLetterButton;
    public LetterH LetterHPrefab;
    public bool CanChangeLetters = true;
    public byte NumberOfLetters = 7;
    public float DistanceBetweenLetters = 1.2f;
    public Vector2 LetterSize;
    public Vector3 OriginalPosition;
    public float DoubleTapDuration = 1;

    private FieldH _currentFieldH;
    public UIGrid LetterGrid;

    private void Start()
    {
        if (_allLetters == null)
            _allLetters = new List<string>
    {
        "а","а","с","и","р","о","р","д","ш","и","и","и","ц","е","н","а","т","щ","л","к","ж",
        "п","в","к","о","в","а","у","н","к","е","м","г","і","н","х","і","н","и","н","а","и",
        "р","л","р","с","п","м","а","у","а","ю","м","с","б","в","я","м","т","ф","с","и","о",
        "я","п","о","о","л","е","а","б","і","е","ь","т","р","ґ","з","д","о","і","і","є","й",
        "е","д","н","о","у","г","ї","ч","о","о","о","к","т","н","в","т","з","'","*","*"
    };
        CurrentLetters = new List<LetterH>();
        _allLetters = _allLetters.OrderBy(letter => letter).ToList();
        _currentFieldH = GameObject.FindGameObjectWithTag("Field").GetComponent<FieldH>();
        DistanceBetweenLetters = LetterSize.x;
        LetterHPrefab.gameObject.GetComponent<RectTransform>().sizeDelta = LetterSize;
        OriginalPosition = gameObject.transform.localPosition;
        ChangeBox(NumberOfLetters);
        if (_currentFieldH.Player2 == this)
            gameObject.transform.localPosition = new Vector3(-2000, -2000);
    }

    //Activates/deactivates ChangeLetters button
    private void Update()
    {
        if (_allLetters == null || _allLetters.Count == 0)
            CanChangeLetters = false;
        else CanChangeLetters = _currentFieldH.CurrentTiles.Count == 0;
        ChangeLetterButton.interactable = CanChangeLetters;
    }

    //Clean _allLetters when exiting to main menu
    private void OnDisable()
    {
        if (PlayerPrefs.GetInt("Exiting", 0) == 1)
        {
            PlayerPrefs.SetInt("Exiting", 0);
            _allLetters = null;
        }
    }

    //Adds letters to the hand of player
    public void ChangeBox(int numberOfLetters, string letter = null)
    {
        if (String.IsNullOrEmpty(letter) && numberOfLetters > _allLetters.Count)
        {
            numberOfLetters = _allLetters.Count;
        }
        for (var i = 0; i < numberOfLetters; i++)
        {
            AddLetter(CurrentLetters.Count, letter);
        }
    }

    //Crates new LetterH on field
    private void AddLetter(int index, string letter)
    {
        var newLetter = Instantiate(LetterHPrefab);
        newLetter.transform.SetParent(gameObject.transform);
        LetterGrid.AddElement(index, 1, newLetter.gameObject);
        LetterGrid.AddElement(index, 0, newLetter.PointsText.gameObject);
        if (String.IsNullOrEmpty(letter))//if new letter is created
        {
            var current = _allLetters[UnityEngine.Random.Range(0, _allLetters.Count)];
            newLetter.ChangeLetter(current);
            _allLetters.Remove(current);
            CurrentLetters.Add(newLetter);
        }
        else//if letter is returned from Field
        {
            newLetter.ChangeLetter(letter);
            CurrentLetters.Add(newLetter);
        }
    }

    //Removes letter from hand when it is dropped on grid
    public void RemoveLetter()
    {
        var currentObject = DragHandler.ObjectDragged.GetComponent<LetterH>();
        var currentIndex = FindIndex(currentObject);
        Debug.Log(currentIndex);
        for (var j = currentIndex; j < CurrentLetters.Count-1; j++)//shifts all letters
        {
            LetterGrid.AddElement(j, 1, CurrentLetters[j + 1].gameObject);
            LetterGrid.AddElement(j, 0, CurrentLetters[j + 1].PointsText.gameObject);
        }
        CurrentLetters.Remove(currentObject);
    }

    //Changes letters for Letters player checked
    public bool ChangeLetters()
    {
        var successful = false;
        foreach (LetterH t in CurrentLetters)
        {
            if (t.isChecked)
            {
                var text = t.LetterText.text;
                t.ChangeLetter(_allLetters[UnityEngine.Random.Range(0, _allLetters.Count)]);
                _allLetters.Add(text);
                _allLetters.Remove(t.LetterText.text);
                t.isChecked = false;
                t.gameObject.GetComponent<Image>().material = t.StandardMaterial;
                successful = true;
            }
        }
        return successful;
    }

    //Finds the index of LetterH in CurrentLetters
    public int FindIndex(LetterH input)
    {
        var j = 0;
        for (; j < CurrentLetters.Count; j++)
        {
            if (CurrentLetters[j] == input)
                return j;
        }
        return -1;
    }

    //Called in endgame to remove points from final score for each letter left
    //Returns sum of points deleted
    public int RemovePoints()
    {
        var result = 0;
        _allLetters = null;
        foreach (var letter in CurrentLetters)
        {
            result += PointsDictionary[letter.LetterText.text];
        }
        Score -= result;
        return result;
    }
}