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
    public ARoom[] RadomExitsRoom;
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

    public ARoom GetRandomPharmacy()
    {
        if (storeRoom.bIsUnlock)
        {
            float index = Random.Range(0.1f, 1.1f);
            Debug.LogError(index);
            if (index > 0.6)
            {
                return pharmacyRoom;
            }
            else
            {
                return storeRoom;

            }
        }
        else
        {
            return pharmacyRoom;
        }

    }
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
        //Debug.LogError("uritiureyituer");

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

    float indexDes;
    //public ARoom GetRoom(DiseaseType diseaseType)
    //{
    //    indexDes = Random.Range(0, 10);
    //    switch (diseaseType)
    //    {
    //        case DiseaseType.Cough: return pharmacyRoom;
    //        case DiseaseType.Cold: return pharmacyRoom;
    //        case DiseaseType.Fever:


    //            return InjectionRoom;

    //        case DiseaseType.Heartworm_Disease:

    //            return InjectionRoom;

    //        case DiseaseType.Ear_Infection:
    //            if (indexDes <= 3)
    //            {
    //                return InjectionRoom;
    //            }
    //            else
    //            {
    //                return GroomingRoom;
    //            }
    //        case DiseaseType.Fleas_and_Ticks: return GroomingRoom;
    //        case DiseaseType.Allergies:
    //            if (indexDes >= 4)
    //            {
    //                return InjectionRoom;
    //            }
    //            else
    //            {
    //                return GroomingRoom;
    //            }
    //        case DiseaseType.Dental_Disease:
    //            if (indexDes >= 3)
    //            {
    //                return InjectionRoom;
    //            }
    //            else
    //            {
    //                return GroomingRoom;
    //            }
    //        case DiseaseType.Skin_Infecction: return GroomingRoom;
    //        case DiseaseType.Rabies:
    //            if (indexDes >= 3)
    //            {
    //                return InjectionRoom;
    //            }
    //            else
    //            {
    //                return GroomingRoom;
    //            }
    //        case DiseaseType.Vomitting:
    //            if (indexDes >= 4)
    //            {
    //                return InjectionRoom;
    //            }
    //            else
    //            {
    //                return GroomingRoom;
    //            }
    //        case DiseaseType.Bloat: return InjectionRoom;
    //        case DiseaseType.Bladder_Stones:
    //            return MriRoom;
    //        case DiseaseType.Fractures:
    //            return MriRoom;

    //        case DiseaseType.Kidney_Disease:
    //            if (indexDes >= 5)
    //            {
    //                return IcuRoom;
    //            }
    //            else
    //            {
    //                return MriRoom;
    //            }
    //        case DiseaseType.Asthma: return IcuRoom;
    //        case DiseaseType.Toy: return storeRoom;
    //        default:
    //            return null;
    //    }
    //}

    public ARoom GetRoom(DiseaseType diseaseType)
    {


        List<ARoom> roomsToCheck = diseaseType switch
        {
            DiseaseType.Cough => new List<ARoom> { pharmacyRoom, MriRoom },
            DiseaseType.Cold => new List<ARoom> { pharmacyRoom, IcuRoom },
            DiseaseType.Fever => new List<ARoom> { InjectionRoom, IcuRoom },
            DiseaseType.Heartworm_Disease => new List<ARoom> { InjectionRoom, MriRoom },
            DiseaseType.Ear_Infection => new List<ARoom> { GroomingRoom, OpreationRoom },
            DiseaseType.Fleas_and_Ticks => new List<ARoom> { GroomingRoom },
            DiseaseType.Allergies => new List<ARoom> { InjectionRoom, GroomingRoom, IcuRoom },
            DiseaseType.Dental_Disease => new List<ARoom> { InjectionRoom, GroomingRoom, OpreationRoom },
            DiseaseType.Skin_Infecction => new List<ARoom> { GroomingRoom, IcuRoom },
            DiseaseType.Rabies => new List<ARoom> { InjectionRoom, GroomingRoom, IcuRoom },
            DiseaseType.Vomitting => new List<ARoom> { InjectionRoom, GroomingRoom, MriRoom },
            DiseaseType.Bloat => new List<ARoom> { MriRoom, InjectionRoom },
            DiseaseType.Bladder_Stones => new List<ARoom> { MriRoom, OpreationRoom },
            DiseaseType.Fractures => new List<ARoom> { MriRoom, IcuRoom },
            DiseaseType.Kidney_Disease => new List<ARoom> { MriRoom, IcuRoom },
            DiseaseType.Asthma => new List<ARoom> { OpreationRoom, IcuRoom },
            DiseaseType.Toy => new List<ARoom> { storeRoom },
            _ => null,
        };

        return GetRandomRoom(roomsToCheck);


    }

    public ARoom GetRandomRoom(List<ARoom> rooms)
    {
        if (rooms == null || rooms.Count == 0)
            return null;

        var availableRooms = rooms.Where(r => r != null && r.bIsUnlock).ToList();
        if (availableRooms.Count == 0)
            return null;

        int randomIndex = Random.Range(0, availableRooms.Count);
        return availableRooms[randomIndex];
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
