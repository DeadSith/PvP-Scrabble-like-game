using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

//Todo: sync number of skipped turns
public class LetterBoxLAN : NetworkBehaviour
{
    #region Letters

    private List<String> _allLetters = new List<string>();
    public List<String> AllLetters = new List<string>();
    private List<String> _lettersToDelete = new List<string>();

    #endregion Letters

    [HideInInspector]
    public List<Vector3> FreeCoordinates;

    public List<LetterLAN> CurrentLetters;
    public LetterLAN LetterPrefab;
    public bool CanChangeLetters = true;
    public Text NumberOfLettersText;

    private const byte NumberOfLetters = 7;
    private float _distanceBetweenLetters = 1.2f;
    private Vector2 _letterSize;
    private Vector3 _pos;
    private float _xOffset;
    private bool _isFirstTurn = true;
    private FieldLAN _currentField;
    private GameObject _waitTextGameObject;
    public List<TileLAN> _currenTiles = new List<TileLAN>();

    #region SyncVars

    [HideInInspector]
    [SyncVar(hook = "OnConnected")]
    public bool ClientConnected = false;

    [HideInInspector]
    [SyncVar(hook = "OnLetterAdd")]
    public string LetterToAdd;

    [HideInInspector]
    [SyncVar(hook = "OnLetterDelete")]
    public string LetterToDelete;

    [HideInInspector]
    [SyncVar]
    public int GridX;

    [HideInInspector]
    [SyncVar]
    public int GridY;

    [HideInInspector]
    [SyncVar(hook = "OnGridChanged")]
    public string LetterToPlace;

    [HideInInspector]
    [SyncVar(hook = "OnPlayerChange")]
    public int CurrentPlayer;

    [HideInInspector]
    [SyncVar(hook = "OnPlayer1ScoreChange")]
    public int Player1Score;

    [HideInInspector]
    [SyncVar(hook = "OnPlayer2ScoreChange")]
    public int Player2Score;

    [HideInInspector]
    [SyncVar(hook = "OnEndGame")]
    public int Winner;

    [HideInInspector]
    [SyncVar]
    public int BonusScore = 0;

    [HideInInspector]
    [SyncVar(hook = "OnShowEnd")]
    public bool End = false;

    [HideInInspector]
    [SyncVar]
    public bool TimerEnabled;

    [HideInInspector]
    [SyncVar(hook = "OnLengthChanged")]
    public int TimerLength;

    [HideInInspector]
    [SyncVar(hook = "OnSync")]
    public float TimeRemaining;

    [HideInInspector]
    [SyncVar]
    public bool IsFirstTurn;

    [HideInInspector]
    [SyncVar(hook = "OnSkip")]
    public string TilesToDelete;

    [HideInInspector]
    [SyncVar(hook = "OnPlayer1NameChanged")]
    public string Player1Name;

    [HideInInspector]
    [SyncVar(hook = "OnPlayer2NameChanged")]
    public string Player2Name;

    #endregion SyncVars

    //Finds all the necessary objects on field
    public override void OnStartClient()
    {
        var prefab = Resources.Load("LetterLAN", typeof(GameObject)) as GameObject;
        LetterPrefab = prefab.GetComponent<LetterLAN>();
        NumberOfLettersText = GameObject.FindGameObjectWithTag("NumberOfLetters").GetComponent<Text>();
        CurrentLetters = new List<LetterLAN>();
        transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
        gameObject.transform.localPosition = new Vector3(0, 0);
        FreeCoordinates = new List<Vector3>();
        _currentField = GameObject.FindGameObjectWithTag("Field").GetComponent<FieldLAN>();
        _distanceBetweenLetters = _currentField.DistanceBetweenTiles;
        _letterSize = new Vector2(_distanceBetweenLetters, _distanceBetweenLetters);
        LetterPrefab.gameObject.GetComponent<RectTransform>().sizeDelta = _letterSize;
        _xOffset = gameObject.transform.position.x - 2 * _distanceBetweenLetters;
        var yOffset = gameObject.transform.position.y + _distanceBetweenLetters;
        _pos = new Vector3(_xOffset, yOffset);
        _allLetters = new List<string>
    {
        "а","а","с","и","р","о","р","д","ш","и","и","и","ц","е","н","а","т","щ","л","к","ж",
        "п","в","к","о","в","а","у","н","к","е","м","г","і","н","х","і","н","и","н","а","и",
        "р","л","р","с","п","м","а","у","а","ю","м","с","б","в","я","м","т","ф","с","и","о",
        "я","п","о","о","л","е","а","б","і","е","ь","т","р","ґ","з","д","о","і","і","є","й",
        "е","д","н","о","у","г","ї","ч","о","о","о","к","т","н","в","т","з","'","*","*"
    };
        _allLetters = _allLetters.OrderBy(letter => letter).ToList();
        AllLetters.AddRange(_allLetters);
        if (_currentField.PlayerToSendCommands == null)
            _currentField.PlayerToSendCommands = this;
        if (isServer)
        {
            _waitTextGameObject = GameObject.FindWithTag("Wait");
            _currentField.PlayerNumber = 1;
            _waitTextGameObject.GetComponent<Text>().enabled = true;
        }
        else
        {
            _currentField.PlayerNumber = 2;
        }
    }

