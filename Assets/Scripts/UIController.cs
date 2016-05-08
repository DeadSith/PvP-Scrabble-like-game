﻿using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Text Player1Text;
    public Text Player2Text;
    public Text NotExistText;
    public Text DeleteText;
    public Text ChangeLetterText;
    public Text WrongTileText;
    public Text StartText;
    public Text WrongTurnText;
    public Text ZeroTilesText;
    public Material PlayerGlowMaterial;
    public Material PlayerIdleMaterial;
    public Button NextTurnButton;
    public Button SkipTurnButton;
    public Button ChangeLettersButton;
    public Button ReturnAllButton;

    public Canvas EndGameCanvas;
    public Text Player1Points;
    public Text Player2Points;
    public Text Player1Name;
    public Text Player2Name;
    public Text Winner;

    public GameObject DisconnectedMenu;

    private static GameObject _currentObject;
    private bool _isLocalTurn;

    private void Start()
    {
        _currentObject = StartText.gameObject;
    }

    public void InvalidatePlayer(int playerNumber, int score)//Pass the player whose turn ended and his score
    {
        if (playerNumber == 1)
        {
            Player1Text.text = score.ToString();
            Player1Text.gameObject.transform.parent.GetComponent<Image>().material = PlayerIdleMaterial;
            Player2Text.gameObject.transform.parent.GetComponent<Image>().material = PlayerGlowMaterial;
        }
        else
        {
            Player2Text.text = score.ToString();
            Player2Text.gameObject.transform.parent.GetComponent<Image>().material = PlayerIdleMaterial;
            Player1Text.gameObject.transform.parent.GetComponent<Image>().material = PlayerGlowMaterial;
        }
        _currentObject.SetActive(false);
        _currentObject = StartText.gameObject;
        _currentObject.SetActive(true);
    }

    public void ShowNotExistError()
    {
        _currentObject.SetActive(false);
        _currentObject = NotExistText.gameObject;
        _currentObject.SetActive(true);
    }

    public void ShowWrongTurnError()
    {
        _currentObject.SetActive(false);
        _currentObject = WrongTurnText.gameObject;
        _currentObject.SetActive(true);
    }

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

    public void FixFirstTurn()//Call on the first turn of server to fix materials
    {
        Player2Text.gameObject.transform.parent.GetComponent<Image>().material = PlayerIdleMaterial;
        Player1Text.gameObject.transform.parent.GetComponent<Image>().material = PlayerGlowMaterial;
    }

    public void SetWinner(int winner, int player1Score, int player2Score,string player1Name, string player2Name)
    {
        GameObject.FindGameObjectWithTag("Pause").GetComponent<PauseBehaviour>().GameOver = true;
        EndGameCanvas.gameObject.SetActive(true);
        if (winner == 1)
            Winner.text = player1Name;
        else Winner.text = player2Name;
        Player1Name.text = "Бали " + player1Name;
        Player2Name.text = "Бали " + player2Name;
        Player1Points.text = player1Score.ToString();
        Player2Points.text = player2Score.ToString();
        gameObject.GetComponent<Canvas>().enabled = false;
    }

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
}