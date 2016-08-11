using System;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    //Score of player
    public Text Player1Text;
    public Text Player2Text;
    public Material PlayerGlowMaterial;
    public Material PlayerIdleMaterial;

    //Error messages
    public Text NotExistText;
    public Text DeleteText;
    public Text ChangeLetterText;
    public Text WrongTileText;
    public Text StartText;
    public Text ZeroTilesText;

#if MP
    public Button NextTurnButton;
    public Button SkipTurnButton;
    public Button ChangeLettersButton;
    public Button ReturnAllButton;
    
    public Text WrongTurnText;
    public GameObject DisconnectedMenu;
#endif

    //Endgame fields
    public Canvas EndGameCanvas;
    public Text Player1EndText;
    public Text Player2EndText;
    public Text Winner;

    private static GameObject _currentObject;
    private bool _isLocalTurn;
    private string _player1Name;
    private string _player2Name;


    private void Start()
    {
        _currentObject = StartText.gameObject;
        _player1Name = PlayerPrefs.GetString("Player1", "Гравець 1");
        _player2Name = PlayerPrefs.GetString("Player2", "Гравець 2");
        Player1Text.text = String.Format("{0}\nБали: 0", _player1Name);
        Player2Text.text = String.Format("{0}\nБали: 0", _player2Name);
    }

    //Makes current player glow, updates points
    //Pass the player whose turn ended and his score
    public void InvalidatePlayer(int playerNumber, int score)
    {
        if (playerNumber == 1)
        {
            Player1Text.text = String.Format("{0}\nБали: {1}",_player1Name,score);
            Player1Text.gameObject.transform.parent.GetComponent<Image>().material = PlayerIdleMaterial;
            Player2Text.gameObject.transform.parent.GetComponent<Image>().material = PlayerGlowMaterial;
        }
        else
        {
            Player2Text.text = String.Format("{0}\nБали: {1}", _player2Name, score);
            Player2Text.gameObject.transform.parent.GetComponent<Image>().material = PlayerIdleMaterial;
            Player1Text.gameObject.transform.parent.GetComponent<Image>().material = PlayerGlowMaterial;
        }
        _currentObject.SetActive(false);
        _currentObject = StartText.gameObject;
        _currentObject.SetActive(true);
    }

#region Error showing

    public void ShowNotExistError()
    {
        _currentObject.SetActive(false);
        _currentObject = NotExistText.gameObject;
        _currentObject.SetActive(true);
    }

#if MP
    public void ShowWrongTurnError()
    {
        _currentObject.SetActive(false);
        _currentObject = WrongTurnText.gameObject;
        _currentObject.SetActive(true);
    }
#endif

    public void ShowDeleteError()
    {
        _currentObject.SetActive(false);
        _currentObject = DeleteText.gameObject;
        _currentObject.SetActive(true);
    }

    public void ShowChangeLetterError()
    {
        _currentObject.SetActive(false);
        _currentObject = ChangeLetterText.gameObject;
        _currentObject.SetActive(true);
    }

    public void ShowWrongTileError()
    {
        _currentObject.SetActive(false);
        _currentObject = WrongTileText.gameObject;
        _currentObject.SetActive(true);
    }

    public void ShowZeroTilesError()
    {
        _currentObject.SetActive(false);
        _currentObject = ZeroTilesText.gameObject;
        _currentObject.SetActive(true);
    }

#endregion Error showing


#if MP
#region Multiplayer only

#region Button activation

    public void SetChangeButtonActive(bool active)
    {
        if (_isLocalTurn && ChangeLettersButton.interactable != active)
            ChangeLettersButton.interactable = active;
    }

    public void SetSkipButtonActive(bool active)
    {
        if (_isLocalTurn && SkipTurnButton.interactable != active)
        {
            SkipTurnButton.interactable = active;
        }
    }

    public void SetNextButtonActive(bool active)
    {
        if (_isLocalTurn && NextTurnButton.interactable != active)
        {
            NextTurnButton.interactable = active;
            ReturnAllButton.interactable = active;
        }
    }

#endregion Button activation

    //Multipalyer envelope for InvalidatePlayer
    //_isLocalTurn is used here to set button active state
    public void InvalidatePlayer(int playerNumber, int score, bool isLocal)
    {
        playerNumber = playerNumber == 1 ? 2 : 1;
        InvalidatePlayer(playerNumber, score);
        _isLocalTurn = true;
        SetChangeButtonActive(isLocal);
        SetNextButtonActive(isLocal);
        SetSkipButtonActive(isLocal);
        _isLocalTurn = isLocal;
    }

    //Call on the first turn of server to fix materials
    public void FixFirstTurn()
    {
        Player2Text.gameObject.transform.parent.GetComponent<Image>().material = PlayerIdleMaterial;
        Player1Text.gameObject.transform.parent.GetComponent<Image>().material = PlayerGlowMaterial;
    }

    //Is showed when connection is lost
    public void ShowConnectionError()
    {
        var pause = GameObject.FindGameObjectWithTag("Pause").GetComponent<PauseBehaviour>().GameOver;
        if (pause)
        {
            gameObject.SetActive(false);
            return;
        }
        var manager = GameObject.FindGameObjectWithTag("Manager").GetComponent<NetworkManager>();
        manager.StopHost();
        DisconnectedMenu.SetActive(true);
        gameObject.SetActive(false);
    }

#endregion Multiplayer only
#endif
    //Creates endgame field with player names and scores
    public void SetWinner(int winner, int player1Score, int player2Score)
    {
        GameObject.FindGameObjectWithTag("Pause").GetComponent<PauseBehaviour>().GameOver = true;
        EndGameCanvas.gameObject.SetActive(true);
        if (winner == 1)
            Winner.text = _player1Name;
        else Winner.text = _player2Name;
        gameObject.GetComponent<Canvas>().enabled = false;
    }
}