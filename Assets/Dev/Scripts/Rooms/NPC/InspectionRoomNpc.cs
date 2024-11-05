using DG.Tweening;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class StaffNPCLevelData
{
    public int levelNum, upgradeCost, nextTask;
    public float processTime;
    public DiseaseType[] DiseaseType;
    public StaffExprinceType StaffExprinceType;
}
public class InspectionRoomNpc : MonoBehaviour
{
    [Header("Room Details")]
    [Tooltip("Cost required to unlock the NPC's room")]
    public int unlockPrice;

    [Tooltip("Current cost required for the NPC's upgrade")]
    internal int currentCost
    {
        get { return upGrader.needMoney; }
        set
        {
            upGrader.needMoney = value;
        }
    }

    [Tooltip("Indicates if the NPC's room is unlocked")]
    public bool bIsUnlock;

    [Tooltip("Indicates if the NPC's upgrader is active")]
    public bool bIsUpgraderActive;

    [Tooltip("Reference to the Animation Controller for NPC animations")]
    internal AnimationController animationController;

    [Tooltip("Reference to the Upgrader component to manage NPC upgrades")]
    public Upgrader upGrader;
    public UnityEvent OnUnlockNpc;

    [Header("Level Details")]
    [Tooltip("Current level of the NPC")]
    public int currentLevel;
    [Tooltip("Next level target for NPC upgrades")]
    internal int nextLevel;
    [Tooltip("Data of the current level for NPC")]
    internal StaffNPCLevelData currentLevelData;
    [Tooltip("Array of level data for each NPC level")]
    public StaffNPCLevelData[] levels;

    [Header("Visuals Details")]
    [Tooltip("Position where NPC can sit")]
    public Transform sitPos;
    [Tooltip("GameObject representing the NPC in the scene")]
    public GameObject npcObj;

    [Tooltip("Particle effects displayed during NPC upgrades")]
    public ParticleSystem[] roundUpgradePartical;

    #region Initializers

    SaveManager saveManager;
    EconomyManager economyManager;
    GameManager gameManager;
    UiManager uiManager;

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
    }

    #endregion


    public void Start()
    {
        animationController = npcObj.GetComponent<AnimationController>();
        currentCost = unlockPrice;
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
        currentLevelData = levels[currentLevel];

        transform.position = sitPos.position;
        transform.rotation = sitPos.rotation;

        if (bIsUnlock)
        {

            gameManager.DropObj(npcObj);
            roundUpgradePartical.ForEach(X => X.Play());

        }
        else
        {
            npcObj.SetActive(false);
        }
        //  gameManager.ReBuildNavmesh();

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
            currentLevel = 0;
            currentLevelData = levels[currentLevel];
            SetVisual();
            OnUnlockNpc.Invoke();
        }
        else
        {
            OnUpgrade();
        }
        if (TaskManager.instance != null)
        {
            TaskManager.instance.OnTaskComplete(currentLevelData.nextTask);
        }
    }

    public void SetTakeMoneyData(int cost)
    {
        DOVirtual.DelayedCall(0.5f, () => upGrader.SetData(cost));
    }

    public void OnUpgrade()
    {
        currentLevel++;
        currentLevelData = levels[currentLevel];
        roundUpgradePartical.ForEach(X => X.Play());
    }

    public void LoadNextUpgrade()
    {
        bIsUpgraderActive = true;
        if (bIsUnlock)
        {
            if (currentLevel + 1 < levels.Length)
            {
                currentCost = levels[currentLevel + 1].upgradeCost;
            }
            else
            {
                bIsUpgraderActive = false;

            }
        }
        SetUpgredeVisual();
    }

    #endregion
}
