using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager Instance;

    SaveManager saveManager;
    EconomyDatas economyDatas;
    UiManager uiManager;


    public TextMeshProUGUI PetMoneyCountText, gemsCountText;
    public event Action OnPetMoneyChanged;


    private void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public double PetMoneyCount
    {
        get { return economyDatas.totalPetMoney; }
        set
        {
            economyDatas.totalPetMoney = value;
            OnPetMoneyChanged?.Invoke();
        }
    }

    public long GemsCount
    {
        get { return economyDatas.totalGems; }
        set
        {
            economyDatas.totalGems = value;
        }
    }

    private void Start()
    {
        saveManager = SaveManager.instance;
        uiManager = UiManager.instance;
        economyDatas = saveManager.gameData.economyDatas;
        UpdatePetMoneyCountUI();
        UpdateGemsCountUI();
    }

    public void UpdatePetMoneyCountUI()
    {
        PetMoneyCountText.text = "" + uiManager.ScoreShow(PetMoneyCount);
    }
    public void UpdateGemsCountUI()
    {

        // gemsCountText.text = "" + uiManager.ScoreShow(GemsCount);
    }


    [Button("Add PetMoney")]
    public void AddPetMoney(double amountToIncrease)
    {
        //if(GameManager.Instance.twoXEarningActive == true)
        //{
        //    amountToIncrease *= 2;
        //}
        PetMoneyCount += amountToIncrease;
        UpdatePetMoneyCountUI();

    }

    public void SpendPetMoney(double amountToReduce)
    {
        PetMoneyCount -= amountToReduce;
        if (PetMoneyCount < 0)
        {
            PetMoneyCount = 0;
        }
        UpdatePetMoneyCountUI();

    }



    public bool bCanWeSpendPetMoney(float Amt)
    {
        float i = (float)PetMoneyCount - Amt;
        if (i >= 0)
        {
            return true;
        }
       
        return false;
    }
    public void AddGems(long amountToIncrease)
    {
        GemsCount += amountToIncrease;
        UpdateGemsCountUI();

    }

    public void SpendGems(long amountToReduce)
    {
        GemsCount -= amountToReduce;
        if (GemsCount < 0)
        {
            GemsCount = 0;
        }
        UpdateGemsCountUI();

    }


    public bool CheckIntractable(double value)
    {
        if (PetMoneyCount >= value)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
