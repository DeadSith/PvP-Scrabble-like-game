using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseBehaviour : MonoBehaviour
{
    public Canvas PauseMenuCanvas;
    public Canvas FieldCanvas;
    public Canvas OverlayCanvas;
    private bool _paused = false;
    public bool GameOver = false;

    private UIGrid _pauseGrid;
    public Button ResumeButton;
    public Button MainMenuButton;
    public Button ExitButton;

    public bool GameStarted;

    private void Start()
    {
        _pauseGrid = PauseMenuCanvas.gameObject.GetComponentInChildren<UIGrid>();
        _pauseGrid.Initialize();
        _pauseGrid.AddElement(4, 1, ResumeButton.gameObject, .1f);
        _pauseGrid.AddElement(3, 1, MainMenuButton.gameObject, .1f);
        _pauseGrid.AddElement(2, 1, ExitButton.gameObject, .1f);
        PauseMenuCanvas.GetComponent<Canvas>().enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _paused = !_paused;
        }
        if (!GameOver && _paused)
        {
            PauseMenuCanvas.enabled = true;
            FieldCanvas.enabled = false;
            OverlayCanvas.enabled = false;
        }
        else if (!GameOver)
        {
            PauseMenuCanvas.enabled = false;
            FieldCanvas.enabled = true;
            OverlayCanvas.enabled = true;
        }
    }

    public void Resume()
    {
        _paused = false;
    }

    public void MainMenu()
    {
        PlayerPrefs.SetInt("Exiting", 1);
        SceneManager.LoadScene(0);
    }

    public void Exit()
    {
        Application.Quit();
    }
}