using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject ObjectDragged;
    public static Vector3 StartPosition;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (ObjectDragged != null)
        {
            //Removes stuck LetterH from field
            //To lazy to write it the right way
            try
            {
                ObjectDragged.GetComponent<LetterH>().Fix();
            }
            catch (Exception)
            {
                ObjectDragged.GetComponent<LetterLAN>().Fix();
            }
        }
        ObjectDragged = gameObject;
        StartPosition = transform.position;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 20));
    }

    //Called when dropped in the wrong place
    public void OnEndDrag(PointerEventData eventData)
    {
        ObjectDragged.transform.position = StartPosition;
        ObjectDragged = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}