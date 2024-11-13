using DG.Tweening;
using MoreMountains.Tools;
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
    public StaffExprinceType StaffExprinceType;
}

public class StaffNPC : MonoBehaviour
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

    [Tooltip("Indicates if the NPC's is on desk or not")]
    public bool bIsOnDesk;

    [Tooltip("Indicates if the NPC's room is Moveable or not")]
    public bool bHaveIteam;

    [Tooltip("Indicates if the NPC's upgrader is active")]
    public bool bIsUpgraderActive;

    [Tooltip("Reference to the Animation Controller for NPC animations")]
    public Transform animalCarryPos;
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
    [Tooltip("GameObject representing the NPC in the scene")]
    public GameObject npcObj;

    [Tooltip("Particle effects displayed during NPC upgrades")]
    public ParticleSystem[] roundUpgradePartical;

    [Header("Equipments Objects")]
    public Equipments playerEquipments;


    internal NPCMovement nPCMovement;
    internal Seat seat;
    #region Initializers

    SaveManager saveManager;
    EconomyManager economyManager;
    GameManager gameManager;
    UiManager uiManager;
    CameraController cameraController;
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
        animationController = npcObj.GetComponent<AnimationController>();
        nPCMovement = gameObject.GetComponent<NPCMovement>();
        saveManager = SaveManager.instance;
        economyManager = saveManager.economyManager;
        gameManager = saveManager.gameManager;
        uiManager = saveManager.uiManager;
        cameraController = saveManager.cameraController;
    }

    #endregion


    public void Start()
    {
        //loadData();
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
            //gameManager.DropObj(npcObj);
            nPCMovement.navmeshAgent.enabled = false;
            npcObj.SetActive(true);
            transform.position = seat.transform.position;
            transform.rotation = seat.transform.rotation;
            animationController.PlayAnimation(seat.idleAnim);
            roundUpgradePartical.ForEach(X => X.Play());
        }
        else
        {
            currentCost = levels[currentLevel].upgradeCost;

            npcObj.SetActive(false);
        }
        currentLevelData = levels[currentLevel];
    }


    #region Upgrade Mechanics 

    public void SetUpgredeVisual()
    {
        if (bIsUpgraderActive)
        {
            cameraController.FocusOnTarget(upGrader.transform);

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
            TaskManager.instance?.OnTaskComplete(currentLevelData.nextTask);
        }
        else
        {
            OnUpgrade();
        }
    }

    public void SetTakeMoneyData(int cost)
    {
        DOVirtual.DelayedCall(0.5f, () => upGrader.SetData(cost));
        if (bIsUnlock) upGrader.SetUpgraderSprite();
    }

    public void OnUpgrade()
    {
        bIsUpgraderActive = false;
        SetUpgredeVisual();
        currentLevel++;
        currentLevelData = levels[currentLevel];
        roundUpgradePartical.ForEach(X => X.Play());
        TaskManager.instance?.OnTaskComplete(currentLevelData.nextTask);
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

    #region MoveAble Thing

    //public void GoToTragetPostion()
    //{
    //    if (Seat != null)
    //    {


    //    }
    //}

    public void SetMainSeat(Seat seat)
    {
        this.seat = seat;
    }

    #endregion
}
