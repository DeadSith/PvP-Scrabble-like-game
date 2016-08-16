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
        MenuGrid.Initialize();
        MenuGrid.AddElement(5, 1, StartButton, .2f);
        MenuGrid.AddElement(4, 1, StartLANButton, .2f);
        MenuGrid.AddElement(3, 1, HelpButton, .2f);
        MenuGrid.AddElement(2, 1, SettingsButton, .2f);
        MenuGrid.AddElement(1, 1, ExitButton, .2f);
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