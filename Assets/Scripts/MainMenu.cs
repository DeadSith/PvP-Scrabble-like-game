using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject HelpMenu;
    public GameObject SettingsMenu;
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

    public void OnSettings()
    {
        SettingsMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
