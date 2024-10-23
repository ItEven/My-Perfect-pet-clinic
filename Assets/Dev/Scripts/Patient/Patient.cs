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
    internal NPCMovement NPCMovement;

    private void Start()
    {
        NPCMovement = GetComponent<NPCMovement>();
    }

}
