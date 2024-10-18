using DG.Tweening;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeHandler : MonoBehaviour
{

    //publice
    public bool bIsUnlock;
    public bool bIsUpgraderActive;

    public Upgrader upGrader;
    public UpgradeHandler[] nextUpgrader;


    [Header("Room Visuals")]
    public GameObject[] unlockObjs;
    public GameObject[] lockedObjs;
    public ParticleSystem[] roundUpgradePartical;


    public int unlockPrice;
    internal int currentCost
    {
        get { return upGrader.needMoney; }
        set
        {
            upGrader.needMoney = value;
        }
    }

    // private


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

    public virtual void Start()
    {
        currentCost = unlockPrice;
        loadData();
    }

    public virtual void loadData()
    {
        UpdateInitializers();
        SetVisual();
        SetUpgredeVisual();
    }

    public virtual void SetVisual()
    {
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
        gameManager.ReBuildNavmesh();

    }

    public virtual void SetUpgredeVisual()
    {

        if (bIsUpgraderActive)
        {
            upGrader.gameObject.SetActive(true);

            SetTakeMoneyData();
        }
        else
        {
            upGrader.gameObject.SetActive(false);
        }

    }

    public virtual void SetTakeMoneyData()
    {
        DOVirtual.DelayedCall(0.5f, () => upGrader.SetData(currentCost));
    }

    public virtual void OnUnlockAndUpgrade()
    {

        if (nextUpgrader.Length > 0)
        {
            foreach (var item in nextUpgrader)
            {
                item.bIsUpgraderActive = true;
                item.SetUpgredeVisual();
            }
        }
        bIsUnlock = true;
        bIsUpgraderActive = false;
        SetVisual();
    }

}
