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
    public InspectionRoomManager inspectionRoomManager_01;
    public PharmacyRoom  pharmacy_room;
    public InspectionRoomManager storage_room;

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
                pharmacy_room.SetUpgredeVisual();
                patientManager.gameObject.SetActive(true);
                break;
            case 3:
                pharmacy_room.bIsUpgraderActive = true;
                storage_room.bIsUpgraderActive = true;
                storage_room.SetUpgredeVisual();
                break;

            case 4:
                receptionManager.LoadNextUpgrade();
                break;

            default: break;

        }
    }
}
