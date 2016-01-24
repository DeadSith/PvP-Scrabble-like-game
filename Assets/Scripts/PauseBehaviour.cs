using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PauseBehaviour : MonoBehaviour
{
    public GameObject NotImplementedGameObject;
    public GameObject PauseMenu;
    public GameObject Game;
    public bool paused = false;
    public bool GameOver = false;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
            NotImplementedGameObject.SetActive(false);
        }
        if (!GameOver&&paused)
        {
            PauseMenu.SetActive(true);
            Game.GetComponent<Canvas>().enabled = false;
            //Game.SetActive(false);
        }
        else if(!GameOver)
        {
            PauseMenu.SetActive(false);
            Game.GetComponent<Canvas>().enabled = true;
            //Game.SetActive(true);
        }
    }

    public void Resume()
    {
        paused = false;
    }

    
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
