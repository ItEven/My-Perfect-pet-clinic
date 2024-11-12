using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;
public class GroomingTable : Bed
{
    [Header("New Grooming Dependencies")]
    bool bIsProcessing = false;
    bool bHasBathDone = false;
    public Image BathProgresBar;
    public Seat bathPos;
    public OnTrigger bathOnTrigger;

    public override void OnPlayerTrigger()
    {
        bIsPlayerOnDesk = true;
        if (!staffNPC.bIsUnlock)
        {
            if (bIsProcessing && bHasBathDone)
            {
                playerController.playerControllerData.characterMovement.enabled = false;
                playerController.enabled = false;
                playerController.playerControllerData.joystick.gameObject.SetActive(false);

                playerController.transform.position = seat.transform.position;
                playerController.playerControllerData.characterMovement.rotatingObj.rotation = seat.transform.rotation;


                DOVirtual.DelayedCall(.5f, () =>
                {
                    playerController.transform.SetParent(null);
                    playerController.playerControllerData.joystick.gameObject.SetActive(true);
                    playerController.playerControllerData.joystick.OnPointerUp(null);
                    playerController.playerControllerData.characterMovement.enabled = true;
                });
                playerController.animationBools.bHasCarringItem = false;
                arrowController.gameObject.SetActive(false);
                DropAnimalToDesk();
                OnProcessComplite(hospitalManager.pharmacyRoom, playerController.animationController, AnimType.Idle);
            }
            else
            {
                SetUpPlayer();
            }
        }
    }
    public override void OnPlayerExit()
    {
        if (!bIsProcessing)
        {
            bIsPlayerOnDesk = false;
        }
        if (!staffNPC.bIsUnlock)
        {
            BreakProcess();
        }
    }
    public override void SetUpPlayer()
    {
      //  playerController.animationController.PlayAnimation(AnimType.Idle);
        base.SetUpPlayer();
    }
    public override void StartProcessPatients()
    {
        if (patient == null) return;
        if (bIsProcessing) return;

        bIsProcessing = true;
        var workingAnimation = seat.workingAnim;
        var processTime = staffNPC.currentLevelData.processTime;

        if (staffNPC.bIsUnlock && staffNPC.bIsOnDesk)
        {
            StartPatientProcessing(staffNPC.animationController, workingAnimation, AnimType.Idle, staffNPC.currentLevelData.processTime, () =>
            {
                staffNPC.nPCMovement.walkingAnimType = AnimType.Walk_With_Object;
                patient.animal.transform.position = staffNPC.animalCarryPos.position;
                patient.animal.transform.rotation = staffNPC.animalCarryPos.rotation;
                staffNPC.nPCMovement.MoveToTarget(bathOnTrigger.seat
                    .transform, () =>
                    {
                        patient.animal.transform.position = bathPos.transform.position;
                        patient.animal.transform.rotation = bathPos.transform.rotation;
                        staffNPC.transform.position = bathOnTrigger.seat.transform.position;
                        staffNPC.transform.rotation = bathOnTrigger.seat.transform.rotation;

                        StartGroomingProcess(staffNPC.animationController, bathOnTrigger.seat.workingAnim, AnimType.Idle, staffNPC.currentLevelData.processTime, () =>
                        {  
                            bHasBathDone = true;
                            BathProgresBar.fillAmount = 0;


                            staffNPC.nPCMovement.walkingAnimType = AnimType.Walk_With_Object;
                            patient.animal.transform.position = staffNPC.animalCarryPos.position;
                            patient.animal.transform.rotation = staffNPC.animalCarryPos.rotation;

                            staffNPC.nPCMovement.MoveToTarget(onTrigger.seat
                    .transform, () =>
                    {

                        staffNPC.transform.position = onTrigger.seat.transform.position;
                        staffNPC.transform.rotation = onTrigger.seat.transform.rotation;
                        DropAnimalToDesk();

                        OnProcessComplite(hospitalManager.pharmacyRoom, staffNPC.animationController, AnimType.Idle);
                    });


                            playerController.animationBools.bHasCarringItem = true;
                            playerController.transform.SetParent(null);
                            playerController.playerControllerData.joystick.gameObject.SetActive(true);
                            playerController.playerControllerData.joystick.OnPointerUp(null);
                            playerController.playerControllerData.characterMovement.enabled = true;
                        });

                    });

            });
        }
        else if (bIsPlayerOnDesk)
        {
            StartPatientProcessing(playerController.animationController, workingAnimation, AnimType.Idle, staffNPC.currentLevelData.processTime, () =>
            {
                if (!arrowController.gameObject.activeInHierarchy && bIsOccupied) arrowController.gameObject.SetActive(true);
                arrowController.SetTarget(bathOnTrigger.seat.transform, 0.5f);

                patient.animal.transform.position = playerController.itemsCarryhandler.itemsPostionArr[0].position;
                patient.animal.transform.rotation = playerController.itemsCarryhandler.itemsPostionArr[0].rotation;


            });
        }
    }

