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
    public DiseaseData diseaseData;


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
        if (!Staff_NPC.bIsUnlock)
        {
            bCanProsses = true;
            StratProssesPatients();
            gameManager.playerController._characterMovement.enabled = false;
            gameManager.playerController.enabled = false;
            gameManager.playerController.bhasSit = true;
            gameManager.playerController.joystick.gameObject.SetActive(false);

            gameManager.playerController.transform.SetParent(Staff_NPC.sitPos);
            gameManager.playerController.transform.position = Staff_NPC.sitPos.position;
            gameManager.playerController._characterMovement.rotatingObj.rotation = Staff_NPC.sitPos.rotation;


            DOVirtual.DelayedCall(3f, () =>
            {
                gameManager.playerController.transform.SetParent(null);
                gameManager.playerController.joystick.gameObject.SetActive(true);
                gameManager.playerController.joystick.OnPointerUp(null);
                gameManager.playerController._characterMovement.enabled = true;


            });
        }
    }

    string tweenID = "worldProgressBarTween";
    public virtual void StratProssesPatients()
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

                    moneyBox.TakeMoney(GetCustomerCost(waitingQueue.patientInQueue[0]));
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


    public int GetCustomerCost(Patient patient)
    {
        if (patient != null)
        {
            for (int i = 0; i < diseaseData.diseases.Length; i++)
            {
                var dis = diseaseData.diseases[i];
                if (dis.Type == patient.diseaseType)
                {
                    switch (Staff_NPC.currentLevelData.StaffExprinceType)
                    {
                        case StaffExprinceType.Intern: return dis.InternFee;
                        case StaffExprinceType.Junior: return dis.juniorVeterinarianFee;
                        case StaffExprinceType.Senior: return dis.seniorVeterinarianFee;
                        case StaffExprinceType.Chief: return dis.chiefVeterinarianFee;
                    }
                }
            }
        }
        return 0;
    }

    


    #endregion
}
