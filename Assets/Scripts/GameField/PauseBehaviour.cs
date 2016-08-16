using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseBehaviour : MonoBehaviour
{
    public GameObject NotImplementedGameObject;
    public GameObject PauseMenu;
    public GameObject Game;
    private bool _paused = false;
    public bool GameOver = false;

    //Used only for multiplayer
    public GameObject StartMenu;

    public bool GameStarted;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            _paused = !_paused;
            NotImplementedGameObject.SetActive(false);
        }
        if (!GameOver && _paused)
        {
            PauseMenu.GetComponent<Canvas>().enabled = true;
            if (StartMenu != null && !GameStarted)
                StartMenu.GetComponent<Canvas>().enabled = false;
            Game.GetComponent<Canvas>().enabled = false;
        }
        else if (!GameOver)
        {
            PauseMenu.GetComponent<Canvas>().enabled = false;
            if (StartMenu != null && !GameStarted)
            {
                StartMenu.GetComponent<Canvas>().enabled = true;
            }
            Game.GetComponent<Canvas>().enabled = true;
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