using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LetterBoxLAN : NetworkBehaviour
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
            {"'", 0}
        };

    private List<String> _allLetters = new List<string>();
    public List<String> AllLetters = new List<string>();
    private List<String> _lettersToDelete = new List<string>(); 

    #endregion Letters and scores

    public List<Vector3> FreeCoordinates;
    public List<Letter> CurrentLetters;
    public int Score = 0;
    public Letter LetterPrefab;
    public bool CanChangeLetters = true;
    public byte NumberOfLetters = 7;
    public float DistanceBetweenLetters = 1.2f;
    public Vector2 LetterSize;
    public Text NumberOfLettersText;
    private Vector3 _pos;
    private float _xOffset = 0;
    private bool _isFirstTurn = true;
    public GridLAN _currentGrid;
    [SyncVar(hook = "OnConnected")]
    public bool ClientConnected = false;

    [SyncVar(hook = "OnLetterAdd")]
    public string LetterToAdd;

    [SyncVar(hook = "OnLetterDelete")]
    public string LetterToDelete;

    [SyncVar]
    public int GridX;

    [SyncVar]
    public int GridY;

    [SyncVar(hook = "OnGridChanged")]
    public string LetterToPlace;

    [SyncVar(hook = "OnPlayerChange")]
    public int CurrentPlayer;

    public override void OnStartClient()
    {
        var pref = Resources.Load("Letter", typeof(GameObject)) as GameObject;
        LetterPrefab = pref.GetComponent<Letter>();
        NumberOfLettersText = GameObject.FindGameObjectWithTag("NumberOfLetters").GetComponent<Text>();
        CurrentLetters = new List<Letter>();
        transform.SetParent(GameObject.FindGameObjectWithTag("Canvas").transform);
        gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
        gameObject.transform.localPosition = new Vector3(0, 0);
        FreeCoordinates = new List<Vector3>();
        _currentGrid = GameObject.FindGameObjectWithTag("Grid").GetComponent<GridLAN>();
        DistanceBetweenLetters = _currentGrid.DistanceBetweenTiles;
        LetterSize = new Vector2(DistanceBetweenLetters, DistanceBetweenLetters);
        LetterPrefab.gameObject.GetComponent<RectTransform>().sizeDelta = LetterSize;
        _xOffset = gameObject.transform.position.x - 2 * DistanceBetweenLetters;
        var yOffset = gameObject.transform.position.y + DistanceBetweenLetters;
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
        if (_currentGrid.PlayerToSendCommands == null)
            _currentGrid.PlayerToSendCommands = this;
        if (isServer)
            _currentGrid.PlayerNumber = 1;
        else
        {
            _currentGrid.PlayerNumber = 2;
        }
    }

    public override void OnStartAuthority()
    {
        if (isServer)
        {
            return;
        }
        ChangeBox(NumberOfLetters);
        CmdStartClient(true);
        ChangePlayer(1);
    }

    

    public void ChangeBox(int numberOfLetters, string letter = "")//Use to add letters
    {
        _lettersToDelete = new List<string>();
        if (numberOfLetters > AllLetters.Count)
        {
            numberOfLetters = AllLetters.Count;
        }
        if (FreeCoordinates.Count == 0)
        {
            for (var i = 0; i < numberOfLetters; i++)
            {
                AddLetter(_pos, letter);
                _pos.x += DistanceBetweenLetters;
                if (i % 4 == 3)
                {
                    _pos.x = _xOffset;
                    _pos.y -= DistanceBetweenLetters;
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
    
    private void AddLetter(Vector3 position, string letter)//Creates new letter
    {
        var newLetter = Instantiate(LetterPrefab, position,
            transform.rotation) as Letter;
        newLetter.transform.SetParent(gameObject.transform);
        if (String.IsNullOrEmpty(letter))
        {
            var current = AllLetters[UnityEngine.Random.Range(0, AllLetters.Count - 1)];
            //CmdDeleteLetter(current);
            _lettersToDelete.Add(current);
            if(_isFirstTurn)
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

    private void Delay(int delay)
    {
        int t = Environment.TickCount;
        while ((Environment.TickCount - t) < delay) ;
    }

    public void RemoveLetter()//Is called when letter is dropped
    {
        var currentObject = DragHandler.ObjectDragged.GetComponent<Letter>();
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
   
    public bool ChangeLetters()//Used to change letters on field
    {
        var successful = false;
        var lettersToAdd = new StringBuilder();
        foreach (Letter t in CurrentLetters.Where(t => t.isChecked))
        {
            lettersToAdd.Append(t.LetterText.text);
        }
        CmdAddLetter(lettersToAdd.ToString());
        var temp = new List<string>();
        temp.AddRange(AllLetters);
        var lettersToDelete = new StringBuilder();
        foreach (Letter t in CurrentLetters.Where(t => t.isChecked))
        {
            var current = temp[UnityEngine.Random.Range(0, temp.Count - 1)];
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

    public int FindIndex(Letter input)
    {
        var j = 0;
        for (; j < CurrentLetters.Count; j++)
        {
            if (CurrentLetters[j] == input)
                return j;
        }
        return -1;
    }

    public int RemovePoints()//Called in the end of game to remove points for ech letter left in box
    {
        var result = 0;
        foreach (var letter in CurrentLetters)
        {
            result += PointsDictionary[letter.LetterText.text];
        }
        Score -= result;
        return result;
    }

    public void ChangeGrid(int row, int column, string letter)
    {
        CmdChangeGrid(row,column,letter);
    }

    public void ChangePlayer(int playerNumber)
    {
        CmdChangeCurrentPlayer(playerNumber);
    }

    #region Commands

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
    private void CmdEndTurn()
    {
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
    #endregion Commands

    public void OnConnected(bool value)
    {
        ClientConnected = value;
        if (isServer)
        {
            Debug.Log("connected");
            ChangeBox(NumberOfLetters);
        }
    }

    public void OnLetterDelete(string value)
    {
        Debug.LogError(value);
        LetterToDelete = value;
        foreach (var letter in value)
        {
            AllLetters.Remove(letter.ToString());
        }
        NumberOfLettersText.text = AllLetters.Count.ToString();
    }

    public void OnLetterAdd(string value)
    {
        LetterToAdd = value;
        foreach (var letter in value)
        {
            AllLetters.Add(letter.ToString());
        }
    }

    public void OnGridChanged(string value)
    {
        Debug.LogError(String.Format("{0} {1} {2}", GridX, GridY, value));
        LetterToPlace = value;
        _currentGrid.Field[GridX,GridY].ChangeLetter(value);
    }

    public void OnPlayerChange(int value)
    {
        CurrentPlayer = value;
        _currentGrid.InvalidatePlayer(CurrentPlayer);
    }
}