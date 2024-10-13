using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Level
{
    public int levelNum, amount, minFee, maxFee;
}


public class ReceptionManager : WaitingQueue
{
    public Level[] levels;
    public GameObject[] unlockObject;
    SaveManager saveManager;
    EconomyManager economyManager;
    ReceptionData receptionData;


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
        receptionData = saveManager.gameData.receptionData;

    }

    public void Start()
    {
        SetVisual();

        if (receptionData.bIsUnlock)
        {
            LoadData();
        }
    }

    public void SetVisual()
    {

    }


    public void LoadData()
    {
        for (int i = 0; i < receptionData.currntLevel; i++)
        {
            for (int j = 0; j < unlockObject.Length; j++)
            {
                var obj = unlockObject[j];
                obj.SetActive(true);
            }
        }
    }
}
