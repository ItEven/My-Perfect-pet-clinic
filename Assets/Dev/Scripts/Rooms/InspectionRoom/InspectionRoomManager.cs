using DG.Tweening;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InspectionRoomManager : MonoBehaviour
{
    [Header("Task Number")]
    public int currentTask;
    [Header("InspectionRoom Details")]
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
    internal bool bCanProsses;

    public Upgrader upGrader;
    public MoneyBox moneyBox;
    internal WaitingQueue waitingQueue;
    public DiseaseType[] diseaseTypes;


    [Header(" NPC Details")]
    public InspectionRoomNpc Staff_NPC;
    public List<Patient> unRegisterPatientList;



    [Header(" Visuals Details")]
    public Image worldProgresBar;
    public GameObject[] unlockObjs;
    public GameObject[] lockedObjs;
    public ParticleSystem[] roundUpgradePartical;

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

    #region Starters
    public void Start()
    {
        currentCost = unlockPrice;
        waitingQueue = GetComponent<WaitingQueue>();
        loadData();
    }
    public void loadData()
    {
        UpdateInitializers();
        SetVisual();
        SetUpgredeVisual();
    }
    public void SetVisual()
    {
        if (bIsUnlock)
        {
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
            Staff_NPC.gameObject.SetActive(true);

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
            Staff_NPC.gameObject.SetActive(false);

        }
        gameManager.ReBuildNavmesh();
    }
    #endregion

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
        Staff_NPC.LoadNextUpgrade();
    }

    #endregion

    #region Patient Prosses Machincs

    public void RegisterPatient(Patient patients)
    {
        if (patients != null)
        {
            if (!waitingQueue.bIsQueueFull())
            {
                waitingQueue.AddInQueue(patients);
                patients.MoveAnimal();
            }
            else
            {
                unRegisterPatientList.Add(patients);
                patients.NPCMovement.MoveToTarget(hospitalManager.GetRandomPos(), null);

            }
        }
    }

    public void OnReachDocter()
    {

    }

    string tweenID = "worldProgressBarTween";
    public void StratProssesPatients()
    {
        Debug.LogError("waitingQueue -1");

        if (waitingQueue.patientInQueue.Count > 0 && !waitingQueue.patientInQueue[0].NPCMovement.bIsMoving && bCanProsses)
        {
            Debug.LogError("waitingQueue");
            //if (!hospitalManager.CheckRegiterPosFull())
            //{
            Debug.LogError("waitingQueue = 2");

            var room = hospitalManager.GetInspectionRoom(waitingQueue.patientInQueue[0]);

            worldProgresBar.fillAmount = 0;
            worldProgresBar.DOFillAmount(1, Staff_NPC.currentLevelData.processTime)
                .SetId(tweenID)
                .OnComplete(() =>
                {
                    Debug.LogError("waitingQu eue = 3");

                    moneyBox.TakeMoney(Staff_NPC.currentLevelData.customerCost);
                    room.RegisterPatient(waitingQueue.patientInQueue[0]);

                    waitingQueue.RemoveFromQueue(waitingQueue.patientInQueue[0]);
                });


            //}
        }
    }

    public void StopProsses()
    {
        bCanProsses = false;
        worldProgresBar.fillAmount = 0;
        DOTween.Kill(tweenID);
    }

    #endregion

}
