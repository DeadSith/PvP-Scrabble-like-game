using UnityEngine;

public class RulesController : MonoBehaviour
{
    public GameObject ReturnButton;
    public GameObject RulesPanel;

    private void Start()
    {
        var grid = gameObject.GetComponentInChildren<UIGrid>();
        grid.Initialize();
        grid.AddElement(0, 1, ReturnButton, .15f);
        grid.AddElement(8, 0, 1, 2, RulesPanel, .05f);
        gameObject.SetActive(false);
    }
}