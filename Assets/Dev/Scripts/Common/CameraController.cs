using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using System;
using DG.Tweening.Core.Easing;
using Unity.VisualScripting;

public class CameraController : MonoBehaviour
{
    private static CameraController cameraController;
    public static CameraController Instance => cameraController;

    public float followDelay = 200f;
    public float followDurtion = 7f;
    public float rectionTime = 1f;


    public bool bCanCameraMove = true;
    bool bIsMoveingToPatient = false;

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
    }

    #region ForUpgr
    public void MoveToTarget(Transform target, Action onComplete = null)
    {
        stopManageCamera();
        bCanCameraMove = false;
        playerController.playerControllerData.characterMovement.enabled = false;
        playerController.enabled = false;
        playerController.playerControllerData.joystick.gameObject.SetActive(false);
        playerController.animationController.PlayAnimation(AnimType.Idle);
        DOVirtual.DelayedCall(1f, () =>
        {
            transform.DOMove(target.position, 1f).OnComplete(() =>
            {
                onComplete?.Invoke();
            });
        });
    }
    public void MoveToTargetPatient(Transform target, Action onComplete = null)
    {
        stopManageCamera();
        bCanCameraMove = false;
        playerController.playerControllerData.characterMovement.enabled = false;
        playerController.enabled = false;
        playerController.playerControllerData.joystick.gameObject.SetActive(false);
        playerController.animationController.PlayAnimation(AnimType.Idle);
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
            playerController.animationController.PlayAnimation(AnimType.Idle);
            ManageCamera();
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
            DOVirtual.DelayedCall(followDurtion, () => { bIsMoveingToPatient = false; MoveToPlayer(); });
        });

    }
    //public void MoveToRecption(Transform target)
    //{
    //    if (bCanCameraMove)
    //    {

    //        bCanCameraMove = false;
    //        bIsMoveingToPatient = true;
    //        MoveToTarget(target, () =>
    //        {
    //            DOVirtual.DelayedCall(rectionTime, () => { bIsMoveingToPatient = false; MoveToPlayer(); });
    //        });
    //    }
    //}
    protected string processTweenId;
    public void ManageCamera()
    {
        if (string.IsNullOrEmpty(processTweenId))
        {
            processTweenId = "ProcessTween_" + Guid.NewGuid().ToString();
        }

        DOVirtual.DelayedCall(followDelay, () =>
        {

            bCanCameraMove = true;
        }).SetId(processTweenId);
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
