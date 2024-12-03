using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using System;
using DG.Tweening.Core.Easing;
using Unity.VisualScripting;
using static UnityEngine.AdaptivePerformance.Provider.AdaptivePerformanceSubsystemDescriptor;

public class CameraController : MonoBehaviour
{
    private static CameraController cameraController;
    public static CameraController Instance => cameraController;


    public float followDelay = 200f;
    public float followDurtion = 7f;
    public float rectionTime = 1f;
    public bool bCanCameraMove = true;
    bool bIsMoveingToPatient = false;

    [Header("Zoom Things")]
    public Camera mainCamera;
    [SerializeField] bool zoomedRecently;
    [SerializeField] CinemachineVirtualCamera cinemachineVirtualCamera;
    public Transform joystic;
    float previousZoom;

    public float pinchZoomSpeed = 0.5f;
    public float mouseScrollSpeed = 0.5f;
    public float minFOV = 10f;
    public float maxFOV = 60f;

    PlayerController playerController;
    private void Awake()
    {
        if (cameraController == null)
        {
            cameraController = this;
        }
    }

    private void Start()
    {
        playerController = gameObject.GetComponentInParent<PlayerController>();
        ManageCamera();
    }

    private void Update()
    {
        // ManageZoom();
    }


    #region Zoom

    void ManageZoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");

        //if (pointerOverUIChecker.IsPointerOverUIElement() == false)
        //{
        if (Input.touchCount == 2)
        {

            joystic.gameObject.SetActive(false);
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
        else
        {
            joystic.gameObject.SetActive(true);

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

    #region ForUpgr
    public void MoveToTarget(Transform target, Action onComplete = null, float delay = 1f)
    {

        bCanCameraMove = false;
        playerController.playerControllerData.characterMovement.enabled = false;
        playerController.enabled = false;
        playerController.playerControllerData.joystick.gameObject.SetActive(false);
        if (playerController.animationController.GetCurrntAnimState() == AnimType.Walk.ToString()) 
        {
            playerController.animationController.PlayAnimation(AnimType.Idle);
        }
        DOVirtual.DelayedCall(delay, () =>
        {
            transform.DOMove(target.position, 1f).OnComplete(() =>
            {
                onComplete?.Invoke();
            });
        });
    }
    public void MoveToTargetPatient(Transform target, Action onComplete = null)
    {

        bCanCameraMove = false;
        playerController.playerControllerData.characterMovement.enabled = false;
        playerController.enabled = false;
        playerController.playerControllerData.joystick.gameObject.SetActive(false);
        if (playerController.animationController.GetCurrntAnimState() == AnimType.Walk.ToString())
        {
            playerController.animationController.PlayAnimation(AnimType.Idle);
        }
        transform.SetParent(target);
        transform.DOMove(target.position, 0.5f).OnComplete(() =>
        {
            onComplete?.Invoke();
        });

    }

    public void MoveToPlayer()
    {

        transform.SetParent(playerController.transform);
        transform.DOMove(playerController.playerControllerData.characterMovement.rotatingObj.position, .5f).OnComplete(() =>
        {

            playerController.playerControllerData.joystick.gameObject.SetActive(true);
            playerController.playerControllerData.joystick.OnPointerUp(null);
            playerController.playerControllerData.characterMovement.enabled = true;
            if (playerController.animationController.GetCurrntAnimState() == AnimType.Walk.ToString())
            {
                playerController.animationController.PlayAnimation(AnimType.Idle);
            }
        });
    }

    public void FocusOnTarget(Transform upGrader)
    {
        StartCoroutine(MoveToUpgrade(upGrader));
    }

    IEnumerator MoveToUpgrade(Transform upGrader)
    {
        yield return new WaitUntil(() => !bIsMoveingToPatient);
        upGrader.transform.localScale = Vector3.zero;
        MoveToTarget(upGrader.transform, () =>
        {
            upGrader.gameObject.SetActive(true);
            Upgrader upgrader = upGrader.gameObject.GetComponent<Upgrader>();
            if (upgrader != null)
            {
                upgrader.StopTakeMoney();
            }
            upGrader.transform.DOScale(Vector3.one, 0.5f).OnComplete(() =>
            {
                DOVirtual.DelayedCall(0.5f, () =>
                {
                    MoveToPlayer();
                });

            });
        });
        yield return null;
    }

    public void FollowPatient(Transform target, Action onComplite = null)
    {
        stopManageCamera();
        bIsMoveingToPatient = true;
        MoveToTargetPatient(target, () =>
        {
            onComplite?.Invoke();

            DOVirtual.DelayedCall(followDurtion, () => { bIsMoveingToPatient = false; MoveToPlayer(); ManageCamera(); ; });
        });

    }

    protected string processTweenId;
    public void ManageCamera()
    {
        if (string.IsNullOrEmpty(processTweenId))
        {
            processTweenId = "ProcessTween_" + Guid.NewGuid().ToString();
        }
        bCanCameraMove = false;

        DOVirtual.DelayedCall(followDelay, () =>
        {

            bCanCameraMove = true;
        }).SetId(processTweenId).SetLoops(-1, LoopType.Yoyo);
    }

    public void stopManageCamera()
    {
        if (processTweenId != null)
        {
            DOTween.Kill(processTweenId);
        }
    }

    #endregion
}
