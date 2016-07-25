using UnityEngine;
using UnityEngine.SceneManagement;

//Navigation in main menu
public class MainMenu : MonoBehaviour
{
    public GameObject HelpMenu;
    public GameObject SettingsMenu;

    public UIGrid MenuGrid;
    public GameObject StartButton;
    public GameObject StartLANButton;
    public GameObject HelpButton;
    public GameObject SettingsButton;
    public GameObject ExitButton;

    public void Start()
    {
        MenuGrid.AddElement(6, 2, StartButton, .2f);
        MenuGrid.AddElement(5, 2, StartLANButton, .2f);
        MenuGrid.AddElement(4, 2, HelpButton, .2f);
        MenuGrid.AddElement(3, 2, SettingsButton, .2f);
        MenuGrid.AddElement(2, 2, ExitButton, .2f);
    }

    public void OnStartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OnStartLan()
    {
        SceneManager.LoadScene(2);
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