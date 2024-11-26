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

       
        var pharmacyRoom = hospitalManager.MriRoom;

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
                //hospitalManager.OnPatientRegister();
                patient.MoveToExit(hospitalManager.GetRandomExit(patient), hospitalManager.GetAnimalMood());
                saveManager.gameData.hospitalData.failedPatientCount++;


            }
            else
            {
                nextRoom.RegisterPatient(patient);
            }
        }
        else
        {
            //hospitalManager.OnPatientRegister();

            patient.MoveToExit(hospitalManager.GetRandomExit(patient), hospitalManager.GetAnimalMood());

        }

        MoveAnimal(patient.animal);
        hospitalManager.OnRoomHaveSpace();

    }
}
