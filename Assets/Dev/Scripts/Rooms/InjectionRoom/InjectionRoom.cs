using DG.Tweening;
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
}
