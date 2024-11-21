using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ReceptionManager : MonoBehaviour
{
    //private ARoomData _save = new ARoomData();
    public string Reception = "Reception";

    /*
     * 
     * 
     * 
     * */

    [Header("Task Number")]
    public int currentTask;

    [Header("Reception Details")]
    public int unlockPrice;
    internal int currentCost
    {
        get { return upGrader.needMoney; }
        set
        {
            upGrader.needMoney = value;
        }
    }

    public bool bIsUnlock;
    public bool bIsUpgraderActive;
    internal bool bIsPlayerOnDesk;


    public Upgrader upGrader;
    public MoneyBox moneyBox;
    // public OnTrigger onTrigger;
    public Seat seat;



    [Header(" NPC Details")]
    public ReceptionNPC npc;


    [Header(" Visuals Details")]
    public Image worldProgresBar;

    public GameObject[] unlockObjs;
    public GameObject[] lockedObjs;
    public ParticleSystem[] roundUpgradePartical;
    public WaitingQueue waitingQueue;

    [Header("Ui Things")]
    public RectTransform warnningTextBox;

    #region Initializers
    SaveManager saveManager;
    EconomyManager economyManager;
    GameManager gameManager;
    UiManager uiManager;
    HospitalManager hospitalManager;
    FloatingJoystick floatingJoystick;
    CameraController cameraController;
    private void OnEnable()
    {
        UpdateInitializers();
    }
    private void OnDisable()
    {
        floatingJoystick.OnHoldOff -= OnHoldUp;
    }

    public void UpdateInitializers()
    {
        saveManager = SaveManager.instance;
        economyManager = saveManager.economyManager;
        gameManager = saveManager.gameManager;
        uiManager = saveManager.uiManager;
        cameraController = saveManager.cameraController;
        hospitalManager = saveManager.hospitalManager;
        floatingJoystick = gameManager.playerController.playerControllerData.joystick;
        floatingJoystick.OnHoldOff += OnHoldUp;
    }

    #endregion


    public void Start()
    {
        warnningTextBox.gameObject.SetActive(false);

        worldProgresBar.fillAmount = 0;

        currentCost = unlockPrice;
        // seat = onTrigger.seat;
        if (PlayerPrefs.HasKey(Reception))
        {
            LoadSaveData();
        }
        else
        {
            LoadData();
        }
    }
    public void LoadData()
    {
        UpdateInitializers();
        SetVisual();
        SetUpgredeVisual();
    }
    public void SetVisual()
    {
        if (bIsUnlock)
        {
            gameManager.SetObjectsStates(lockedObjs, false);
            gameManager.SetObjectsStates(unlockObjs, true);
            // foreach (var item in unlockObjs)
            // {
            //     gameManager.DropObj(item);
            // }
            //// LoadBedData();
            // gameManager.PlayParticles(roundUpgradePartical);
            // //  Destroy(upGrader.gameObject);

        }
        else
        {
            gameManager.SetObjectsStates(unlockObjs, false);
            gameManager.SetObjectsStates(lockedObjs, true);
        }

    }

    public void OnUnlock()
    {
        if (bIsUnlock)
        {
            gameManager.SetObjectsStates(lockedObjs, false);
            foreach (var item in unlockObjs)
            {
                gameManager.DropObj(item);
            }
            // LoadBedData();
            gameManager.PlayParticles(roundUpgradePartical);
            //  Destroy(upGrader.gameObject);

        }
        else
        {
            gameManager.SetObjectsStates(unlockObjs, false);
            gameManager.SetObjectsStates(lockedObjs, true);
        }
    }

    #region Upgrade Mechanics 
    public void SetUpgredeVisual()
    {

        if (bIsUpgraderActive)
        {
            CameraController.Instance.FocusOnTarget(upGrader.transform);

            SetTakeMoneyData(currentCost);
        }
        else
        {
            upGrader.gameObject.SetActive(false);
        }
    }

    public void OnUnlockAndUpgrade()
    {
        AudioManager.i.OnUpgrade();

        if (!bIsUnlock)
        {
            bIsUnlock = true;
            bIsUpgraderActive = false;
            OnUnlock();
            if (TaskManager.instance != null)
            {
                TaskManager.instance.OnTaskComplete(currentTask);
            }
        }
    }

    public void SetTakeMoneyData(int cost)
    {
        DOVirtual.DelayedCall(0.5f, () => upGrader.SetData(cost));
    }


    public void LoadNextUpgrade()
    {
        npc.LoadNextUpgrade();
    }

    #endregion

    #region Prosece Mechanics

    public void OnPlayerTrigger()
    {
        bIsPlayerOnDesk = true;
        if (!npc.bIsUnlock)
        {
            SetUpPlayer();
            StratProssesPatients();
        }
    }

    bool bIsProcessing;
    bool bIsSfitedOnece;
    public void OnPlayerStay()
    {
        if (bIsPlayerOnDesk && !gameManager.playerController.playerControllerData.joystick.bIsOnHold && !bIsSfitedOnece)
        {
            bIsSfitedOnece = true;
            if (!bIsProcessing)
            {

                gameManager.playerController.animationController.PlayAnimation(AnimType.Sti_Idle);
            }
            else
            {
                gameManager.playerController.animationController.PlayAnimation(AnimType.Typing);
            }

            gameManager.playerController.playerControllerData.characterMovement.enabled = false;
            gameManager.playerController.enabled = false;
            gameManager.playerController.transform.position = seat.transform.position;
            gameManager.playerController.playerControllerData.characterMovement.rotatingObj.rotation = seat.transform.rotation;

        }
    }
    public void OnHoldUp()
    {
          bIsSfitedOnece = false;
    }

    public void OnPlayerExit()
    {

        bIsPlayerOnDesk = false;
        if (!npc.bIsUnlock)
        {
            StopProsses();
        }
    }

    //Seat seat;
    public void SetUpPlayer()
    {

        gameManager.playerController.playerControllerData.characterMovement.enabled = false;
        gameManager.playerController.enabled = false;
        gameManager.playerController.playerControllerData.joystick.gameObject.SetActive(false);

        gameManager.playerController.transform.position = seat.transform.position;
        gameManager.playerController.playerControllerData.characterMovement.rotatingObj.rotation = seat.transform.rotation;


        DOVirtual.DelayedCall(1f, () =>
        {
            gameManager.playerController.transform.SetParent(null);
            gameManager.playerController.playerControllerData.joystick.gameObject.SetActive(true);
            gameManager.playerController.playerControllerData.joystick.OnPointerUp(null);
            gameManager.playerController.playerControllerData.characterMovement.enabled = true;
        });

    }


    protected string processTweenId;
    bool bIsRunning = false;

    [Button("StratProsses")]
    public void StratProssesPatients()
    {
        if (bIsRunning) return;
       

        if (bIsPlayerOnDesk)
        {
            gameManager.playerController.animationController.PlayAnimation(seat.idleAnim);
        }



        if (waitingQueue.patientInQueue.Count > 0 && !waitingQueue.patientInQueue[0].NPCMovement.bIsMoving)
        {
            warnningTextBox.gameObject.SetActive(false);

            //if (!hospitalManager.CheckRegiterPosFull())
            //{
            var room = hospitalManager.GetInspectionRoom();
            

            if (room == null)
            {

                Debug.LogError(gameObject.name + "this is null");
            }
            if (!room.bIsUnRegisterQueIsFull())
            {

                if (bIsPlayerOnDesk)
                {
                    bIsRunning = true;
                    bIsProcessing = true;
                    gameManager.playerController.animationController.PlayAnimation(seat.workingAnim);

                    if (string.IsNullOrEmpty(processTweenId))
                    {
                        processTweenId = "ProcessTween_" + Guid.NewGuid().ToString();
                    }
                    worldProgresBar.DOFillAmount(1, npc.currentLevelData.processTime).SetId(processTweenId)
                       .OnComplete(() =>
                       {
                           hospitalManager.OnPatientRegister();

                           gameManager.playerController.animationController.PlayAnimation(seat.idleAnim);

                           moneyBox.TakeMoney(npc.currentLevelData.customerCost);
                           room.RegisterPatient(waitingQueue.patientInQueue[0]);

                           waitingQueue.RemoveFromQueue(waitingQueue.patientInQueue[0]);
                           worldProgresBar.fillAmount = 0;
                           bIsProcessing = false;
                           bIsRunning = false;

                       });

                }
                else if (npc.bIsUnlock)
                {
                    bIsRunning = true;
                    npc.animationController.PlayAnimation(seat.workingAnim);
                    if (string.IsNullOrEmpty(processTweenId))
                    {
                        processTweenId = "ProcessTween_" + Guid.NewGuid().ToString();
                    }
                    worldProgresBar.DOFillAmount(1, npc.currentLevelData.processTime).SetId(processTweenId)
                        .OnComplete(() =>
                        {
                            npc.animationController.PlayAnimation(seat.idleAnim);
                            moneyBox.TakeMoney(npc.currentLevelData.customerCost);
                            room.RegisterPatient(waitingQueue.patientInQueue[0]);
                            waitingQueue.RemoveFromQueue(waitingQueue.patientInQueue[0]);
                            worldProgresBar.fillAmount = 0;
                            bIsRunning = false;

                            DOTween.Kill(processTweenId);
                        });
                }
            }
            else
            {
                warnningTextBox.gameObject.SetActive(true);
                cameraController.MoveToRecption(seat.transform);

            }
            //}
        }
      
    }

    public void StopProsses()
    {

        DOTween.Kill("ProgressTween");

        gameManager.playerController.animationController.PlayAnimation(seat.idleAnim);
        bIsPlayerOnDesk = false;


    }

    #endregion

    #region Data Functions
    public void SaveData()
    {
        ARoomData aRoom = new ARoomData();
        aRoom.bIsUnlock = bIsUnlock;
        aRoom.bIsUpgraderActive = bIsUpgraderActive;
        aRoom.currentCost = currentCost;
        aRoom.currnetMoney = moneyBox.totalMoneyInBox;
        var bed = new BedData();
        bed.staffData = new StaffData
        {
            bIsUnlock = npc.bIsUnlock,
            bIsUpgraderActive = npc.bIsUpgraderActive,
            currentCost = npc.currentCost,
            currentLevel = npc.currentLevel,
            nextLevel = npc.nextLevel,
        };
        aRoom.bedDatas.Add(bed);

        string JsonData = JsonUtility.ToJson(aRoom);
        PlayerPrefs.SetString(Reception, JsonData);

    }
    public void LoadSaveData()
    {
        string JsonData = PlayerPrefs.GetString(Reception, string.Empty);
        if (string.IsNullOrEmpty(JsonData))
        {
            // Handle the case where no data has been saved yet
            return;
        }
        ARoomData receivefile = JsonUtility.FromJson<ARoomData>(JsonData);

        bIsUnlock = receivefile.bIsUnlock;
        bIsUpgraderActive = receivefile.bIsUpgraderActive;
        currentCost = receivefile.currentCost;
        moneyBox.TakeMoney(receivefile.currnetMoney);
        LoadData();

        var bedData = receivefile.bedDatas[0];
        npc.bIsUnlock = bedData.staffData.bIsUnlock;
        npc.bIsUpgraderActive = bedData.staffData.bIsUpgraderActive;
        npc.currentCost = bedData.staffData.currentCost;
        npc.currentLevel = bedData.staffData.currentLevel;
        npc.nextLevel = bedData.staffData.nextLevel;
        npc.loadData();
    }

    void OnApplicationQuit()
    {
        SaveData();
    }
    void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveData();
        }
    }
    #endregion

}
