using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject HelpMenu;
    public void OnStartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OnExit()
    {
        Application.Quit();
    }

    public void OnHelp()
    {
        HelpMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
