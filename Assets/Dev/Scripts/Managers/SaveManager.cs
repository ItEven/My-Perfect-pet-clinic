using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    [Header("All Managers")]
    public EconomyManager economyManager;
    public ReceptionManager receptionManager;
    public UiManager uiManager;

    public GameData gameData = new GameData();

    private void Awake()
    {
        instance = this;

        if (PlayerPrefs.HasKey("GameData"))
        {
            LoadData();
        }
        else
        {
            //for (int i = 0; i < universityManager.listOfUniversityHandlers.Count; i++)
            //{
            //    universityManager.listOfUniversityHandlers[i].LoadData();
            //}
        }

    }



    public void SaveData()
    {

        string jsonData = JsonUtility.ToJson(gameData);
        PlayerPrefs.SetString("GameData", jsonData);
    }

    public void LoadData()
    {
        string jsonData = PlayerPrefs.GetString("GameData");
        gameData = JsonUtility.FromJson<GameData>(jsonData);

    }
    private void OnApplicationQuit()
    {

        SaveData();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveData();
        }
    }
}

[Serializable, HideInInspector]
public class GameData
{
    public EconomyDatas economyDatas;
    public ReceptionData receptionData;
}

[Serializable]
public class EconomyDatas
{
    public double totalPetMoney;
    public long totalGems;
}

[Serializable]
public class ReceptionData
{
    public int currntLevel;
    public double totalCash;
    public bool bIsUnlock;
    public bool bIsAnyUpgradeAveilable;
}