    //On client: creates field and calls the function to create field on server
    public override void OnStartAuthority()
    {
        if (isServer)
        {
            return;
        }
        ChangeBox(NumberOfLetters);
        CmdStartClient(true);
        CmdSetPlayer2(PlayerPrefs.GetString("Player1", "Гравець 2"));
        ChangePlayer(1, 0);
        GameObject.FindGameObjectWithTag("Pause").GetComponent<PauseBehaviour>().GameStarted = true;
        GameObject.FindGameObjectWithTag("Pause").GetComponent<PauseBehaviour>().Resume();
    }

    //Checks if Letters can be changed
    public void Update()
    {
        if (AllLetters.Count == 0)
            CanChangeLetters = false;
        else if (_currentField.PlayerNumber == CurrentPlayer && _currentField.CurrentTiles.Count == 0)
            CanChangeLetters = true;
        else
        {
            CanChangeLetters = false;
        }
    }

    //Adds letters to the hand of player
    public void ChangeBox(int numberOfLetters, string letter = "")
    {
        _lettersToDelete = new List<string>();
        if (String.IsNullOrEmpty(letter) && numberOfLetters > AllLetters.Count)
        {
            numberOfLetters = AllLetters.Count;
        }
        if (FreeCoordinates.Count == 0)
        {
            for (var i = 0; i < numberOfLetters; i++)
            {
                AddLetter(_pos, letter);
                _pos.x += _distanceBetweenLetters;
                if (i % 4 == 3)
                {
                    _pos.x = _xOffset;
                    _pos.y -= _distanceBetweenLetters;
                }
            }
        }
        else
        {
            for (var j = 0; j < numberOfLetters; j++)
            {
                AddLetter(FreeCoordinates[FreeCoordinates.Count - 1], letter);
                FreeCoordinates.RemoveAt(FreeCoordinates.Count - 1);
            }
        }
        if (_lettersToDelete.Count > 0)
        {
            var syncBuilder = new StringBuilder(_lettersToDelete.Count);
            foreach (var str in _lettersToDelete)
            {
                syncBuilder.Append(str);
            }
            CmdDeleteLetter(syncBuilder.ToString());
        }
        NumberOfLettersText.text = AllLetters.Count.ToString();
        _isFirstTurn = false;
    }

    //Crates new LetterH on field
    private void AddLetter(Vector3 position, string letter)//Creates new letter
    {
        var newLetter = Instantiate(LetterPrefab, position,
            transform.rotation) as LetterLAN;
        newLetter.transform.SetParent(gameObject.transform);
        if (String.IsNullOrEmpty(letter))
        {
            var current = AllLetters[UnityEngine.Random.Range(0, AllLetters.Count)];
            //CmdDeleteLetter(current);
            _lettersToDelete.Add(current);
            if (_isFirstTurn)
                AllLetters.Remove(current);
            newLetter.ChangeLetter(current);
            CurrentLetters.Add(newLetter);
        }
        else
        {
            newLetter.ChangeLetter(letter);
            CurrentLetters.Add(newLetter);
        }
    }

    //Removes letter from hand when it is dropped on grid
    public void RemoveLetter()
    {
        var currentObject = DragHandler.ObjectDragged.GetComponent<LetterLAN>();
        var currentIndex = FindIndex(currentObject);
        Vector3 previousCoordinates = DragHandler.StartPosition;
        for (int j = currentIndex + 1; j < CurrentLetters.Count; j++)
        {
            var tempCoordinates = CurrentLetters[j].gameObject.transform.position;
            CurrentLetters[j].gameObject.transform.position = previousCoordinates;
            previousCoordinates = tempCoordinates;
        }
        FreeCoordinates.Add(previousCoordinates);
        CurrentLetters.Remove(currentObject);
    }

