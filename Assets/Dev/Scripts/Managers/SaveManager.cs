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
            //for (int i = 0; i < roomManager.roomHandlers.Length; i++)
            //{
            //    var room = roomManager.roomHandlers[i];
            //    room.loadData();
            //}
        }

    }

    public void SaveData()
    {
        gameData.roomHandlerDatas.Clear();


        //for (int i = 0; i < roomManager.roomHandlers.Length; i++)
        //{
        //    RoomHandlersData roomHandler = new RoomHandlersData();
        //    var room = roomManager.roomHandlers[i];
        //    if (room != null)
        //    {

        //        roomHandler.bIsUnlock = room.bIsUnlock;
        //        roomHandler.bIsUpgraderActive = room.bIsUpgraderActive;
        //        roomHandler.currentCost = room.currentCost;
        //    }
        //    gameData.roomHandlerDatas.Add(roomHandler);

        //}

        string jsonData = JsonUtility.ToJson(gameData);
        PlayerPrefs.SetString("GameData", jsonData);
    }

    public void LoadData()
    {
        string jsonData = PlayerPrefs.GetString("GameData");
        gameData = JsonUtility.FromJson<GameData>(jsonData);

        // RoomHandlers

        //for (int i = 0; i < gameData.roomHandlerDatas.Count; i++)
        //{
        //    var room = gameData.roomHandlerDatas[i];
        //    roomManager.roomHandlers[i].bIsUnlock = room.bIsUnlock;
        //    roomManager.roomHandlers[i].bIsUpgraderActive = room.bIsUpgraderActive;
        //    roomManager.roomHandlers[i].currentCost = room.currentCost;
        //    roomManager.roomHandlers[i].loadData();
        //}

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
    public List<RoomHandlersData> roomHandlerDatas = new List<RoomHandlersData>();


}

[Serializable]
public class EconomyDatas
{
    public double totalPetMoney;
    public long totalGems;
}

[Serializable]
public class RoomHandlersData
{
    public int currentCost;
    public bool bIsUnlock;
    public bool bIsUpgraderActive;
}





