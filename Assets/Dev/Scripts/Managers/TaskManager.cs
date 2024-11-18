using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Sirenix.OdinInspector;

public class TaskManager : MonoBehaviour
{
    public static TaskManager instance;
    // public event Action<int> OnTaskComplite;
    [Header("Other Scripts")]
    public CameraController cameraController;
    public PatientManager patientManager;

    [Header("Halls")]
    public HallManager hallManager_01;
    public HallManager hallManager_02;
    public HallManager hallManager_03;
    public HallManager hallManager_04;
    public HallManager hallManager_05;
    public HallManager hallManager_06;

    [Header("Reseption")]
    public ReceptionManager receptionManager;

    [Header("Rooms")]
    public ARoom InspectionRoom;
    public ARoom InspectionRoom_2;
    public ARoom InspectionRoom_3;
    public ARoom pharmacyRoom;
    public ARoom pharmacyRoom_2;
    public ARoom InjectionRoom;
    public ARoom GroomingRoom;
    public ARoom OpreationRoom;
    public ARoom MriRoom;
    public ARoom IcuRoom;
    public StorageRoom storageRoom;

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
                InspectionRoom.bIsUpgraderActive = true;
                InspectionRoom.SetUpgradeVisual(); break;
            case 2:
                pharmacyRoom.bIsUpgraderActive = true;
                pharmacyRoom.SetUpgradeVisual();
                break;
            case 3:
                patientManager.AddDisease(DiseaseType.Cough);
                patientManager.bIsUnlock = true;
                patientManager.gameObject.SetActive(true);
                patientManager.LoadData();
                //storageRoom.bIsUpgraderActive = true;
                //storageRoom.SetUpgradeVisual();
                OnTaskComplete(4);
                break;
            case 4:
                InspectionRoom.LoadNextForStaff(0);
                break;
            case 5:
                patientManager.AddDisease(DiseaseType.Cold);
                pharmacyRoom.LoadNextForStaff(0);
                break;
            case 6:
                InspectionRoom.LoadNextForStaff(0);
                break;
            case 7:
                receptionManager.LoadNextUpgrade();
                break;
            case 8:
                patientManager.AddDisease(DiseaseType.Heartworm_Disease);
                pharmacyRoom.LoadNextForStaff(0);
                break;
            case 9:
                InspectionRoom.LoadNextForStaff(0);
                break;
            case 10:
                receptionManager.LoadNextUpgrade();
                break;
            case 11:
                //  storageRoom.LoadNextUpgrade();
                OnTaskComplete(12);
                break;
            case 12:
                pharmacyRoom.LoadNextForStaff(0);
                break;
            case 13:
                receptionManager.LoadNextUpgrade();
                break;
            case 14:
                InspectionRoom.LoadNextForStaff(0);
                break;
            case 15:
                hallManager_02.bIsUpgraderActive = true;
                hallManager_02.SetUpgredeVisual(); break;
            case 16:
                InjectionRoom.bIsUpgraderActive = true;
                InjectionRoom.SetUpgradeVisual(); break;
            case 17:
                patientManager.AddDisease(DiseaseType.Fever);
                OnTaskComplete(18); break;
            case 18:
                InspectionRoom_2.bIsUpgraderActive = true;
                InspectionRoom_2.SetUpgradeVisual(); break;
            case 19:

