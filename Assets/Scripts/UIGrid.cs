using UnityEngine;

public class UIGrid : MonoBehaviour
{
    public GameObject[,] Items;
    public bool IsSquare = false;
    public int RowCount = 1;
    public int ColCount = 1;
    public float Padding = 0;
    public GameObject CellPrefab;

    private void Start()
    {
        Items = new GameObject[RowCount, ColCount];
        var gridSize = GetComponent<RectTransform>().rect;
        var xSize = gridSize.width / ColCount;
        var ySize = gridSize.height / RowCount;
        var cellTransform = CellPrefab.gameObject.GetComponent<RectTransform>();
        if (IsSquare)
        {
            if (xSize > ySize)
                xSize = ySize;
            else ySize = xSize;
        }
        cellTransform.sizeDelta = new Vector2(xSize, ySize);
        var xStart = transform.position.x + (cellTransform.rect.width - ColCount * xSize) / 2;
        var curPosition = new Vector3(xStart, transform.position.y + (cellTransform.rect.height - RowCount * ySize) / 2);
        for (var i = 0; i < RowCount; i++)
        {
            for (var j = 0; j < ColCount; j++)
            {
                var curCell = Instantiate(CellPrefab, curPosition, transform.rotation) as GameObject;
                curCell.transform.SetParent(gameObject.transform);
                Items[i, j] = curCell;
                curPosition.x += xSize;
            }
            curPosition.y += ySize;
            curPosition.x = xStart;
        }
    }

    public void AddElement(int row, int column, GameObject element, bool isSquare = false, float padding = 0)//element should have anchors in middle and centre
    {
        element.transform.position = new Vector3(Items[row, column].transform.position.x, Items[row, column].transform.position.y);
        if (!isSquare)
            element.gameObject.GetComponent<RectTransform>().sizeDelta =
                new Vector2((1 - padding) * Items[row, column].GetComponent<RectTransform>().rect.width,
                    (1 - padding) * Items[row, column].GetComponent<RectTransform>().rect.height);
        else
        {
            var min = (Items[row, column].GetComponent<RectTransform>().rect.width <
                      Items[row, column].GetComponent<RectTransform>().rect.height
                ? Items[row, column].GetComponent<RectTransform>().rect.width
                : Items[row, column].GetComponent<RectTransform>().rect.height) * (1 - padding);
            element.gameObject.GetComponent<RectTransform>().sizeDelta =
                new Vector2(min, min);
        }
    }

    public void AddElement(int upperRow, int upperColumn, int lowerRow, int lowerColumn, GameObject element, bool isSquare = false, float padding = 0)
    {
        element.transform.position = new Vector3((Items[upperRow, upperColumn].transform.position.x + Items[lowerRow, lowerColumn].transform.position.x) / 2,
            (Items[upperRow, upperColumn].transform.position.y + Items[lowerRow, lowerColumn].transform.position.y) / 2);
        var xSize = (upperRow - lowerRow) * Items[0, 0].GetComponent<RectTransform>().rect.height * (1 - padding);
        var ySize = (upperColumn - lowerColumn) * Items[0, 0].GetComponent<RectTransform>().rect.width * (1 - padding);
        if (isSquare)
        {
            if (xSize < ySize)
                ySize = xSize;
            else xSize = ySize;
        }
        element.gameObject.GetComponent<RectTransform>().sizeDelta =
                new Vector2(xSize, ySize);
    }
}