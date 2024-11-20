using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;



public class InjectionBed : Bed
{
    public override void StartProcessPatients()
    {
        if (patient == null) return;

        var pharmacyRoom = hospitalManager.pharmacyRoom;
        var workingAnimation = seat.workingAnim;
        var processTime = staffNPC.currentLevelData.processTime;
        if (staffNPC.bIsUnlock && staffNPC.bIsOnDesk)
        {
            staffNPC.SetItemState(needIteam, true);
            StartPatientProcessing(staffNPC.animationController, workingAnimation, AnimType.Idle, staffNPC.currentLevelData.processTime, () =>
            {
                staffNPC.animationController.PlayAnimation(AnimType.StopInjecting);
                DOVirtual.DelayedCall(0.2f, () =>
                {
                    staffNPC.SetItemState(needIteam, false);
                    OnProcessComplite(pharmacyRoom, staffNPC.animationController, AnimType.Idle);
                });
            });
        }
        else if (bIsPlayerOnDesk)
        {
            playerController.SetItemState(needIteam, true);
            bIsProcessing = true;

            StartPatientProcessing(playerController.animationController, workingAnimation, AnimType.Idle, staffNPC.currentLevelData.processTime, () =>
            {
                playerController.animationController.PlayAnimation(AnimType.StopInjecting);
                DOVirtual.DelayedCall(0.2f, () =>
                {
                    playerController.SetItemState(needIteam, false);
                    OnProcessComplite(pharmacyRoom, playerController.animationController, AnimType.Idle);
                });

            });
        }

    }
}
