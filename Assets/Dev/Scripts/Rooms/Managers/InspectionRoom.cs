using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class InspectionRoom : RoomManager
{
    public override void RegisterPatient(Patient patients)
    {
        if (patients != null)
        {
            if (!waitingQueue.bIsQueueFull())
            {
                waitingQueue.AddInQueue(patients);
                patients.MoveAnimal();
            }
            else
            {
                unRegisterPatientList.Add(patients);
                Transform transform = hospitalManager.GetRandomPos(patients);
                patients.NPCMovement.MoveToTarget(transform, null);
                patients.MoveAnimal();
            }
        }
    }
    public override void NextPatient()
    {
        Patient patient = unRegisterPatientList[0];
        RegisterPatient(patient);
        patient.registerPos.bIsRegiseter = false;
        unRegisterPatientList.RemoveAt(0);
        PatientManager.instance.receptionManager.StratProssesPatients();
    }
    public override void StratProssesPatients()
    {
        if (bIsPlayerOnDesk)
        {
            gameManager.playerController.animationController.PlayAnimation(seat.idleAnim);
        }


        if (waitingQueue.patientInQueue.Count > 0 && !waitingQueue.patientInQueue[0].NPCMovement.bIsMoving)
        {
            var room = hospitalManager.pharmacyRoom;

            if (!room.bIsUnRegisterQueIsFull())
            {
                if (bIsPlayerOnDesk)
                {
                    gameManager.playerController.animationController.PlayAnimation(seat.workingAnim);

                    worldProgresBar.fillAmount = 0;
                    Tw_Filler = worldProgresBar.DOFillAmount(1, Staff_NPC.currentLevelData.processTime)
                        .OnComplete(() =>
                        {
                            gameManager.playerController.animationController.PlayAnimation(seat.idleAnim);
                            //moneyBox.TakeMoney(GetCustomerCost(waitingQueue.patientInQueue[0]));
                            moneyBox.TakeMoney(hospitalManager.GetCustomerCost(waitingQueue.patientInQueue[0], diseaseData, Staff_NPC.currentLevelData.StaffExprinceType));
                            room.RegisterPatient(waitingQueue.patientInQueue[0]);
                            var p = waitingQueue.patientInQueue[0];
                            p.MoveAnimal();
                            waitingQueue.RemoveFromQueue(waitingQueue.patientInQueue[0]);

                            if (unRegisterPatientList.Count > 0)
                            {
                                NextPatient();
                            }
                            worldProgresBar.fillAmount = 0;

                        });

                }
                else if (Staff_NPC.bIsUnlock)
                {
                    Staff_NPC.animationController.PlayAnimation(seat.workingAnim);

                    worldProgresBar.fillAmount = 0;
                    Tw_Filler = worldProgresBar.DOFillAmount(1, Staff_NPC.currentLevelData.processTime)
                        .OnComplete(() =>
                        {

                            Staff_NPC.animationController.PlayAnimation(seat.idleAnim);
                            moneyBox.TakeMoney(GetCustomerCost(waitingQueue.patientInQueue[0]));
                            room.RegisterPatient(waitingQueue.patientInQueue[0]);
                            var p = waitingQueue.patientInQueue[0];
                            p.MoveAnimal();
                            waitingQueue.RemoveFromQueue(waitingQueue.patientInQueue[0]);

                            if (unRegisterPatientList.Count > 0)
                            {
                                NextPatient();
                            }
                            worldProgresBar.fillAmount = 0;

                        });

                }
            }
        }

    }
    public override void StopProsses()
    {
        gameManager.playerController.animationController.PlayAnimation(seat.idleAnim);
        bIsPlayerOnDesk = false;
        worldProgresBar.fillAmount = 0;
        DOTween.Kill(Tw_Filler);
    }
}