                patientManager.AddDisease(DiseaseType.Rabies);
                patientManager.AddDisease(DiseaseType.Dental_Disease);
                patientManager.AddDisease(DiseaseType.Vomitting);
                InjectionRoom.LoadNextForStaff(0); break;
            case 20:
                GroomingRoom.bIsUpgraderActive = true;
                GroomingRoom.SetUpgradeVisual(); break;
            case 21:
                patientManager.AddDisease(DiseaseType.Fleas_and_Ticks);
                patientManager.AddDisease(DiseaseType.Ear_Infection);
                InspectionRoom_2.LoadNextForStaff(0); break;
            case 22:
                InjectionRoom.LoadNextForStaff(0); break;
            case 23:
                GroomingRoom.LoadNextForStaff(0); break;
            case 24:
                patientManager.AddDisease(DiseaseType.Allergies);
                patientManager.AddDisease(DiseaseType.Skin_Infecction);
                InspectionRoom_2.LoadNextForStaff(0);
                break;
            case 25:
                OnTaskComplete(26);
                break;
            case 26: InjectionRoom.LoadNextForStaff(0); break;
            case 27: GroomingRoom.LoadNextForStaff(0); break;
            case 28: InspectionRoom_2.LoadNextForStaff(0); break;
            case 29:
                GroomingRoom.LoadNextForStaff(0); break;
            case 30:
                InjectionRoom.LoadNextForStaff(0); break;
            case 31: InjectionRoom.LoadNextUpgrade(); break;
            case 32: GroomingRoom.LoadNextForStaff(0); break;
            case 33:
                InjectionRoom.LoadNextForStaff(1);
                break;
            case 34: InspectionRoom_2.LoadNextForStaff(0); break;
            case 35: InjectionRoom.LoadNextForStaff(1); break;
            case 36:
                MriRoom.bIsUpgraderActive = true;
                MriRoom.SetUpgradeVisual();
                break;
            case 37:
                InspectionRoom_3.bIsUpgraderActive = true;
                InspectionRoom_3.SetUpgradeVisual();
                break;
            case 38:
                patientManager.AddDisease(DiseaseType.Bloat);
                InjectionRoom.LoadNextForStaff(1);
                break;
            case 39:
                MriRoom.LoadNextForStaff(0);
                break;
            case 40:
                InspectionRoom_3.LoadNextForStaff(0); break;
            case 41: InjectionRoom.LoadNextForStaff(1); break;
            case 42:
                OpreationRoom.bIsUpgraderActive = true;
                OpreationRoom.SetUpgradeVisual();
                break;
            case 43:
                patientManager.AddDisease(DiseaseType.Bladder_Stones);
                patientManager.AddDisease(DiseaseType.Fractures);
                MriRoom.LoadNextForStaff(0);
                break;
            case 44:
                OpreationRoom.LoadNextForStaff(0); break;
            case 45:
                InspectionRoom_3.LoadNextForStaff(0); break;
            case 46:
                OpreationRoom.LoadNextForStaff(0); break;
            case 47:
                InspectionRoom_3.LoadNextForStaff(0); break;
            case 48:
                MriRoom.LoadNextForStaff(0); break;
            case 49: OpreationRoom.LoadNextForStaff(0); break;
            case 50:
                IcuRoom.bIsUpgraderActive = true;
                IcuRoom.SetUpgradeVisual(); break;
            case 51:
                patientManager.AddDisease(DiseaseType.Kidney_Disease);
                patientManager.AddDisease(DiseaseType.Asthma);
                InspectionRoom_3.LoadNextForStaff(0); break;
            case 52:
                MriRoom.LoadNextForStaff(0); break;
            case 53:
                MriRoom.LoadNextUpgrade(); break;
            case 54:
                IcuRoom.LoadNextForStaff(0); break;
            case 55:
                OpreationRoom.LoadNextForStaff(0); break;
            case 56:
                OpreationRoom.LoadNextUpgrade(); break;
            case 57:
                IcuRoom.LoadNextForStaff(0); break;
            case 58:
                OpreationRoom.LoadNextForStaff(1); break;
            case 59:
                MriRoom.LoadNextForStaff(1); break;
            case 60:
                OpreationRoom.LoadNextForStaff(1); break;
            case 61:
                IcuRoom.LoadNextForStaff(0); break;
            case 62:
                MriRoom.LoadNextForStaff(1); break;
            case 63:
                IcuRoom.LoadNextForStaff(0); break;
            case 64:
                OpreationRoom.LoadNextForStaff(1); break;
            case 65:
                MriRoom.LoadNextForStaff(1); break;
            case 66:
                OpreationRoom.LoadNextForStaff(1); break;
            case 67:
                MriRoom.LoadNextForStaff(1); break;
            case 68:
                IcuRoom.LoadNextUpgrade(); break;
            case 69:
                MriRoom.LoadNextUpgrade(); break;
            case 70:
                OpreationRoom.LoadNextUpgrade(); break;
            case 71:
                IcuRoom.LoadNextForStaff(1); break;
            case 72:
                MriRoom.LoadNextForStaff(2); break;
            case 73:
                OpreationRoom.LoadNextForStaff(2); break;
            case 74:
                IcuRoom.LoadNextForStaff(1); break;
            case 75:
                OpreationRoom.LoadNextForStaff(2); break;
            case 76:
                IcuRoom.LoadNextForStaff(1); break;
            case 77:
                MriRoom.LoadNextForStaff(2); break;
            case 78:
                IcuRoom.LoadNextForStaff(1); break;
            case 79:
                OpreationRoom.LoadNextForStaff(2); break;
            case 80:
                IcuRoom.LoadNextUpgrade(); break;
            case 81:
                MriRoom.LoadNextForStaff(2); break;
            case 82:
                IcuRoom.LoadNextForStaff(2); break;
            case 83:
                IcuRoom.LoadNextForStaff(2); break;
            case 84:
                MriRoom.LoadNextForStaff(2); break;
            case 85:
                IcuRoom.LoadNextForStaff(2); break;
            case 86:
                OpreationRoom.LoadNextForStaff(2); break;
            case 87:
                IcuRoom.LoadNextForStaff(2); break;

            default: break;

        }
    }
}
