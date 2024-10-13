using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PinchZoom : MonoBehaviour
{
    [System.Serializable]
    public class OnPinchZoomEvent : UnityEvent<PinchZoomData>
    {
    }

    [System.Serializable]
    public struct PinchZoomData
    {
        public Vector2 initialPosition1, initialPosition2, currentPosition1, currentPosition2;
        public Touch t1, t2;

    }

    Vector2 pos1, pos2;
    Touch t1, t2;
    bool InitialEvent = false;

    public OnPinchZoomEvent onPinchZoomEvent = new OnPinchZoomEvent();
    public OnPinchZoomEvent onPinchZoomEndEvent = new OnPinchZoomEvent();
    public OnPinchZoomEvent onPinchZoomStarEvent = new OnPinchZoomEvent();

    void Update()
    {
        if (Input.touchCount == 0)
            return;

        t1 = Input.touches[0];

        //Debug.Log("Position =>" + t1.position);
        // Debug.Log("Raw Position =>" + t1.rawPosition);

        if (t1.phase == TouchPhase.Began)
        {
            pos1 = t1.position;
        }

        if (Input.touchCount < 2)
        {
            //Debug.Log("Touch Count  - " + 1);
            return;
        }

        t2 = Input.touches[1];
        if (t2.phase == TouchPhase.Began)
        {
            pos2 = t2.position;
        }

        if (!InitialEvent)
        {
            InitialEvent = true;

            PinchZoomData data = new PinchZoomData();
            data.initialPosition1 = pos1;
            data.initialPosition2 = pos2;
            data.t1 = t1;
            data.t2 = t2;
            data.currentPosition1 = t1.position;
            data.currentPosition2 = t2.position;
            onPinchZoomStarEvent.Invoke(data);
        }

        if (t1.phase == TouchPhase.Moved || t2.phase == TouchPhase.Moved)
        {
            InitialEvent = true;

            PinchZoomData data = new PinchZoomData();
            data.initialPosition1 = pos1;
            data.initialPosition2 = pos2;
            data.t1 = t1;
            data.t2 = t2;
            data.currentPosition1 = t1.position;
            data.currentPosition2 = t2.position;
            onPinchZoomEvent.Invoke(data);
        }

        if (t1.phase == TouchPhase.Ended || t2.phase == TouchPhase.Ended)
        {
            InitialEvent = false;


            PinchZoomData data = new PinchZoomData();

            data.initialPosition1 = pos1;
            data.initialPosition2 = pos2;
            data.t1 = t1;
            data.t2 = t2;
            data.currentPosition1 = t1.position;
            data.currentPosition2 = t2.position;
            onPinchZoomEndEvent.Invoke(data);
        }
    }
}
