using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class FieldH : MonoBehaviour
{
    public enum Direction
    {
        Horizontal, Vertical, None
    }

    #region Prefabs and materials

    public TileH TileHPrefab;
    public Material StandardMaterial;
    public Material StartMaterial;
    public Material WordX2Material;
    public Material WordX3Material;
    public Material LetterX2Material;
    public Material LetterX3Material;

    #endregion Prefabs and materials

    public GameObject TimerImage;
    public Text TimerText;
    public Text Player1Text;
    public Text Player2Text;
    public GameObject EndGameCanvas;
    public UIController Controller;
    public Button SkipTurnButton;

    public UIGrid FieldGrid;
    public Direction CurrentDirection = Direction.None;
    public int CurrentTurn = 1;
    public bool isFirstTurn = true;
    public byte NumberOfRows = 15;
    public byte NumberOfColumns = 15;
    public LetterBoxH Player1;
    public LetterBoxH Player2;
    public byte CurrentPlayer = 1;
    public TileH[,] Field;
    public List<TileH> CurrentTiles;

    private int _turnsSkipped = 0;
    private SqliteConnection _dbConnection;
    private List<TileH> _wordsFound;
    private bool _timerEnabled;
    private int _timerLength;
    private float _timeRemaining;

    private void Start()
    {
        CurrentTiles = new List<TileH>();
        var conection = @"URI=file:" + Application.streamingAssetsPath + @"/words.db";
        _dbConnection = new SqliteConnection(conection);
        _dbConnection.Open();
        _wordsFound = new List<TileH>();
        _timerEnabled = PlayerPrefs.GetInt("TimerEnabled") == 1;
        if (_timerEnabled)
        {
            TimerImage.SetActive(true);
            _timerLength = PlayerPrefs.GetInt("Length");
            _timeRemaining = (float)_timerLength + 1;
        }
        FieldGrid.Initialize();
        var letterSize = FieldGrid.Items[0, 0].gameObject.GetComponent<RectTransform>().rect.width;
        Player1.LetterSize = new Vector2(letterSize, letterSize);
        Player2.LetterSize = new Vector2(letterSize, letterSize);
        CreateField();
        Player1Text.text = PlayerPrefs.GetString("Player1", "Гравець 1");
        Player2Text.text = PlayerPrefs.GetString("Player2", "Гравець 2");
    }

    private void Update()
    {
        if (SkipTurnButton.interactable != (CurrentTiles.Count == 0))
            SkipTurnButton.interactable = CurrentTiles.Count == 0;
        if (Input.GetKeyDown(KeyCode.A))
            EndGame(null);
        if (_timerEnabled)
        {
            _timeRemaining -= Time.deltaTime;
            var timerValue = (int)_timeRemaining - 1;
            if (timerValue < 0)
                timerValue = 0;
            TimerText.text = timerValue.ToString();
            if (_timeRemaining < 0)
                OnEndTimer();
        }
    }

    private void CreateField()
    {
        Field = new TileH[NumberOfRows, NumberOfColumns];
        for (var i = 0; i < NumberOfRows; i++)
        {
            for (var j = 0; j < NumberOfColumns; j++)
            {
                var newTile = Instantiate(TileHPrefab);
                newTile.transform.SetParent(gameObject.transform);
                newTile.Column = j;
                var render = newTile.GetComponent<Image>();
                render.material = StandardMaterial;
                newTile.Row = i;
                Field[i, j] = newTile;
                FieldGrid.AddElement(i, j, newTile.gameObject);
            }
        }
        Field[7, 7].CanDrop = true;
        Field[7, 7].GetComponent<Image>().material = StartMaterial;
        AssignMaterials();
        AssignMultipliers();
    }

    #region Field generation

    private void AssignMaterials()
    {
        Field[0, 0].GetComponent<Image>().material = WordX3Material;
        Field[0, 14].GetComponent<Image>().material = WordX3Material;
        Field[14, 0].GetComponent<Image>().material = WordX3Material;
        Field[14, 14].GetComponent<Image>().material = WordX3Material;
        Field[0, 7].GetComponent<Image>().material = WordX3Material;
        Field[14, 7].GetComponent<Image>().material = WordX3Material;
        Field[7, 0].GetComponent<Image>().material = WordX3Material;
        Field[7, 14].GetComponent<Image>().material = WordX3Material;
        for (var i = 1; i < 5; i++)
        {
            Field[i, i].GetComponent<Image>().material = WordX2Material;
            Field[i, NumberOfRows - i - 1].GetComponent<Image>().material = WordX2Material;
            Field[NumberOfRows - i - 1, i].GetComponent<Image>().material = WordX2Material;
            Field[NumberOfRows - i - 1, NumberOfRows - i - 1].GetComponent<Image>().material = WordX2Material;
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
        Field[14, 11].GetComponent<Image>().material = LetterX2Material;
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

    private void AssignMultipliers()
    {
        Field[0, 0].WordMultiplier = 3;
        Field[0, 14].WordMultiplier = 3;
        Field[14, 0].WordMultiplier = 3;
        Field[14, 14].WordMultiplier = 3;
        Field[0, 7].WordMultiplier = 3;
        Field[14, 7].WordMultiplier = 3;
        Field[7, 0].WordMultiplier = 3;
        Field[7, 14].WordMultiplier = 3;
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
        Field[14, 11].LetterMultiplier = 2;
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

    #endregion Field generation

    private void OnEndTimer()
    {
        _timeRemaining = (float)_timerLength + 1;
        OnRemoveAll();
        OnSkipTurn();
    }

    public void OnEndTurn()
    {
        if (CurrentTiles.Count > 0)
        {
            if (CheckWords())
            {
                _turnsSkipped = 0;
                CurrentTurn++;
                var points = CountPoints();
                if (CurrentPlayer == 1)
                {
                    Player1.ChangeBox(7 - Player1.CurrentLetters.Count);
                    Player1.Score += points;
                    if (Player1.CurrentLetters.Count == 0)
                    {
                        EndGame(Player1);
                    }
                    Player1.gameObject.SetActive(false);
                    Player2.gameObject.SetActive(true);
                    CurrentTiles.Clear();
                    CurrentDirection = Direction.None;
                    CurrentPlayer = 2;
                    Controller.InvalidatePlayer(1, Player1.Score);
                    isFirstTurn = false;
                }
                else
                {
                    Player2.ChangeBox(7 - Player2.CurrentLetters.Count);
                    Player2.Score += points;
                    if (Player2.CurrentLetters.Count == 0)
                        EndGame(Player2);
                    Player1.gameObject.SetActive(true);
                    Player2.gameObject.SetActive(false);
                    CurrentDirection = Direction.None;
                    CurrentTiles.Clear();
                    CurrentPlayer = 1;
                    Controller.InvalidatePlayer(2, Player2.Score);
                    isFirstTurn = false;
                }
                if (_timerEnabled)
                    _timeRemaining = (float)_timerLength + 1;
            }
            else Controller.ShowNotExistError();
        }
        else Controller.ShowZeroTilesError();
        _wordsFound = new List<TileH>();
    }

    public void OnChangeLetters()
    {
        if (CurrentPlayer == 1)
        {
            if (!Player1.ChangeLetters())
            {
                Controller.ShowChangeLetterError();
                return;
            }
            _turnsSkipped = 0;
            Player1.gameObject.SetActive(false);
            Player2.gameObject.SetActive(true);
            CurrentPlayer = 2;
            Controller.InvalidatePlayer(1, Player1.Score);
            CurrentTiles.Clear();
        }
        else
        {
            if (!Player2.ChangeLetters())
            {
                Controller.ShowChangeLetterError();
                return;
            }
            _turnsSkipped = 0;
            Player1.gameObject.SetActive(true);
            Player2.gameObject.SetActive(false);
            CurrentPlayer = 1;
            Controller.InvalidatePlayer(2, Player2.Score);
            CurrentTiles.Clear();
        }
        if (_timerEnabled)
            _timeRemaining = (float)_timerLength + 1;
    }

    public void OnSkipTurn()
    {
        if (CurrentPlayer == 1)
        {
            Player1.gameObject.SetActive(false);
            Player2.gameObject.SetActive(true);
            CurrentPlayer = 2;
            Controller.InvalidatePlayer(1, Player1.Score);
        }
        else
        {
            Player1.gameObject.SetActive(true);
            Player2.gameObject.SetActive(false);
            CurrentPlayer = 1;
            Controller.InvalidatePlayer(2, Player2.Score);
        }
        if (_timerEnabled)
            _timeRemaining = (float)_timerLength + 1;
        if (++_turnsSkipped == 4)
            EndGame(null);
    }

    public void OnRemoveAll()
    {
        for (var i = CurrentTiles.Count - 1; i >= 0; i--)
        {
            CurrentTiles[i].RemoveTile();
        }
        CurrentTiles.Clear();
    }

    #region Word cheking

    private bool CheckWords()
    {
        switch (CurrentDirection)
        {
            case Direction.None:
                bool wordFound = false;
                int currentStart;
                int currentEnd;
                FindWord(CurrentTiles[0], Direction.Horizontal, out currentStart, out currentEnd);
                string current;
                bool wordExists;
                if (currentStart != currentEnd)
                {
                    current = CreateWord(Direction.Horizontal, Field[CurrentTiles[0].Row, currentStart], currentEnd);
                    wordExists = CheckWord(current);
                    if (wordExists)
                    {
                        _wordsFound.Add(Field[CurrentTiles[0].Row, currentStart]);
                        _wordsFound.Add(Field[CurrentTiles[0].Row, currentEnd]);
                    }
                    else return false;
                    wordFound = true;
                }
                FindWord(CurrentTiles[0], Direction.Vertical, out currentStart, out currentEnd);
                if (currentStart != currentEnd)
                {
                    current = CreateWord(Direction.Vertical, Field[currentStart, CurrentTiles[0].Column], currentEnd);
                    wordExists = CheckWord(current);
                    if (wordExists)
                    {
                        _wordsFound.Add(Field[currentStart, CurrentTiles[0].Column]);
                        _wordsFound.Add(Field[currentEnd, CurrentTiles[0].Column]);
                    }
                    else return false;
                    wordFound = true;
                }
                return wordFound;

            case Direction.Vertical:
                return CheckVertical();

            case Direction.Horizontal:
                return CheckHorizontal();

            default:
                return false;
        }
    }

    private bool CheckHorizontal()
    {
        int currentStart, currentEnd;
        string current;
        bool wordExists;
        FindWord(CurrentTiles[0], CurrentDirection, out currentStart, out currentEnd);
        if (currentStart != currentEnd)
        {
            current = CreateWord(CurrentDirection, Field[CurrentTiles[0].Row, currentStart], currentEnd);
            wordExists = CheckWord(current);
            if (wordExists)
            {
                _wordsFound.Add(Field[CurrentTiles[0].Row, currentStart]);
                _wordsFound.Add(Field[CurrentTiles[0].Row, currentEnd]);
            }
            else return false;
        }
        else return false;
        CurrentDirection = Direction.Vertical;
        foreach (var tile in CurrentTiles)
        {
            FindWord(tile, CurrentDirection, out currentStart, out currentEnd);
            if (currentStart != currentEnd)
            {
                current = CreateWord(CurrentDirection, Field[currentStart, tile.Column], currentEnd);
                wordExists = CheckWord(current);
                if (wordExists)
                {
                    _wordsFound.Add(Field[currentStart, tile.Column]);
                    _wordsFound.Add(Field[currentEnd, tile.Column]);
                }
                else return false;
            }
        }
        return true;
    }

    private bool CheckVertical()
    {
        int currentStart, currentEnd;
        string current;
        bool wordExists;
        FindWord(CurrentTiles[0], CurrentDirection, out currentStart, out currentEnd);
        if (currentStart != currentEnd)
        {
            current = CreateWord(CurrentDirection, Field[currentStart, CurrentTiles[0].Column], currentEnd);
            wordExists = CheckWord(current);
            if (wordExists)
            {
                _wordsFound.Add(Field[currentStart, CurrentTiles[0].Column]);
                _wordsFound.Add(Field[currentEnd, CurrentTiles[0].Column]);
            }
            else return false;
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
                if (wordExists)
                {
                    _wordsFound.Add(Field[tile.Row, currentStart]);
                    _wordsFound.Add(Field[tile.Row, currentEnd]);
                }
                else return false;
            }
        }
        return true;
    }

    private int CountPoints()
    {
        var result = 0;
        var wordMultiplier = 1;
        var score = new int[_wordsFound.Count / 2];
        for (var i = 0; i < _wordsFound.Count; i += 2)
        {
            var tempRes = 0;
            if (_wordsFound[i].Row == _wordsFound[i + 1].Row)
                for (var j = _wordsFound[i].Column; j <= _wordsFound[i + 1].Column; j++)
                {
                    var tile = Field[_wordsFound[i].Row, j];
                    tempRes += LetterBoxH.PointsDictionary[tile.CurrentLetter.text] * tile.LetterMultiplier;
                    tile.LetterMultiplier = 1;
                    wordMultiplier *= tile.WordMultiplier;
                    tile.WordMultiplier = 1;
                }
            else
            {
                for (var j = _wordsFound[i].Row; j <= _wordsFound[i + 1].Row; j++)
                {
                    var tile = Field[j, _wordsFound[i].Column];
                    tempRes += LetterBoxH.PointsDictionary[tile.CurrentLetter.text] * tile.LetterMultiplier;
                    tile.LetterMultiplier = 1;
                    wordMultiplier *= tile.WordMultiplier;
                    tile.WordMultiplier = 1;
                }
            }
            result += tempRes;
            score[i / 2] = tempRes;
        }
        var start = 7 + _wordsFound.Count / 2;
        foreach (var i in score)
        {
            Field[start, 0].SetPoints(i * wordMultiplier);
            start--;
        }
        return result * wordMultiplier;
    }

    private string CreateWord(Direction current, TileH start, int end)
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

    private void FindWord(TileH currentTileH, Direction current, out int startPosition, out int endPosition)
    {
        if (current == Direction.Vertical)
        {
            var j = currentTileH.Row;
            while (j >= 0 && Field[j, currentTileH.Column].HasLetter)
            {
                j--;
            }
            j++;
            startPosition = j;
            j = currentTileH.Row;
            while (j < NumberOfRows && Field[j, currentTileH.Column].HasLetter)
            {
                j++;
            }
            j--;
            endPosition = j;
        }
        else
        {
            var j = currentTileH.Column;
            while (j >= 0 && Field[currentTileH.Row, j].HasLetter)
            {
                j--;
            }
            j++;
            startPosition = j;
            j = currentTileH.Column;
            while (j < NumberOfRows && Field[currentTileH.Row, j].HasLetter)
            {
                j++;
            }
            j--;
            endPosition = j;
        }
    }

    private bool CheckWord(string word)
    {
        var sql = "SELECT count(*) FROM AllWords WHERE Word like \"" + word.ToLower() + "\"";
        var command = new SqliteCommand(sql, _dbConnection);
        var inp = command.ExecuteScalar();
        return Convert.ToInt32(inp) != 0;
        /*if (Convert.ToInt32(inp) != 0)
            return true;
        else
        {
            sql = "SELECT count(*) FROM AllWords WHERE Word like \"" + word.ToLower() + "\"";
            command = new SqliteCommand(sql, _dbConnection);
            inp = command.ExecuteScalar();
            return Convert.ToInt32(inp) != 0;
        }*/
    }

    #endregion Word cheking

    private void EndGame(LetterBoxH playerOut)//Player, who ran out of letters is passed
    {
        var tempPoints = Player1.RemovePoints();
        tempPoints += Player2.RemovePoints();
        if (playerOut != null)
        {
            playerOut.Score += tempPoints;
        }
        var winner = Player1.Score > Player2.Score ? 1 : 2;
        Controller.SetWinner(winner, Player1.Score, Player2.Score, Player1Text.text, Player2Text.text);
    }
}