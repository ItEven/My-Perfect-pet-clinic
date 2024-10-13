using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
    public RoomHandler nextRoomUpgrader;
}


public class RoomHandler : MonoBehaviour
{

    public RoomData roomData;
    public RoomLevelData[] roomLevels;


    [Header("Room Visuals")]
    public GameObject[] unlockObjs;
    public GameObject[] lockedObjs;
    public TakeMoney takeMoney;
    public TextMesh currntUpgradeCostText;


    [Header("Room datas")]
    public int unlockPrice;
    public int baseUpgradeCost;
    public int CurrntUpgradeCost;
    internal int currntUpgradeCost
    {
        get { return CurrntUpgradeCost; }
        set
        {
            CurrntUpgradeCost = value;
            currntUpgradeCostText.text = $"{uiManager.ScoreShow(CurrntUpgradeCost)}";
        }
    }
    public int baseUpgradeCostMultipliers;
    public int maxLevel;

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
        gameManager = GameManager.Instance;
        uiManager = saveManager.uiManager;
    }
    private void Start()
    {
        loadData();

    }

    public void loadData()
    {
        SetVisual();

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
    }

    public void SetData()
    {
        if (roomData.bIsUnlock)
        {
            if (roomData.bIsTakeMoneyActive)
            {
                currntUpgradeCost = roomData.currntUpgradeCost;
             
            }
            else
            {
                takeMoney.gameObject.SetActive(false);
            }
        }
        else
        {
            currntUpgradeCost = unlockPrice;
        }
    }

    public void SetTakeMoneyData()
    {

    }


    #region Upgrade Mechanics


    #endregion
}
