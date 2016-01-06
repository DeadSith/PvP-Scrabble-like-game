using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
//Todo: Shift letters to free place after dragging
//Todo: Add leters only in the end of the turn
public class LetterBox : MonoBehaviour
{
    private static List<string> _allLetters = new List<string> { "a", "b", "c", "d", "e", "f", "g", "h", "j", "k", "a", "b", "c", "d", "e", "f", "g", "h", "j", "k" };
    private List<Vector3> _freeCoordinates; 
    public  List<Letter> CurrentLetters; 
    public Letter LetterPrefab;
    private Vector3 _pos;
    private int i = 0;
    public byte NumberOfLetters = 7;
    public float DistanceBetweenLetters = 1.2f;
    // Use this for initialization
    void Start () {
        CurrentLetters= new List<Letter>();
        _freeCoordinates = new List<Vector3>();
        _pos = new Vector3(transform.position.x, transform.position.y);
        ChangeBox(NumberOfLetters);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ChangeBox(byte numberOfLetters,string letter = null)
    {
        if (_freeCoordinates.Count == 0)
        {
            int max = i + numberOfLetters;
            for (; i < max; i++)
            {
                AddLetter(_pos, letter);
                _pos.x += DistanceBetweenLetters;
                if (i%4 == 3)
                {
                    _pos.x = transform.position.x;
                    _pos.y -= DistanceBetweenLetters;
                }
            }
        }
        else
        {
            AddLetter(_freeCoordinates[0],letter);
            _freeCoordinates.RemoveAt(0);
        }
    }

    void AddLetter(Vector3 position, string letter)
    {
        var newLetter = Instantiate(LetterPrefab,
            position,
            transform.rotation) as Letter;
        newLetter.transform.SetParent(gameObject.transform);
        if (String.IsNullOrEmpty(letter))
        {
            var current = _allLetters[UnityEngine.Random.Range(0, _allLetters.Count - 1)];
            newLetter.ChangeLetter(current);
            _allLetters.Remove(current);
            CurrentLetters.Add(newLetter);
        }
        else
        {
            newLetter.ChangeLetter(letter);
            CurrentLetters.Add(newLetter);
        }
    }

    public void ChangeLetter(string input)
    {
        //Todo: Rewrite this method
        //Todo: Test
        //CurrentLetters.Remove(input);
        var currentObject = DragHandler.ObjectDragged.GetComponent<Letter>();
        if (_allLetters.Count != 0)
        {
            AddLetter(DragHandler.StartPosition, "");
        }
        else
        {
            Debug.Log("Out of letters");
            _freeCoordinates.Add(DragHandler.StartPosition);
        }
        //_freeCoordinates.Add(DragHandler.StartPosition);
        var currentIndex = FindIndex(currentObject);
        Vector3 previousCoordinates = DragHandler.StartPosition;
        for (int j = currentIndex+1; j < CurrentLetters.Count; j++)
        {
            var tempCoordinates = CurrentLetters[j].gameObject.transform.position;
            CurrentLetters[j].gameObject.transform.position = previousCoordinates;
            previousCoordinates = tempCoordinates;
        }
        _freeCoordinates.Add(previousCoordinates);
        CurrentLetters.Remove(currentObject);
        Debug.Log(currentIndex);

    }

    int FindIndex(Letter input)
    {
        int j = 0;
        for (; j < CurrentLetters.Count; j++)
        {
            if (CurrentLetters[j] == input)
                return j;
        }
        return -1;
    }
}
