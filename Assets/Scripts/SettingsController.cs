using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    private bool _timerEnabled;
    private int _length;
    public Text ButtonText;//Button to enable/disable timer
    public InputField LengthField;// Length of timer
    public InputField Player1Field;
    public InputField Player2Field;

    //Checks if necessary values are present and sets default values for them
    private void Start()
    {
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
            PlayerPrefs.SetInt("Length", 60);
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
    }

    //Changes text of enable timer button
    private void ChangeText()
    {
        ButtonText.text = _timerEnabled ? "Вимкнути таймер" : "Увімкнути таймер";
    }

    //Checks if new timer length is correct and writes it to prefs
    public void OnLengthChanged()
    {
        var tempLength = int.Parse(LengthField.text);
        if (tempLength != _length && tempLength > 19 && tempLength < 301)
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