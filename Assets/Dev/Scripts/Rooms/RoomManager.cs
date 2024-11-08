using DG.Tweening;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;



public class RoomManager : MonoBehaviour
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


    // Room Dependencies
    public Upgrader upGrader;
    public MoneyBox moneyBox;
    public Seat seat;
    internal WaitingQueue waitingQueue;
    public int unRegisterLimit;
    public List<Patient> unRegisterPatientList;

    // Disease Information
    public DiseaseType[] diseaseTypes;
    public DiseaseData diseaseData;

    [Header("NPC Details")]
    public Transform animalDignosPos;
    public StaffNPC Staff_NPC;

    [Header("Visuals")]
    public Image worldProgresBar;
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
        worldProgresBar.fillAmount = 0;

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
        Staff_NPC.LoadNextUpgrade();
    }

    #endregion

    #region Patient Prosses Machincs

    public virtual void RegisterPatient(Patient patients)
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

    public virtual void NextPatient()
    {
        //Patient patient = unRegisterPatientList[0];
        //RegisterPatient(patient);
        //patient.registerPos.bIsRegiseter = false;
        //unRegisterPatientList.RemoveAt(0);
        //PatientManager.instance.receptionManager.StratProssesPatients();
    }

    public bool bIsUnRegisterQueIsFull()
    {
        if (unRegisterPatientList.Count >= unRegisterLimit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void OnPlayerTrigger()
    {
        bIsPlayerOnDesk = true;
        if (!Staff_NPC.bIsUnlock)
        {
            SetUpPlayer();
        }
    }

    public void OnPlayerExit()
    {
        bIsPlayerOnDesk = false;

        if (!Staff_NPC.bIsUnlock)
        {
            StopProsses();
        }
    }
    public void SetUpPlayer()
    {
        StratProssesPatients();
        gameManager.playerController.playerControllerData.characterMovement.enabled = false;
        gameManager.playerController.enabled = false;
       //gameManager.playerController.bIsDiagnosing = true;
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

    protected Tween Tw_Filler;
    public virtual void StratProssesPatients()
    {
        //if (bIsPlayerOnDesk)
        //{
        //    gameManager.playerController.animationController.PlayAnimation(seat.idleAnim);
        //}


        //if (waitingQueue.patientInQueue.Count > 0 && !waitingQueue.patientInQueue[0].NPCMovement.bIsMoving)
        //{
        //    if (!hospitalManager.CheckRegiterPosFull())
        //    {
        //        var room = hospitalManager.pharmacyRoom;
        //        if (bIsPlayerOnDesk)
        //        {
        //            gameManager.playerController.animationController.PlayAnimation(seat.workingAnim);

        //            worldProgresBar.fillAmount = 0;
        //            Tw_Filler = worldProgresBar.DOFillAmount(1, Staff_NPC.currentLevelData.processTime)
        //                .OnComplete(() =>
        //                {

        //                    gameManager.playerController.animationController.PlayAnimation(seat.idleAnim);
        //                    moneyBox.TakeMoney(GetCustomerCost(waitingQueue.patientInQueue[0]));
        //                    room.RegisterPatient(waitingQueue.patientInQueue[0]);
        //                    var p = waitingQueue.patientInQueue[0];
        //                    p.MoveAnimal();
        //                    waitingQueue.RemoveFromQueue(waitingQueue.patientInQueue[0]);

        //                    if (unRegisterPatientList.Count > 0)
        //                    {
        //                        NextPatient();
        //                    }
        //                    worldProgresBar.fillAmount = 0;

        //                });

        //        }
        //        else if (Staff_NPC.bIsUnlock)
        //        {
        //            Staff_NPC.animationController.PlayAnimation(seat.workingAnim);

        //            worldProgresBar.fillAmount = 0;
        //            Tw_Filler = worldProgresBar.DOFillAmount(1, Staff_NPC.currentLevelData.processTime)
        //                .OnComplete(() =>
        //                {

        //                    Staff_NPC.animationController.PlayAnimation(seat.idleAnim);
        //                    moneyBox.TakeMoney(GetCustomerCost(waitingQueue.patientInQueue[0]));
        //                    room.RegisterPatient(waitingQueue.patientInQueue[0]);
        //                    var p = waitingQueue.patientInQueue[0];
        //                    p.MoveAnimal();
        //                    waitingQueue.RemoveFromQueue(waitingQueue.patientInQueue[0]);

        //                    if (unRegisterPatientList.Count > 0)
        //                    {
        //                        NextPatient();
        //                    }
        //                    worldProgresBar.fillAmount = 0;

        //                });

        //        }
        //    }
        //}

    }
    public void OnReachTable()
    {
        DOVirtual.DelayedCall(0.2f, () =>
        {

            if (waitingQueue.patientInQueue.Count > 0 && !waitingQueue.patientInQueue[0].NPCMovement.bIsMoving)
            {
                Patient p = waitingQueue.patientInQueue[0];
                Animal animal = p.animal;
                animal.navmeshAgent.enabled = false;
                p.transform.rotation = Quaternion.identity;


                animal.gameObject.transform.position = animalDignosPos.position;
                animal.gameObject.transform.rotation = animalDignosPos.rotation;


                animal.animator.PlayAnimation(seat.idleAnim);
                StratProssesPatients();

            }
        });
    }


    public virtual void StopProsses()
    {
        //gameManager.playerController.animationController.PlayAnimation(seat.idleAnim);
        //bIsPlayerOnDesk = false;
        //worldProgresBar.fillAmount = 0;
        //DOTween.Kill(Tw_Filler);
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
