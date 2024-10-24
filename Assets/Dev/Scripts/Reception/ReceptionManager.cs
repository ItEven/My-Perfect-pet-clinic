using DG.Tweening;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ReceptionManager : MonoBehaviour
{
    [Header("Task Number")]
    public int currentTask;

    [Header("Reception Details")]
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
    public MoneyBox moneyBox;
    internal WaitingQueue waitingQueue;



    [Header(" NPC Details")]
    public ReceptionNPC npc;


    [Header(" Visuals Details")]
    public GameObject[] unlockObjs;
    public GameObject[] lockedObjs;
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
            npc.gameObject.SetActive(true);

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
            npc.gameObject.SetActive(false);

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
        npc.LoadNextUpgrade();
    }

    #endregion

    #region Prosece Mechanics

    public void OnPlayerTriger()
    {
        if (!npc.bIsUnlock)
        {

            gameManager.playerController._characterMovement.enabled = false;
            gameManager.playerController.enabled = false;
            gameManager.playerController.bhasSit = true;
            gameManager.playerController.joystick.gameObject.SetActive(false);
            gameManager.playerController.animationController.PlayAnimation(AnimType.Typing);
            gameManager.playerController.transform.SetParent(npc.sitPos);
            gameManager.playerController.transform.position = npc.sitPos.position;
            gameManager.playerController._characterMovement.rotatingObj.rotation = npc.sitPos.rotation;


            DOVirtual.DelayedCall(3f, () =>
            {
                gameManager.playerController.transform.SetParent(null);
                gameManager.playerController.joystick.gameObject.SetActive(true);
                gameManager.playerController.joystick.OnPointerUp(null);
                gameManager.playerController._characterMovement.enabled = true;




            });
        }
    }


    #endregion

}
