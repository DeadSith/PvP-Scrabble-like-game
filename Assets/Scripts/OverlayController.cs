using UnityEngine;
using UnityEngine.UI;

public class OverlayController : MonoBehaviour
{
    public GameObject LetterPanel;
    public GameObject ButtonPanel;
    public int WidthCoef;//Width is divided by this coefficient
    public int HeightCoef;

    public Button NextTurnButton;
    public Button SkipTurnButton;
    public Button ChangeLetterButton;
    public Button RemoveAllButton;

    private void Start()
    {
        var parent = gameObject.GetComponent<RectTransform>();
        var width = parent.rect.width;
        var height = parent.rect.height;
        LetterPanel.transform.localPosition = new Vector3((width / WidthCoef - width) / 2, height / 2 / HeightCoef);
        LetterPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(width / WidthCoef, height - height / HeightCoef);
        ButtonPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(ButtonPanel.GetComponent<RectTransform>().sizeDelta.x, height / HeightCoef);
        ButtonPanel.transform.localPosition = new Vector3(ButtonPanel.transform.localPosition.x, (height / HeightCoef - height) / 2);
        var letterGrid = LetterPanel.GetComponent<UIGrid>();
        letterGrid.Initialize();
        var buttonGrid = ButtonPanel.GetComponent<UIGrid>();
        buttonGrid.Initialize();
        //HideMenuButton
        buttonGrid.AddElement(0, 1, RemoveAllButton.gameObject, .05f);
        buttonGrid.AddElement(0, 2, SkipTurnButton.gameObject, .05f);
        buttonGrid.AddElement(0, 3, NextTurnButton.gameObject, .05f);
        buttonGrid.AddElement(0, 4, ChangeLetterButton.gameObject, .05f);
        //CenterButton
    }
}