    public void StartBathing()
    {
        if (bIsPlayerOnDesk && !bHasBathDone)
        {
            playerController.playerControllerData.characterMovement.enabled = false;
            playerController.enabled = false;
            playerController.playerControllerData.joystick.gameObject.SetActive(false);
            patient.animal.transform.position = bathPos.transform.position;
            patient.animal.transform.rotation = bathPos.transform.rotation;
            playerController.transform.position = bathOnTrigger.seat.transform.position;
            playerController.playerControllerData.characterMovement.rotatingObj.rotation = bathOnTrigger.seat.transform.rotation;
            playerController.animationBools.bHasCarringItem = false;

            StartGroomingProcess(playerController.animationController, bathOnTrigger.seat.workingAnim, AnimType.Idle, staffNPC.currentLevelData.processTime, () =>
            {
                BathProgresBar.fillAmount = 0;

                if (!arrowController.gameObject.activeInHierarchy && bIsOccupied) arrowController.gameObject.SetActive(true);
                arrowController.SetTarget(onTrigger.seat.transform, 0.5f);

                patient.animal.transform.position = playerController.itemsCarryhandler.itemsPostionArr[0].position;
                patient.animal.transform.rotation = playerController.itemsCarryhandler.itemsPostionArr[0].rotation;

                playerController.animationBools.bHasCarringItem = true;
                playerController.transform.SetParent(null);
                playerController.playerControllerData.joystick.gameObject.SetActive(true);
                playerController.playerControllerData.joystick.OnPointerUp(null);
                playerController.playerControllerData.characterMovement.enabled = true;
                bHasBathDone = true;

            });
        }
    }

    public void DropAnimalToDesk()
    {
        patient.animal.transform.position = petDignosPos.transform.position;
        patient.animal.transform.rotation = petDignosPos.transform.rotation;
        patient.animal.animator.PlayAnimation(petDignosPos.idleAnim);
    }

    protected string processTween;

    public void StartGroomingProcess(AnimationController animationController, AnimType workingAnim, AnimType idleAnim, float processTime, Action onComplete)
    {
        if (string.IsNullOrEmpty(processTweenId))
        {
            processTweenId = "ProcessTween_" + Guid.NewGuid().ToString();
        }

        animationController.PlayAnimation(workingAnim);

        BathProgresBar.DOFillAmount(1, processTime).SetId(processTweenId)
            .OnComplete(() =>
            {
                animationController.PlayAnimation(idleAnim);
                onComplete?.Invoke();
            });
    }
    public override void OnProcessComplite(ARoom nextRoom, AnimationController animationController, AnimType idleAnim)
    {
        room.moneyBox.TakeMoney(hospitalManager.GetCustomerCost(patient, room.diseaseData, staffNPC.currentLevelData.StaffExprinceType));
        worldProgresBar.fillAmount = 0;

        if (nextRoom.bIsUnRegisterQueIsFull() || nextRoom == null || !nextRoom.bIsUnlock)
        {
            animationController.PlayAnimation(idleAnim);
            patient.MoveToExit(hospitalManager.GetRandomExit());

            patient.animal.emojisController.PlayEmoji(hospitalManager.GetAnimalMood());
        }
        else
        {
            nextRoom.RegisterPatient(patient);
        }


        bIsProcessing = false;
        bHasBathDone = false;
        MoveAnimal(patient.animal);



    }
}
