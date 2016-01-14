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
        if (CanDrop&&!HasLetter)
        {
            if (parent.CurrentDirection==Grid.Direction.None ||
                (parent.CurrentDirection==Grid.Direction.Horizontal&&Row ==parent.CurrentTiles[0].Row)||
                (parent.CurrentDirection==Grid.Direction.Vertical&&Column==parent.CurrentTiles[0].Column)
                )
            {
                parent.CurrentTiles.Add(this);
                if (parent.CurrentTurn != 1 && parent.CurrentTiles.Count == 1)
                {
                    CheckDirection();
                }
                if (parent.CurrentTiles.Count == 2)
                {
                    if(parent.CurrentTiles[0].Row== Row) parent.CurrentDirection=Grid.Direction.Horizontal;
                    else if (parent.CurrentTiles[0].Column == Column) parent.CurrentDirection = Grid.Direction.Vertical;
                    else
                    {
                        parent.CurrentTiles.RemoveAt(1);
                        return;
                    }
                }
                HasLetter = true;
                CurrentLetter.text = DragHandler.ObjectDragged.GetComponent<Letter>().LetterText.text;
                var letterPanel = DragHandler.ObjectDragged.transform.parent.gameObject.GetComponent<LetterBox>();
                letterPanel.ChangeLetter(CurrentLetter.text);
                if (Column != 0) parent.Field[Row, Column - 1].CanDrop = true;
                if (Column != parent.NumberOfColumns - 1) parent.Field[Row, Column + 1].CanDrop = true;
                if (Row != 0) parent.Field[Row - 1, Column].CanDrop = true;
                if (Row != parent.NumberOfRows - 1) parent.Field[Row + 1, Column].CanDrop = true;
                //Debug.Log(parent.CurrentDirection.ToString() + " " + DateTime.Now.ToString());
                Destroy(DragHandler.ObjectDragged);
            }
        }
    }

    void CheckDirection()
    {
        var parent = gameObject.transform.parent.transform.GetComponent<Grid>();
        parent.CurrentDirection = Grid.Direction.None;
        if ((Row != 0 && parent.Field[Row - 1, Column].HasLetter) ||
            (Row != parent.NumberOfRows - 1 && parent.Field[Row + 1, Column].HasLetter))
        {
            parent.CurrentDirection = Grid.Direction.Vertical;
        }
        if  ((Column!=0 && parent.Field[Row,Column-1].HasLetter)||(Column!= parent.NumberOfColumns-1&&parent.Field[Row,Column+1].HasLetter))
            parent.CurrentDirection = parent.CurrentDirection== Grid.Direction.Vertical ? Grid.Direction.None : Grid.Direction.Horizontal;
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
        var parent = transform.parent.gameObject.GetComponent<Grid>();
        //Todo: Add direction checks
        if (parent.CurrentTiles.Contains(this))
        {
            HasLetter = false;
            if (parent.CurrentPlayer == 1)
                parent.Player1.ChangeBox(1, CurrentLetter.text);
            else parent.Player2.ChangeBox(1,CurrentLetter.text);
            CurrentLetter.text = "";
            parent.CurrentTiles.Remove(this);
            if (Row != 0) parent.Field[Row - 1, Column].CanDrop = CheckTile(parent.Field[Row - 1, Column]);
            if (Row != parent.NumberOfRows - 1) parent.Field[Row + 1, Column].CanDrop = CheckTile(parent.Field[Row + 1, Column]);
            if (Column != 0) parent.Field[Row, Column - 1].CanDrop = CheckTile(parent.Field[Row, Column - 1]);
            if (Column != parent.NumberOfColumns - 1) parent.Field[Row, Column + 1].CanDrop = CheckTile(parent.Field[Row, Column + 1]);
            CanDrop = CheckTile(this);
            if(parent.CurrentTiles.Count==1) CheckDirection();
            if (parent.CurrentTiles.Count == 0)
            {
                parent.CurrentDirection=Grid.Direction.None;
                if (parent.CurrentTurn == 1)
                    parent.Field[7, 7].CanDrop = true;
            }
        }
    }
}
