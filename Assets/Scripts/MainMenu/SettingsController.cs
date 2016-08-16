using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    private bool _timerEnabled;
    private int _length;
    private Text _buttonText;//Button to enable/disable timer

    public Button TimerButton;
    public InputField LengthField;// Length of timer
    public Text Player1Text;
    public InputField Player1Field;
    public Text Player2Text;
    public InputField Player2Field;

    public Button ReturnButton;

    //Checks if necessary values are present and sets default values for them
    private void Start()
    {
        _buttonText = TimerButton.GetComponentInChildren<Text>();
        InitializeGrid();
        if (PlayerPrefs.HasKey("TimerEnabled"))
        {
            _timerEnabled = PlayerPrefs.GetInt("TimerEnabled") == 1;
            ChangeText();
        }
        else
        {
            PlayerPrefs.SetInt("TimerEnabled", 0);
            _timerEnabled = false;
            ChangeText();
        }
        if (PlayerPrefs.HasKey("Length"))
        {
            _length = PlayerPrefs.GetInt("Length");
            if (_length == 0)
                _length = 60;
            //PlayerPrefs.SetInt("Length", 60);
            LengthField.text = _length.ToString();
        }
        else
        {
            PlayerPrefs.SetInt("Length", 60);
            _length = 60;
            LengthField.text = _length.ToString();
        }
        Player1Field.text = PlayerPrefs.GetString("Player1", "Player1");
        Player2Field.text = PlayerPrefs.GetString("Player2", "Player2");
        LengthField.text = _length.ToString();
        gameObject.SetActive(false);
    }

    private void InitializeGrid()
    {
        var grid = gameObject.GetComponentInChildren<UIGrid>();
        grid.Initialize();
        grid.AddElement(4, 1, TimerButton.gameObject, .1f);
        grid.AddElement(4, 2, LengthField.gameObject, .1f);
        grid.AddElement(3, 1, Player1Text.gameObject, .1f);
        grid.AddElement(3, 2, Player1Field.gameObject, .1f);
        grid.AddElement(2, 1, Player2Text.gameObject, .1f);
        grid.AddElement(2, 2, Player2Field.gameObject, .1f);
        grid.AddElement(0, 1, 0, 2, ReturnButton.gameObject, .1f);
    }

    //Changes text of enable timer button
    private void ChangeText()
    {
        _buttonText.text = _timerEnabled ? "Вимкнути таймер" : "Увімкнути таймер";
    }

    //Checks if new timer length is correct and writes it to prefs
    public void OnLengthChanged()
    {
        var tempLength = int.Parse(LengthField.text);
        if (tempLength != _length && tempLength > 39 && tempLength < 361)
        {
            _length = tempLength;
            PlayerPrefs.SetInt("Length", _length);
        }
        else
        {
            LengthField.text = _length.ToString();
        }
    }

    //Enables/disabes timer
    public void OnEnabledClick()
    {
        _timerEnabled = !_timerEnabled;
        PlayerPrefs.SetInt("TimerEnabled", _timerEnabled ? 1 : 0);
        ChangeText();
    }

    //Writes Player 1 name to prefs
    public void OnPlayer1Changed()
    {
        PlayerPrefs.SetString("Player1", Player1Field.text);
    }

    //Writes Player 2 name to prefs
    public void OnPlayer2Changed()
    {
        PlayerPrefs.SetString("Player2", Player2Field.text);
    }
}