using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DragController : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public EventSystem eventSys;

    Vector2 lastPosition = Vector2.zero;
    public Events.Vector3 dragStart = new Events.Vector3();
    public Events.Vector3 dragEnd = new Events.Vector3();
    public Events.Vector2 mainDrag = new Events.Vector2();
    public Vector2 directionTouch;

    private void OnEnable()
    {
        // Uncomment and adjust if needed
        // int defaultValue = eventSys.pixelDragThreshold;
        // eventSys.pixelDragThreshold =
        //     Mathf.Max(
        //         defaultValue,
        //         (int) (defaultValue * Screen.dpi / 100f));
        // dragEnd.AddListener(LevelManager.instance.thisLevel.playerControls.StuffToDoOnMouseUp);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        lastPosition = eventData.position;
        dragStart.Invoke(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragEnd.Invoke(eventData.position);
        SetMovingStatus(false); // Reset directionTouch when dragging ends
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 direction = eventData.position - lastPosition;
        direction = direction.normalized;
        directionTouch = direction;

        // Update last position for smoother dragging
        lastPosition = eventData.position;
        mainDrag.Invoke(direction);
    }

    public void Dragging()
    {
        // This method can be used for additional drag handling if needed
    }

    public void SetMovingStatus(bool status)
    {
        if (!status)
        {
            directionTouch = Vector2.zero;
            mainDrag.Invoke(Vector2.zero); // Ensure mainDrag event reflects the reset
        }
    }
}

/// <summary>
/// Container class for Serializable Events in UnityEditor.
/// </summary>
public static class Events
{
    // Additional event definitions...
    [Serializable]
    public class Vector3 : UnityEvent<UnityEngine.Vector3>
    {
    }

    [Serializable]
    public class Vector2 : UnityEvent<UnityEngine.Vector2>
    {
    }
}
