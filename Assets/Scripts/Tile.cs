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
    public int Row;
    public int Column;

    public void OnDrop(PointerEventData eventData)
    {
        HasLetter = true;
        CurrentLetter.text = DragHandler.ObjectDragged.GetComponent<Letter>().LetterText.text;
        var letterParent = DragHandler.ObjectDragged.transform.parent.gameObject.GetComponent<LetterBox>();
        letterParent.ChangeLetter(CurrentLetter.text);
        var parent = gameObject.transform.parent.transform.GetComponent<Grid>();
        parent.Field[Row, Column] = CurrentLetter.text;
        #region Checks
        if (Row == 0)
        {
            if (Column == 0)
            {
                if (!String.IsNullOrEmpty(parent.Field[Row, Column + 1]))
                {
                    parent.CurrentDirection = parent.CurrentDirection!=Grid.Direction.Vertical ? Grid.Direction.Horizontal : Grid.Direction.None;
                }
                else if (!String.IsNullOrEmpty(parent.Field[Row + 1, Column]))
                    parent.CurrentDirection = Grid.Direction.Vertical;
                else
                {
                    parent.CurrentDirection = Grid.Direction.None;
                }
            }
            else if (Column == parent.NumberOfColumns - 1)
            {
                if (!String.IsNullOrEmpty(parent.Field[Row, Column - 1]))
                {
                    parent.CurrentDirection = parent.CurrentDirection != Grid.Direction.Vertical
                        ? Grid.Direction.Horizontal
                        : Grid.Direction.None;
                }
                else if (!String.IsNullOrEmpty(parent.Field[Row + 1, Column]))
                    parent.CurrentDirection = Grid.Direction.Vertical;
                else
                {
                    parent.CurrentDirection = Grid.Direction.None;
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(parent.Field[Row, Column - 1])||!String.IsNullOrEmpty(parent.Field[Row,Column+1]))
                {
                    parent.CurrentDirection = parent.CurrentDirection != Grid.Direction.Vertical ? Grid.Direction.Horizontal : Grid.Direction.None;
                }
                else if (!String.IsNullOrEmpty(parent.Field[Row + 1, Column]))
                    parent.CurrentDirection = Grid.Direction.Vertical;
                else
                {
                    parent.CurrentDirection = Grid.Direction.None;
                }
            }
        }
        else if (Row == parent.NumberOfRows - 1)
        {
            if (Column == 0)
            {
                if (!String.IsNullOrEmpty(parent.Field[Row, Column + 1]))
                {
                    parent.CurrentDirection = parent.CurrentDirection != Grid.Direction.Vertical
                        ? Grid.Direction.Horizontal
                        : Grid.Direction.None;
                }
                else if (!String.IsNullOrEmpty(parent.Field[Row - 1, Column]))
                    parent.CurrentDirection = Grid.Direction.Vertical;
                else
                {
                    parent.CurrentDirection = Grid.Direction.None;
                }
            }
            else if (Column == parent.NumberOfColumns - 1)
            {
                if (!String.IsNullOrEmpty(parent.Field[Row, Column - 1]))
                {
                    parent.CurrentDirection = parent.CurrentDirection != Grid.Direction.Vertical
                        ? Grid.Direction.Horizontal
                        : Grid.Direction.None;
                }
                else if (!String.IsNullOrEmpty(parent.Field[Row - 1, Column]))
                    parent.CurrentDirection = Grid.Direction.Vertical;
                else
                {
                    parent.CurrentDirection = Grid.Direction.None;
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(parent.Field[Row, Column - 1]) ||
                    !String.IsNullOrEmpty(parent.Field[Row, Column + 1]))
                {
                    parent.CurrentDirection = parent.CurrentDirection != Grid.Direction.Vertical
                        ? Grid.Direction.Horizontal
                        : Grid.Direction.None;
                }
                else if (!String.IsNullOrEmpty(parent.Field[Row - 1, Column]))
                    parent.CurrentDirection = Grid.Direction.Vertical;
                else
                {
                    parent.CurrentDirection = Grid.Direction.None;
                }
            }
        }
        else
        {
            if (!String.IsNullOrEmpty(parent.Field[Row+1, Column]) || !String.IsNullOrEmpty(parent.Field[Row-1, Column]))
                parent.CurrentDirection=Grid.Direction.Vertical;
            else if (!String.IsNullOrEmpty(parent.Field[Row, Column - 1]) || !String.IsNullOrEmpty(parent.Field[Row, Column + 1]))
            {
                parent.CurrentDirection = parent.CurrentDirection != Grid.Direction.Vertical ? Grid.Direction.Horizontal : Grid.Direction.None;
            }
            else parent.CurrentDirection = Grid.Direction.None;
        }
#endregion
        Debug.Log(parent.CurrentDirection.ToString()+" "+DateTime.Now.ToString());
        Destroy(DragHandler.ObjectDragged);
    }
    void OnMouseDown()
    {
        if (HasLetter)
        {
            HasLetter = false;
            var parent = transform.parent.gameObject.GetComponent<Grid>();
            if (parent.CurrentPlayer == 1)
                parent.Player1.ChangeBox(1, CurrentLetter.text);
            else parent.Player2.ChangeBox(1,CurrentLetter.text);
            CurrentLetter.text = "";
        }
    }
}
