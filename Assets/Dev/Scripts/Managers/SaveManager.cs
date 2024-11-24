using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    [Header("All Managers")]
    public EconomyManager economyManager;
    public GameManager gameManager;
    public UiManager uiManager;
    public HospitalManager hospitalManager;
    public CameraController cameraController;
    public TaskManager taskManager;

    public GameData gameData = new GameData();

    private void Awake()
    {
        instance = this;

        if (PlayerPrefs.HasKey("GameData"))
        {
            LoadData();
        }
    }

    public void SaveData()
    {
        if (!taskManager.hallManager_01.bIsUnlock) return;
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
    public PlayerData playerData;
    public HospitalData hospitalData;
  
}

[Serializable]
public class EconomyDatas
{
    public double totalPetMoney;
    public long totalGems;
}



[Serializable]
public class HospitalData
{
    public int patientCount;
    public int failedPatientCount;
}

[Serializable]
public class PlayerData
{
    public int speedLevel;
    public int profitLevel;
}







