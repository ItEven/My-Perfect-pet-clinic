using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyBox : MonoBehaviour
{
    public float moneyGivingSpeed;
    public float totalMoneyInBox;
    public Transform[] moneyBricksPos;
    public List<SingleMoneybrick> singleMoneybricks = new List<SingleMoneybrick>();
    PlayerController playerController;

    SaveManager saveManager;
    UiManager uiManager;
    EconomyManager economyManager;


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
        uiManager = saveManager.uiManager;
    }

    #region Money give to player
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController = other.gameObject.GetComponent<PlayerController>();
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
            singleMoneybricks[singleMoneybricks.Count - 1].StartJump(playerController.moneyCollectPoint);
            int money = (int)(totalMoneyInBox / singleMoneybricks.Count);

            economyManager.AddPetMoney(money);
            totalMoneyInBox -= money;
            singleMoneybricks.RemoveAt(singleMoneybricks.Count - 1);

            yield return new WaitForSeconds(moneyGivingSpeed);

            if (singleMoneybricks.Count <= 0)
            {
                StopGiveMoney();
            }
        }

    }
    #endregion



}
