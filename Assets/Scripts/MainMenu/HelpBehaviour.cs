using UnityEngine;
using UnityEngine.UI;

//Used for navigation in help menu
public class HelpBehaviour : MonoBehaviour
{
    public GameObject RulesMenu;
    public GameObject BonusMenu;

    public Button RulesButton;
    public Button BonusButton;
    public Button ReturnButton;

    public void Start()
    {
        var grid = gameObject.GetComponent<UIGrid>();
        grid.Initialize();
        grid.AddElement(3, 1, RulesButton.gameObject, .1f);
        grid.AddElement(2, 1, BonusButton.gameObject, .1f);
        grid.AddElement(1, 1, ReturnButton.gameObject, .1f);
        gameObject.SetActive(false);
    }

    public void OnRules()
    {
        RulesMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    public void OnBonus()
    {
        BonusMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}