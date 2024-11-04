using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

public class TaskManager : MonoBehaviour
{
    public static TaskManager instance;
    // public event Action<int> OnTaskComplite;
    public PatientManager patientManager;

    public HallManager hallManager_01;

    public ReceptionManager receptionManager;
    public RoomManager inspectionRoomManager_01;
    public PharmacyRoom pharmacy_room;
    public RoomManager storage_room;

    #region Initializers
    SaveManager saveManager;
    EconomyManager economyManager;
    GameManager gameManager;
    UiManager uiManager;

    private void Awake()
    {
        instance = this;
    }
    private void OnEnable()
    {
        UpdateInitializers();
        // OnTaskComplite += OnChcekTask;

    }
    private void OnDisable()
    {
        UpdateInitializers();
        //  OnTaskComplite -= OnChcekTask;

    }

    public void UpdateInitializers()
    {
        saveManager = SaveManager.instance;
        economyManager = saveManager.economyManager;
        gameManager = saveManager.gameManager;
        uiManager = saveManager.uiManager;
    }

    #endregion


    [Button("On Task Complite")]
    public void OnTaskComplete(int taskNum)
    {
        switch (taskNum)
        {
            case 0:
                receptionManager.bIsUpgraderActive = true;
                receptionManager.SetUpgredeVisual(); break;
            case 1:
                inspectionRoomManager_01.bIsUpgraderActive = true;
                inspectionRoomManager_01.SetUpgredeVisual();
                break;
            case 2:
                pharmacy_room.bIsUpgraderActive = true;
                pharmacy_room.SetUpgredeVisual();
                break;
            case 3:
                //pharmacy_room.bIsUpgraderActive = true;
                //storage_room.bIsUpgraderActive = true;
                //storage_room.SetUpgredeVisual();
                inspectionRoomManager_01.LoadNextUpgrade();
                patientManager.AddDisease(DiseaseType.Cold);
                patientManager.gameObject.SetActive(true);
                break;
            case 4:
                pharmacy_room.LoadNextUpgrade();
                break;
            case 5:
                receptionManager.LoadNextUpgrade();
                break;
            case 6:
                inspectionRoomManager_01.LoadNextUpgrade();
                break;
            case 7:
                receptionManager.LoadNextUpgrade();
                break;
            case 8:
                pharmacy_room.LoadNextUpgrade();
                break;
            case 9:
                inspectionRoomManager_01.LoadNextUpgrade();
                break;
            case 10:
                receptionManager.LoadNextUpgrade();
                break;
            case 11:
                pharmacy_room.LoadNextUpgrade();
                break;
            case 12:
                inspectionRoomManager_01.LoadNextUpgrade();
                break;
            case 13:
                receptionManager.LoadNextUpgrade();
                break;
            case 14:
                pharmacy_room.LoadNextUpgrade();
                break;


            default: break;

        }
    }
}
