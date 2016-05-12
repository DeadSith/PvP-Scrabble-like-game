using UnityEngine;

//Used for navigation in help menu
public class HelpBehaviour : MonoBehaviour
{
    public GameObject RulesMenu;
    public GameObject BonusMenu;

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