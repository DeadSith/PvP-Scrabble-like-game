using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Networking;

public class TileLAN : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    public Text CurrentLetter;
    public bool HasLetter;
    public bool CanDrop;
    public int Row;
    public int Column;
    public int LetterMultiplier = 1;
    public int WordMultiplier = 1;
    private GridLAN parent;

    private void Start()
    {
            parent = transform.parent.gameObject.GetComponent<GridLAN>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (CanDrop && !HasLetter)
        {
            if (parent.CurrentDirection == GridLAN.Direction.None ||
                (parent.CurrentDirection == GridLAN.Direction.Horizontal && Row == parent.CurrentTiles[0].Row) ||
                (parent.CurrentDirection == GridLAN.Direction.Vertical && Column == parent.CurrentTiles[0].Column))
            {
                parent.CurrentTiles.Add(this);
                if (parent.CurrentTiles.Count == 2)
                {
                    if (parent.CurrentTiles[0].Row == Row) parent.CurrentDirection = GridLAN.Direction.Horizontal;
                    else if (parent.CurrentTiles[0].Column == Column) parent.CurrentDirection = GridLAN.Direction.Vertical;
                    else
                    {
                        parent.Controller.ShowWrongTileError();
                        parent.CurrentTiles.RemoveAt(1);
                        return;
                    }
                }
                var letter = DragHandler.ObjectDragged.GetComponent<Letter>().LetterText.text;
                var letterPanel = parent.Player1;
                parent.CanChangeLetters = false;
                letterPanel.RemoveLetter();
                Destroy(DragHandler.ObjectDragged);
                parent.Player1.ChangeGrid(Row, Column, letter);
            }
            else parent.Controller.ShowWrongTileError();
        }
        else parent.Controller.ShowWrongTileError();
    }

    public void ChangeLetter(string letter)
    {
        HasLetter = true;
        CurrentLetter.text = letter;
        if (Column != 0) parent.Field[Row, Column - 1].CanDrop = true;
        if (Column != parent.NumberOfColumns - 1) parent.Field[Row, Column + 1].CanDrop = true;
        if (Row != 0) parent.Field[Row - 1, Column].CanDrop = true;
        if (Row != parent.NumberOfRows - 1) parent.Field[Row + 1, Column].CanDrop = true;
    }

    private bool CheckTile(TileLAN checkedTile) //checks if one of the nearby tiles has letter
    {
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

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        RemoveTile();
    }

    public void RemoveTile()
    {
        if ((parent.CurrentTiles.Count != 0 && parent.CurrentTiles[parent.CurrentTiles.Count - 1] != this) || parent.CurrentTiles.Count == 0)
        {
            parent.Controller.ShowDeleteError();
            return;
        }
        HasLetter = false;
        if (parent.CurrentPlayer == 1)
            //parent.Player1.ChangeBox(1); //for testing
            parent.Player1.ChangeBox(1, CurrentLetter.text);
        Debug.LogError(parent.Player1.isServer);
        CurrentLetter.text = "";
        parent.CurrentTiles.Remove(this);
        if (Row != 0) parent.Field[Row - 1, Column].CanDrop = CheckTile(parent.Field[Row - 1, Column]);
        if (Row != parent.NumberOfRows - 1) parent.Field[Row + 1, Column].CanDrop = CheckTile(parent.Field[Row + 1, Column]);
        if (Column != 0) parent.Field[Row, Column - 1].CanDrop = CheckTile(parent.Field[Row, Column - 1]);
        if (Column != parent.NumberOfColumns - 1) parent.Field[Row, Column + 1].CanDrop = CheckTile(parent.Field[Row, Column + 1]);
        CanDrop = CheckTile(this);
        if (parent.CurrentTiles.Count == 1) parent.CurrentDirection = GridLAN.Direction.None;
        if (parent.isFirstTurn)
        {
            parent.Player1.CanChangeLetters = true;
            parent.CurrentDirection = GridLAN.Direction.None;
            if (parent.CurrentTurn == 1)
                parent.Field[7, 7].CanDrop = true;
        }
    }
}