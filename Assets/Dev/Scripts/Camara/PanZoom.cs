using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace MyUtility.MyUIComponents.PanZoom
{
    public class PanZoom : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerMoveHandler
    {
        [System.Serializable]
        public class OnPanZoomEvent : UnityEvent<PanZoomData>
        {
        }

        public delegate void OnPanZoomNativeEvent(PanZoomData panZoomData);

        [System.Serializable]
        public struct PanZoomData
        {
            public Vector2 initialPosition1, currentPosition1;
            public float deltaEditor;
            public MoveAxis moveAxis;
            public MoveDir moveDir;
            public int dirSign;

            public PointerEventData pointerEventData;
            public Vector3 initialPosition1World, currentPosition1World;
        }

        public enum MoveAxis
        {
            Horizontal = 0,
            Vertical = 1
        }

        public enum MoveDir
        {
            Up = 0,
            Down = 1,
            Left = 2,
            Right = 3
        }

        PanZoomData initialData;
        bool InitialEvent = false;

        public OnPanZoomEvent onPanZoomEvent;
        public OnPanZoomEvent onPanZoomEndEvent;
        public OnPanZoomEvent onPanZoomStarEvent;


        public event OnPanZoomNativeEvent onPanZoomNativeEvent;
        public event OnPanZoomNativeEvent onPanZoomEndNativeEvent;
        public event OnPanZoomNativeEvent onPanZoomStarNativeEvent;

        private void Awake()
        {
            if (onPanZoomEvent == null)
                onPanZoomEvent = new OnPanZoomEvent();

            if (onPanZoomEndEvent == null)
                onPanZoomEndEvent = new OnPanZoomEvent();

            if (onPanZoomStarEvent == null)
                onPanZoomStarEvent = new OnPanZoomEvent();
        }

        private void Start()
        {
            
        }

        private void OnDisable()
        {
            //Debug.Log("Call Disable");
            touchIdPair.Clear();
        }

        private bool beginDrag = true;
        private void OnEnable()
        {
            //Debug.Log("Call Enable");
            beginDrag = false;
        }

        public Dictionary<int, bool> touchIdPair = new System.Collections.Generic.Dictionary<int, bool>();

        public void OnBeginDrag(PointerEventData eventData)
        {
            //Debug.Log("Touch Count - " + Input.touchCount);
            //Debug.Log("Begin Drag - Pan Zoom - ");

            if (!touchIdPair.ContainsKey(eventData.pointerId))
                touchIdPair.Add(eventData.pointerId, false);

            if (touchIdPair.Count == 1)
            {
                //Debug.Log("Begin Drag - Pan Zoom");
                InitialEvent = true;
                PanZoomData data = new PanZoomData();
                data.initialPosition1 = eventData.position;
                data.currentPosition1 = eventData.position;
                data.initialPosition1World = eventData.pointerCurrentRaycast.worldPosition;
                data.currentPosition1World = eventData.pointerCurrentRaycast.worldPosition;
                data.pointerEventData = eventData;
                initialData = data;

                onPanZoomStarEvent.Invoke(data);
                onPanZoomStarNativeEvent?.Invoke(data);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {

            //Debug.Log("OnEndDrag - " + eventData.pointerId + " - " + eventData.position);

            if (!touchIdPair.ContainsKey(eventData.pointerId))
                return;

            touchIdPair.Remove(eventData.pointerId);

            if (touchIdPair.Count == 0)
            {
                //Debug.Log("OnEndDrag " + eventData.pointerId);
                InitialEvent = false;
                PanZoomData data = new PanZoomData();
                data.initialPosition1 = initialData.initialPosition1;
                data.currentPosition1 = eventData.position;
                data.initialPosition1World = initialData.initialPosition1;
                data.currentPosition1World = eventData.pointerCurrentRaycast.worldPosition;
                data.pointerEventData = eventData;
                onPanZoomEndEvent.Invoke(data);
                onPanZoomEndNativeEvent?.Invoke(data);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (InitialEvent)
            {
                InitialEvent = true;
                PanZoomData data = new PanZoomData();
                data.initialPosition1 = initialData.initialPosition1;
                data.currentPosition1 = Input.mousePosition;

                data.initialPosition1World = initialData.initialPosition1;
                data.currentPosition1World = eventData.pointerCurrentRaycast.worldPosition;

                data.pointerEventData = eventData;

                Vector3 t1 = Input.mousePosition;
                float vd = Mathf.Abs(t1.y - initialData.initialPosition1.y);
                float hd = Mathf.Abs(t1.x - initialData.initialPosition1.x);

                if (vd > hd)
                {
                    data.moveAxis = MoveAxis.Vertical;
                    if (t1.y < initialData.initialPosition1.y)
                    {
                        data.moveDir = MoveDir.Down;
                        data.dirSign = -1;
                    }
                    else
                    {
                        data.moveDir = MoveDir.Up;
                        data.dirSign = 1;
                    }

                }
                else
                {
                    data.moveAxis = MoveAxis.Horizontal;
                    if (t1.x < initialData.initialPosition1.x)
                    {
                        data.moveDir = MoveDir.Left;
                        data.dirSign = -1;
                    }
                    else
                    {
                        data.moveDir = MoveDir.Right;
                        data.dirSign = 1;
                    }
                }
                Debug.Log(data);
                onPanZoomEvent.Invoke(data);
                onPanZoomNativeEvent?.Invoke(data);
            }
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            //Debug.Log("Touch ID Pair - " + eventData.pointerId);
            if (!beginDrag && eventData.pointerId != -1)
            {
                //Debug.Log("Move - " + enabled);
                beginDrag = true;
                OnBeginDrag(eventData);
            }
        }

        //public void PassAfterPinch(PointerEventData pointerEventData)
        //{
        //    Debug.Log("Call Again OnBeginDrag");
        //    OnBeginDrag(pointerEventData);
        //}

        /*
    void Update1()
    {
        if (InitialEvent)
        {
            InitialEvent = true;
            PanZoomData data = new PanZoomData();
            data.initialPosition1 = initialPos;
            data.currentPosition1 = Input.mousePosition;
            Vector3 t1 = Input.mousePosition;
            float vd = Mathf.Abs(t1.y - initialPos.y);
            float hd = Mathf.Abs(t1.x - initialPos.x);

            if (vd > hd)
            {
                data.moveAxis = MoveAxis.Vertical;
                if (t1.y < initialPos.y)
                {
                    data.moveDir = MoveDir.Down;
                    data.dirSign = -1;
                }
                else
                {
                    data.moveDir = MoveDir.Up;
                    data.dirSign = 1;
                }

            }
            else
            {
                data.moveAxis = MoveAxis.Horizontal;
                if (t1.x < initialPos.x)
                {
                    data.moveDir = MoveDir.Left;
                    data.dirSign = -1;
                }
                else
                {
                    data.moveDir = MoveDir.Right;
                    data.dirSign = 1;
                }
            }
            onPanZoomEvent.Invoke(data);
        }

        if (Input.GetMouseButtonDown(0))
        {
            InitialEvent = true;
            initialPos = Input.mousePosition;
            PanZoomData data = new PanZoomData();
            data.initialPosition1 = initialPos;
            data.currentPosition1 = initialPos;
            onPanZoomStarEvent.Invoke(data);
        }

        if (Input.GetMouseButtonUp(0))
        {
            InitialEvent = false;
            PanZoomData data = new PanZoomData();
            data.initialPosition1 = initialPos;
            data.currentPosition1 = Input.mousePosition;
            onPanZoomEndEvent.Invoke(data);
        }
    }




    */
        /*
    void Update()
    {
        if (Input.touchCount == 0)
            return;

        t1 = Input.touches[0];

        if (t1.phase == TouchPhase.Began)
        {
            initialPos = t1.position;
        }

        if (!InitialEvent)
        {
            InitialEvent = true;

            PanZoomData data = new PanZoomData();
            data.initialPosition1 = initialPos;
            data.t1 = t1;
            data.currentPosition1 = t1.position;
            onPanZoomStarEvent.Invoke(data);
        }

        if (t1.phase == TouchPhase.Moved)
        {
            InitialEvent = true;

            PanZoomData data = new PanZoomData();
            data.initialPosition1 = initialPos;
            data.t1 = t1;
            data.currentPosition1 = t1.position;

            float vd = Mathf.Abs(t1.position.y - initialPos.y);
            float hd = Mathf.Abs(t1.position.x - initialPos.x);

            if (vd > hd)
            {
                data.moveAxis = MoveAxis.Vertical;
                if (t1.position.y < initialPos.y)
                {
                    data.moveDir = MoveDir.Down;
                    data.dirSign = -1;
                }
                else
                {
                    data.moveDir = MoveDir.Up;
                    data.dirSign = 1;
                }

            }
            else
            {
                data.moveAxis = MoveAxis.Horizontal;
                if (t1.position.x < initialPos.x)
                {
                    data.moveDir = MoveDir.Left;
                    data.dirSign = -1;
                }
                else
                {
                    data.moveDir = MoveDir.Right;
                    data.dirSign = 1;
                }
            }
            onPanZoomEvent.Invoke(data);
        }

        if (t1.phase == TouchPhase.Ended)
        {
            InitialEvent = false;
            PanZoomData data = new PanZoomData();
            data.initialPosition1 = initialPos;
            data.t1 = t1;
            data.currentPosition1 = t1.position;
            onPanZoomEndEvent.Invoke(data);
        }
    }
    */
    }
}
