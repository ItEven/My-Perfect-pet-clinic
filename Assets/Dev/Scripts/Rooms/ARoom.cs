using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEditor.Search;

[Serializable]

public class ARoomData
{
    public bool bIsUnlock;
    public bool bIsUpgraderActive;
    public int currentCost;
    public int currentOpneBed;
    public int currnetMoney;
    public List<BedData> bedDatas = new List<BedData>();
}
[Serializable]

public class BedData
{
    public bool bIsUnlock;
    public bool bIsUpgraderActive;
    public int currentCost;
    public StaffData staffData;
}
[Serializable]

public class StaffData
{
    public bool bIsUnlock;
    public bool bIsUpgraderActive;
    public int currentCost;
    public int currentLevel;
    internal int nextLevel;
}

public class ARoom : MonoBehaviour
{
    //private ARoomData _save = new ARoomData();
    public string ROOMNAME;


    /*
     * 
     * 
     * 
     * */
    [Header("Room Datiles")]
    public string roomName;
    public Sprite roomIcon;

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


    [Header("Disease Dependencies")]
    public DiseaseType[] diseaseTypes;
    public DiseaseData diseaseData;

    [Header("Room Dependencies")]
    public Upgrader upGrader;
    public MoneyBox moneyBox;
    internal WaitingQueue waitingQueue;
    public int unRegisterLimit;
    public float unregisterQueShuffleTimer = 15f;
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