    //Changes letters for Letters player checked
    public bool ChangeLetters()
    {
        var successful = false;
        var lettersToAdd = new StringBuilder();
        foreach (LetterLAN t in CurrentLetters.Where(t => t.isChecked))
        {
            lettersToAdd.Append(t.LetterText.text);
        }
        CmdAddLetter(lettersToAdd.ToString());
        var temp = new List<string>();
        temp.AddRange(AllLetters);
        var lettersToDelete = new StringBuilder();
        foreach (LetterLAN t in CurrentLetters.Where(t => t.isChecked))
        {
            var current = temp[UnityEngine.Random.Range(0, temp.Count)];
            lettersToDelete.Append(current);
            temp.Remove(current);
            t.ChangeLetter(current);
            t.isChecked = false;
            t.gameObject.GetComponent<Image>().material = t.StandardMaterial;
            successful = true;
        }
        CmdDeleteLetter(lettersToDelete.ToString());
        return successful;
    }

    //Finds the index of LetterH in CurrentLetters
    public int FindIndex(LetterLAN input)
    {
        var j = 0;
        for (; j < CurrentLetters.Count; j++)
        {
            if (CurrentLetters[j] == input)
                return j;
        }
        return -1;
    }

    #region Envelopes for commands

    public void ChangeGrid(int row, int column, string letter)
    {
        CmdChangeGrid(row, column, letter);
    }

    //Score received in a turn is passed
    //Resets timer, calls commands to change player and score
    public void ChangePlayer(int nextPlayer, int score)
    {
        if (CurrentPlayer == _currentField.PlayerNumber)
            CmdSetTimeRemaining(TimerLength);
        else
        {
            _currentField.TimeRemaining = TimeRemaining;
        }
        if (_currentField.IsFirstTurn && score != 0)
            CmdSetFirstTurn();
        score += CurrentPlayer == 1 ? Player1Score : Player2Score;
        CmdChangeScore(CurrentPlayer, score);
        CmdChangeCurrentPlayer(nextPlayer);
    }

    public void EndGame()
    {
        CmdEndGame(1111);
    }

    public void DeleteOnSkip(string tilesToDelete)
    {
        CmdSkip(tilesToDelete);
    }

    #endregion Envelopes for commands

    #region Commands

    [Command]
    private void CmdSkip(string value)
    {
        TilesToDelete = value;
    }

    [Command]
    private void CmdStartClient(bool connected)
    {
        ClientConnected = connected;
    }

    [Command]
    private void CmdDeleteLetter(string letter)
    {
        LetterToDelete = letter;
    }

    [Command]
    private void CmdAddLetter(string letter)
    {
        LetterToAdd = letter;
    }

    [Command]
    private void CmdChangeCurrentPlayer(int playerNumber)
    {
        CurrentPlayer = playerNumber;
    }

    [Command]
    private void CmdChangeGrid(int row, int column, string letter)
    {
        GridX = row;
        GridY = column;
        LetterToPlace = letter;
    }

    [Command]
    private void CmdChangeScore(int playerNumber, int score)
    {
        if (playerNumber == 1)
            Player1Score = score;
        else Player2Score = score;
    }

    [Command]
    private void CmdEndGame(int winner)
    {
        Winner = winner;
    }

    [Command]
    private void CmdAddBonusScore(int score)
    {
        BonusScore = BonusScore + score;
    }

    [Command]
    private void CmdShowEnd(bool value)
    {
        End = value;
    }

    [Command]
    private void CmdSetTimer(bool enabled, int lenght)
    {
        TimerEnabled = enabled;
        TimerLength = lenght;
    }

    [Command]
    private void CmdSetFirstTurn()
    {
        IsFirstTurn = true;
    }

    [Command]
    private void CmdSetTimeRemaining(float remaining)
    {
        TimeRemaining = remaining;
    }

    [Command]
    private void CmdSetPlayer1(string name)
    {
        Player1Name = name;
    }

    [Command]
    private void CmdSetPlayer2(string name)
    {
        Player2Name = name;
    }

    #endregion Commands

    #region Hooks

    //Creates hand and field on server
    public void OnConnected(bool value)
    {
        ClientConnected = value;
        if (isServer)
        {
            CmdSetPlayer1(PlayerPrefs.GetString("Player1", "Гравець 1"));
            _currentField.SetName(1, PlayerPrefs.GetString("Player1", "Гравець 1"));
            GameObject.FindGameObjectWithTag("Pause").GetComponent<PauseBehaviour>().GameStarted = true;
            GameObject.FindGameObjectWithTag("Pause").GetComponent<PauseBehaviour>().Resume();
            _waitTextGameObject.SetActive(false);
            var enabled = PlayerPrefs.GetInt("TimerEnabled") == 1;
            var length = PlayerPrefs.GetInt("Length");
            CmdSetTimer(enabled, length);
            _currentField.SetTimer(enabled, length);
            ChangeBox(NumberOfLetters);
        }
    }

