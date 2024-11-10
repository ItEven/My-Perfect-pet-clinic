using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspestionBed : Bed
{
    public override void SetUpPlayer()
    {
        base.SetUpPlayer();
        playerController.animationController.PlayAnimation(AnimType.Idle);
    }
    public override void StartProcessPatients()
    {
        if(patient == null) return;

        var nextRoom = hospitalManager.GetRoom(patient.diseaseType);
        var workingAnimation = seat.workingAnim;
        var processTime = staffNPC.currentLevelData.processTime;

        if (staffNPC.bIsUnlock && staffNPC.bIsOnDesk)
        {
            StartPatientProcessing(staffNPC.animationController, workingAnimation, AnimType.Idle, staffNPC.currentLevelData.processTime, () =>
            {
                OnProcessComplite(nextRoom,staffNPC.animationController, AnimType.Idle);
            });
        }
        else if (bIsPlayerOnDesk)
        {
            StartPatientProcessing(playerController.animationController, workingAnimation, AnimType.Idle, staffNPC.currentLevelData.processTime, () =>
            {
                OnProcessComplite(nextRoom,playerController.animationController, AnimType.Idle);
            });
        }
    }

    public override void OnProcessComplite(ARoom nextRoom, AnimationController animationController, AnimType idleAnim)
    {
        room.moneyBox.TakeMoney(hospitalManager.GetCustomerCost(patient, room.diseaseData, staffNPC.currentLevelData.StaffExprinceType));
        worldProgresBar.fillAmount = 0;

        if (nextRoom.bIsUnRegisterQueIsFull() || nextRoom == null)
        {
            animationController.PlayAnimation(idleAnim);
            patient.NPCMovement.MoveToTarget(hospitalManager.GetRandomExit(), () =>
            {
                Destroy(patient.gameObject);
            });
            patient.animal.SetPartical(hospitalManager.GetAnimalMood());
        }
        else
        {
            nextRoom.RegisterPatient(patient);
        }
        patient.MoveAnimal();
        patient = null;
    }
}