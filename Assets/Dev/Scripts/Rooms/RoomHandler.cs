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
    public GameObject groundCanvas;
    public TextMesh currntUpgradeCostText;

    [Header("Room datas")]
    public int unlockPrice;
    public int baseUpgradeCost;
    public int CurrntUpgradeCost;
    public int currentNeedMoney;
    public float totalTimeforMoneyCollect = 0.75f;

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
    PlayerController player;


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
        currentNeedMoney = roomData.currntUpgradeCost;
        if (roomData.bIsUnlock)
        {
            if (roomData.bIsTakeMoneyActive)
            {
                currntUpgradeCost = roomData.currntUpgradeCost;
            }
            else
            {
                groundCanvas.gameObject.SetActive(false);
            }
        }
        else
        {
            currntUpgradeCost = unlockPrice;
        }
    }

    public void SetNextUpgrader()
    {
        if (!roomLevels[roomData.currntLevel].nextRoomUpgrader.gameObject.activeInHierarchy)
        {
          roomLevels[roomData.currntLevel].nextRoomUpgrader.gameObject.SetActive(true);
        }
        roomLevels[roomData.currntLevel].nextRoomUpgrader.SetData();
    }




    #region Upgrade Mechanics


    private Coroutine takeMoneyCoroutine;

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && economyManager.bCanWeSpendPetMoney(currntUpgradeCost))
        {
            player = other.gameObject.GetComponent<PlayerController>();
            if (player.IsMoving())
            {
                StartTakeMoney();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StopTakeMoney();
        }
    }

    private void StartTakeMoney()
    {
        if (takeMoneyCoroutine == null)
        {
            takeMoneyCoroutine = StartCoroutine(TakingMoney());
        }
    }

    private void StopTakeMoney()
    {
        if (takeMoneyCoroutine != null)
        {
            currentNeedMoney = (int)currntUpgradeCost;
            StopCoroutine(takeMoneyCoroutine);
            takeMoneyCoroutine = null;
        }
    }

    float lastSub = 0f;
    private IEnumerator TakingMoney()
    {
        if (currntUpgradeCost <= 0) yield break;

        float elapsedTime = 0f;

        while (currntUpgradeCost > 0)
        {
            elapsedTime += Time.deltaTime;
            float percentageComplete = Mathf.Clamp01(elapsedTime / totalTimeforMoneyCollect);
            currntUpgradeCost = Mathf.RoundToInt(Mathf.Lerp(currntUpgradeCost, 0, percentageComplete));
            var val = currentNeedMoney - currntUpgradeCost;

            if (economyManager.bCanWeSpendPetMoney(val - lastSub))
            {
                Debug.Log("Current Value: " + currntUpgradeCost);

                GameObject brickInstance = Instantiate(gameManager.singleMoneybrick, player.moneyCollectPoint.position, Quaternion.identity, player.transform);
                var brick = brickInstance.GetComponent<SingleMoneybrick>();

                economyManager.SpendPetMoney(val - lastSub);
                lastSub = val;


                if (brick != null)
                {
                    brick.StartJump(transform);
                }

                if (currntUpgradeCost <= 0)
                {
                    groundCanvas.gameObject.SetActive(false);
                    SetNextUpgrader();
                    StopTakeMoney();
                    yield break;
                }
            }
            else
            {
                StopTakeMoney();
                yield break;
            }

            yield return null;
        }

        //if (needMoney <= 0)
        //{
        //    OnMoneyTakingFinish.Invoke();
        //    gameObject.SetActive(false);
        //    StopTakeMoney();
        //}
    }


    #endregion
}
