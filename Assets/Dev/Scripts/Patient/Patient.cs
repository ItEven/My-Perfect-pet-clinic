using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Patient : MonoBehaviour
{
    public GameObject patientMeshObj;
    public Transform animalFollowPos;
    public NPCMovement NPCMovement;
    public Animal animal;
    public DiseaseType diseaseType;
    internal RegisterPos registerPos;

    public void MoveAnimal()
    {
        animal.player = animalFollowPos;
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
