using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using System;
using DG.Tweening.Core.Easing;

public class CameraController : MonoBehaviour
{
    private static CameraController cameraController;
    public static CameraController Instance => cameraController;

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
    public void MoveToTarget(Transform target, Action onComplete = null)
    {
        playerController.playerControllerData.characterMovement.enabled = false;
        playerController.enabled = false;
        playerController.playerControllerData.joystick.gameObject.SetActive(false);
        playerController.animationController.PlayAnimation(AnimType.Idle);
        Vector3 pos = target.position;
        DOVirtual.DelayedCall(1f, () =>
        {
            transform.DOMove(pos, .5f).OnComplete(() =>
            {
                onComplete?.Invoke();
            });
        });


    }

    public void FocusOnTarget(Transform upGrader)
    {
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
                    transform.DOMove(transform.parent.position, .5f).OnComplete(() =>
                {
                    playerController.playerControllerData.joystick.gameObject.SetActive(true);
                    playerController.playerControllerData.joystick.OnPointerUp(null);
                    playerController.playerControllerData.characterMovement.enabled = true;
                    playerController.animationController.PlayAnimation(AnimType.Idle);


                });
                });

            });
        });
    }
}
