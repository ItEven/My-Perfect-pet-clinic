using MoreMountains.Tools;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspestionBed : Bed
{
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

            StartPatientProcessing(staffNPC.animationController, workingAnimation, seat.idleAnim, staffNPC.currentLevelData.processTime, () =>
            {

                OnProcessComplite(nextRoom, staffNPC.animationController, seat.idleAnim);
            });
        }
        else if (bIsPlayerOnDesk)
        {

            bIsProcessing = true;

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
            if (nextRoom.bIsUnRegisterQueIsFull())
            {
                if (nextRoom != null)
                {
                    Debug.LogError(nextRoom.gameObject.name + "room is not null" + nextRoom.bIsUnRegisterQueIsFull() + "que is full" + nextRoom.waitingQueue.gameObject.name + nextRoom.bIsUnlock + "the was not unlock");

                }
                else
                {
                    Debug.LogError(room.gameObject.name + "room is null");
                }
                hospitalManager.OnPatientRegister();
                patient.MoveToExit(hospitalManager.GetRandomExit(patient));
                patient.emojisController.PlayEmoji(hospitalManager.GetAnimalMood());

            }
            else
            {
                nextRoom.RegisterPatient(patient);
            }
        }
        else
        {
            hospitalManager.OnPatientRegister();

            patient.MoveToExit(hospitalManager.GetRandomExit(patient));
            patient.emojisController.PlayEmoji(hospitalManager.GetAnimalMood());
        }

        MoveAnimal(patient.animal);
        hospitalManager.OnRoomHaveSpace();

    }
}
