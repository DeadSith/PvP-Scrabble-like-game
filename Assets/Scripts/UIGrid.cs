using UnityEngine;

public class UIGrid : MonoBehaviour
{
    public GameObject[,] Items;
    public bool IsSquare = false;
    public int RowCount = 1;
    public int ColCount = 1;
    public GameObject CellPrefab;
    public bool IsIntialized { get; private set; }

    public void Initialize()
    {
        IsIntialized = true;
        Items = new GameObject[RowCount, ColCount];
        var gridSize = GetComponent<RectTransform>().rect;
        var xSize = gridSize.width / ColCount * gameObject.transform.lossyScale.x;
        var ySize = gridSize.height / RowCount * gameObject.transform.lossyScale.y;
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
                var curCell = Instantiate(CellPrefab);
                curCell.transform.SetParent(gameObject.transform);
                curCell.transform.position = curPosition;
                Items[i, j] = curCell;
                curPosition.x += xSize;
            }
            curPosition.y += ySize;
            curPosition.x = xStart;
        }
    }

    public void AddElement(int row, int column, GameObject element, float padding = 0, bool isSquare = false, bool preserveSize = false)//element should have anchors in middle and centre
    {
        element.transform.position = new Vector3(Items[row, column].transform.position.x, Items[row, column].transform.position.y);
        if (preserveSize)
            return;
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

    public void AddElement(int upperRow, int upperColumn, int lowerRow, int lowerColumn, GameObject element, float padding = 0, bool isSquare = false, bool preserveSize = false)
    {
        element.transform.position = new Vector3((Items[upperRow, upperColumn].transform.position.x + Items[lowerRow, lowerColumn].transform.position.x) / 2,
            (Items[upperRow, upperColumn].transform.position.y + Items[lowerRow, lowerColumn].transform.position.y) / 2);
        if (preserveSize)
            return;
        var ySize = (upperRow - lowerRow + 1) * Items[0, 0].GetComponent<RectTransform>().rect.height * (1 - padding);
        var xSize = (lowerColumn - upperColumn + 1) * Items[0, 0].GetComponent<RectTransform>().rect.width * (1 - padding);
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