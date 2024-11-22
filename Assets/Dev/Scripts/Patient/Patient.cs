using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;



public class Patient : MonoBehaviour
{
    internal bool bIsWating;
    public GameObject patientMeshObj;
    public Transform RightFollowPos;
    public Transform leftFollowPos;
    public NPCMovement NPCMovement;
    public Animal animal;
    public DiseaseType diseaseType;
    internal RegisterPos registerPos;

    internal WaitingQueue waitingQueue;
    internal ARoom currnetRoom;

    [Header("Watting Duration")]
    public float wattingTime = 10f;
    public float waitStandTime = 2f;

    [Header("Exclamation mark")]
    public Transform markForLock;
    public Transform markForFull;

    [Header("Emojie Controller")]
    public EmojisController emojisController;

    [Header("TextBox")]
    public RectTransform sloganTextBox;
    public TextMeshProUGUI sloganText;

    [Button("MoveAnimal")]
    public void MoveAnimal()
    {
        animal.player = RightFollowPos;
        animal.startFollow();
    }
    public void MoveToExit(Transform ExitPoint, MoodType moodType)
    {
        emojisController.PlayEmoji(moodType);
        if(moodType == MoodType.Happy)
        {
            NPCMovement.walkingAnimType = AnimType.Happy_Walk;
            if (CameraController.Instance.bCanCameraMove)
            {
                CameraController.Instance.FollowPatient(transform);
            }
        }
        NPCMovement.MoveToTarget(ExitPoint, () =>
        {
            Destroy(animal.gameObject);
            Destroy(gameObject);
        });
    }

    protected string processTweenId;

    public void MoveNewShufflePos(Transform traget)
    {
        if (string.IsNullOrEmpty(processTweenId))
        {
            processTweenId = "ProcessTween_" + Guid.NewGuid().ToString();
        }

        int index = Random.Range(0, 5);
        DOVirtual.DelayedCall(index, () =>
        {

            NPCMovement.MoveToTarget(traget, null);
            MoveAnimal();
        }).SetId(processTweenId);

    }

    public void BrakeDally()
    {
        DOTween.Kill(processTweenId);
    }

    protected string WattingTweenId;

    public void StartWatting(Action onCompliet = null)
    {
        if (string.IsNullOrEmpty(WattingTweenId))
        {
            WattingTweenId = "WattingTween" + Guid.NewGuid().ToString();
        }
        StopWatting();
        int index = (int)Random.Range(wattingTime - 50, wattingTime);
        DOVirtual.DelayedCall(index, () =>
        {
            onCompliet?.Invoke();
            NPCMovement.animator.PlayAnimation(AnimType.Angry);
            NPCMovement.walkingAnimType = AnimType.Angry_Walk;
            if (CameraController.Instance.bCanCameraMove)
            {
                CameraController.Instance.FollowPatient(transform, () =>
                {
                    DOVirtual.DelayedCall(waitStandTime, () =>
                    {
                        MoveFromQ();
                    });
                });
            }
            else
            {
                MoveFromQ();
            }

        }).SetId(WattingTweenId);
    }

    public void MoveFromQ()
    {

        RemoveFromWattingQ();
        RemoveFromUnregisterd();
        MoveToExit(SaveManager.instance.hospitalManager.GetRandomExit(this), MoodType.Angry);
        MoveAnimal();

    }
    public void StopWatting()
    {
        DOTween.Kill(WattingTweenId);
    }

    public void RemoveFromUnregisterd()
    {
        if (currnetRoom != null)
        {
            currnetRoom.RemovePatientFromUnRegisterQ(this);
        }
        else
        {
            Debug.LogError("Room is Null");
        }
    }

    public void RemoveFromWattingQ()
    {
        if (waitingQueue != null)
        {
            waitingQueue.RemoveFromQueue(this);
        }
        else
        {
            Debug.LogError("WattingQ is Null");
        }
    }
}
