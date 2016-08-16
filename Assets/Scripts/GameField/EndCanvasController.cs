using UnityEngine;

public class EndCanvasController : MonoBehaviour
{
    public GameObject EndText;
    public GameObject ExitButton;
    public GameObject MainMenuButton;

    private void Start()
    {
        var grid = gameObject.GetComponent<UIGrid>();
        grid.Initialize();
        grid.AddElement(4, 0, 1, 1, EndText, .01f);
        grid.AddElement(0, 0, ExitButton, .05f);
        grid.AddElement(0, 1, MainMenuButton, .05f);
        gameObject.SetActive(false);
    }
}