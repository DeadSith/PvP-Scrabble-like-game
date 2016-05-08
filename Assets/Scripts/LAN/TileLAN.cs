using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TileLAN : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    public Text CurrentLetter;
    public Text PointsText;
    public Text ScoreForWord;
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
            DragHandler.ObjectDragged.transform.position = new Vector3(-1500, -1500);
            if (parent.PlayerNumber != parent.Player1.CurrentPlayer)
            {
                parent.Controller.ShowWrongTurnError();
                return;
            }
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
                var letter = DragHandler.ObjectDragged.GetComponent<LetterLAN>().LetterText.text;
                var letterPanel = parent.Player1;
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
        RemoveOnClick();
    }

    public void RemoveOnClick(bool skip = false)
    {
        if ((parent.CurrentTiles.Count != 0 && parent.CurrentTiles[parent.CurrentTiles.Count - 1] != this) || parent.CurrentTiles.Count == 0)
        {
            parent.Controller.ShowDeleteError();
            return;
        }
        parent.Player1.ChangeBox(1, CurrentLetter.text);
        Remove();
        parent.CurrentTiles.Remove(this);
        if (!skip)
            parent.Player1.ChangeGrid(Row, Column, "");
        if (parent.CurrentTiles.Count == 1) parent.CurrentDirection = GridLAN.Direction.None;
    }

    public void Remove()
    {
        if (String.IsNullOrEmpty(CurrentLetter.text))
        {
            return;
        }
        HasLetter = false;
        CurrentLetter.text = "";
        Debug.Log("");
        if (Row != 0) parent.Field[Row - 1, Column].CanDrop = CheckTile(parent.Field[Row - 1, Column]);
        if (Row != parent.NumberOfRows - 1) parent.Field[Row + 1, Column].CanDrop = CheckTile(parent.Field[Row + 1, Column]);
        if (Column != 0) parent.Field[Row, Column - 1].CanDrop = CheckTile(parent.Field[Row, Column - 1]);
        if (Column != parent.NumberOfColumns - 1) parent.Field[Row, Column + 1].CanDrop = CheckTile(parent.Field[Row, Column + 1]);
        CanDrop = CheckTile(this);
        if (parent.IsFirstTurn)
        {
            parent.CurrentDirection = GridLAN.Direction.None;
            parent.Field[7, 7].CanDrop = true;
        }
    }

    public void OnMouseEnter()
    {
        if (!String.IsNullOrEmpty(CurrentLetter.text))
        {
            PointsText.enabled = true;
            PointsText.text = LetterBox.PointsDictionary[CurrentLetter.text].ToString();
            CurrentLetter.enabled = false;
        }
    }

    public void OnMouseExit()
    {
        PointsText.enabled = false;
        CurrentLetter.enabled = true;
    }

    public void SetPoints(int score)
    {
        ScoreForWord.gameObject.SetActive(true);
        ScoreForWord.text = score.ToString();
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        var c = ScoreForWord.color;
        var f = 3f;
        for (; f >= 2; f -= 0.1f)
        {
            yield return new WaitForSeconds(.1f);
        }
        for (; f > 0; f -= 0.1f)
        {
            c.a = f * 0.5f;
            ScoreForWord.color = c;
            yield return new WaitForSeconds(.1f);
        }
        c.a = 1;
        ScoreForWord.color = c;
        ScoreForWord.text = String.Empty;
        ScoreForWord.gameObject.SetActive(false);
    }
}