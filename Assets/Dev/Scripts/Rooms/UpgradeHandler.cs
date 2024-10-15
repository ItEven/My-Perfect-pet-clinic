using DG.Tweening;
using Sirenix.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


[System.Serializable]
public class RoomData
{
    public bool bIsUnlock;
    public bool bIsTakeMoneyActive;
    public int currntLevel;
    public int currntUpgradeCost;
}

[System.Serializable]
public class RoomLevelData
{
    public int upgradeCost;
    public UpgradeHandler[] nextUpgrader;
}


public class UpgradeHandler : MonoBehaviour
{

    public RoomData roomData;
    public RoomLevelData[] roomLevels;


    [Header("Room Visuals")]
    public GameObject[] unlockObjs;
    public GameObject[] lockedObjs;
    public ParticleSystem[] roundUpgradePartical;

    public static Transform STAFF_unlock_pos;
    public Upgrader upGrader;


    [Header("Room datas")]
    public int unlockPrice;
    internal int currntUpgradeCost
    {
        get { return upGrader.needMoney; }
        set
        {
            upGrader.needMoney = value;
        }
    }
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
    private void Start()
    {
        loadData();
    }

    public void loadData()
    {
        SetVisual();
        SetData();
    }

    public void SetVisual()
    {
        if (roomData.bIsUnlock)
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

    public void SetData()
    {
        if (roomData.bIsUnlock)
        {
            if (roomData.bIsTakeMoneyActive)
            {
                currntUpgradeCost = roomData.currntUpgradeCost;
                SetTakeMoneyData();
            }
            else
            {
                upGrader.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.LogError("Eles" + gameObject.name);

            if (!roomData.bIsTakeMoneyActive)
            {
                upGrader.gameObject.SetActive(false);
                Debug.LogError("upGrader.gameObject.SetActive(false)" + gameObject.name);

            }
            else
            {
                upGrader.gameObject.SetActive(true);
                Debug.LogError("upGrader.gameObject.SetActive(true)" + gameObject.name);


                currntUpgradeCost = unlockPrice;
                DOVirtual.DelayedCall(0.5f, () => upGrader.SetData(currntUpgradeCost));
            }
        }
    }

    public void SetTakeMoneyData()
    {
        DOVirtual.DelayedCall(0.5f, () => upGrader.SetData(currntUpgradeCost));

    }

    public void UpgradeChacker()
    {
        if (roomLevels[roomData.currntLevel].nextUpgrader != null)
        {
            foreach (var item in roomLevels[roomData.currntLevel].nextUpgrader)
            {
                item.roomData.bIsTakeMoneyActive = true;
                item.SetData();
            }
        }


        if (!roomData.bIsUnlock)
        {
            roomData.bIsTakeMoneyActive = false;
            roomData.bIsUnlock = true;
            SetVisual();


        }
        roomData.currntLevel++;
        //if (roomData.currntLevel <= roomLevels.Length)
        //{
        //    currntUpgradeCost = roomLevels[roomData.currntLevel].upgradeCost;
        //}

    }



}
