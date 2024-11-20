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
        Vector3 bedOldPos = bedSeat.position;
        if (staffNPC.bIsUnlock && staffNPC.bIsOnDesk)
        {
            patient.animal.transform.SetParent(bedSeat);
            bedSeat.DOMoveZ(0f, 1f).OnComplete(() =>
            {
                StartPatientProcessing(staffNPC.animationController, workingAnimation, AnimType.Idle, staffNPC.currentLevelData.processTime, () =>
                {
                    bedSeat.DOMoveZ(bedOldPos.z, 1f).OnComplete(() =>
                    {
                        OnProcessComplite(pharmacyRoom, staffNPC.animationController, AnimType.Idle);
                    });
                });
            });

        }
        else if (bIsPlayerOnDesk)
        {
            bIsProcessing = true;       
            patient.animal.transform.SetParent(bedSeat);
            bedSeat.DOMoveZ(0f, 1f).OnComplete(() =>
            {
                StartPatientProcessing(playerController.animationController, workingAnimation, AnimType.Idle, staffNPC.currentLevelData.processTime, () =>
                {
                    bedSeat.DOMoveZ(bedOldPos.z, 1f).OnComplete(() =>
                    {
                        OnProcessComplite(pharmacyRoom, playerController.animationController, AnimType.Idle);
                    });
                });
            });
        }
    }
}
