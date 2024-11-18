using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MriBed : Bed
{
    public Transform bedSeat;
    public float InOutTime;
    public override void StartProcessPatients()
    {
        if (patient == null) return;


        var pharmacyRoom = hospitalManager.pharmacyRoom;
        var workingAnimation = seat.workingAnim;
        var processTime = staffNPC.currentLevelData.processTime;
        if (staffNPC.bIsUnlock && staffNPC.bIsOnDesk)
        {
            patient.animal.transform.SetParent(bedSeat);
           

            StartPatientProcessing(staffNPC.animationController, workingAnimation, AnimType.Idle, staffNPC.currentLevelData.processTime, () =>
            {
                OnProcessComplite(pharmacyRoom, staffNPC.animationController, AnimType.Idle);
            });
        }
        else if (bIsPlayerOnDesk)
        {
            StartPatientProcessing(playerController.animationController, workingAnimation, AnimType.Idle, staffNPC.currentLevelData.processTime, () =>
            {
                OnProcessComplite(pharmacyRoom, playerController.animationController, AnimType.Idle);
            });
        }

    }
}
