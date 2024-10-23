using DG.Tweening;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectionRoomNpc : MonoBehaviour
{
    [Header(" Room Details")]

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


    [Header(" Level Details")]
    public int currentLevel;
    internal int nextLevel;
    internal ReceptionNPCLevelDetail currentLevelData;
    public ReceptionNPCLevelDetail[] levels;

    [Header(" Visuals Details")]
    public Transform sitPos;
    public GameObject npcObj;
    //public GameObject[] unlockObjs;
    //public GameObject[] lockedObjs;
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
        npcObj.transform.position = sitPos.position;
        npcObj.transform.rotation = sitPos.rotation;

        if (bIsUnlock)
        {

            gameManager.DropObj(npcObj);
            roundUpgradePartical.ForEach(X => X.Play());
        }
        else
        {
            npcObj.SetActive(false);
        }
        gameManager.ReBuildNavmesh();

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

        if (!bIsUnlock)
        {
            bIsUnlock = true;
            bIsUpgraderActive = false;
            currentLevel = 0;
            currentLevelData = levels[currentLevel];
            SetVisual();

        }
        else
        {

            OnUpgrade();
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
