using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MriBed : Bed
{

    public Transform bedSeat;
    public float InOutTime;
    Transform lastPerent;

    public override void SetVisual()
    {
        DOVirtual.DelayedCall(1f, () =>
        {
           // Debug.LogError(bIsUnlock);
        base.SetVisual();
        });

        //if (bIsUnlock)
        //{
        //    foreach (var item in unlockObjs)
        //    {
        //        item.gameObject.SetActive(true);
        //    }
        //}
    }
    public override void StartProcessPatients()
    {
        if (patient == null) return;

        lastPerent = null;
        var opreationRoom = hospitalManager.OpreationRoom;

        var workingAnimation = seat.workingAnim;
        var processTime = staffNPC.currentLevelData.processTime;
        Vector3 bedOldPos = bedSeat.localPosition;
        lastPerent = bedSeat.transform.parent;
        patient.animal.transform.SetParent(bedSeat);
        if (staffNPC.bIsUnlock && staffNPC.bIsOnDesk)
        {
            patient.StopWatting();

            bedSeat.DOLocalMoveZ(0f, 1f).OnComplete(() =>
            {

                StartPatientProcessing(staffNPC.animationController, workingAnimation, AnimType.Idle, staffNPC.currentLevelData.processTime, () =>
                {
                    bedSeat.DOLocalMoveZ(bedOldPos.z, 1f).OnComplete(() =>
                    {
                        OnProcessComplite(opreationRoom, staffNPC.animationController, AnimType.Idle);
                    });
                });
            });
        }
        else if (bIsPlayerOnDesk)
        {
            bIsProcessing = true;
            patient.StopWatting();
            bedSeat.DOLocalMoveZ(0f, 1f).OnComplete(() =>
            {

                StartPatientProcessing(playerController.animationController, workingAnimation, AnimType.Idle, staffNPC.currentLevelData.processTime, () =>
                {
                    bedSeat.DOLocalMoveZ(bedOldPos.z, 1f).OnComplete(() =>
                    {
                        OnProcessComplite(opreationRoom, playerController.animationController, AnimType.Idle);
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

        animationController.PlayAnimation(onTrigger.seat.idleAnim);
        patient.animal.transform.SetParent(lastPerent);
        if (nextRoom != null)
        {
            if (nextRoom.bIsUnRegisterQueIsFull())
            {
                if (nextRoom != null)
                {
                    Debug.LogError(nextRoom.gameObject.name + "room is not null" + nextRoom.bIsUnRegisterQueIsFull() + "que is full" + nextRoom.waitingQueue.gameObject.name + nextRoom.bIsUnlock + "the was unlock");

                }

                //hospitalManager.OnPatientRegister();
                patient.MoveToExit(hospitalManager.GetRandomExit(patient), hospitalManager.GetAnimalMood());
                saveManager.gameData.hospitalData.failedPatientCount++;


            }
            else
            {
                nextRoom.RegisterPatient(patient);
                Debug.LogError(room.gameObject.name + "room is full");
                // patient = null;
            }
        }
        else
        {
            //hospitalManager.OnPatientRegister();

            patient.MoveToExit(hospitalManager.GetRandomExit(patient), hospitalManager.GetAnimalMood());
            Debug.LogError(room.gameObject.name + "room is null");

        }
        MoveAnimal(patient.animal);
        hospitalManager.OnRoomHaveSpace();

    }
}
