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

    //  internal WaitingQueue waitingQueue;
    internal ARoom currnetRoom;
    internal Bed currnetBed;

    [Header("Watting Duration")]
    public float wattingTime = 10f;
    public float SloganDuration = 100f;
    public float sloganVisibalTime = 10f;
    public float waitStandTime = 2f;

    [Header("Exclamation mark")]
    public Transform markForLock;
    public Transform markForFull;

    [Header("Emojie Controller")]
    public EmojisController emojisController;

    [Header("TextBox")]
    public RectTransform sloganTextBox;
    public TextMeshProUGUI sloganText;


    private void Start()
    {
        sloganTextBox.gameObject.SetActive(false);
    }


    [Button("MoveAnimal")]

    public void MoveAnimal()
    {
        animal.player = RightFollowPos;
        animal.startFollow();
    }
    public void MoveToExit(Transform ExitPoint, MoodType moodType)
    {
        StopSlogan();
        sloganTextBox.gameObject.SetActive(false);

        emojisController.PlayEmoji(moodType);
        if (moodType == MoodType.Happy)
        {
            NPCMovement.walkingAnimType = AnimType.Happy_Walk;
            animal.walkAnim = AnimType.Happy_Walk;
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
        StopWatting();
        DOVirtual.DelayedCall(index, () =>
        {
            NPCMovement.MoveToTarget(traget, () =>
            {
                StartWatting();
            });
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
        if (TutorialManager.instance.bIsTutorialRunning) return;
        if (string.IsNullOrEmpty(WattingTweenId))
        {
            WattingTweenId = "WattingTween" + Guid.NewGuid().ToString();
        }
        StopWatting();
        StartPlayingSalogan();
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
    protected string SaloganTweenId;
    public void StartPlayingSalogan()
    {
        int index = (int)Random.Range(SloganDuration - 50, SloganDuration);
        if (string.IsNullOrEmpty(SaloganTweenId))
        {
            SaloganTweenId = "SaloganTween_" + Guid.NewGuid().ToString();
        }
        StopSlogan();
        DOVirtual.DelayedCall(index, () =>
        {
            if (animal != null)
            {
                sloganTextBox.gameObject.SetActive(true);
                DOVirtual.DelayedCall(sloganVisibalTime, () =>
                {
                    sloganTextBox.gameObject.SetActive(false);
                });
            }
        }).SetId(SaloganTweenId).SetLoops(-1, LoopType.Restart);
    }

    public void StopSlogan()
    {
        DOTween.Kill(SaloganTweenId);

    }

    public void MoveFromQ()
    {
        MoveToExit(SaveManager.instance.hospitalManager.GetRandomExit(this), MoodType.Angry);
        MoveAnimal();
        if (currnetRoom != null)
        {
            currnetRoom.waitingQueue.RemoveFromQueue(this);
            currnetRoom.RemovePatientFromUnRegisterQ(this);
            currnetRoom.RearngeQue();
            if(currnetBed != null )
            {
                currnetBed.bIsOccupied = false;
            }
        }
    }
    public void StopWatting()
    {
        StopSlogan();
        DOTween.Kill(WattingTweenId);
    }

    public void MarkOff()
    {
        StopSlogan();
        StopWatting();
        sloganTextBox.gameObject.SetActive(false);
        markForFull.gameObject.SetActive(false);
        markForLock.gameObject.SetActive(false);
    }
}
