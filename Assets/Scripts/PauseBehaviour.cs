using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PauseBehaviour : MonoBehaviour
{
    public GameObject NotImplementedGameObject;
    public GameObject PauseMenu;
    public GameObject Game;
    public bool paused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
            NotImplementedGameObject.SetActive(false);
        }
        if (paused)
        {
            PauseMenu.SetActive(true);
            Game.SetActive(false);
        }
        else
        {
            PauseMenu.SetActive(false);
            Game.SetActive(true);
        }
    }

    public void Resume()
    {
        paused = false;

        NotImplementedGameObject.SetActive(false);
    }

    public void Save()
    {
        NotImplementedGameObject.SetActive(true);
    }

    public void MainMenu()
    {
        NotImplementedGameObject.SetActive(true);
    }
    public void Exit()
    {
        Application.Quit();
    }
}
