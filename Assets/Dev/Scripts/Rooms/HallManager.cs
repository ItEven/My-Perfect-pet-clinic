using DG.Tweening;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HallManager : MonoBehaviour
{
    //private ARoomData _save = new ARoomData();
    public string HallName;

    /*
     * 
     * 
     * 
     * */
    [Header("Task Number")]
    public int currentTask;

    [Header("Hall Details")]
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

    [Header("New Watting Posses")]
    public List<RegisterPos> registerPos = new List<RegisterPos>();
    [Header(" Visuals Details")]
    public GameObject[] unlockObjs;
    public GameObject[] lockedObjs;
    public ParticleSystem[] roundUpgradePartical;

    #region Initializers

    SaveManager saveManager;
    GameManager gameManager;
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
        gameManager = saveManager.gameManager;
        hospitalManager = saveManager.hospitalManager;
    }

    #endregion


    public void Start()
    {
        currentCost = unlockPrice;
        if (PlayerPrefs.HasKey(HallName))
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

    #endregion
    #region Data Functions
    public void SaveData()
    {
        ARoomData aRoom = new ARoomData();
        aRoom.bIsUnlock = bIsUnlock;
        aRoom.bIsUpgraderActive = bIsUpgraderActive;
        aRoom.currentCost = currentCost;

        string JsonData = JsonUtility.ToJson(aRoom);
        PlayerPrefs.SetString(HallName, JsonData);

    }
    public void LoadSaveData()
    {
        string JsonData = PlayerPrefs.GetString(HallName);
        ARoomData receivefile = JsonUtility.FromJson<ARoomData>(JsonData);

        bIsUnlock = receivefile.bIsUnlock;
        bIsUpgraderActive = receivefile.bIsUpgraderActive;
        currentCost = receivefile.currentCost;
        gameManager.SetPlayerPos();
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

}
