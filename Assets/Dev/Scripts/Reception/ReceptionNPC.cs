using DG.Tweening;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ReceptionNPCLevelDetail
{
    public int levelNum, upgradeCost, customerCost, nextTask;
    public float processTime;
}
public class ReceptionNPC : MonoBehaviour
{
    [Header(" Reception Details")]

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
    public UnityEvent OnUnlockNpc;


    [Header(" Level Details")]
    public int currentLevel;
    internal int nextLevel;
    internal ReceptionNPCLevelDetail currentLevelData;
    public ReceptionNPCLevelDetail[] levels;

    [Header(" Visuals Details")]
    public AnimationController animationController;
    public Transform sitPos;
    public GameObject npcObj;
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
        currentLevelData = levels[currentLevel];
        if (currentLevel >= levels.Length - 1)
        {
            bIsUpgraderActive = false;
            upGrader.gameObject.SetActive(false);
        }

        npcObj.transform.position = sitPos.position;
        npcObj.transform.rotation = sitPos.rotation;

        if (bIsUnlock)
        {
            gameManager.DropObj(npcObj);
    
        }
        else
        {
            npcObj.SetActive(false);
        }
        // gameManager.ReBuildNavmesh();
    }

    public void SetUpgredeVisual()
    {

        if (bIsUpgraderActive)
        {
            if (upGrader)
            {
                TaskManager.instance.target = upGrader.transform;

                CameraController.Instance.FocusOnTarget(upGrader.transform);
                SetTakeMoneyData(currentCost);

            }
        }
        else
        {
            upGrader.gameObject.SetActive(false);
        }

    }


    #region Upgrade Mechanics 
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
            roundUpgradePartical.ForEach(X => X.Play());
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
        if (bIsUnlock) upGrader.SetUpgraderSprite();
    }

    public void OnUpgrade()
    {
        bIsUpgraderActive = false;
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
