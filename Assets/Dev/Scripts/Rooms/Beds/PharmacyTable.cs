using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PharmacyTable : Bed
{
    public override void SetUpPlayer()
    {
        //playerController.animationController.PlayAnimation(seat.idleAnim);
        base.SetUpPlayer();
    }
    public override void StartProcessPatients()
    {
        if (patient == null) return;

        var workingAnimation = seat.workingAnim;
        var IdleAnimation = seat.idleAnim;
        var processTime = staffNPC.currentLevelData.processTime;

        if (staffNPC.bIsUnlock && staffNPC.bIsOnDesk)
        {
            StartPatientProcessing(staffNPC.animationController, workingAnimation, IdleAnimation, staffNPC.currentLevelData.processTime, () =>
            {
                OnProcessComplite(null,staffNPC.animationController, IdleAnimation);
            });
        }
        else if (bIsPlayerOnDesk)
        {

                bIsProcessing = true;
            StartPatientProcessing(playerController.animationController, workingAnimation, IdleAnimation, staffNPC.currentLevelData.processTime, () =>
            {

                OnProcessComplite(null,playerController.animationController, IdleAnimation);
            });
        }
    }
    public override void OnProcessComplite(ARoom nextRoom, AnimationController animationController, AnimType idleAnim)
    {
        room.moneyBox.TakeMoney(hospitalManager.GetCustomerCost(patient, room.diseaseData, staffNPC.currentLevelData.StaffExprinceType));
        worldProgresBar.fillAmount = 0;
        bIsProcessing = false;

        animationController.PlayAnimation(idleAnim);
        patient.MoveToExit(hospitalManager.GetRandomExit(patient));
        patient.emojisController.PlayEmoji(hospitalManager.GetAnimalMood());

        MoveAnimal(patient.animal);

    }
}
