using System;
using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Linq;
using Mono.Data.Sqlite;

public class Grid : MonoBehaviour
{
    public enum Direction
    {
        Horizontal, Vertical, None
    }
    //Todo: Implement points count
    public Tile TilePrefab;
    public Direction CurrentDirection = Direction.None;
    public int CurrentTurn = 1;
    //public bool isFirstCurrentTurn = true;
    public byte NumberOfRows = 15;
    public byte NumberOfColumns = 15;
    public LetterBox Player1;
    public LetterBox Player2;
    public byte CurrentPlayer = 1;
    public float DistanceBetweenTiles = 1.2f;
    public Tile[,] Field;
    public List<Tile> CurrentTiles;
    private SqliteConnection dbConnection;


    void Start()
    {
        CurrentTiles = new List<Tile>();
        CreateField();
        var conection = @"URI=file:" + Application.dataPath + @"/words.db";
        dbConnection = new SqliteConnection(conection);
        dbConnection.Open();
    }

    // Update is called once per frame
    void Update()
    {

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
                    new Vector2(transform.position.x + xOffset, transform.position.y + yOffset),
                    transform.rotation) as Tile;
                newTile.transform.SetParent(gameObject.transform);
                newTile.Column = j;
                newTile.Row = i;
                Field[i, j] = newTile;
                xOffset += DistanceBetweenTiles;
            }
            xOffset = 0;
            yOffset += DistanceBetweenTiles;
        }
        Field[7, 7].CanDrop = true;
    }

    public void OnEndTurn()
    {
        if (CheckWords())
        {
            CurrentTurn++;
            if (CurrentPlayer == 1)
            {
                Player1.ChangeBox(7-Player1.CurrentLetters.Count);
                Player1.gameObject.SetActive(false);
                Player2.gameObject.SetActive(true);
                CurrentTiles = new List<Tile>();
                CurrentDirection = Direction.None;
                CurrentPlayer = 2;
            }
            else
            {
                Player2.ChangeBox(7-Player2.CurrentLetters.Count);
                Player1.gameObject.SetActive(true);
                Player2.gameObject.SetActive(false);
                CurrentPlayer = 1;
                CurrentDirection = Direction.None;
                CurrentTiles = new List<Tile>();
            }
            Debug.Log("Current player: " + CurrentPlayer);
        }
        else Debug.Log("Not exist");
    }

    bool CheckWords()
    {
        //Todo: change to bool
        //Check the first word. If not in db return false
        int currentStart, currentEnd, globalStart, globalEnd;
        string current;
        bool wordExists;
        if (CurrentDirection == Direction.Vertical)
        {
            FindWord(CurrentTiles[0], CurrentDirection, out globalStart, out globalEnd);
            current = CreateWord(CurrentDirection, Field[globalStart, CurrentTiles[0].Column], globalEnd);
            Debug.Log(current);
            wordExists = CheckWord(current);
            if (!wordExists)
                return false;
            CurrentDirection = Direction.Horizontal;
            for (int j = globalStart; j <= globalEnd; j++)
            {
                FindWord(Field[j, CurrentTiles[0].Column], CurrentDirection, out currentStart, out currentEnd);
                current = CreateWord(CurrentDirection, Field[j, currentStart], currentEnd);
                wordExists = CheckWord(current);
                Debug.Log(current);
            }
        }
        else
        {
            FindWord(CurrentTiles[0], CurrentDirection, out globalStart, out globalEnd);
            current = CreateWord(CurrentDirection, Field[CurrentTiles[0].Row, globalStart], globalEnd);
            Debug.Log(current);
            wordExists = CheckWord(current);
            if (!wordExists)
                return false;
            CountPoints(Field[CurrentTiles[0].Row, globalStart], Field[CurrentTiles[0].Row, globalEnd]);
            CurrentDirection = Direction.Vertical;
            for (int j = globalStart; j <= globalEnd; j++)
            {
                FindWord(Field[CurrentTiles[0].Row, j], CurrentDirection, out currentStart, out currentEnd);
                current = CreateWord(CurrentDirection, Field[currentStart, j], currentEnd);
                wordExists = CheckWord(current);
                Debug.Log(current);
            }
        }
        return true;
    }

    private void CountPoints(Tile start, Tile end)
    {
        //Todo
        return;
    }

    string CreateWord(Direction current, Tile start, int end)
    {
        var sb = new StringBuilder();
        if (current == Direction.Vertical)
        {
            for (int j = end; j >= start.Row; j--)
            {
                string temp = Field[j, start.Column].CurrentLetter.text;
                if (String.Equals("*", temp))
                    temp = "_";
                sb.Append(temp);
            }
            return sb.ToString();
        }
        else
        {
            for (int j = start.Column; j <= end; j++)
            {
                var temp = Field[start.Row, j].CurrentLetter.text;
                if (String.Equals("*", temp))
                    temp = "_";
                sb.Append(temp);
            }
            return sb.ToString();
        }
    }
    void FindWord(Tile currentTile, Direction current, out int startPosition, out int endPosition)
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

    bool CheckWord(string word)
    {
        var sql = "SELECT count(*) FROM AllWords WHERE Word like \"" + word.ToUpper() + "\"";
        var command = new SqliteCommand(sql, dbConnection);
        var inp = command.ExecuteScalar();
        return Convert.ToInt32(inp) != 0;
    }

    }

