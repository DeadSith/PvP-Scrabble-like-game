using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

//Todo: Add leters only in the end of the turn
public class LetterBox : MonoBehaviour
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
            {"*", 0}
        }; 
    private static List<string> _allLetters = new List<string>
    {
        "а","а","с","и","р","о","р","д","ш","и","и","и","ц","е","н","а","т","щ","л","к","ж",
        "п","в","к","о","в","а","у","н","к","е","м","г","і","н","х","і","н","и","н","а","и",
        "р","л","р","с","п","м","а","у","а","ю","м","с","б","в","я","м","т","ф","с","и","о",
        "я","п","о","о","л","е","а","б","і","е","ь","т","р","ґ","з","д","о","і","і","є","й",
        "е","д","н","о","у","г","ї","ч","о","о","о","к","т","н","в","т","з","*","*","*"
    };
    #endregion

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
        _allLetters = _allLetters.OrderBy(letter => letter).ToList();
        _freeCoordinates = new List<Vector3>();
        _pos = new Vector3(transform.position.x, transform.position.y);
        ChangeBox(NumberOfLetters);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    
    public void ChangeBox(int numberOfLetters,string letter = null)
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
            for (int j = 0; j < numberOfLetters; j++)
            {
                AddLetter(_freeCoordinates[_freeCoordinates.Count-1], letter);
                _freeCoordinates.RemoveAt(_freeCoordinates.Count-1);
            }
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
        var currentObject = DragHandler.ObjectDragged.GetComponent<Letter>();
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
