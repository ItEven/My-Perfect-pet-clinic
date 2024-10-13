using System;
using System.Collections.Generic;
using MyUtility.MyUIComponents.PanZoom;
//using MyUtility.MyUIComponents.PinchZoom;
//using MyUtility.MyUIComponents.PinchZoom;
using UnityEngine;
using UnityEngine.EventSystems;
using static PinchZoom;


public class CameraController : MonoBehaviour
{
    public static CameraController Instance;
    public static Action<Vector2> Camfoucus;

    [SerializeField] Vector2 cameraVerticalRange;
    [SerializeField] Vector2 cameraHorizontalRange;
    public Vector2 cameraVerticalRangeOriginal;
    public Vector2 cameraHorizontalRangeOriginal;
    public PanZoom panZoom;
    public PinchZoom pinchZoom;

    public Transform cameraVerticalTransform;
    public Transform cameraCTR;
    public Camera camera;
    public Vector3 cameraRotation;
    public Vector2 zoomMaxMinLimit;
    public float StorelastZoom;
    public float lastZoom
    {

        get
        {

            return StorelastZoom;
        }
        private set
        {
            StorelastZoom = value;
        }
    }


    //public Vector2 movementSpeedBasedOnZoom;

    public float speed = 1;
    public float maxSpeed;
    public float lerpSpeed = 1;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        Camfoucus += FocusCam;
    }
    private void OnDisable()
    {
        Camfoucus -= FocusCam;
    }

    void UpdateCameraRangeArea()
    {
        var size = camera.GetCameraSizeInUnityUnit();
        var panWidth = Mathf.Abs(cameraHorizontalRangeOriginal.x - cameraHorizontalRangeOriginal.y);

        if (panWidth < size.x * 2)
        {
            cameraHorizontalRange.x = 0;
            cameraHorizontalRange.y = 0;
        }
        else
        {
            cameraHorizontalRange.x = (cameraHorizontalRangeOriginal.x + size.x);
            cameraHorizontalRange.y = (cameraHorizontalRangeOriginal.y - size.x);
        }
        cameraVerticalRange.x = (cameraVerticalRangeOriginal.x + size.y);
        cameraVerticalRange.y = (cameraVerticalRangeOriginal.y - size.y);
    }

    //public void Init()
    void Start()
    {

        pinchZoom.onPinchZoomStarEvent.AddListener(OnZoomStart);
        pinchZoom.onPinchZoomEndEvent.AddListener(OnZoomEnd);
        pinchZoom.onPinchZoomEvent.AddListener(OnZoom);
        panZoom.onPanZoomEndNativeEvent += OnTouchUp;
        panZoom.onPanZoomStarNativeEvent += OnTouchDown;
        panZoom.onPanZoomNativeEvent += OnTouch;
        //64.6 unit width of factory
        //maxOrthographicSize = CameraExtension.GetOrthographicSizeUsingWidth(camera, 64.6f);
        //zoomLerp = zoomMaxMinLimit.y;
        //zoomValue = zoomMaxMinLimit.y;
        //range = new Vector2(35, 47);
        //range = new Vector2(-20, 15);
        //range = new Vector2(-52.3f, -3.6f);
        //range = new Vector2(-31f, -10f);
        //range = new Vector2(-61.86f, -6.5f);
        //lerp = range = new Vector2(-80.3f, -0.6f);
        maxSpeed = speed;


        // make a if condition is dont wanrt to start wirth this value for the tutorial  

        lerp = range;
        //camera.fieldOfView = 18.11698f;
        zoomValue = 35;
        zoomMaxMinLimit.x = 28;
        zoomMaxMinLimit.y = 50;
        ZoomSpeed = 5;

    }

    //const float maxOrthographicSize = 70;

    Vector2 tempRange, initRange/*, lerp*/;
    public Vector2 range, lerp;
    //private float cameraRangeDistance;

    public void Focus(Vector3 target)
    {
        range.x = target.x;
        range.y = target.z;
        initRange = Vector2.zero;
        tempRange = Vector2.zero;

        range.y = Mathf.Clamp(range.y, cameraVerticalRange.x, cameraVerticalRange.y);
        range.x = Mathf.Clamp(range.x, cameraHorizontalRange.x, cameraHorizontalRange.y);
    }

    public void FocusCam(Vector2 target)
    {

        range.x = target.x;
        range.y = target.y;
        initRange = Vector2.zero;
        tempRange = Vector2.zero;
        range.y = Mathf.Clamp(range.y, cameraVerticalRange.x, cameraVerticalRange.y);
        range.x = Mathf.Clamp(range.x, cameraHorizontalRange.x, cameraHorizontalRange.y);
    }



    Transform continueFocusTarget;

    public void ContinueFocus(Transform target)
    {
        //Debug.Log(" - " + Tutorial.instance.tutorialIndex);

        continueFocusTarget = target;
        //if(Tutorial.instance.tutorialIndex == 3)
        //{
        //    zoomValue = 18.11698f;
        //}
        //else
        //{
        //    zoomValue = 25;
        //}        
    }

    public Transform GetTarget()
    {
        return continueFocusTarget;
    }


    public void UpdateRange(Vector2 vRange, Vector2 hRange)
    {
        cameraVerticalRangeOriginal = vRange;
        cameraHorizontalRangeOriginal = hRange;
        // cameraRangeDistance = Mathf.Abs(Mathf.Abs(cameraRange.x) - Mathf.Abs(cameraRange.y));
    }

    public void OnTouchDown(PanZoom.PanZoomData data)
    {
        //Debug.Log("On Touch Down Camera Controller");

        //foreach(Touch touch in Input.touches)
        //{
        //    Debug.Log("Touch Id - " + touch.fingerId);
        //}
        if (StopPaning)
            return;

        if (Input.touchCount == 2 || isZoomRunning)
        {
            return;
        }

        //Debug.Log("Pan Start");
        tempRange = Vector2.zero;
        initRange = range;
        range = initRange + tempRange;
        isBegan = true;
        //Debug.Log(string.Format("initRange - {0}, tempRange - {1}", initRange, tempRange));
    }

    public void OnTouchUp(PanZoom.PanZoomData data)
    {

        //foreach (Touch touch in Input.touches)
        //{
        //    Debug.Log("Touch up Id - " + touch.fingerId);
        //}

        if (Input.touchCount == 2 || isZoomRunning)
        {
            return;
        }

        //Debug.Log("Pan End");

        //if (delayToRun < 1)
        //    return;

        if (isBegan)
        {
            //Debug.Log(string.Format("initRange - {0}, tempRange - {1}", initRange, tempRange));
            isBegan = false;

            range = initRange - tempRange;
            UpdateCameraRangeArea();
            range.y = Mathf.Clamp(range.y, cameraVerticalRange.x, cameraVerticalRange.y);
            range.x = Mathf.Clamp(range.x, cameraHorizontalRange.x, cameraHorizontalRange.y);
            initRange = Vector2.zero;
            tempRange = Vector2.zero;
            isTouch = false;
        }
    }

    public bool isTouch = false;
    public bool isBegan = false;

    public void OnTouch(PanZoom.PanZoomData eventData)
    {

        //panZoomData.pointerEventData = eventData.pointerEventData;
        //Debug.Log("Add in pointer Data -");


        if (Input.touchCount == 2 || isZoomRunning)
        {
            return;
        }
        //if (delayToRun < 1)
        //{
        //    //Debug.Log("Return From delay to run");
        //    return;
        //}

        if (!isBegan) return;

        //Debug.Log("Panning");

        isTouch = true;


        //Debug.Log("current Position - " + eventData.currentPosition1 + " touch - " + )

        Vector3 curpos = new Vector3(eventData.currentPosition1.x, 0, eventData.currentPosition1.y);
        Vector3 pos = Quaternion.Euler(cameraRotation) * curpos;

        Vector3 curInitialpos = new Vector3(eventData.initialPosition1.x, 0, eventData.initialPosition1.y);
        Vector3 posInitial = Quaternion.Euler(cameraRotation) * curInitialpos;


        tempRange.y = ((pos.z - posInitial.z) / (Screen.height * 5)) * speed;
        tempRange.x = ((pos.x - posInitial.x) / (Screen.height * 5)) * speed;

        range = initRange - tempRange;
        range.y = Mathf.Clamp(range.y, cameraVerticalRange.x, cameraVerticalRange.y);
        range.x = Mathf.Clamp(range.x, cameraHorizontalRange.x, cameraHorizontalRange.y);
    }

    void OnZoomStart(PinchZoom.PinchZoomData pinchZoomData)
    {
        //Debug.Log("On Zoom Start");
        if (StopZooming || IsPointerOverUIObject())
            return;

        //delayToRun = 0f;
        isZoomRunning = true;
        //tempZoom = 0;
        zoomValue = Mathf.Clamp(zoomValue, zoomMaxMinLimit.x, zoomMaxMinLimit.y);
        //initialZoom = zoomValue;
        previousDistance = Vector2.Distance(pinchZoomData.currentPosition1, pinchZoomData.currentPosition2);

        panZoom.enabled = false;

    }

    [SerializeField]
    bool isZoomRunning = false;
    [SerializeField]
    bool isClickZoomRunning = false;

    void OnZoomEnd(PinchZoom.PinchZoomData pinchZoomData)
    {
        //Debug.Log("On Zoom End");

        isZoomRunning = false;
        //delayToRun = 1f;
        //zoomValue = initialZoom + tempZoom;
        zoomValue = Mathf.Clamp(zoomValue, zoomMaxMinLimit.x, zoomMaxMinLimit.y);

        panZoom.enabled = true;

        //panZoom.PassAfterPinch(panZoomData.pointerEventData);

        //tempZoom = 0;
        //initialZoom = 0;
    }

    //float initialZoom, tempZoom;
    public float zoomValue, zoomLerp, previousDistance;


    public bool TutorialMask;
    void OnZoom(PinchZoom.PinchZoomData pinchZoomData)
    {

        //Debug.Log("On Zoom");

        //delayToRun = 0f;

        if (StopZooming || IsPointerOverUIObject())
            return;

        isZoomRunning = true;
        //float previousDistance = Vector2.Distance(pinchZoomData.initialPosition1, pinchZoomData.initialPosition2);
        float distanceCurrent = Vector2.Distance(pinchZoomData.currentPosition1, pinchZoomData.currentPosition2);

        if (Input.touchCount == 2)
        {
            var tempZoom = (((previousDistance - distanceCurrent) / Screen.height) * 100);
            ///zoomValue = initialZoom + tempZoom;
            zoomValue += tempZoom;
            //Debug.Log(zoomValue);
            zoomValue = Mathf.Clamp(zoomValue, zoomMaxMinLimit.x, zoomMaxMinLimit.y);
            previousDistance = distanceCurrent;
        }
    }

    //[SerializeField]
    //float delayToRun;


    [SerializeField]
    float desiredDuration = 3;
    float elapsedTime;
    Vector3 velocity;
    public float ZoomSpeed;
    public bool StopZooming;
    public bool StopPaning;
    public void Update()
    {

        //Debug.Log("Touch Up - " + Input.touchCount);


        //if (delayToRun < 1)
        //{
        //    delayToRun += Time.deltaTime;
        //}
        //else
        //{
        //    isZoomRunning = false;
        //}






        if (continueFocusTarget)
        {
            Focus(continueFocusTarget.position);
        }

        //if (!isTouch)
        //    return;

        UpdateCameraRangeArea();
        lerp = Vector2.Lerp(lerp, range, Time.deltaTime * 5);
        Vector3 pos = cameraVerticalTransform.localPosition;
        Vector3 pos_0 = pos;
        pos.z = Mathf.Clamp(lerp.y, cameraVerticalRange.x, cameraVerticalRange.y);
        pos.x = Mathf.Clamp(lerp.x, cameraHorizontalRange.x, cameraHorizontalRange.y);
        //cameraVerticalTransform.localPosition = pos;

        elapsedTime += Time.deltaTime;
        float percent = elapsedTime / desiredDuration;
        //Debug.Log(percent);
        //Debug.Log("Percent - " + percent);
        //cameraVerticalTransform.localPosition = Vector3.Lerp(pos_0, pos, percent) * Quaternion.Euler(new Vector3(30, -45, 0));
        //cameraVerticalTransform.localPosition = Vector3.SmoothDamp(cameraVerticalTransform.localPosition, pos, ref velocity, 0.3f);
        cameraVerticalTransform.localPosition = Vector3.Lerp(pos_0, pos, percent);

        //cameraVerticalTransform.localPosition = Vector3.Lerp(pos_0, pos, Time.smoothDeltaTime * lerpSpeed);

#if UNITY_EDITOR
        //if (Tutorial.instance.planeMoveCollider.enabled)
        zoomValue += Input.GetAxis("Mouse ScrollWheel");
        //Debug.Log("Zoom Value : " + zoomValue);

#endif
        //if (isZoomRunning)
        //{
        zoomValue = Mathf.Clamp(zoomValue, zoomMaxMinLimit.x, zoomMaxMinLimit.y);
        zoomLerp = Mathf.Lerp(zoomLerp, zoomValue, Time.deltaTime * ZoomSpeed);
        camera.orthographicSize = zoomLerp;

        speed = Mathf.Clamp((zoomValue * maxSpeed) / zoomMaxMinLimit.y, 300f, maxSpeed);
        // Debug.Log("The Camera Speed =" + speed);
        //Debug.Log("Camera Move Speed - " + Mathf.Clamp((zoomValue * maxSpeed) / zoomMaxMinLimit.y, 114f, maxSpeed));

        //}
        //else if (isClickZoomRunning)
        //{
        //    zoomValue = Mathf.Clamp(zoomValue, zoomMaxMinLimit.x, zoomMaxMinLimit.y);
        //    zoomLerp = Mathf.Lerp(zoomLerp, zoomValue, Time.deltaTime * 5);
        //    camera.fieldOfView = zoomLerp;
        //}
        //else
        //{
        //    zoomLerp = zoomValue;
        //    camera.fieldOfView = zoomValue;
        //}



    }


    public void OnOpenUI(Vector3 lastPos)
    {
        //Debug.Log("Open ui");
        range.x = lastPos.x;
        range.y = lastPos.z;
        zoomValue = 20;//zoomMaxMinLimit.x;//10;
    }

    public void OnCloseUI()
    {
        //    if(Tutorial.instance.tutorialIndex == 7)
        //    {
        //        zoomValue = 25;
        //        return;
        //    }
        zoomValue = zoomMaxMinLimit.y;//25;
    }

    [SerializeField] Vector3 Min;
    [SerializeField] Vector3 Max;


    private void OnValidate()
    {
        UpdateCameraRangeArea();
        Min = worldMinBottomLeft;
        Max = worldMaxTopRight;
    }

    Vector3 initialPos;

    [SerializeField] Vector3 worldMinBottomLeft => new Vector3(cameraHorizontalRange.y + initialPos.x, transform.position.y, cameraVerticalRange.y + initialPos.z);
    [SerializeField] Vector3 worldMaxTopRight => new Vector3(cameraHorizontalRange.x + initialPos.x, transform.position.y, cameraVerticalRange.x + initialPos.z);

    void OnDrawGizmos()
    {
        UpdateCameraRangeArea();
        Gizmos.color = Color.red;

        Gizmos.DrawLine(worldMinBottomLeft, new Vector3(worldMaxTopRight.x, worldMinBottomLeft.y, worldMinBottomLeft.z));
        Gizmos.DrawLine(new Vector3(worldMaxTopRight.x, worldMinBottomLeft.y, worldMinBottomLeft.z), worldMaxTopRight);
        Gizmos.DrawLine(worldMaxTopRight, new Vector3(worldMinBottomLeft.x, worldMaxTopRight.y, worldMaxTopRight.z));
        Gizmos.DrawLine(new Vector3(worldMinBottomLeft.x, worldMaxTopRight.y, worldMaxTopRight.z), worldMinBottomLeft);

    }
    public List<RaycastResult> results;
    public bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        /* List<RaycastResult>*/
        results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        results.RemoveAll(r => r.gameObject.layer == 4);

        return results.Count > 0;
    }

}