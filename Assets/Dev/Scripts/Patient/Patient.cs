using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Patient : MonoBehaviour
{
    public Transform animalFollowPos;
    public NPCMovement NPCMovement;
    public Animal animal;
    public DiseaseType diseaseType;

 
    public void MoveAnimal()
    {
        animal.player = animalFollowPos;
        animal.startFollow();
    }

}
