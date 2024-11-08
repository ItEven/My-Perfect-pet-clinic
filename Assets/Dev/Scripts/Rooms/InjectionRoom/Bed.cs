using DG.Tweening;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class Bed : MonoBehaviour
{
    [Header("Bed Dependencies")]
    public bool bIsUnlock;
    public bool bIsUpgraderActive;
    internal bool bIsPlayerOnDesk;
    public bool bIsOccupied;
    public InjectionRoom room;
    public Upgrader upGrader;
    public OnTrigger onTrigger;

    [Header("Tranfroms")]
    public Transform petDignosPos;
    public Seat petOwnerSeat;

    [Header("NPC Details")]
    public StaffNPC staffNPC;


    [Header("Item Details")]
    public ItemsTyps itemsTyps;
    public Transform ItemGiver;


    [Header("Visuals")]
    public Image worldProgresBar;
    public GameObject unlockObjs;
    public SpriteRenderer groundCanvas;
    public ParticleSystem roundUpgradePartical;

    Seat seat;
    Patient patient;

    #region Initializers
    internal SaveManager saveManager;
    internal GameManager gameManager;
    internal PlayerController playerController;
    protected ArrowController arrowController;
    internal HospitalManager hospitalManager;


    private void OnEnable()
    {
        UpdateInitializers();
    }

    public void UpdateInitializers()
    {
        saveManager = SaveManager.instance;
        gameManager = saveManager.gameManager;
        playerController = gameManager.playerController;
        arrowController = playerController.arrowController;
        hospitalManager = saveManager.hospitalManager;
    }
    #endregion

    #region Starters
    private void Start()
    {
        seat = onTrigger.seat;
        LoadData();
    }

    private void LoadData()
    {
        UpdateInitializers();
        SetVisual();
    }

    private void SetVisual()
    {
        if (bIsUnlock)
        {
            gameManager.DropObj(unlockObjs);
            gameManager.PlayParticles(roundUpgradePartical);
            Destroy(upGrader.gameObject);
            LoadNpcData();
        }
    }

    public void LoadNpcData() 
    {
        staffNPC.SetMainSeat(onTrigger.seat);
    }


    #endregion

    #region Proses Mechanics
    public void OnPlayerTrigger()
    {
        bIsPlayerOnDesk = true;
        if (!staffNPC.bIsUnlock)
        {
            SetUpPlayer();
        }
    }

    public void OnPlayerExit()
    {
        bIsPlayerOnDesk = false;
        if (!staffNPC.bIsUnlock)
        {
            // StopProsses();
        }
    }

    public void SetUpPlayer()
    {
        StartProcessPatients();
        playerController.playerControllerData.characterMovement.enabled = false;
        playerController.enabled = false;
        playerController.playerControllerData.joystick.gameObject.SetActive(false);

        playerController.transform.position = seat.transform.position;
        playerController.playerControllerData.characterMovement.rotatingObj.rotation = seat.transform.rotation;


        DOVirtual.DelayedCall(1f, () =>
        {
            playerController.transform.SetParent(null);
            playerController.playerControllerData.joystick.gameObject.SetActive(true);
            playerController.playerControllerData.joystick.OnPointerUp(null);
            playerController.playerControllerData.characterMovement.enabled = true;
        });

    }

    public bool bCanDoProcess()
    {
        return playerController.bHaveItems;
    }
    public void StartProcessPatients()
    {

        if (bCanDoProcess())
        {
            var pharmacyRoom = hospitalManager.pharmacyRoom;
            var workingAnimation = seat.workingAnim;
            var processTime = staffNPC.currentLevelData.processTime;
            StartPatientProcessing(playerController.animationController, workingAnimation, AnimType.Idle, processTime, () =>
            {
                room.moneyBox.TakeMoney(hospitalManager.GetCustomerCost(patient, room.diseaseData, staffNPC.currentLevelData.StaffExprinceType));
                worldProgresBar.fillAmount = 0;
                if (room.bIsUnRegisterQueIsFull())
                {
                    playerController.animationController.PlayAnimation(seat.idleAnim);
                    patient.NPCMovement.MoveToTarget(hospitalManager.GetRandomExit(), () =>
                    {
                        Destroy(patient.gameObject);
                    });
                    patient.animal.SetPartical(hospitalManager.GetAnimalMood());
                }
                else
                {
                    room.RegisterPatient(patient);
                }
                patient.MoveAnimal();

            });
        }
        else
        {
            if (!arrowController.gameObject.activeInHierarchy && bIsOccupied) arrowController.gameObject.SetActive(true);
            arrowController.SetTarget(ItemGiver.transform, 1f);

        }

    }

    private string processTweenId;

    private void StartPatientProcessing(AnimationController animationController, AnimType workingAnim, AnimType idleAnim, float processTime, Action onComplete)
    {

        if (string.IsNullOrEmpty(processTweenId))
        {
            processTweenId = "ProcessTween_" + Guid.NewGuid().ToString();
        }

        animationController.PlayAnimation(workingAnim);
        worldProgresBar.fillAmount = 0;

        worldProgresBar.DOFillAmount(1, processTime).SetId(processTweenId)
            .OnComplete(() =>
            {
                animationController.PlayAnimation(idleAnim);
                onComplete?.Invoke();
            });
    }


    public void GetItemForProcess()
    {
        if (!playerController.bHaveItems)
        {
            playerController.SetItemState(itemsTyps, true);
        }

    }
    public void DropItem()
    {
        if (playerController.bHaveItems)
        {
            playerController.SetItemState(itemsTyps, false);
        }
    }
    #endregion
}
