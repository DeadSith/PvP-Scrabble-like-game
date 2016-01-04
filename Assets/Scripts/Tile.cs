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
    void OnMouseDown()
    {
        //Todo: Add checks for nearby cells CanDrop values
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
