using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    private bool _timerEnabled;
    private int _length;
    public Text ButtonText;
    public InputField LengthField;
    public InputField Player1Field;
    public InputField Player2Field;



    // Use this for initialization
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
        LengthField.text = _length.ToString();
    }

    private void ChangeText()
    {
        ButtonText.text = _timerEnabled ? "Вимкнути таймер" : "Увімкнути таймер";
    }

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

    public void OnEnabledClick()
    {
        _timerEnabled = !_timerEnabled;
        PlayerPrefs.SetInt("TimerEnabled", _timerEnabled ? 1 : 0);
        ChangeText();
    }

    public void OnPlayer1Changed()
    {
        PlayerPrefs.SetString("Player1",Player1Field.text);
    }

    public void OnPlayer2Changed()
    {
        PlayerPrefs.SetString("Player2",Player2Field.text);
    }
}