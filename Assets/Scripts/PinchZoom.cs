/*#if DEBUG
    #undef DEBUG*/

//Uncomment upper lines for build
using UnityEngine;

public class PinchZoom : MonoBehaviour
{
    public int MinSize = 80;
    public int MaxSize = 200;
    public float ZoomSpeed = 15;
    public float MoveSpeed = 0.5f;
    private Vector3 LastPoss = Vector3.zero;

    private void Update()
    {
        var camera = gameObject.GetComponent<Camera>();
        if (Application.platform == RuntimePlatform.Android && Input.multiTouchEnabled)
        {
            if (Input.touchCount > 1)
            {
                var touchZero = Input.GetTouch(0);
                var touchOne = Input.GetTouch(1);
                var touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
                var touchOnePrevPos = touchOne.position - touchOne.deltaPosition;
                var magnitudeDiff = (touchZeroPrevPos - touchOnePrevPos).magnitude -
                                    (touchZero.position - touchOne.position).magnitude;
                camera.orthographicSize += magnitudeDiff * ZoomSpeed;
            }
            else if (Input.touchCount == 1)
            {
                var touchZero = Input.GetTouch(0);
                var diff = (touchZero.position - touchZero.deltaPosition) * MoveSpeed;
                camera.transform.position += new Vector3(diff.x, diff.y);
            }
        }
#if DEBUG
        else
        {
            camera.orthographicSize += Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;
            if (Input.GetMouseButton(1))
            {
                var diff = LastPoss - Input.mousePosition;
                camera.transform.position += diff * MoveSpeed;
            }
            LastPoss = Input.mousePosition;
        }
#endif
        if (camera.orthographicSize < MinSize)
            camera.orthographicSize = MinSize;
        else if (camera.orthographicSize > MaxSize)
            camera.orthographicSize = MaxSize;
    }
}