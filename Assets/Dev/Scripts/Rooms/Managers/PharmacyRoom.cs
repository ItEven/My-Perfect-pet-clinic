using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PharmacyRoom : RoomManager
{
    public override void StratProssesPatients()
    {

        if (bIsPlayerOnDesk)
        {
            gameManager.playerController.animationController.PlayAnimation(AnimType.Sti_Idle);
        }
        Debug.LogError("waitingQueue -1");

        if (waitingQueue.patientInQueue.Count > 0 && !waitingQueue.patientInQueue[0].NPCMovement.bIsMoving)
        {
            if (!hospitalManager.CheckRegiterPosFull())
            {
                if (bIsPlayerOnDesk)
                {

                    gameManager.playerController.animationController.PlayAnimation(AnimType.Talking);


                    worldProgresBar.fillAmount = 0;
                    worldProgresBar.DOFillAmount(1, Staff_NPC.currentLevelData.processTime)
                        .SetId(tweenID)
                        .OnComplete(() =>
                        {
                            gameManager.playerController.animationController.PlayAnimation(AnimType.Sti_Idle);
                            moneyBox.TakeMoney(GetCustomerCost(waitingQueue.patientInQueue[0]));
                            var p = waitingQueue.patientInQueue[0];
                            p.NPCMovement.MoveToTarget(hospitalManager.GetRandomExit(), () =>
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

                    Staff_NPC.animationController.PlayAnimation(AnimType.Talking);

                    worldProgresBar.fillAmount = 0;
                    worldProgresBar.DOFillAmount(1, Staff_NPC.currentLevelData.processTime)
                        .SetId(tweenID)
                        .OnComplete(() =>
                        {

                            Staff_NPC.animationController.PlayAnimation(AnimType.Sti_Idle);

                            moneyBox.TakeMoney(GetCustomerCost(waitingQueue.patientInQueue[0]));
                            var p = waitingQueue.patientInQueue[0];
                            p.NPCMovement.MoveToTarget(hospitalManager.GetRandomExit(), () =>
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
    }

    public override void StopProsses()
    {
        gameManager.playerController.animationController.PlayAnimation(AnimType.Sti_Idle);
        bIsPlayerOnDesk = false;
        worldProgresBar.fillAmount = 0;
        DOTween.Kill(tweenID);
    }
}