        if (PlayerPrefs.HasKey(ROOMNAME))
        {
            LoadSaveData();
        }
        else
        {
            LoadData();
        }
    }

    private void LoadData()
    {
        UpdateInitializers();
        SetVisual();
        SetUpgradeVisual();
    }

    public virtual void SetVisual()
    {
        if (bIsUnlock)
        {
            gameManager.SetObjectsStates(lockedObjs, false);
            gameManager.SetObjectsStates(unlockObjs, true);
            uiManager.AddRoomUi(roomIcon, roomName, bIsUnlock.ToString(), unlockObjs[0].transform);
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
            uiManager.AddRoomUi(roomIcon, roomName, bIsUnlock.ToString(), upGrader.transform);

            // LoadBedData();
            gameManager.PlayParticles(roundUpgradePartical);
            AddAllUnregisterPatient();
            //  Destroy(upGrader.gameObject);
            //AddAllUnregisterPatient();
        }
        else
        {
            gameManager.SetObjectsStates(unlockObjs, false);
            gameManager.SetObjectsStates(lockedObjs, true);
        }
    }
    #endregion

    #region Upgrade Mechanics 
    public void SetUpgradeVisual()
    {
        if (bIsUpgraderActive)
        {
            TaskManager.instance.target = upGrader.transform;
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
            TaskManager.instance?.OnTaskComplete(currentTask);
        }
    }

    private void SetTakeMoneyData(int cost)
    {
        DOVirtual.DelayedCall(0.5f, () => upGrader.SetData(cost));
    }

    public void LoadBedData()
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
        //Debug.LogError(gameObject.name + " currntOpenBeds" + currntOpenBeds);
        bedsArr[currntOpenBeds].bIsUpgraderActive = true;
        bedsArr[currntOpenBeds].gameObject.SetActive(true);
        //Debug.LogError(gameObject.name + " object na,e " + bedsArr[currntOpenBeds].gameObject.name);

        bedsArr[currntOpenBeds].LoadData();
    }

    public void LoadNextForStaff(int index)
    {

        bedsArr[index].staffNPC.LoadNextUpgrade();
    }
    #endregion

    #region Prosece Mechanics
    public void RegisterPatient(Patient patients)
    {
        if (patients != null)
        {
            patients.currnetRoom = this;
            if (!waitingQueue.bIsQueueFull() && bIsUnlock)
            {
                patients.markForFull.gameObject.SetActive(false);
                patients.markForLock.gameObject.SetActive(false);
                waitingQueue.AddInQueue(patients);
                patients.MoveAnimal();
            }
            else
            {
                if (bIsUnlock)
                {
                    patients.markForFull.gameObject.SetActive(true);
                    patients.markForLock.gameObject.SetActive(false);
                }
                else
                {
                    patients.markForFull.gameObject.SetActive(false);
                    patients.markForLock.gameObject.SetActive(true);
                }
                unRegisterPatientList.Add(patients);
                Transform transform = hospitalManager.GetRandomPos(patients);
                patients.NPCMovement.MoveToTarget(transform, null);
                patients.MoveAnimal();
                ////StartStuffle();
                //patients.StartWatting(() =>
                //{
                //    patients.MarkOff();
                //    RemovePatientFromUnRegisterQ(patients);
                //});

            }
        }
        else
        {
            Debug.LogError("Patient is null");
        }
    }
    public void AddAllUnregisterPatient()
    {
        if (unRegisterPatientList.Count > 0)
        {
            foreach (Patient patient in unRegisterPatientList)
            {
                patient.StopWatting();
                RegisterPatient(patient);
                patient.registerPos.bIsRegiseter = false;
                patient.BrakeDally();
            }
            unRegisterPatientList.Clear();
            hospitalManager.OnRoomHaveSpace();
        }
    }


    public void NextPatientFromUnRegisterQ()
    {
        if (unRegisterPatientList.Count > 0)
        {
            Patient patient = unRegisterPatientList[0];
            RemovePatientFromUnRegisterQ(patient);
            RegisterPatient(patient);
        }
    }


    public void RemovePatientFromUnRegisterQ(Patient patient)
    {
        if (unRegisterPatientList.Contains(patient))
        {
           
            patient.registerPos.bIsRegiseter = false;
            patient.BrakeDally();
            unRegisterPatientList.Remove(patient);
        }
    }
    public void RearngeQue()
    {
        waitingQueue.OnReachedQueueAction(waitingQueue.patientInQueue[0]);
        NextPatientFromUnRegisterQ();
    }
    public void OnReachQueEnd()
    {

        if (waitingQueue.patientInQueue.Count > 0 && !waitingQueue.patientInQueue[0].NPCMovement.bIsMoving)
        {

            Bed bed = GetBed();
            Patient patient = waitingQueue.patientInQueue[0];

            if (bed != null)
            {
                if (bed.bIsOccupied)
                {
                    Debug.LogError(bed.room.name);
                    return;
                }

                bed.bIsOccupied = true;
                waitingQueue.RemoveFromQueue(patient);

                patient.NPCMovement.navmeshAgent.enabled = enabled;
                patient.NPCMovement.MoveToTarget(bed.petOwnerSeat.transform, () =>
                {
                    OnReachTable(bed, patient);
                    //patient.StartWatting(() =>
                    //{
                    //    patient.MoveToExit(hospitalManager.GetRandomExit(patient), hospitalManager.GetAnimalMood());
                    //    bed.MoveAnimal(patient.animal);
                    //});
                });
                hospitalManager.OnRoomHaveSpace();
                patient.MoveAnimal();
                // NextPatientFromUnRegisterQ();
            }
        }
    }

    public Bed GetBed()
    {
        for (int i = 0; i < bedsArr.Length; i++)
        {
            var bed = bedsArr[i];
            if (bed != null && !bed.bIsOccupied && bed.bIsUnlock)
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

            Animal animal = patient.animal;
            animal.navmeshAgent.enabled = false;


            patient.transform.DORotate(bed.petOwnerSeat.transform.rotation.eulerAngles, 0.2f);

            if (bed.petDignosPos != null)
            {

                animal.gameObject.transform.position = bed.petDignosPos.transform.position;
                animal.gameObject.transform.rotation = bed.petDignosPos.transform.rotation;
            }
            else
            {
                Debug.LogError("bed.PetDignosPos Null");

            }

            animal.animator.PlayAnimation(bed.petDignosPos.idleAnim);
            bed.patient = null;
            bed.patient = patient;
            bed.StartProcessPatients();
        });
    }
    public bool bIsUnRegisterQueIsFull()
    {
        return unRegisterLimit < unRegisterPatientList.Count;
    }


    #endregion

    #region Data Functions
    public void SaveData()
    {
        ARoomData aRoomData = new ARoomData();
        aRoomData.bIsUnlock = bIsUnlock;
        aRoomData.bIsUpgraderActive = bIsUpgraderActive;
        aRoomData.currentOpneBed = currntOpenBeds;
        aRoomData.currnetMoney = moneyBox.totalMoneyInBox;


        if (upGrader != null)
        {
            aRoomData.currentCost = currentCost;
        }
        for (int i = 0; i < bedsArr.Length; i++)
        {
            var bed = new BedData();
            bed.bIsUnlock = bedsArr[i].bIsUnlock;
            bed.bIsUpgraderActive = bedsArr[i].bIsUpgraderActive;
            if (bedsArr[i].upGrader != null)
            {
                bed.currentCost = bedsArr[i].currentCost;
            }

            bed.staffData = new StaffData
            {
                bIsUnlock = bedsArr[i].staffNPC.bIsUnlock,
                bIsUpgraderActive = bedsArr[i].staffNPC.bIsUpgraderActive,
                currentCost = bedsArr[i].staffNPC.currentCost,
                currentLevel = bedsArr[i].staffNPC.currentLevel,
                nextLevel = bedsArr[i].staffNPC.nextLevel

            };

            aRoomData.bedDatas.Add(bed);
        }

        string JsonData = JsonUtility.ToJson(aRoomData);
        PlayerPrefs.SetString(ROOMNAME, JsonData);

    }
    public void LoadSaveData()
    {
        string JsonData = PlayerPrefs.GetString(ROOMNAME);
        ARoomData receivefile = JsonUtility.FromJson<ARoomData>(JsonData);

        bIsUnlock = receivefile.bIsUnlock;
        bIsUpgraderActive = receivefile.bIsUpgraderActive;
        currentCost = receivefile.currentCost;
        currntOpenBeds = receivefile.currentOpneBed;
        moneyBox.TakeMoney(receivefile.currnetMoney);

        for (int i = 0; i < receivefile.bedDatas.Count; i++)
        {
            var bedData = receivefile.bedDatas[i];
            var bed = bedsArr[i];
            bed.bIsUnlock = bedData.bIsUnlock;
            bed.bIsUpgraderActive = bedData.bIsUpgraderActive;
            bed.currentCost = bedData.currentCost;


            bed.staffNPC.bIsUnlock = bedData.staffData.bIsUnlock;
            if (bed.staffNPC.bIsUnlock)
            {
                bed.staffNPC.bIsOnDesk = true;
            }
            bed.staffNPC.bIsUpgraderActive = bedData.staffData.bIsUpgraderActive;
            bed.staffNPC.currentCost = bedData.staffData.currentCost;
            bed.staffNPC.currentLevel = bedData.staffData.currentLevel;
            bed.staffNPC.nextLevel = bedData.staffData.nextLevel;

        }
        LoadBedData();
        LoadData();

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

    public void StartStuffle()
    {
        if (shuffle == null)
        {
            shuffle = StartCoroutine(ShuffleUnresgisterPatient());
        }
    }
    public void StopStuffle()
    {
        if (shuffle != null)
        {
            StopCoroutine(shuffle);
            shuffle = null;
        }
    }

    Coroutine shuffle;
    IEnumerator ShuffleUnresgisterPatient()
    {
        if (unRegisterPatientList == null || unRegisterPatientList.Count == 0)
        {
            Debug.LogError("unRegisterPatientList is null or empty.");
            yield break;
        }

        while (unRegisterPatientList.Count > 0)
        {
            yield return new WaitForSeconds(unregisterQueShuffleTimer);

            if (unRegisterPatientList.Count > 0)
            {
                foreach (var p in unRegisterPatientList)
                {
                    if (p == null)
                    {
                        Debug.LogError("Patient in the list is null.");
                        continue;
                    }

                    if (p.registerPos == null)
                    {
                        Debug.LogError($"registerPos is null for patient {p}");
                        continue;
                    }

                    try
                    {
                        p.registerPos.bIsRegiseter = false;
                        p.BrakeDally();

                        Transform transform = hospitalManager?.GetRandomPos(p);
                        if (transform == null)
                        {
                            Debug.LogError($"GetRandomPos returned null for patient {p}");
                            continue;
                        }

                        p.MoveNewShufflePos(transform);
                    }
                    catch (NullReferenceException ex)
                    {
                        Debug.LogError($"Error processing patient {p}: {ex.Message}");
                    }
                }
            }
            else
            {
                StopStuffle();
            }
        }
    }
}
