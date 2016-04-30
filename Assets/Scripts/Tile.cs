using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tile : MonoBehaviour, IDropHandler, IPointerClickHandler
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

    private Grid parent;

    private void Start()
    {
        parent = transform.parent.gameObject.GetComponent<Grid>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (CanDrop && !HasLetter)
        {
            DragHandler.ObjectDragged.transform.position = new Vector3(-1500, -1500);
            if (parent.CurrentDirection == Grid.Direction.None ||
                (parent.CurrentDirection == Grid.Direction.Horizontal && Row == parent.CurrentTiles[0].Row) ||
                (parent.CurrentDirection == Grid.Direction.Vertical && Column == parent.CurrentTiles[0].Column))
            {
                parent.CurrentTiles.Add(this);
                if (parent.CurrentTiles.Count == 2)
                {
                    if (parent.CurrentTiles[0].Row == Row) parent.CurrentDirection = Grid.Direction.Horizontal;
                    else if (parent.CurrentTiles[0].Column == Column) parent.CurrentDirection = Grid.Direction.Vertical;
                    else
                    {
                        parent.Controller.ShowWrongTileError();
                        parent.CurrentTiles.RemoveAt(1);
                        return;
                    }
                }
                HasLetter = true;
                CurrentLetter.text = DragHandler.ObjectDragged.GetComponent<Letter>().LetterText.text;
                var letterPanel = DragHandler.ObjectDragged.transform.parent.gameObject.GetComponent<LetterBox>();
                letterPanel.CanChangeLetters = false;
                letterPanel.RemoveLetter();
                if (Column != 0) parent.Field[Row, Column - 1].CanDrop = true;
                if (Column != parent.NumberOfColumns - 1) parent.Field[Row, Column + 1].CanDrop = true;
                if (Row != 0) parent.Field[Row - 1, Column].CanDrop = true;
                if (Row != parent.NumberOfRows - 1) parent.Field[Row + 1, Column].CanDrop = true;
                Destroy(DragHandler.ObjectDragged);
            }
            else parent.Controller.ShowWrongTileError();
        }
        else parent.Controller.ShowWrongTileError();
    }

    private bool CheckTile(Tile checkedTile) //checks if one of the nearby tiles has letter
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
            parent.Player1.ChangeBox(1, CurrentLetter.text);
        else parent.Player2.ChangeBox(1, CurrentLetter.text);
        CurrentLetter.text = "";
        parent.CurrentTiles.Remove(this);
        if (Row != 0) parent.Field[Row - 1, Column].CanDrop = CheckTile(parent.Field[Row - 1, Column]);
        if (Row != parent.NumberOfRows - 1) parent.Field[Row + 1, Column].CanDrop = CheckTile(parent.Field[Row + 1, Column]);
        if (Column != 0) parent.Field[Row, Column - 1].CanDrop = CheckTile(parent.Field[Row, Column - 1]);
        if (Column != parent.NumberOfColumns - 1) parent.Field[Row, Column + 1].CanDrop = CheckTile(parent.Field[Row, Column + 1]);
        CanDrop = CheckTile(this);
        if (parent.CurrentTiles.Count == 1) parent.CurrentDirection = Grid.Direction.None;
        if (parent.isFirstTurn)
        {
            parent.CurrentDirection = Grid.Direction.None;
            if (parent.CurrentTurn == 1)
                parent.Field[7, 7].CanDrop = true;
        }
        if (parent.CurrentTiles.Count == 0)
        {
            if (parent.CurrentPlayer == 1)
                parent.Player1.CanChangeLetters = true;
            else parent.Player2.CanChangeLetters = true;
        }
    }

    public void SetPoints(int score)
    {
        ScoreForWord.gameObject.SetActive(true);
        ScoreForWord.text = score.ToString();
        StartCoroutine(Fade());
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
            c.a = f*0.5f;
            ScoreForWord.color = c;
            yield return new WaitForSeconds(.1f);
        }
        c.a = 1;
        ScoreForWord.color = c;
        ScoreForWord.text=String.Empty;
        ScoreForWord.gameObject.SetActive(false);
    }
}