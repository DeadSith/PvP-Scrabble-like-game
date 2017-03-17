using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TileH : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    public Text CurrentLetter;
    public Text PointsText;
    public string TempLetter;

    //Is used to show points for single word in the end of turn
    public Text ScoreForWord;

    public bool HasLetter;
    public bool CanDrop;
    public int Row;
    public int Column;
    public int LetterMultiplier = 1;
    public int WordMultiplier = 1;

    private FieldH parent;

    private void Start()
    {
        parent = transform.parent.gameObject.GetComponent<FieldH>();
    }

    //Checks if letter can be dropped, if it is, writes letter in this TileH
    public void OnDrop(PointerEventData eventData)
    {
        if (CanDrop && !HasLetter)
        {
            DragHandler.ObjectDragged.transform.position = new Vector3(-1500, -1500);//Prevents letter getting stuck on the field
            if (parent.CurrentDirection == FieldH.Direction.None ||
                (parent.CurrentDirection == FieldH.Direction.Horizontal && Row == parent.CurrentTiles[0].Row) ||
                (parent.CurrentDirection == FieldH.Direction.Vertical && Column == parent.CurrentTiles[0].Column))
            {
                parent.CurrentTiles.Add(this);
                if (parent.CurrentTiles.Count == 2)
                {
                    if (parent.CurrentTiles[0].Row == Row) parent.CurrentDirection = FieldH.Direction.Horizontal;
                    else if (parent.CurrentTiles[0].Column == Column) parent.CurrentDirection = FieldH.Direction.Vertical;
                    else
                    {
                        parent.Controller.ShowWrongTileError();
                        parent.CurrentTiles.RemoveAt(1);
                        return;
                    }
                }
                HasLetter = true;
                CurrentLetter.text = DragHandler.ObjectDragged.GetComponent<LetterH>().LetterText.text;
                var letterPanel = DragHandler.ObjectDragged.transform.parent.gameObject.GetComponent<LetterBoxH>();
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

    //checks if one of the nearby tiles has letter
    private bool CheckTile(TileH checkedTileH)
    {
        if (checkedTileH.Row != 0 && parent.Field[checkedTileH.Row - 1, checkedTileH.Column].HasLetter)
        {
            return true;
        }
        if (checkedTileH.Row != parent.NumberOfRows - 1 && parent.Field[checkedTileH.Row + 1, checkedTileH.Column].HasLetter)
        {
            return true;
        }
        if (checkedTileH.Column != 0 && parent.Field[checkedTileH.Row, checkedTileH.Column - 1].HasLetter)
        {
            return true;
        }
        if (checkedTileH.Column != parent.NumberOfColumns - 1 && parent.Field[checkedTileH.Row, checkedTileH.Column + 1].HasLetter)
        {
            return true;
        }
        return false;
    }

    //Checks what button is pressed and calls RemoveTile
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;
        RemoveTile();
    }

    //Removes letter from this TileH and returns it to hand of the player who droped it
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
        if (parent.CurrentTiles.Count == 1 || parent.CurrentTiles.Count == 0)
            parent.CurrentDirection = FieldH.Direction.None;
        if (parent.isFirstTurn)
        {
            parent.CurrentDirection = FieldH.Direction.None;
            if (parent.CurrentTurn == 1)
                parent.Field[7, 7].CanDrop = true;
        }
    }

    //Sets score for word to show in the end of turn.
    public void SetPoints(int score)
    {
        ScoreForWord.gameObject.SetActive(true);
        ScoreForWord.text = score.ToString();
        StartCoroutine(Fade());
    }

    //Shows points for current letter
    public void OnMouseEnter()
    {
        if (!String.IsNullOrEmpty(CurrentLetter.text))
        {
            PointsText.enabled = true;
            PointsText.text = (LetterBoxH.PointsDictionary[CurrentLetter.text] * LetterMultiplier).ToString();
            CurrentLetter.enabled = false;
        }
    }

    //Hides points and shows letter
    public void OnMouseExit()
    {
        PointsText.enabled = false;
        CurrentLetter.enabled = true;
    }

    //Visual effect for fading score for words in the end of turn
    private IEnumerator Fade()
    {
        var c = ScoreForWord.color;
        var f = 3f;
        for (; f >= 2; f -= 0.1f) //Show score for 1 second
        {
            yield return new WaitForSeconds(.1f);
        }
        for (; f > 0; f -= 0.1f) //Start slowly fading text
        {
            c.a = f * 0.5f;
            ScoreForWord.color = c;
            yield return new WaitForSeconds(.1f);
        }
        //Resets all the values
        c.a = 1;
        ScoreForWord.color = c;
        ScoreForWord.text = String.Empty;
        ScoreForWord.gameObject.SetActive(false);
    }
}