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
        Vector3 bedOldPos = bedSeat.localPosition;
        if (staffNPC.bIsUnlock && staffNPC.bIsOnDesk)
        {
            patient.animal.transform.SetParent(bedSeat);
            bedSeat.DOLocalMoveZ(0f, 1f).OnComplete(() =>
            {
                patient.StopWatting();

                StartPatientProcessing(staffNPC.animationController, workingAnimation, AnimType.Idle, staffNPC.currentLevelData.processTime, () =>
                {
                    bedSeat.DOLocalMoveZ(bedOldPos.z, 1f).OnComplete(() =>
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
            bedSeat.DOLocalMoveZ(0f, 1f).OnComplete(() =>
            {
                patient.StopWatting();

                StartPatientProcessing(playerController.animationController, workingAnimation, AnimType.Idle, staffNPC.currentLevelData.processTime, () =>
                {
                    bedSeat.DOLocalMoveZ(bedOldPos.z, 1f).OnComplete(() =>
                    {
                        OnProcessComplite(pharmacyRoom, playerController.animationController, AnimType.Idle);
                    });
                });
            });
        }
    }
}
