using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;



public class Patient : MonoBehaviour
{
    internal bool bIsWating;
    public GameObject patientMeshObj;
    public Transform RightFollowPos;
    public Transform leftFollowPos;
    public NPCMovement NPCMovement;
    public Animal animal;
    public DiseaseType diseaseType;
    internal RegisterPos registerPos;

    [Header("Exclamation mark")]
    public Transform markForLock;
    public Transform markForFull;

    [Header("Emojie Controller")]
    public EmojisController emojisController;

    [Header("TextBox")]
    public RectTransform sloganTextBox;
    public TextMeshProUGUI sloganText;

    [Button("MoveAnimal")]
    public void MoveAnimal()
    {
        animal.player = RightFollowPos;
        animal.startFollow();
    }
    public void MoveToExit(Transform ExitPoint)
    {
        NPCMovement.MoveToTarget(ExitPoint, () =>
        {
            Destroy(animal.gameObject);
            Destroy(gameObject);
        });
    }

    protected string processTweenId;

    public void MoveNewShufflePos(Transform traget)
    {
        if (string.IsNullOrEmpty(processTweenId))
        {
            processTweenId = "ProcessTween_" + Guid.NewGuid().ToString();
        }

        int index = Random.Range(0, 5);
        DOVirtual.DelayedCall(index, () =>
        {

            NPCMovement.MoveToTarget(traget, null);
            MoveAnimal();
        }).SetId(processTweenId);

    }

    public void BrakeDally()
    {
        DOTween.Kill(processTweenId);
    }

   


}