    //Removes letter from AllLetters on server and client
    public void OnLetterDelete(string value)
    {
        LetterToDelete = "qwe";
        foreach (var letter in value)
        {
            AllLetters.Remove(letter.ToString());
        }
        NumberOfLettersText.text = AllLetters.Count.ToString();
    }

    //Adds letter to AllLetter on server and client
    public void OnLetterAdd(string value)
    {
        LetterToAdd = "qwe";
        foreach (var letter in value)
        {
            AllLetters.Add(letter.ToString());
        }
    }

    //Writes letter to cell in Field
    public void OnGridChanged(string value)
    {
        LetterToPlace = "xyz";
        if (!String.IsNullOrEmpty(value))
        {
            _currentField.Field[GridX, GridY].ChangeLetter(value);
            _currenTiles.Add(_currentField.Field[GridX, GridY]);
        }
        else
        {
            _currentField.Field[GridX, GridY].Remove();
            _currenTiles.Remove(_currentField.Field[GridX, GridY]);
        }
    }

    //Sync tile multipliers on client and server
    public void OnPlayerChange(int value)
    {
        foreach (var tile in _currenTiles)
        {
            tile.WordMultiplier = 1;
            if (tile.LetterMultiplier != 0)
                tile.LetterMultiplier = 1;
        }
        _currenTiles.Clear();
        CurrentPlayer = value;
        _currentField.InvalidatePlayer(CurrentPlayer, CurrentPlayer == 1 ? Player2Score : Player1Score);
    }

    //Called in the end of game to remove points for each letter left in box
    public void OnEndGame(int value)
    {
        var result = 0;
        foreach (var letter in CurrentLetters)
        {
            result += LetterBoxH.PointsDictionary[letter.LetterText.text];
        }
        CmdAddBonusScore(result);
        if (_currentField.PlayerNumber == 1)
            CmdChangeScore(1, Player1Score - result);
        else
        {
            CmdChangeScore(2, Player2Score - result);
        }
        if (AllLetters.Count == 0)
        {
            if (CurrentLetters.Count == 0)
                if (_currentField.PlayerNumber == 2)
                    CmdChangeScore(2, Player2Score + BonusScore);
                else
                {
                    CmdChangeScore(1, Player1Score + BonusScore);
                }
        }
        if (isServer)
        {
            return;
        }
        CmdShowEnd(true);
    }

    #region Workaroung to incorrect syncing

    public void OnPlayer1ScoreChange(int value)
    {
        Player1Score = value;
        if (isServer)
            return;
        _currentField.InvalidatePlayer(CurrentPlayer, value);
    }

    public void OnPlayer2ScoreChange(int value)
    {
        Player2Score = value;
        if (isServer)
            return;
        _currentField.InvalidatePlayer(CurrentPlayer, value);
    }

    #endregion Workaroung to incorrect syncing

    //Called on client and server to show endgame canvas
    public void OnShowEnd(bool value)
    {
        End = value;
        var winner = Player1Score > Player2Score ? 1 : 2;
        _currentField.EndGame(winner, Player1Score, Player2Score);
    }

    //Changes length of timer
    public void OnLengthChanged(int value)
    {
        TimerLength = value;
        _currentField.SetTimer(TimerEnabled, value);
    }

    //Starts timer on client and server
    public void OnSync(float value)
    {
        TimeRemaining = 1;
        _currentField.TimeRemaining = value;
        if (!isServer)
            _currentField.TimeRemaining += 1;
    }

    //Called on server and client when turn is skipped
    //Removes all the letters placed on field in the last turn
    public void OnSkip(string value)
    {
        if (String.IsNullOrEmpty(value))
            return;
        var letters = value.Split(' ');
        var player = int.Parse(letters[letters.Length - 1]);
        Debug.LogError(value);
        for (int i = 0; i < letters.Length - 1; i++)
        {
            var row = int.Parse(letters[i]);
            var column = int.Parse(letters[++i]);
            _currenTiles.Remove(_currentField.Field[row, column]);
            if (_currentField.PlayerNumber == player)
                _currentField.Field[row, column].RemoveOnClick(true);
            else
            {
                _currentField.Field[row, column].Remove();
            }
        }
    }

    //Called on server and client in the beggining of the game to sync names of Players
    public void OnPlayer1NameChanged(string value)
    {
        Player1Name = value;
        _currentField.SetName(1, value);
    }

    public void OnPlayer2NameChanged(string value)
    {
        Player2Name = value;
        _currentField.SetName(2, value);
    }

    #endregion Hooks
}