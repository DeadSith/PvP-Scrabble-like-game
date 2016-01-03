using System;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;

public class LetterBox : MonoBehaviour
{
    private List<string> AllLetters;
    private List<Vector3> FreeCoordinates; 
    public  List<string> CurrentLetters; 
    public Letter LetterPrefab;
    private Vector3 pos;
    private int i = 0;
    public byte NumberOfLetters = 7;
    public float DistanceBetweenLetters = 1.2f;
    // Use this for initialization
    void Start () {
	AllLetters = new List<string> {"a","b","c","d","e","f","g","h","j","k"};
        CurrentLetters= new List<string>();
        FreeCoordinates = new List<Vector3>();
        pos = new Vector3(transform.position.x, transform.position.y);
        ChangeBox(NumberOfLetters);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void ChangeBox(byte numberOfLetters,string letter = null)
    {
        if (FreeCoordinates.Count == 0)
        {
            int max = i + numberOfLetters;
            for (; i < max; i++)
            {
                AddLetter(pos, letter);
                pos.x += DistanceBetweenLetters;
                if (i%2 == 1)
                {
                    pos.x = transform.position.x;
                    pos.y -= DistanceBetweenLetters;
                }
            }
        }
        else
        {
            AddLetter(FreeCoordinates[0],letter);
            FreeCoordinates.RemoveAt(0);
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
            var current = AllLetters[UnityEngine.Random.Range(0, AllLetters.Count - 1)];
            newLetter.ChangeLetter(current);
            AllLetters.Remove(current);
            CurrentLetters.Add(current);
        }
        else
        {
            newLetter.ChangeLetter(letter);
            CurrentLetters.Add(letter);
        }
        
    }

    public void ChangeLetter(string input)
    {
        CurrentLetters.Remove(input);
        if (AllLetters.Count != 0)
            AddLetter(DragHandler.StartPosition, "");
        else
        {
            Debug.Log("Out of letters");
            FreeCoordinates.Add(DragHandler.StartPosition);
        }
    }
}
