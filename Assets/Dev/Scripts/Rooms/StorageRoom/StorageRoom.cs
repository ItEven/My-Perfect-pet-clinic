using DG.Tweening;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageRoom : MonoBehaviour
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


    [Header("Rek Details")]
    public int currntOpenReks;
    public Rek[] reks;


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
            gameManager.SetObjectsStates(lockedObjs, false);
            foreach (var item in unlockObjs)
            {
                gameManager.DropObj(item);
            }
            LoadRekData();
            gameManager.PlayParticles(roundUpgradePartical);
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
        for (int i = 0; i < currntOpenReks; i++)
        {
            var rek = reks[i];
            rek.gameObject.SetActive(true);
        }
    }
    public void LoadNextUpgrade()
    {
        currntOpenReks++;
        reks[currntOpenReks].gameObject.SetActive(true);
    }
    #endregion
}