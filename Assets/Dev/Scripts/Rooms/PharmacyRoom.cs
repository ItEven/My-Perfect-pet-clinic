using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PharmacyRoom : InspectionRoomManager
{


    public override void StratProssesPatients()
    {
        gameManager.playerController.animationController.PlayAnimation(AnimType.Sti_Idle);
        if (!gameManager.playerController.bhasSit && !gameManager.playerController.bIsDiagnosing)
        {
            gameManager.playerController.animationController.PlayAnimation(AnimType.Sti_Idle);
        }
        Debug.LogError("waitingQueue -1");

        if (waitingQueue.patientInQueue.Count > 0 && !waitingQueue.patientInQueue[0].NPCMovement.bIsMoving && bCanProsses)
        {
            Debug.LogError("waitingQueue");
            if (!hospitalManager.CheckRegiterPosFull())
            {
                Debug.LogError("waitingQueue = 2");

                var room = hospitalManager.pharmacyRoom;
                gameManager.playerController.animationController.PlayAnimation(AnimType.Talking);

                worldProgresBar.fillAmount = 0;
                worldProgresBar.DOFillAmount(1, Staff_NPC.currentLevelData.processTime)
                    .SetId(tweenID)
                    .OnComplete(() =>
                    {
                        Debug.LogError("waitingQu eue = 3");

                        moneyBox.TakeMoney(GetCustomerCost(waitingQueue.patientInQueue[0]));
                        var p = waitingQueue.patientInQueue[0];
                        p.NPCMovement.MoveToTarget(hospitalManager.GetRandomExit(), () =>
                        {
                            Destroy(p.gameObject);
                        });
                        p.MoveAnimal();
                        waitingQueue.RemoveFromQueue(waitingQueue.patientInQueue[0]);
                    });
            }
        }
    }

    public override void StopProsses()
    {
        gameManager.playerController.animationController.PlayAnimation(AnimType.Sti_Idle);
        bCanProsses = false;
        worldProgresBar.fillAmount = 0;
        DOTween.Kill(tweenID);
    }
}
