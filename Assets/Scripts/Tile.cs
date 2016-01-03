using UnityEngine;
using System.Collections;
using System.Linq;
using UnityEngine.UI;
using  UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IDropHandler
{

    public Text CurrentLetter;
    public bool HasLetter;
    

    public void OnDrop(PointerEventData eventData)
    {
        HasLetter = true;
        CurrentLetter.text = DragHandler.ObjectDragged.GetComponentsInChildren<Text>().First().text;
        var parent = DragHandler.ObjectDragged.transform.parent.gameObject.GetComponent<LetterBox>();
        parent.ChangeLetter(CurrentLetter.text);
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
