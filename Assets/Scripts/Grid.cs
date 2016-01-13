using System;
using UnityEngine;
using System.Collections.Generic;
using System.Text;

public class Grid : MonoBehaviour
{
    public enum Direction
    {
        Horizontal, Vertical, None
    }
    //Make Direction Checks
    public Tile TilePrefab;
    public Direction CurrentDirection= Direction.None;
    public int CurrentTurn=1;
    //public bool isFirstCurrentTurn = true;
    public byte NumberOfRows = 15;
    public byte NumberOfColumns = 15;
    public LetterBox Player1;
    public LetterBox Player2;
    public byte CurrentPlayer = 1;
    public float DistanceBetweenTiles = 1.2f;
    public Tile[,] Field;
    public List<Tile> CurrentTiles;
    
    
     
	void Start () {
     CurrentTiles = new List<Tile>();
	CreateField();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void CreateField()
    {
        var xOffset = 0f;
        var yOffset = 0f;
        Field = new Tile[NumberOfRows, NumberOfColumns];
        for (var i = 0; i < NumberOfRows; i++)
        {
            for (var j = 0; j < NumberOfColumns; j++)
            {
                var newTile = Instantiate(TilePrefab,
                    new Vector2(transform.position.x + xOffset, transform.position.y+ yOffset),
                    transform.rotation) as Tile;
                newTile.transform.SetParent(gameObject.transform);
                newTile.Column = j;
                newTile.Row = i;
                Field[i,j] = newTile;
                xOffset += DistanceBetweenTiles;
            }
            xOffset = 0;
            yOffset += DistanceBetweenTiles;
        }
        Field[7, 7].CanDrop = true;
    }

    public void OnEndTurn()
    {
        CurrentTurn++;
        if (CurrentPlayer == 1)
        {
            CheckWords();
            Player1.gameObject.SetActive(false);
            Player2.gameObject.SetActive(true);
            CurrentTiles = new List<Tile>();
            CurrentDirection = Direction.None;
            CurrentPlayer = 2;
        }
        else
        {
            CheckWords();
            Player1.gameObject.SetActive(true);
            Player2.gameObject.SetActive(false);
            CurrentPlayer = 1;
            CurrentDirection=Direction.None;
            CurrentTiles = new List<Tile>();
        }
        Debug.Log("Current player: "+CurrentPlayer);
    }

    void CheckWords()
    {
        //Todo: change to bool
        //Todo: (Main) Fix parallel lines
        //Check the first word. If not in db return false
        int curentStart, curentEnd, globalStart, globalEnd;
        string curent;
        if (CurrentDirection == Direction.Vertical)
        {
            FindWord(CurrentTiles[0], CurrentDirection, out globalStart, out globalEnd);
            curent = CreateWord(CurrentDirection, Field[globalStart, CurrentTiles[0].Column], globalEnd);
            Debug.Log(curent);
            if (CurrentTiles[0].Column != 0)
            {
                FindWord(Field[CurrentTiles[0].Row, CurrentTiles[0].Column - 1], CurrentDirection, out curentStart, out curentEnd);
                curent = CreateWord(CurrentDirection, Field[CurrentTiles[0].Row, curentStart], curentEnd);
                Debug.Log(curent);
            }
            if (CurrentTiles[0].Column != NumberOfColumns - 1)
            {
                FindWord(Field[CurrentTiles[0].Row, CurrentTiles[0].Column + 1], CurrentDirection, out curentStart, out curentEnd);
                curent = CreateWord(CurrentDirection, Field[CurrentTiles[0].Row, curentStart], curentEnd);
                Debug.Log(curent);
            }
            CurrentDirection = Direction.Horizontal;
            
            for (int j = globalStart; j <= globalEnd; j++)
            {
                FindWord(Field[j, CurrentTiles[0].Column], CurrentDirection, out curentStart, out curentEnd);
                curent = CreateWord(CurrentDirection, Field[j, curentStart], curentEnd);
                Debug.Log(curent);
            }
        }
        else
        {
            FindWord(CurrentTiles[0], CurrentDirection, out globalStart, out globalEnd);
            curent = CreateWord(CurrentDirection, Field[CurrentTiles[0].Row, globalStart], globalEnd);
            Debug.Log(curent);
            if (CurrentTiles[0].Row != 0)
            {
                FindWord(Field[CurrentTiles[0].Row - 1, CurrentTiles[0].Column], CurrentDirection, out curentStart, out curentEnd);
                curent = CreateWord(CurrentDirection, Field[curentStart, CurrentTiles[0].Column], curentEnd);
                Debug.Log(curent);
            }
            if (CurrentTiles[0].Row != NumberOfRows - 1)
            {
                FindWord(Field[CurrentTiles[0].Row + 1, CurrentTiles[0].Column], CurrentDirection, out curentStart, out curentEnd);
                curent = CreateWord(CurrentDirection, Field[curentStart, CurrentTiles[0].Column], curentEnd);
                Debug.Log(curent);
            }
            CurrentDirection = Direction.Vertical;
            for (int j = globalStart; j <= globalEnd; j++)
            {
                FindWord(Field[CurrentTiles[0].Row, j], CurrentDirection, out curentStart, out curentEnd);
                curent = CreateWord(CurrentDirection, Field[curentStart, j], curentEnd);
                Debug.Log(curent);
            }
        }
    }
    string CreateWord(Direction current, Tile start, int end)
    {
        var sb = new StringBuilder();
        if (current == Direction.Vertical)
        {
            for (int j = start.Row; j <= end; j++)
                sb.Append(Field[j, start.Column].CurrentLetter.text);
            return sb.ToString();
        }
        else
        {
            for (int j = start.Column; j <= end; j++)
                sb.Append(Field[start.Row,j].CurrentLetter.text);
            return sb.ToString();
        }
    }
    void FindWord(Tile currentTile,Direction current, out int startPosition, out int endPosition)
    {
        if (current == Direction.Vertical)
        {
            var j = currentTile.Row;
            while (j >= 0 && Field[j, currentTile.Column].HasLetter)
            {
                j--;
            }
            j++;
            startPosition = j;
            //Debug.Log(j);
            j = currentTile.Row;
            while (j < NumberOfRows && Field[j, currentTile.Column].HasLetter)
            {
                j++;
            }
            j--;
            //Debug.Log(j);
            endPosition = j;
        }
        else
        {
            var j = currentTile.Column;
            while (j >= 0 && Field[currentTile.Row, j].HasLetter)
            {
                j--;
            }
            j++;
            //Debug.Log(j);
            startPosition = j;
            j = currentTile.Column;
            while (j < NumberOfRows && Field[currentTile.Row, j].HasLetter)
            {
                j++;
            }
            j--;
            endPosition = j;
            //Debug.Log(j);
        }
    }

}

