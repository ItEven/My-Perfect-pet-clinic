using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PharmacyRoom : RoomManager
{
    public override void NextPatient()
    {
        Patient patient = unRegisterPatientList[0];
        RegisterPatient(patient);
        patient.registerPos.bIsRegiseter = false;
        unRegisterPatientList.RemoveAt(0);
        hospitalManager.OnRoomHaveSpace();
    }
    public override void StratProssesPatients()
    {
        
        if (bIsPlayerOnDesk)
        {
            gameManager.playerController.animationController.PlayAnimation(AnimType.Sti_Idle);
        }

        if (waitingQueue.patientInQueue.Count > 0 && !waitingQueue.patientInQueue[0].NPCMovement.bIsMoving)
        {
            if (bIsPlayerOnDesk)
            {

                gameManager.playerController.animationController.PlayAnimation(seat.workingAnim);


                worldProgresBar.fillAmount = 0;
                worldProgresBar.DOFillAmount(1, Staff_NPC.currentLevelData.processTime)
                    .SetId(Tw_Filler)
                    .OnComplete(() =>
                    {
                        gameManager.playerController.animationController.PlayAnimation(seat.idleAnim);
                        moneyBox.TakeMoney(GetCustomerCost(waitingQueue.patientInQueue[0]));
                        var p = waitingQueue.patientInQueue[0];
                        p.NPCMovement.MoveToTarget(hospitalManager.GetRandomExit(p), () =>
                        {
                            Destroy(p.gameObject);
                        });
                        p.MoveAnimal();
                        waitingQueue.RemoveFromQueue(waitingQueue.patientInQueue[0]);
                        worldProgresBar.fillAmount = 0;

                    });

            }
            else if (Staff_NPC.bIsUnlock)
            {

                Staff_NPC.animationController.PlayAnimation(seat.workingAnim);

                worldProgresBar.fillAmount = 0;
                worldProgresBar.DOFillAmount(1, Staff_NPC.currentLevelData.processTime)
                    .SetId(Tw_Filler)
                    .OnComplete(() =>
                    {

                        Staff_NPC.animationController.PlayAnimation(seat.idleAnim);

                        moneyBox.TakeMoney(GetCustomerCost(waitingQueue.patientInQueue[0]));
                        var p = waitingQueue.patientInQueue[0];
                        p.NPCMovement.MoveToTarget(hospitalManager.GetRandomExit(p), () =>
                        {
                            Destroy(p.gameObject);
                        });
                        p.MoveAnimal();
                        waitingQueue.RemoveFromQueue(waitingQueue.patientInQueue[0]);
                        worldProgresBar.fillAmount = 0;

                    });
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
