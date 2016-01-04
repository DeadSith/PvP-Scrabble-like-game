using System;
using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;
using  UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IDropHandler
{

    public Text CurrentLetter;
    public bool HasLetter;
    public bool CanDrop;
    public int Row;
    public int Column;

    public void OnDrop(PointerEventData eventData)
    {
        var parent = gameObject.transform.parent.transform.GetComponent<Grid>();
        if (parent.isFirstGeneral || CanDrop)
        {
            if (parent.CurrentCoordinates.Count == 2 ||
                parent.CurrentCoordinates.Count == 0 ||
                (parent.CurrentDirection==Grid.Direction.Horizontal&&Row ==parent.CurrentCoordinates[0])||
                (parent.CurrentDirection==Grid.Direction.Vertical&&Column==parent.CurrentCoordinates[1])
                )
            {
                parent.CurrentCoordinates.Add(Row);
                parent.CurrentCoordinates.Add(Column);
                if (parent.CurrentCoordinates.Count == 4)
                {
                    if(parent.CurrentCoordinates[0]== Row) parent.CurrentDirection=Grid.Direction.Horizontal;
                    else if (parent.CurrentCoordinates[1] == Column) parent.CurrentDirection = Grid.Direction.Vertical;
                    else
                    {
                        parent.CurrentCoordinates.RemoveAt(2);
                        parent.CurrentCoordinates.RemoveAt(3);
                        return;
                    }
                }
                HasLetter = true;
                CurrentLetter.text = DragHandler.ObjectDragged.GetComponent<Letter>().LetterText.text;
                var letterParent = DragHandler.ObjectDragged.transform.parent.gameObject.GetComponent<LetterBox>();
                letterParent.ChangeLetter(CurrentLetter.text);
                parent.isFirstGeneral = false;
                if (Column != 0) parent.Field[Row, Column - 1].CanDrop = true;
                if (Column != parent.NumberOfColumns - 1) parent.Field[Row, Column + 1].CanDrop = true;
                if (Row != 0) parent.Field[Row - 1, Column].CanDrop = true;
                if (Row != parent.NumberOfRows - 1) parent.Field[Row + 1, Column].CanDrop = true;
                Debug.Log(parent.CurrentDirection.ToString() + " " + DateTime.Now.ToString());
                Destroy(DragHandler.ObjectDragged);
            }
        }
    }

    bool CheckTile(Tile checkedTile) //checks if one of the nearby tiles has letter
    {
        var parent = checkedTile.gameObject.transform.parent.transform.GetComponent<Grid>();
        if (checkedTile.Row != 0 && parent.Field[checkedTile.Row - 1, checkedTile.Column].HasLetter)
        {
            return true;
        }
        if (checkedTile.Row != parent.NumberOfRows - 1 && parent.Field[checkedTile.Row + 1, checkedTile.Column].HasLetter)
        {
            return true;
        }
        if (checkedTile.Column != 0 && parent.Field[checkedTile.Row, checkedTile.Column - 1].HasLetter)
        {
           return true;
        }
        if (checkedTile.Column != parent.NumberOfColumns - 1 && parent.Field[checkedTile.Row, checkedTile.Column + 1].HasLetter)
        {
           return true;
        }
        return false;
    }
    void OnMouseDown()
    {
        //Todo: Add direction checks
        if (HasLetter)
        {
            HasLetter = false;
            var parent = transform.parent.gameObject.GetComponent<Grid>();
            if (parent.CurrentPlayer == 1)
                parent.Player1.ChangeBox(1, CurrentLetter.text);
            else parent.Player2.ChangeBox(1,CurrentLetter.text);
            CurrentLetter.text = "";
            parent.CurrentCoordinates.RemoveAt(parent.CurrentCoordinates.Count-1);
            parent.CurrentCoordinates.RemoveAt(parent.CurrentCoordinates.Count - 1);
            if (Row != 0) parent.Field[Row - 1, Column].CanDrop = CheckTile(parent.Field[Row - 1, Column]);
            if (Row != parent.NumberOfRows - 1) parent.Field[Row + 1, Column].CanDrop = CheckTile(parent.Field[Row + 1, Column]);
            if (Column != 0) parent.Field[Row, Column - 1].CanDrop = CheckTile(parent.Field[Row, Column - 1]);
            if (Column != parent.NumberOfColumns - 1) parent.Field[Row, Column + 1].CanDrop = CheckTile(parent.Field[Row, Column + 1]);
            CanDrop = CheckTile(this);
        }
    }
}
