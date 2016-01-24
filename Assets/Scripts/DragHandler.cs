using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject ObjectDragged;
    public static Vector3 StartPosition;
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (ObjectDragged != null)
        {
            ObjectDragged.GetComponent<Letter>().Fix();
        }
        ObjectDragged = gameObject;
        StartPosition = transform.position;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,20));
        //transform.position  = new Vector3(Input.mousePosition.x,Input.mousePosition.y,20);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
            ObjectDragged.transform.position = StartPosition;
            ObjectDragged = null;
            GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}
