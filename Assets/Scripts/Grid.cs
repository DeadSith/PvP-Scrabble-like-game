using System;
using UnityEngine;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Linq;
using Mono.Data.Sqlite;
using UnityEngine.UI;

public class Grid : MonoBehaviour
{
    public enum Direction
    {
        Horizontal, Vertical, None
    }
    //Todo: Implement points count
    #region Prefabs and materials
    public Tile TilePrefab;
    public Material StandardMaterial;
    public Material StartMaterial;
    public Material WordX2Material;
    public Material WordX3Material;
    public Material LetterX2Material;
    public Material LetterX3Material;
    #endregion
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
                var render = newTile.GetComponent<Image>();
                render.material = StandardMaterial;
                newTile.Row = i;
                Field[i, j] = newTile;
                xOffset += DistanceBetweenTiles;
            }
            xOffset = 0;
            yOffset += DistanceBetweenTiles;
        }
        Field[7, 7].CanDrop = true;
        Field[7, 7].GetComponent<Image>().material=StartMaterial;
        AssignMaterials();
        AssignMultipliers();
    }
    #region Some shitty code
    //Todo: rewrite to cycles
    void AssignMaterials()
    {
        Field[0, 0].GetComponent<Image>().material = WordX3Material;
        Field[0, 14].GetComponent<Image>().material = WordX3Material;
        Field[14, 0].GetComponent<Image>().material = WordX3Material;
        Field[14, 14].GetComponent<Image>().material = WordX3Material;
        Field[0, 7].GetComponent<Image>().material = WordX3Material;
        Field[14, 7].GetComponent<Image>().material = WordX3Material;
        Field[7, 0].GetComponent<Image>().material = WordX3Material;
        Field[14, 7].GetComponent<Image>().material = WordX3Material;
        for (var i = 1; i < 5; i++)
        {
            Field[i,i].GetComponent<Image>().material = WordX2Material;
            Field[i,NumberOfRows-i-1].GetComponent<Image>().material = WordX2Material;
            Field[NumberOfRows - i-1, i].GetComponent<Image>().material = WordX2Material;
            Field[NumberOfRows - i-1, NumberOfRows - i-1].GetComponent<Image>().material = WordX2Material;
        }
        Field[5, 1].GetComponent<Image>().material = LetterX3Material;
        Field[5, 5].GetComponent<Image>().material = LetterX3Material;
        Field[5, 9].GetComponent<Image>().material = LetterX3Material;
        Field[5, 13].GetComponent<Image>().material = LetterX3Material;
        Field[9, 1].GetComponent<Image>().material = LetterX3Material;
        Field[9, 5].GetComponent<Image>().material = LetterX3Material;
        Field[9, 9].GetComponent<Image>().material = LetterX3Material;
        Field[9, 13].GetComponent<Image>().material = LetterX3Material;
        Field[1, 5].GetComponent<Image>().material = LetterX3Material;
        Field[1, 9].GetComponent<Image>().material = LetterX3Material;
        Field[13, 5].GetComponent<Image>().material = LetterX3Material;
        Field[13, 9].GetComponent<Image>().material = LetterX3Material;
        Field[0, 3].GetComponent<Image>().material = LetterX2Material;
        Field[0, 11].GetComponent<Image>().material = LetterX2Material;
        Field[14, 3].GetComponent<Image>().material = LetterX2Material;
        Field[14, 3].GetComponent<Image>().material = LetterX2Material;
        Field[2, 6].GetComponent<Image>().material = LetterX2Material;
        Field[2, 8].GetComponent<Image>().material = LetterX2Material;
        Field[12, 6].GetComponent<Image>().material = LetterX2Material;
        Field[12, 8].GetComponent<Image>().material = LetterX2Material;
        Field[3, 0].GetComponent<Image>().material = LetterX2Material;
        Field[3, 7].GetComponent<Image>().material = LetterX2Material;
        Field[3, 14].GetComponent<Image>().material = LetterX2Material;
        Field[11, 0].GetComponent<Image>().material = LetterX2Material;
        Field[11, 7].GetComponent<Image>().material = LetterX2Material;
        Field[11, 14].GetComponent<Image>().material = LetterX2Material;
        Field[6, 2].GetComponent<Image>().material = LetterX2Material;
        Field[6, 6].GetComponent<Image>().material = LetterX2Material;
        Field[6, 8].GetComponent<Image>().material = LetterX2Material;
        Field[6, 12].GetComponent<Image>().material = LetterX2Material;
        Field[8, 2].GetComponent<Image>().material = LetterX2Material;
        Field[8, 6].GetComponent<Image>().material = LetterX2Material;
        Field[8, 8].GetComponent<Image>().material = LetterX2Material;
        Field[8, 12].GetComponent<Image>().material = LetterX2Material;
        Field[7, 3].GetComponent<Image>().material = LetterX2Material;
        Field[7, 11].GetComponent<Image>().material = LetterX2Material;
    }
    void AssignMultipliers()
    {
        Field[0, 0].WordMultiplier = 3;
        Field[0, 14].WordMultiplier = 3;
        Field[14, 0].WordMultiplier = 3;
        Field[14, 14].WordMultiplier = 3;
        Field[0, 7].WordMultiplier = 3;
        Field[14, 7].WordMultiplier = 3;
        Field[7, 0].WordMultiplier = 3;
        Field[14, 7].WordMultiplier = 3;
        for (var i = 1; i < 5; i++)
        {
            Field[i, i].WordMultiplier = 2;
            Field[i, NumberOfRows - i - 1].WordMultiplier = 2;
            Field[NumberOfRows - i - 1, i].WordMultiplier = 2;
            Field[NumberOfRows - i - 1, NumberOfRows - i - 1].WordMultiplier = 2;
        }
        Field[5, 1].LetterMultiplier = 3;
        Field[5, 5].LetterMultiplier = 3;
        Field[5, 9].LetterMultiplier = 3;
        Field[5, 13].LetterMultiplier = 3;
        Field[9, 1].LetterMultiplier = 3;
        Field[9, 5].LetterMultiplier = 3;
        Field[9, 9].LetterMultiplier = 3;
        Field[9, 13].LetterMultiplier = 3;
        Field[1, 5].LetterMultiplier = 3;
        Field[1, 9].LetterMultiplier = 3;
        Field[13, 5].LetterMultiplier = 3;
        Field[13, 9].LetterMultiplier = 3;
        Field[0, 3].LetterMultiplier = 2;
        Field[0, 11].LetterMultiplier = 2;
        Field[14, 3].LetterMultiplier = 2;
        Field[14, 3].LetterMultiplier = 2;
        Field[2, 6].LetterMultiplier = 2;
        Field[2, 8].LetterMultiplier = 2;
        Field[12, 6].LetterMultiplier = 2;
        Field[12, 8].LetterMultiplier = 2;
        Field[3, 0].LetterMultiplier = 2;
        Field[3, 7].LetterMultiplier = 2;
        Field[3, 14].LetterMultiplier = 2;
        Field[11, 0].LetterMultiplier = 2;
        Field[11, 7].LetterMultiplier = 2;
        Field[11, 14].LetterMultiplier = 2;
        Field[6, 2].LetterMultiplier = 2;
        Field[6, 6].LetterMultiplier = 2;
        Field[6, 8].LetterMultiplier = 2;
        Field[6, 12].LetterMultiplier = 2;
        Field[8, 2].LetterMultiplier = 2;
        Field[8, 6].LetterMultiplier = 2;
        Field[8, 8].LetterMultiplier = 2;
        Field[8, 12].LetterMultiplier = 2;
        Field[7, 3].LetterMultiplier = 2;
        Field[7, 11].LetterMultiplier = 2;
    }
    #endregion

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
        //Todo: check nearby words(they should also be in dictionary)
        int currentStart, currentEnd, globalStart, globalEnd;
        string current;
        bool wordExists;
        switch (CurrentDirection)
        {
            case Direction.None:
                bool wordFound = false;
                FindWord(CurrentTiles[0], Direction.Horizontal, out globalStart, out globalEnd);
                if (globalStart != globalEnd)
                {
                    current = CreateWord(Direction.Horizontal, Field[globalStart, CurrentTiles[0].Column], globalEnd);
                    Debug.Log(current);
                    wordExists = CheckWord(current);
                    if (!wordExists)
                        return false;
                    wordFound = true;
                }
                FindWord(CurrentTiles[0], Direction.Vertical, out globalStart, out globalEnd);
                if (globalStart != globalEnd)
                {
                    current = CreateWord(Direction.Vertical, Field[globalStart, CurrentTiles[0].Column], globalEnd);
                    Debug.Log(current);
                    wordExists = CheckWord(current);
                    if (!wordExists)
                        return false;
                    wordFound = true;
                }
                return wordFound;
            case Direction.Vertical:
                FindWord(CurrentTiles[0], CurrentDirection, out globalStart, out globalEnd);
                if(globalStart!=globalEnd)
                {
                    current = CreateWord(CurrentDirection, Field[globalStart, CurrentTiles[0].Column], globalEnd);
                Debug.Log(current);
                wordExists = CheckWord(current);
                if (!wordExists)
                    return false;
                CurrentDirection = Direction.Horizontal;
                }
                else return false;
                foreach (var tile in CurrentTiles)
                {
                    FindWord(tile, CurrentDirection, out currentStart, out currentEnd);
                    if (currentStart != currentEnd)
                    {
                        current = CreateWord(CurrentDirection, Field[tile.Row, currentStart], currentEnd);
                        wordExists = CheckWord(current);
                        if (!wordExists)
                            return false;
                        Debug.Log(current);
                    }
                }
                break;
            case Direction.Horizontal:
                FindWord(CurrentTiles[0], CurrentDirection, out globalStart, out globalEnd);
                if (globalStart != globalEnd)
                {
                    current = CreateWord(CurrentDirection, Field[CurrentTiles[0].Row, globalStart], globalEnd);
                    Debug.Log(current);
                    wordExists = CheckWord(current);
                    if (!wordExists)
                        return false;
                }
                else return false;
                CurrentDirection = Direction.Vertical;
                foreach (var tile in CurrentTiles)
                {
                    FindWord(tile, CurrentDirection, out currentStart, out currentEnd);
                    if(currentStart!=currentEnd)
                    {
                        current = CreateWord(CurrentDirection, Field[currentStart, tile.Column], currentEnd);
                        wordExists = CheckWord(current);
                        if (!wordExists)
                            return false;
                        Debug.Log(current);
                    }
                }
                break;
            default:
                return false;
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

