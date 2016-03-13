using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UIController : MonoBehaviour
{
    
    public Text Player1Text;
    public Text Player2Text;
    public Text NotExistText;
    public Text DeleteText;
    public Text ChangeLetterText;
    public Text WrongTileText;
    public Text StartText;
    public Text ZeroTilesText;
    public Material PlayerGlowMaterial;
    public Material PlayerIdleMaterial;
    private static GameObject _currentObject;

    void Start()
    {
        _currentObject = StartText.gameObject;
    }

    public void InvalidatePlayer(int playerNumber, int score)
    {
        if (playerNumber == 1)
        {
            Player1Text.text = score.ToString();
            Player2Text.gameObject.transform.parent.GetComponent<Image>().material = PlayerIdleMaterial;
            Player1Text.gameObject.transform.parent.GetComponent<Image>().material = PlayerGlowMaterial;
        }
        else
        {
            Player2Text.text = score.ToString();
            Player1Text.gameObject.transform.parent.GetComponent<Image>().material = PlayerIdleMaterial;
            Player2Text.gameObject.transform.parent.GetComponent<Image>().material = PlayerGlowMaterial;
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
}
