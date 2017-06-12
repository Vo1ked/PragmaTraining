using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class CameraMove : MonoBehaviour
{
    public float DragSpeed;
    public float MaxDistanceToCircle;
    public float MaxHight;
    public float MinHight;

    public Vector3 CenterOfCircle;

    private Vector2 _dragOrigin;


    public void GetTouchPosition(BaseEventData eventData)
    {
        _dragOrigin = ((PointerEventData) eventData).position;
    }

    public void CameraDrag(BaseEventData eventData)
    {
        var currentPosition = ((PointerEventData) eventData).position;
        var heading = _dragOrigin - currentPosition;
        var distance = heading.magnitude;
        var direction = heading/distance;
        var directionV3 = new Vector3(direction.x, 0, direction.y);
        if (Vector3.Distance(CenterOfCircle, transform.position) < MaxDistanceToCircle + 0.4f)
        {
            transform.Translate(directionV3*DragSpeed, Space.World);
        }
        if (Vector3.Distance(CenterOfCircle, transform.position) >= MaxDistanceToCircle)
        {
            transform.Translate(-directionV3*DragSpeed, Space.World);

        }
    }

    public void IncreaseHight ()
        {
            if (transform.position.y <= MaxHight)
            {
                transform.position += Vector3.up;
                CenterOfCircle.y++;
            }
        }

    public void DecreaseHight()
    {
        if (transform.position.y >= MinHight)
        {
            transform.position += Vector3.down;
            CenterOfCircle.y--;
        }
    }
}
    