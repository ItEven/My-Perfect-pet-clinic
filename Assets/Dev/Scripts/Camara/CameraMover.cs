using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;

[DefaultExecutionOrder(-1)]
public class CameraMover : MonoBehaviour
{
    private static CameraMover cameraMover;
    public static CameraMover Instance => cameraMover;


    public Camera mainCamera;
    public SpriteRenderer level;

    public AnimationCurve dragSpeed;

    public bool isDragging = false;
    private Vector3 lastMousePosition;
    bool canCheckForClamp;
    private bool isCameraClamped = false;

    public float pinchZoomSpeed = 0.5f;
    public float mouseScrollSpeed = 0.5f;
    public float minFOV = 10f;
    public float maxFOV = 60f;
    [SerializeField] bool zoomedRecently;
    Coroutine zoomReset;
    //PointerOverUIChecker pointerOverUIChecker;
    public Transform cameraTarget;
    [SerializeField] CinemachineVirtualCamera cinemachineVirtualCamera;
    public float moveDelta, moveDeltaTapDetectThreshold;
    public bool canMove;
    float previousZoom;

    private void Awake()
    {
        if (cameraMover == null)
        {
            cameraMover = this;
        }
        mainCamera = Camera.main;
    }

    private void Start()
    {
        //pointerOverUIChecker = PointerOverUIChecker.Instance;
        previousZoom = maxFOV;
        mainCamera.orthographicSize = maxFOV;
    }

    private void Update()
    {
        if (level == null || canMove == false)
        {
            return;
        }

        ManageZoom();
        Move();
    }

    private void LateUpdate()
    {
        if (level != null)
        {
            ClampCameraWithinBounds();
        }

        if (!Input.GetMouseButton(0) && moveDelta > 0)
        {
            moveDelta = Mathf.Clamp(moveDelta - Time.deltaTime, 0, 0.2f);
        }
    }

    #region Move
    void Move()
    {
        if (zoomedRecently == true)
        {
            isDragging = false;
            if (Input.touchCount == 0)
            {
                if (zoomReset == null)
                {
                    zoomReset = StartCoroutine(ResetRecentZoom());
                }
            }
            moveDelta = 0.2f;
            return;
        }
        if (Input.touchCount >= 2)
        {
            moveDelta = Mathf.Clamp(moveDelta + Time.deltaTime, 0, 0.2f);
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            //if (pointerOverUIChecker.IsPointerOverUIElement() == false)
            //{s
            StartDragging();
            canCheckForClamp = true;
            //}
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopDragging();
            canCheckForClamp = false;
        }

        if (isDragging == true)
        {
            ContinueDragging();
        }
    }

    IEnumerator ResetRecentZoom()
    {
        yield return new WaitForSeconds(0.02f);
        zoomedRecently = false;
        zoomReset = null;

    }

    void StartDragging()
    {
        isDragging = true;
        lastMousePosition = Input.mousePosition;
    }

    void ContinueDragging()
    {
        Vector3 delta = Input.mousePosition - lastMousePosition;
        delta = -new Vector3(delta.x, 0, delta.y);
        delta = Quaternion.Euler(0, mainCamera.transform.eulerAngles.y, 0) * delta;
        delta = delta * dragSpeed.Evaluate(mainCamera.orthographicSize) * 3.65f * Time.deltaTime;
        cameraTarget.position += new Vector3(delta.x, 0, delta.z);
        lastMousePosition = Input.mousePosition;
        moveDelta = Mathf.Clamp(moveDelta + Time.deltaTime, 0, 0.2f);
    }

    void StopDragging()
    {
        isDragging = false;
    }

    #endregion

    #region Zoom

    void ManageZoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        //if (pointerOverUIChecker.IsPointerOverUIElement() == false)
        //{
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            Vector2 touch0PrevPos = touch0.position - touch0.deltaPosition;
            Vector2 touch1PrevPos = touch1.position - touch1.deltaPosition;

            float prevTouchDeltaMag = (touch0PrevPos - touch1PrevPos).magnitude;
            float touchDeltaMag = (touch0.position - touch1.position).magnitude;

            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            ZoomCamera(deltaMagnitudeDiff * pinchZoomSpeed * Time.deltaTime);
        }
        else if (scrollInput != 0)
        {
            ZoomCamera(-scrollInput * mouseScrollSpeed * Time.deltaTime);
        }
        //}
    }

    private void ZoomCamera(float deltaFOV)
    {
        zoomedRecently = true;
        //  mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize + deltaFOV, minFOV, maxFOV);
        float zoom = Mathf.Clamp(mainCamera.orthographicSize + deltaFOV, minFOV, maxFOV);
        cinemachineVirtualCamera.m_Lens.OrthographicSize = zoom;
        previousZoom = zoom;
    }
    #endregion

    #region Clamp
    private void ClampCameraWithinBounds()
    {
        // Get the bounds of the sprite
        Bounds spriteBounds = level.bounds;

        // Get the camera's position
        Vector3 cameraPosition = cameraTarget.position;

        // Calculate the camera's allowed range within the sprite bounds
        float minX = spriteBounds.min.x + mainCamera.orthographicSize * Screen.width / Screen.height;
        float maxX = spriteBounds.max.x - mainCamera.orthographicSize * Screen.width / Screen.height;
        float minY = spriteBounds.min.z + mainCamera.orthographicSize;
        float maxY = spriteBounds.max.z - mainCamera.orthographicSize;

        // Clamp the camera's position within the calculated range
        float clampedX = Mathf.Clamp(cameraPosition.x, minX, maxX);
        float clampedY = Mathf.Clamp(cameraPosition.z, minY, maxY);


        isCameraClamped = (clampedX != cameraPosition.x || clampedY != cameraPosition.z);

        if (canCheckForClamp == true)
        {
            if (isCameraClamped)
            {
                canCheckForClamp = false;
            }
        }

        cameraTarget.position = new Vector3(clampedX, cameraPosition.y, clampedY);
    }
    #endregion

    public bool ChcekNeedOpneUI()
    {
        if (moveDeltaTapDetectThreshold < moveDelta)
        {
            return false;
        }
        return true;
    }
    public void ChangeCameraPos(Vector3 pos)
    {


        cameraTarget.DOMove(pos, .5f).OnComplete(() =>
        {

            isDragging = false;
        });

    }

    public void ChangeToPreviousZoom()
    {

        ChangeZoom(previousZoom);
    }

    public void ChangeZoom(float toValue)
    {
        DOTween.To(() => cinemachineVirtualCamera.m_Lens.OrthographicSize, x => cinemachineVirtualCamera.m_Lens.OrthographicSize = x, toValue, 0.25f);
    }

}
