using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using DG.Tweening;
using System;
using static UnityEditor.PlayerSettings;

public class CameraController : MonoBehaviour
{
    private static CameraController cameraController;
    public static CameraController Instance => cameraController;


    private void Awake()
    {
        if (cameraController == null)
        {
            cameraController = this;
        }
    }
    public void MoveToTarget(Transform target, Action onComplete = null)
    {
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
                DOVirtual.DelayedCall(0.5f, () => { transform.DOMove(transform.parent.position, .5f); });

            });
        });
    }
}
