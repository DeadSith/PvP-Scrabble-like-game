using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static GameObject ObjectDragged;
    public static Vector3 StartPosition;
    public void OnBeginDrag(PointerEventData eventData)
    {
        ObjectDragged = gameObject;
        StartPosition = transform.position;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        //Debug.Log("OnBeginDrag");
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,20));
        //Debug.Log("OnDrag");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ObjectDragged = null;
        transform.position = StartPosition;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        //Debug.Log("OnEndDrag");
    }
}
