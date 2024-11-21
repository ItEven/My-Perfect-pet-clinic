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
    public ARoom StoreRoom;
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
                patientManager.AddDisease(DiseaseType.Cold);
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
                patientManager.AddDisease(DiseaseType.Fever);
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
                patientManager.AddDisease(DiseaseType.Heartworm_Disease);

                receptionManager.LoadNextUpgrade();
                break;
            case 14:
                InspectionRoom.LoadNextForStaff(0);
                break;
            case 15:
                hallManager_02.bIsUpgraderActive = true;
                hallManager_02.SetUpgredeVisual(); break;
            case 16:
                patientManager.AddDisease(DiseaseType.Ear_Infection);

                InjectionRoom.bIsUpgraderActive = true;
                InjectionRoom.SetUpgradeVisual(); break;
            case 17:
                patientManager.AddDisease(DiseaseType.Fleas_and_Ticks);
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
                InspectionRoom_2.LoadNextForStaff(0);
                 break;
            case 21:
                patientManager.AddDisease(DiseaseType.Fleas_and_Ticks);
                patientManager.AddDisease(DiseaseType.Allergies);
                InjectionRoom.LoadNextForStaff(0);
                break;
            case 22:
                GroomingRoom.bIsUpgraderActive = true;
                GroomingRoom.SetUpgradeVisual();
                 break;

            case 23:
                InjectionRoom.LoadNextForStaff(0);
                break;
            case 24:
                GroomingRoom.LoadNextForStaff(0);
               
                
                break;
            case 25:
                InspectionRoom_2.LoadNextForStaff(0);
                break;
            case 26:
                patientManager.AddDisease(DiseaseType.Toy);
                OnTaskComplete(27);
                 break;
            case 27:
                hallManager_03.bIsUpgraderActive = true;
                hallManager_03.SetUpgredeVisual(); break;
            case 28:
                StoreRoom.bIsUpgraderActive = true;
                StoreRoom.SetUpgradeVisual(); break;
            case 29:
                patientManager.AddDisease(DiseaseType.Vomitting);
                GroomingRoom.LoadNextForStaff(0); break;
            case 30:
                InspectionRoom_2.LoadNextForStaff(0);
                break;
            case 31: GroomingRoom.LoadNextForStaff(0); break;
            case 32: StoreRoom.LoadNextForStaff(0); break;
            case 33:
                InjectionRoom.LoadNextForStaff(0);
                break;
            case 34: InjectionRoom.LoadNextUpgrade(); 
                patientManager.AddDisease(DiseaseType.Dental_Disease);
                ; break;
            case 35:
                hallManager_04.bIsUpgraderActive = true;
                patientManager.AddDisease(DiseaseType.Bloat);
                hallManager_04.SetUpgredeVisual(); break;
            case 36:
                InspectionRoom_3.bIsUpgraderActive = true;
                InspectionRoom_3.SetUpgradeVisual();
                break;
            case 37:
                GroomingRoom.LoadNextForStaff(0);
                break;
            case 38:

                StoreRoom.LoadNextForStaff(0); 
                break;
            case 39:
                InjectionRoom.LoadNextForStaff(1);
                break;
            case 40:
                StoreRoom.LoadNextForStaff(0);
                patientManager.AddDisease(DiseaseType.Rabies); break;
            case 41: InspectionRoom_2.LoadNextForStaff(0); break;
            case 42:
                InjectionRoom.LoadNextForStaff(1);
                break;
            case 43:
                patientManager.AddDisease(DiseaseType.Bladder_Stones);
                StoreRoom.LoadNextForStaff(0);
                break;
            case 44:
                InspectionRoom_3.LoadNextForStaff(0); break;
            case 45:
                InjectionRoom.LoadNextForStaff(1); break;
            case 46:
                hallManager_05.bIsUpgraderActive = true;
                hallManager_05.SetUpgredeVisual();  break;
            case 47:
                patientManager.AddDisease(DiseaseType.Fractures);

                MriRoom.bIsUpgraderActive = true;
                MriRoom.SetUpgradeVisual(); break;
            case 48:
                InjectionRoom.LoadNextForStaff(1); break;
            case 49: MriRoom.LoadNextForStaff(0); break;
            case 50:
                InspectionRoom_3.LoadNextForStaff(0); break;
            case 51:
                MriRoom.LoadNextForStaff(0); break;
            case 52:
                patientManager.AddDisease(DiseaseType.Kidney_Disease);
                hallManager_06.bIsUpgraderActive = true;
                hallManager_06.SetUpgredeVisual(); break; 
            case 53:
                OpreationRoom.bIsUpgraderActive = true;
                OpreationRoom.SetUpgradeVisual(); 
                break;
            case 54:
                InspectionRoom_3.LoadNextForStaff(0); break;
            case 55:
                OpreationRoom.LoadNextForStaff(0); break;
            case 56:
                InspectionRoom_3.LoadNextForStaff(0);
                patientManager.AddDisease(DiseaseType.Asthma);
                 break;
            case 57:
                MriRoom.LoadNextUpgrade(); break;
            case 58:
                OpreationRoom.LoadNextForStaff(0); break;
            case 59:
                MriRoom.LoadNextForStaff(0); break;
            case 60:
                IcuRoom.bIsUpgraderActive = true;
                IcuRoom.SetUpgradeVisual(); break;
            case 61:
                MriRoom.LoadNextForStaff(1); break;
            case 62:
                OpreationRoom.LoadNextForStaff(0); break;
            case 63:
                MriRoom.LoadNextForStaff(1); break;
            case 64:
                OpreationRoom.LoadNextUpgrade(); break;
            case 65:
                MriRoom.LoadNextUpgrade(); break;
            case 66:
                OpreationRoom.LoadNextForStaff(1); break;
            case 67:
                MriRoom.LoadNextForStaff(2); break;
            case 68:
                IcuRoom.LoadNextUpgrade(); break;
            case 69:
                OpreationRoom.LoadNextForStaff(1); break;
            case 70:
                MriRoom.LoadNextForStaff(1); break;
            case 71:
                IcuRoom.LoadNextUpgrade(); break;
            case 72:
                MriRoom.LoadNextForStaff(0); break;
            case 73:
                OpreationRoom.LoadNextUpgrade(); break;
            case 74:
                MriRoom.LoadNextForStaff(2); break;
            case 75:
                IcuRoom.LoadNextForStaff(0); break;
            case 76:
                MriRoom.LoadNextForStaff(2); break;
            case 77:
                IcuRoom.LoadNextForStaff(1); break;
            case 78:
                MriRoom.LoadNextForStaff(1); break;
            case 79:
                OpreationRoom.LoadNextForStaff(2); break;
            case 80:
                OpreationRoom.LoadNextUpgrade(); break;
            case 81:
                IcuRoom.LoadNextForStaff(0); break;
            case 82:
                MriRoom.LoadNextForStaff(2); break;
            case 83:
                OpreationRoom.LoadNextForStaff(2); break;
            case 84:
                IcuRoom.LoadNextUpgrade(); break;
            case 85:
                IcuRoom.LoadNextForStaff(2); break;
            case 86:
                OpreationRoom.LoadNextForStaff(1); break;
            case 87:
                IcuRoom.LoadNextForStaff(1); break;
            case 88:
                OpreationRoom.LoadNextForStaff(0); break;
            case 89:
                IcuRoom.LoadNextForStaff(2); break;
            case 90:
                OpreationRoom.LoadNextForStaff(2); break;
            case 91:
                IcuRoom.LoadNextForStaff(1); break;
            case 92:
                IcuRoom.LoadNextForStaff(1); break;
            case 93:
                OpreationRoom.LoadNextForStaff(2); break;
            case 94:
                IcuRoom.LoadNextForStaff(2); break;
            case 95:
                IcuRoom.LoadNextForStaff(0); break;
            case 96:
                IcuRoom.LoadNextForStaff(2); break;

            default: break;

        }
    }
}
