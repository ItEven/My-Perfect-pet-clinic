using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PharmacyTable : Bed
{
    public override void SetUpPlayer()
    {
        base.SetUpPlayer();
        playerController.animationController.PlayAnimation(AnimType.Idle);
    }
    public override void StartProcessPatients()
    {
        if (patient == null) return;

        var workingAnimation = seat.workingAnim;
        var processTime = staffNPC.currentLevelData.processTime;

        if (staffNPC.bIsUnlock && staffNPC.bIsOnDesk)
        {
            StartPatientProcessing(staffNPC.animationController, workingAnimation, AnimType.Idle, staffNPC.currentLevelData.processTime, () =>
            {
                OnProcessComplite(null,staffNPC.animationController, AnimType.Idle);
            });
        }
        else if (bIsPlayerOnDesk)
        {
            StartPatientProcessing(playerController.animationController, workingAnimation, AnimType.Idle, staffNPC.currentLevelData.processTime, () =>
            {
                OnProcessComplite(null,playerController.animationController, AnimType.Idle);
            });
        }
    }
    public override void OnProcessComplite(ARoom nextRoom, AnimationController animationController, AnimType idleAnim)
    {
        room.moneyBox.TakeMoney(hospitalManager.GetCustomerCost(patient, room.diseaseData, staffNPC.currentLevelData.StaffExprinceType));
        worldProgresBar.fillAmount = 0;

        animationController.PlayAnimation(idleAnim);
        patient.NPCMovement.MoveToTarget(hospitalManager.GetRandomExit(), () =>
        {
            Destroy(patient.gameObject);
        });
        patient.animal.SetPartical(hospitalManager.GetAnimalMood());
        patient.MoveAnimal();
        patient = null;
       
    }
}
