using DG.Tweening;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InjectionRoom : MonoBehaviour
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
    public bool bIsUnlock;
    public bool bIsUpgraderActive;
    internal bool bIsPlayerOnDesk;

    [Header("Room Dependencies")]
    public Upgrader upGrader;
    public MoneyBox moneyBox;
    internal WaitingQueue waitingQueue;
    public int unRegisterLimit;
    public List<Patient> unRegisterPatientList;

    [Header("Bed Details")]
    public int currntOpenBeds;
    public Bed[] bedsArr;


    [Header("Visuals")]
    public GameObject[] unlockObjs;
    public GameObject[] lockedObjs;
    public ParticleSystem[] roundUpgradePartical;

    #region Initializers
    internal SaveManager saveManager;
    internal EconomyManager economyManager;
    internal GameManager gameManager;
    internal UiManager uiManager;
    internal HospitalManager hospitalManager;

    private void OnEnable()
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
    private void Start()
    {
        currentCost = unlockPrice;
        waitingQueue = GetComponent<WaitingQueue>();
        LoadData();
    }

    private void LoadData()
    {
        UpdateInitializers();
        SetVisual();
        SetUpgradeVisual();
    }

    private void SetVisual()
    {
        if (bIsUnlock)
        {
            gameManager.SetObjectsState(lockedObjs, false);
            foreach (var item in unlockObjs)
            {
                gameManager.DropObj(item);
            }
            LoadRekData();
            gameManager.PlayParticles(roundUpgradePartical);
            Destroy(upGrader.gameObject);

        }
        else
        {
            gameManager.SetObjectsState(unlockObjs, false);
            gameManager.SetObjectsState(lockedObjs, true);
        }

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

    public void LoadRekData()
    {
        for (int i = 0; i < currntOpenBeds; i++)
        {
            var rek = bedsArr[i];
            rek.gameObject.SetActive(true);
        }
    }


    public void LoadNextUpgrade()
    {
        currntOpenBeds++;
        bedsArr[currntOpenBeds].gameObject.SetActive(true);
    }

    #endregion

    #region Prosece Mechanics
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
                Transform transform = hospitalManager.GetRandomPos(patients);
                patients.NPCMovement.MoveToTarget(transform, null);
                patients.MoveAnimal();

            }
        }
    }
    public void NextPatient()
    {
        if (unRegisterPatientList.Count > 0)
        {
            Patient patient = unRegisterPatientList[0];
            RegisterPatient(patient);
            patient.registerPos.bIsRegiseter = false;
            unRegisterPatientList.RemoveAt(0);
            hospitalManager.OnRoomHaveSpace();
        }
    }
    public void OnReachQueEnd()
    {
        if (waitingQueue.patientInQueue.Count > 0 && !waitingQueue.patientInQueue[0].NPCMovement.bIsMoving)
        {

            Patient patient = waitingQueue.patientInQueue[0];
            Bed bed = GetBed();
            if (bed != null)
            {
                bed.bIsOccupied = true;
                waitingQueue.RemoveFromQueue(patient);
                NextPatient();
                patient.NPCMovement.MoveToTarget(GetBed().petOwnerSeat.transform, () =>
                {
                    OnReachTable(GetBed(), patient);
                });
            }
        }
    }

    public Bed GetBed()
    {
        for (int i = 0; i < bedsArr.Length; i++)
        {
            var bed = bedsArr[i];
            if (bed != null && !bed.bIsOccupied)
            {
                return bed;
            }
        }
        return null;
    }
    public void OnReachTable(Bed bed, Patient patient)
    {
        DOVirtual.DelayedCall(0.2f, () =>
        {

            if (waitingQueue.patientInQueue.Count > 0 && !waitingQueue.patientInQueue[0].NPCMovement.bIsMoving)
            {

                Animal animal = patient.animal;
                animal.navmeshAgent.enabled = false;
                patient.transform.rotation = Quaternion.identity;


                animal.gameObject.transform.position = bed.petDignosPos.position;
                animal.gameObject.transform.rotation = bed.petDignosPos.rotation;


                animal.animator.PlayAnimation(bed.petOwnerSeat.idleAnim);


            }
        });
    }
    #endregion
}
