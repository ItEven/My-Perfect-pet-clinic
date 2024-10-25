using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class RegisterPos
{
    public bool bIsRegiseter;
    public Transform pos;
}
public class HospitalManager : MonoBehaviour
{
    //public RoomHandler[] roomHandlers;

    public InspectionRoomManager[] InspectionRoom;
   

    [Header("Sprade Poses List")]
    public RegisterPos[] registerPoses;


    #region InspectionRoom Machenics
    public InspectionRoomManager GetInspectionRoom(Patient patient)
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


    public bool CheckRegiterPosFull()
    {
        for (int i = 0; i < registerPoses.Length; i++)
        {
            var pos = registerPoses[i];
            if (!pos.bIsRegiseter)
            {
                return false;
            }
        }

        return true;
    }
    public Transform GetRandomPos()
    {
        for (int i = 0; i < registerPoses.Length; i++)
        {
            var p = registerPoses[i];
            if (!p.bIsRegiseter)
            {
                return p.pos;
            }
        }
        return null;
    }
}
