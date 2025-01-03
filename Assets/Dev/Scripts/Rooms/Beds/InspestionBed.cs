using MoreMountains.Tools;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspestionBed : Bed
{
    // public bool bCanFindNextRoom = false;
    public override void SetUpPlayer()
    {
        //   playerController.animationController.PlayAnimation(AnimType.Idle);
        base.SetUpPlayer();
    }

    [Button("StratProsses")]
    public override void StartProcessPatients()
    {
        if (patient == null) return;

        var nextRoom = hospitalManager.GetRoom(patient.diseaseType);
        var workingAnimation = seat.workingAnim;
        var processTime = staffNPC.currentLevelData.processTime;


        if (staffNPC.bIsUnlock && staffNPC.bIsOnDesk)
        {
            patient.StopWatting();

            StartPatientProcessing(staffNPC.animationController, workingAnimation, seat.idleAnim, staffNPC.currentLevelData.processTime, () =>
            {

                OnProcessComplite(nextRoom, staffNPC.animationController, seat.idleAnim);
            });
        }
        else if (bIsPlayerOnDesk)
        {

            bIsProcessing = true;

            patient.StopWatting();
            StartPatientProcessing(playerController.animationController, workingAnimation, seat.idleAnim, staffNPC.currentLevelData.processTime, () =>
            {
                OnProcessComplite(nextRoom, playerController.animationController, seat.idleAnim);
            });
        }
    }

    public override void OnProcessComplite(ARoom nextRoom, AnimationController animationController, AnimType idleAnim)
    {
        // Debug.LogError("OnProcessComplite6");
        room.moneyBox.TakeMoney(hospitalManager.GetCustomerCost(patient, room.diseaseData, staffNPC.currentLevelData.StaffExprinceType));
        worldProgresBar.fillAmount = 0;
        bIsProcessing = false;

        animationController.PlayAnimation(idleAnim);
        if (nextRoom != null)
        {
            if (!nextRoom.bIsUnRegisterQueIsFull())
            //if (!nextRoom.waitingQueue.bIsQueueFull())
            {
                nextRoom.RegisterPatient(patient);
                Debug.LogError(" ques is not full");
                MoveAnimal(patient.animal);
            }
            else
            {
                Debug.LogError(" room is not null and que is full");

                patient.MoveToExit(hospitalManager.GetRandomExit(patient), hospitalManager.GetAnimalMood());
                saveManager.gameData.hospitalData.failedPatientCount++;
                MoveAnimal(patient.animal);
            }
        }
        else
        {
            //  hospitalManager.OnPatientRegister();
            Debug.LogError(" room is null");
            patient.MoveToExit(hospitalManager.GetRandomExit(patient), hospitalManager.GetAnimalMood());

            MoveAnimal(patient.animal);
        }
        hospitalManager.OnRoomHaveSpace();

    }
}
