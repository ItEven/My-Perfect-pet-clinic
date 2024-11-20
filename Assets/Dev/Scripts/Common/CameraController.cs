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

    public float followDelay = 300f;
    public float followDurtion = 4f;


    bool bCanCameraMove = true;
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
        bCanCameraMove = false;
        playerController.playerControllerData.characterMovement.enabled = false;
        playerController.enabled = false;
        playerController.playerControllerData.joystick.gameObject.SetActive(false);
        playerController.animationController.PlayAnimation(AnimType.Idle);
        transform.SetParent(target);
        DOVirtual.DelayedCall(1f, () =>
        {
            transform.DOMove(Vector3.zero, .5f).OnComplete(() =>
            {
                onComplete?.Invoke();
            });
        });
    }

    public void MoveToPlayer()
    {
        transform.SetParent(playerController.transform);
        transform.DOMove(playerController.playerControllerData.characterMovement.rotatingObj.position, .5f).OnComplete(() =>
        {
            StartCoroutine(ManageCameraTrems());
            playerController.playerControllerData.joystick.gameObject.SetActive(true);
            playerController.playerControllerData.joystick.OnPointerUp(null);
            playerController.playerControllerData.characterMovement.enabled = true;
            playerController.animationController.PlayAnimation(AnimType.Idle);
        });
    }

    public void FocusOnTarget(Transform upGrader)
    {
        StartCoroutine(MoveToUpgrade(upGrader));
    }

    IEnumerator MoveToUpgrade(Transform upGrader)
    {
        yield return new WaitUntil(() => !bIsMoveingToPatient);
        StopCoroutine(ManageCameraTrems());
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

    public void FollowPatient(Transform target)
    {
        bIsMoveingToPatient = true;
        MoveToTarget(target, () =>
        {
            DOVirtual.DelayedCall(followDurtion, () => { MoveToPlayer(); });
        });

    }
    IEnumerator ManageCameraTrems()
    {
        while (true)
        {
            yield return new WaitForSeconds(followDelay);
            bCanCameraMove = true;
        }
    }

    #endregion
}
