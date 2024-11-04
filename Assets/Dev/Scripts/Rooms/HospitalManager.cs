using System.Collections;
using System.Collections.Generic;
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
    [Header("All Room")]
    public RoomManager[] InspectionRoom;
    public PharmacyRoom pharmacyRoom;

    [Header("ExitTransfrom")]
    public Transform[] exitsPosses;

    [Header("Sprade Poses List")]
    public RegisterPos[] registerPoses;


    #region InspectionRoom Machenics
    public RoomManager GetInspectionRoom(Patient patient)
    {
        for (int i = 0; i < InspectionRoom.Length; i++)
        {
            var room = InspectionRoom[i];
            if (room != null && room.bIsUnlock)
            {
                for (int j = 0; j < room.diseaseTypes.Length; j++)
                {
                    if (patient.diseaseType == room.diseaseTypes[j])
                    {
                        return room;
                    }
                }
            }
        }
        return null;
    }

    #endregion

    #region Pharmacy Room Machenics



    #endregion

    public Transform GetRandomExit()
    {
        int randomIndex = Random.Range(0, exitsPosses.Length);
        return exitsPosses[randomIndex];
    }

    public bool CheckRegiterPosFull()
    {
        for (int i = 0; i < registerPoses.Length; i++)
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
        for (int i = 0; i < registerPoses.Length; i++)
        {
            var p = registerPoses[i];
            if (!p.bIsRegiseter)
            {
                p.bIsRegiseter = true;
                patient.registerPos = p;
                return p.pos;
            }
        }
        return null;
    }

}
