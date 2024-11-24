using System.Collections;
using System;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;
using DG.Tweening;


public class Animal : MonoBehaviour
{
    public AnimalType animalType;
    public NavMeshAgent navmeshAgent;
    public AnimationController animator;
    public Transform player;
    public AnimType idleAnim = AnimType.Idle;
    public AnimType walkAnim = AnimType.Walk;

    //private void Start()
    //{
    //    Init();
    //}

    public void Init()
    {
        PatientUpdater.patientAIUpdate += PerformUpdate;
    }

    private void OnDisable()
    {
        PatientUpdater.patientAIUpdate -= PerformUpdate;
    }


    public void PerformUpdate()
    {

        if (!navmeshAgent.enabled)
            return;

        if (navmeshAgent.isStopped)
        {
            animator.PlayAnimation(idleAnim);
            return;
        }
        if (player != null)
        {
            MoveToTarget(player);
        }


        if (!navmeshAgent.pathPending)
        {
            if (navmeshAgent.remainingDistance <= navmeshAgent.stoppingDistance)
            {
                if (navmeshAgent.isActiveAndEnabled)
                    navmeshAgent.ResetPath();
                StopAnimal();
            }
            else
            {
                animator.PlayAnimation(walkAnim);
            }

        }
    }


    public void MoveToTarget(Transform target)
    {

        navmeshAgent.SetDestination(target.position);
    }

    public void startFollow()
    {
        DOVirtual.DelayedCall(0.5f, () =>
        {
            navmeshAgent.enabled = true;
            navmeshAgent.isStopped = false;
            animator.PlayAnimation(walkAnim);
        });
    }
    public void StopAnimal()
    {
        animator.PlayAnimation(idleAnim);
        navmeshAgent.isStopped = true;
        //navmeshAgent.enabled = false;
    }


}
