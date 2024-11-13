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
            //  Debug.LogError("OnProcessComplite1");
            StartPatientProcessing(staffNPC.animationController, workingAnimation, AnimType.Idle, staffNPC.currentLevelData.processTime, () =>
            {
                //  Debug.LogError("OnProcessComplite3");
                OnProcessComplite(nextRoom, staffNPC.animationController, AnimType.Idle);
            });
        }
        else if (bIsPlayerOnDesk)
        {
            //  Debug.LogError("OnProcessComplite4");
            StartPatientProcessing(playerController.animationController, workingAnimation, AnimType.Idle, staffNPC.currentLevelData.processTime, () =>
            {
                //  Debug.LogError("OnProcessComplite5");
                OnProcessComplite(nextRoom, playerController.animationController, AnimType.Idle);
            });
        }
    }

    public override void OnProcessComplite(ARoom nextRoom, AnimationController animationController, AnimType idleAnim)
    {
        // Debug.LogError("OnProcessComplite6");
        room.moneyBox.TakeMoney(hospitalManager.GetCustomerCost(patient, room.diseaseData, staffNPC.currentLevelData.StaffExprinceType));
        worldProgresBar.fillAmount = 0;

        animationController.PlayAnimation(idleAnim);
        if (nextRoom.bIsUnRegisterQueIsFull() || nextRoom == null || !nextRoom.bIsUnlock)
        {
            // Debug.LogError("OnProcessComplite8");
            patient.MoveToExit(hospitalManager.GetRandomExit(patient));
            patient.animal.emojisController.PlayEmoji(hospitalManager.GetAnimalMood());

        }
        else
        {
            // Debug.LogError("OnProcessComplite9");
            nextRoom.RegisterPatient(patient);
        }

        MoveAnimal(patient.animal);
    }
}
