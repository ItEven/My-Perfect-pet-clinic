using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MoneyBox : MonoBehaviour
{
    public float moneyGivingSpeed;
    public int totalMoneyInBox;
    public float spreadRadius = 5f;
    public float spreadDuration = 0.5f;
    public Transform[] moneyBricksPos;
    public List<SingleMoneybrick> singleMoneybricks = new List<SingleMoneybrick>();

    int currntIndex;

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

    #region Money give to player
    private void Start()
    {
        TakeMoney(totalMoneyInBox);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //GiveMoney();
            StartGiveMoney();
        }
    } 
    
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //GiveMoney();
            StartGiveMoney();
        }
    }

    Coroutine giveMoneyCoroutine;

    public void StartGiveMoney()
    {
        if (giveMoneyCoroutine == null)
        {
            giveMoneyCoroutine = StartCoroutine(GiveingMoney());
        }
    }

    public void StopGiveMoney()
    {
        if (giveMoneyCoroutine != null)
        {
            StopCoroutine(giveMoneyCoroutine);
            giveMoneyCoroutine = null;
        }
    }


    IEnumerator GiveingMoney()
    {
        if (singleMoneybricks.Count <= 0)
        {
            StopGiveMoney();
        }
        while (singleMoneybricks.Count > 0)
        {
            singleMoneybricks[singleMoneybricks.Count - 1].StartJump(gameManager.playerController.moneyCollectPoint);
            int money = (int)(totalMoneyInBox / singleMoneybricks.Count);

            economyManager.AddPetMoney(money);
            totalMoneyInBox -= money;
            singleMoneybricks.RemoveAt(singleMoneybricks.Count - 1);
            currntIndex--;
            AudioManager.i.OnMonenyCollect();

            yield return new WaitForSeconds(moneyGivingSpeed);

            if (singleMoneybricks.Count <= 0)
            {
                totalMoneyInBox = 0;
                singleMoneybricks.Clear();
                StopGiveMoney();
            }
        }

    }
    #endregion



    void GiveMoney()
    {
        foreach (var obj in singleMoneybricks)
        {
            Vector3 randomDirection = new Vector3(
                Random.Range(-1f, 1f),
                Mathf.Abs(Random.Range(0f, 1f)),
                Random.Range(-1f, 1f)
            ).normalized * spreadRadius;


            obj.transform.DORotate(new Vector3(360, 360, 360), 1f, RotateMode.FastBeyond360)
    .SetLoops(-1, LoopType.Yoyo);

            obj.transform.DOMove(randomDirection, spreadDuration).OnComplete(() =>
            {
                obj.StartJump(gameManager.playerController.moneyCollectPoint);

                currntIndex--;
            }).SetEase(Ease.OutQuad);
        }

        singleMoneybricks.Clear();
        economyManager.AddPetMoney(totalMoneyInBox);
        totalMoneyInBox = 0;
    }
    
    [Button("TakeMoney")]
    public void TakeMoney(int amount)
    {
        totalMoneyInBox += amount;
        for (int i = 0; i < GetHowManyBrickSpwan(amount); i++)
        {
            GameObject brickInstance = Instantiate(gameManager.singleMoneybrick, moneyBricksPos[currntIndex]);
            var brick = brickInstance.GetComponent<SingleMoneybrick>();
            singleMoneybricks.Add(brick);
            currntIndex++;

        }
    }

    public int GetHowManyBrickSpwan(int amount)
    {
        return amount switch
        {

            >= 2000 => 40,
            >= 1200 => 35,
            >= 900 => 30,
            >= 700 => 25,
            >= 500 => 20,
            >= 400 => 16,
            >= 300 => 12,
            >= 240 => 9,
            >= 180 => 8,
            >= 120 => 7,
            >= 70 => 5,
            >= 40 => 4,
            >= 20 => 3,
            >= 10 => 2,
            >= 5 => 1,
            _ => -1,
        };
    }

}
