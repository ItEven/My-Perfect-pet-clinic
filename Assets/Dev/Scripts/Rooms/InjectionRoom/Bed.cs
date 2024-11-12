using DG.Tweening;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class Bed : MonoBehaviour
{
    [Header("Task Details")]
    public int currentTask;

    [Header("Inspection Room Details")]
    public int unlockPrice;
    internal int currentCost
    {
        get { return upGrader.needMoney; }
        set { upGrader.needMoney = value; }
    }

    [Header("Bed Dependencies")]
    public bool bIsUnlock;
    public bool bIsUpgraderActive;
    public bool bIsOccupied;
    internal bool bIsPlayerOnDesk;

    public ARoom room;
    public Upgrader upGrader;
    public OnTrigger onTrigger;

    [Header("Tranfroms")]
    public Seat petDignosPos;
    public Seat petOwnerSeat;

    [Header("NPC Details")]
    public StaffNPC staffNPC;


    [Header("Item Details")]
    public ItemsTyps itemsTyps;
    public Transform ItemGiver;


    [Header("Visuals")]
    public Image worldProgresBar;
    public GameObject[] unlockObjs;
    public SpriteRenderer groundCanvas;
    public ParticleSystem roundUpgradePartical;

    protected Seat seat;
    internal Patient patient;
    Collider Collider;
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
        Collider = GetComponent<Collider>();
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
        worldProgresBar.fillAmount = 0;
        LoadData();
    }

    public void LoadData()
    {
        UpdateInitializers();
        SetVisual();
        SetUpgradeVisual();

    }

    private void SetVisual()
    {
        if (bIsUnlock)
        {
            if (unlockObjs != null)
            {
                foreach (var item in unlockObjs)
                {
                    gameManager.DropObj(item);
                }
            }
            gameManager.PlayParticles(roundUpgradePartical);
            LoadNpcData();
            staffNPC.gameObject.SetActive(true);
            staffNPC.loadData();
            Collider.enabled = true;
            Destroy(upGrader.gameObject);
        }
        else
        {
            Collider.enabled = false;
            staffNPC.gameObject.SetActive(false);
            gameObject.SetActive(false);
            gameManager.SetObjectsState(unlockObjs, false);
        }
    }

    public void LoadNpcData()
    {
        staffNPC.SetMainSeat(onTrigger.seat);
    }

    #endregion

    #region Upgrade Mechanics 
    public void SetUpgradeVisual()
    {
        upGrader.gameObject.SetActive(bIsUpgraderActive);

        if (bIsUpgraderActive)
            SetTakeMoneyData(currentCost);
    }

    public void OnUnlockAndUpgrade()
    {
        AudioManager.i.OnUpgrade();

        if (!bIsUnlock)
        {
            bIsUnlock = true;
            bIsUpgraderActive = false;
            SetVisual();
            TaskManager.instance?.OnTaskComplete(currentTask);
        }
    }

    private void SetTakeMoneyData(int cost)
    {
        DOVirtual.DelayedCall(0.5f, () => upGrader.SetData(cost));
    }

    #endregion

    #region Proses Mechanics
    public virtual void OnPlayerTrigger()
    {
        bIsPlayerOnDesk = true;
        if (!staffNPC.bIsUnlock)
        {
            SetUpPlayer();
        }
    }

    public virtual void OnPlayerExit()
    {

        bIsPlayerOnDesk = false;
        if (!staffNPC.bIsUnlock)
        {
            BreakProcess();
        }
    }

    public virtual void SetUpPlayer()
    {
        playerController.animationController.PlayAnimation(seat.idleAnim);
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

    //public bool bCanDoProcess()
    //{
    //    //return playerController.bHaveItems;
    //}
    public virtual void StartProcessPatients()
    {
        if (patient == null) return;


        var pharmacyRoom = hospitalManager.pharmacyRoom;
        var workingAnimation = seat.workingAnim;
        var processTime = staffNPC.currentLevelData.processTime;
        if (staffNPC.bIsUnlock && staffNPC.bIsOnDesk)
        {
            StartPatientProcessing(staffNPC.animationController, workingAnimation, AnimType.Idle, staffNPC.currentLevelData.processTime, () =>
            {
                OnProcessComplite(pharmacyRoom, staffNPC.animationController, AnimType.Idle);
            });
        }
        else if (bIsPlayerOnDesk)
        {
            StartPatientProcessing(playerController.animationController, workingAnimation, AnimType.Idle, staffNPC.currentLevelData.processTime, () =>
            {
                OnProcessComplite(pharmacyRoom, playerController.animationController, AnimType.Idle);
            });
        }

    }

    protected string processTweenId;

    protected void StartPatientProcessing(AnimationController animationController, AnimType workingAnim, AnimType idleAnim, float processTime, Action onComplete)
    {

        if (string.IsNullOrEmpty(processTweenId))
        {
            processTweenId = "ProcessTween_" + Guid.NewGuid().ToString();
        }
        Debug.Log(workingAnim + "Anim Type" + animationController.gameObject.name);

        animationController.PlayAnimation(workingAnim);

        worldProgresBar.DOFillAmount(1, processTime).SetId(processTweenId)
            .OnComplete(() =>
            {
                animationController.PlayAnimation(idleAnim);
                onComplete?.Invoke();
            });
    }

    public virtual void OnProcessComplite(ARoom nextRoom, AnimationController animationController, AnimType idleAnim)
    {
        room.moneyBox.TakeMoney(hospitalManager.GetCustomerCost(patient, room.diseaseData, staffNPC.currentLevelData.StaffExprinceType));
        worldProgresBar.fillAmount = 0;

        if (nextRoom.bIsUnRegisterQueIsFull() || nextRoom == null || !nextRoom.bIsUnlock)
        {
            animationController.PlayAnimation(idleAnim);
            patient.MoveToExit(hospitalManager.GetRandomExit());
            patient.animal.emojisController.PlayEmoji(hospitalManager.GetAnimalMood());
        }
        else
        {
            nextRoom.RegisterPatient(patient);
        }

        MoveAnimal(patient.animal);
    }

    protected void BreakProcess()
    {
        worldProgresBar.fillAmount = 0;
        DOTween.Kill(processTweenId);
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

    public void MoveAnimal(Animal animal)
    {
        //animal.transform.position = patient.animalFollowPos.position;
        //animal.transform.rotation = patient.animalFollowPos.rotation;
        //animal.navmeshAgent.enabled = true;
        //animal.enabled = true;
        patient.MoveAnimal();
        bIsOccupied = false;
        patient = null;
        room.RearngeQue();

    }
    #endregion

    #region Some Call backs
    public void OnStaffUnlock()
    {

        staffNPC.bIsOnDesk = false;
        staffNPC.transform.position = staffNPC.upGrader.transform.position;
        staffNPC.transform.rotation = staffNPC.upGrader.transform.rotation;
        staffNPC.nPCMovement.Init();
        staffNPC.nPCMovement.MoveToTarget(seat.transform, () =>
        {
            staffNPC.animationController.PlayAnimation(seat.idleAnim);
            staffNPC.bIsOnDesk = true;
            staffNPC.transform.DORotate(seat.transform.rotation.eulerAngles, 0.2f);

            staffNPC.nPCMovement.enabled = false;
            StartProcessPatients();
        });
    }

    //public virtual void TakeIteam(AnimType animType)
    //{
    //    switch (animType)
    //    {

    //    }
    //}

    #endregion
}
