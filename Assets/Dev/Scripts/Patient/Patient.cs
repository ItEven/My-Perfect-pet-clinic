using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Patient : MonoBehaviour
{
    public GameObject patientMeshObj;
    public Transform RightFollowPos;
    public Transform leftFollowPos;
    public NPCMovement NPCMovement;
    public Animal animal;
    public DiseaseType diseaseType;
    internal RegisterPos registerPos;


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
    
}
