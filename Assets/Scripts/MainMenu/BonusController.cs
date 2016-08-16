using UnityEngine;

public class BonusController : MonoBehaviour
{
    public GameObject StartTile;
    public GameObject StartText;
    public GameObject DoubleLetterTile;
    public GameObject DoubleLetterText;
    public GameObject TripleLetterTile;
    public GameObject TripleLetterText;
    public GameObject DoubleWordTile;
    public GameObject DoubleWordText;
    public GameObject TripleWordTile;
    public GameObject TripleWordText;

    public GameObject ReturnButton;

    private void Start()
    {
        var grid = gameObject.GetComponentInChildren<UIGrid>();
        grid.Initialize();
        grid.AddElement(6, 1, StartTile, .05f, true);
        grid.AddElement(6, 2, 6, 3, StartText, .05f);
        grid.AddElement(5, 1, DoubleLetterTile, .05f, true);
        grid.AddElement(5, 2, 5, 3, DoubleLetterText, .05f);
        grid.AddElement(4, 1, TripleLetterTile, .05f, true);
        grid.AddElement(4, 2, 4, 3, TripleLetterText, .05f);
        grid.AddElement(3, 1, DoubleWordTile, .05f, true);
        grid.AddElement(3, 2, 3, 3, DoubleWordText, .05f);
        grid.AddElement(2, 1, TripleWordTile, .05f, true);
        grid.AddElement(2, 2, 2, 3, TripleWordText, .05f);
        grid.AddElement(0, 2, ReturnButton, .1f);
        gameObject.SetActive(false);
    }
}