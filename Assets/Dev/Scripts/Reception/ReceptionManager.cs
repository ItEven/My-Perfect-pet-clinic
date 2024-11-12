using DG.Tweening;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
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
    #region Initializers

    SaveManager saveManager;
    EconomyManager economyManager;
    GameManager gameManager;
    UiManager uiManager;
    HospitalManager hospitalManager;

    private void OnEnable()
    {
        UpdateInitializers();
    }
    private void OnDisable()
    {
        UpdateInitializers();
    }

    public void UpdateInitializers()
    {
        saveManager = SaveManager.instance;
        economyManager = saveManager.economyManager;
        gameManager = saveManager.gameManager;
        uiManager = saveManager.uiManager;
        hospitalManager = saveManager.hospitalManager;
    }

    #endregion


    public void Start()
    {
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
            worldProgresBar.fillAmount = 0;

            foreach (var item in lockedObjs)
            {
                if (item.activeInHierarchy)
                {
                    item.SetActive(false);
                }
            }
            foreach (var item in unlockObjs)
            {
                gameManager.DropObj(item);
            }


            roundUpgradePartical.ForEach(X => X.Play());
        }
        else
        {
            foreach (var item in unlockObjs)
            {
                if (item.activeInHierarchy)
                {
                    item.SetActive(false);
                }
            }
            foreach (var item in lockedObjs)
            {
                if (!item.activeInHierarchy)
                {
                    item.SetActive(true);
                }
            }


        }
    }


    #region Upgrade Mechanics 
    public void SetUpgredeVisual()
    {

        if (bIsUpgraderActive)
        {
            upGrader.gameObject.SetActive(true);

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
            SetVisual();
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


    [Button("StratProsses")]
    public void StratProssesPatients()
    {
        Debug.LogError("NIga0");

        if (bIsPlayerOnDesk)
        {
            gameManager.playerController.animationController.PlayAnimation(seat.idleAnim);
        }


        if (waitingQueue.patientInQueue.Count > 0 && !waitingQueue.patientInQueue[0].NPCMovement.bIsMoving)
        {
            //if (!hospitalManager.CheckRegiterPosFull())
            //{
            var room = hospitalManager.GetInspectionRoom(waitingQueue.patientInQueue[0]);
            if (!room.bIsUnRegisterQueIsFull())
            {

                if (bIsPlayerOnDesk)
                {
                    gameManager.playerController.animationController.PlayAnimation(seat.workingAnim);

                    worldProgresBar.DOFillAmount(1, npc.currentLevelData.processTime).SetId("ProgressTween")
                       .OnComplete(() =>
                       {
                           gameManager.playerController.animationController.PlayAnimation(seat.idleAnim);

                           moneyBox.TakeMoney(npc.currentLevelData.customerCost);
                           room.RegisterPatient(waitingQueue.patientInQueue[0]);

                           waitingQueue.RemoveFromQueue(waitingQueue.patientInQueue[0]);
                           worldProgresBar.fillAmount = 0;
                       });

                }
                else if (npc.bIsUnlock)
                {
                    npc.animationController.PlayAnimation(seat.workingAnim);

                    worldProgresBar.DOFillAmount(1, npc.currentLevelData.processTime)
                        .OnComplete(() =>
                        {
                            npc.animationController.PlayAnimation(seat.idleAnim);
                            moneyBox.TakeMoney(npc.currentLevelData.customerCost);
                            room.RegisterPatient(waitingQueue.patientInQueue[0]);

                            waitingQueue.RemoveFromQueue(waitingQueue.patientInQueue[0]);
                            worldProgresBar.fillAmount = 0;
                        });
                }
            }
            else
            {
                Debug.LogError("NIga1");

            }
            //}
        }
        else
        {
            Debug.LogError("NIga2");

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
