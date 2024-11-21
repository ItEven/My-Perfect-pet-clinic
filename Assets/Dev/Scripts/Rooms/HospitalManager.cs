using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;


[System.Serializable]
public class RegisterPos
{
    public bool bIsRegiseter;
    public Transform pos;
}
public class HospitalManager : MonoBehaviour
{
    //public RoomHandler[] roomHandlers;
    public event Action OnPatientCountUpdate;

    [Header("All Room")]
    public ReceptionManager receptionManager;
    public ARoom[] InspectionRoom;
    public ARoom pharmacyRoom;
    public ARoom storeRoom;
    public StorageRoom storageRoom;
    public ARoom InjectionRoom;
    public ARoom GroomingRoom;
    public ARoom OpreationRoom;
    public ARoom MriRoom;
    public ARoom IcuRoom;


    [Header("ExitTransfrom")]
    public Transform[] exitsPosses;

    [Header("Sprade Poses List")]
    public List<RegisterPos> registerPoses = new List<RegisterPos>();


    #region InspectionRoom Machenics
    public ARoom GetInspectionRoom()
    {

        var avaiableRooms = InspectionRoom.Where(r => r.bIsUnlock).ToList();
        if (avaiableRooms.Count == 0)
            return null;
        int randomIndex = Random.Range(0, avaiableRooms.Count);
        var randomPos = avaiableRooms[randomIndex];
        return avaiableRooms[randomIndex];

        //for (int i = 0; i < InspectionRoom.Length; i++)
        //{
        //    var room = InspectionRoom[i];
        //    if (room != null && room.bIsUnlock)
        //    {
        //        for (int j = 0; j < room.diseaseTypes.Length; j++)
        //        {
        //            if (patient.diseaseType == room.diseaseTypes[j])
        //            {
        //                OnPatientRegister();
        //                return room;
        //            }
        //        }
        //    }
        //}
        //return null;
    }

    #endregion

    #region Pharmacy Room Machenics

    public void OnRoomHaveSpace()
    {
      //  Debug.LogError("yoyoooyo");
        receptionManager.StratProssesPatients();
    }

    #endregion

    public Transform GetRandomExit(Patient patient)
    {
        int randomIndex = Random.Range(0, exitsPosses.Length);
        if (randomIndex == 0)
        {
            patient.RightFollowPos = patient.leftFollowPos;
        }
        return exitsPosses[randomIndex];
    }

    public bool CheckRegiterPosFull()
    {
        for (int i = 0; i < registerPoses.Count; i++)
        {
            RegisterPos pos = registerPoses[i];
            if (!pos.bIsRegiseter)
            {
                return false;
            }
        }
        return true;
    }




    public Transform GetRandomPos(Patient patient)
    {

        var availablePositions = registerPoses.Where(p => !p.bIsRegiseter).ToList();


        if (availablePositions.Count == 0)
            return null;


        int randomIndex = Random.Range(0, availablePositions.Count);
        var randomPos = availablePositions[randomIndex];


        randomPos.bIsRegiseter = true;
        patient.registerPos = randomPos;

        return randomPos.pos;
    }

    public void OnPatientRegister()
    {
        OnPatientCountUpdate?.Invoke();

    }

    public int GetCustomerCost(Patient patient, DiseaseData diseaseData, StaffExprinceType StaffExprinceType)
    {
        if (patient != null)
        {
            for (int i = 0; i < diseaseData.diseases.Length; i++)
            {
                var dis = diseaseData.diseases[i];
                if (dis.Type == patient.diseaseType)
                {
                    switch (StaffExprinceType)
                    {
                        case StaffExprinceType.Intern: return dis.InternFee;
                        case StaffExprinceType.Junior: return dis.juniorVeterinarianFee;
                        case StaffExprinceType.Senior: return dis.seniorVeterinarianFee;
                        case StaffExprinceType.Chief: return dis.chiefVeterinarianFee;
                    }
                }
            }
        }
        return 0;
    }

    int indexDes;
    public ARoom GetRoom(DiseaseType diseaseType)
    {
        indexDes = Random.Range(0, 1);
        switch (diseaseType)
        {
            case DiseaseType.Cough: return pharmacyRoom;
            case DiseaseType.Cold: return pharmacyRoom;
            case DiseaseType.Fever:
                if (indexDes == 0)
                {
                    return pharmacyRoom;
                }
                else
                {
                    return InjectionRoom;
                }
            case DiseaseType.Heartworm_Disease:
                if (indexDes == 0)
                {
                    return pharmacyRoom;
                }
                else
                {
                    return InjectionRoom;
                }
            case DiseaseType.Ear_Infection:
                if (indexDes == 0)
                {
                    return InjectionRoom;
                }
                else
                {
                    return GroomingRoom;
                }
            case DiseaseType.Fleas_and_Ticks: return GroomingRoom;
            case DiseaseType.Allergies:
                if (indexDes == 0)
                {
                    return InjectionRoom;
                }
                else
                {
                    return GroomingRoom;
                }
            case DiseaseType.Dental_Disease:
                if (indexDes == 0)
                {
                    return InjectionRoom;
                }
                else
                {
                    return GroomingRoom;
                }
            case DiseaseType.Skin_Infecction: return GroomingRoom;
            case DiseaseType.Rabies:
                if (indexDes == 0)
                {
                    return InjectionRoom;
                }
                else
                {
                    return GroomingRoom;
                }
            case DiseaseType.Vomitting:
                if (indexDes == 0)
                {
                    return InjectionRoom;
                }
                else
                {
                    return GroomingRoom;
                }
            case DiseaseType.Bloat: return InjectionRoom;
            case DiseaseType.Bladder_Stones:
                if (indexDes == 0)
                {
                    return MriRoom;
                }
                else
                {
                    return pharmacyRoom;
                }
            case DiseaseType.Fractures:
                if (indexDes == 0)
                {
                    return MriRoom;
                }
                else
                {
                    return pharmacyRoom;
                }
            case DiseaseType.Kidney_Disease:
                if (indexDes == 0)
                {
                    return IcuRoom;
                }
                else
                {
                    return MriRoom;
                }
            case DiseaseType.Asthma: return IcuRoom;
            case DiseaseType.Toy: return storeRoom;
            default:
                return null;
        }
    }

    public MoodType GetAnimalMood()
    {
        switch (GetHowManyBrickSpwan())
        {
            case 1: return MoodType.Angry;
            case 2: return MoodType.Sad;
            case 3: return MoodType.Happy;
            case 4: return MoodType.CuteHappy;
            default:
                return MoodType.Happy;
        }
    }

    public int GetHowManyBrickSpwan()
    {
        int randomIndex = Random.Range(0, 10);

        return randomIndex switch
        {
            >= 9 => 4,
            >= 5 => 3,
            >= 3 => 2,
            >= 2 => 1,
            _ => -1,
        };
    }

}
