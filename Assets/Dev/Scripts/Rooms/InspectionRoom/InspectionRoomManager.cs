using DG.Tweening;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

    public Upgrader upGrader;
    public MoneyBox moneyBox;
    internal WaitingQueue waitingQueue;
    public DiseaseType[] diseaseTypes;


    [Header(" NPC Details")]
    public InspectionRoomNpc DocterNpc;
    public List<Patient> unRegisterPatientList;



    [Header(" Visuals Details")]
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
            DocterNpc.gameObject.SetActive(true);

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
            DocterNpc.gameObject.SetActive(false);

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
        DocterNpc.LoadNextUpgrade();
    }

    #endregion

    #region Patient Prosses Machincs

    public void RegisterPatient(Patient patients)
    {
        if(patients != null)
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

    #endregion

}
