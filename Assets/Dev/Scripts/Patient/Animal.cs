using System.Collections;
using System;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;
using DG.Tweening;

public class Animal : MonoBehaviour
{
    public NavMeshAgent navmeshAgent;
    public AnimationController animator;
    public Transform player;


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
            animator.PlayAnimation(AnimType.Idle);
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
                if (!navmeshAgent.hasPath || navmeshAgent.velocity.sqrMagnitude == 0f)
                {
                    if (navmeshAgent.isActiveAndEnabled)
                        navmeshAgent.ResetPath();
                    StopAnimal();

                }
            }
            else
            {
                animator.PlayAnimation(AnimType.Walk);
            }

        }
    }


    public void MoveToTarget(Transform target)
    {
        navmeshAgent.SetDestination(target.position);
    }

    public void startFollow()
    {
        DOVirtual.DelayedCall(1.5f, () =>
        {
            navmeshAgent.enabled = true;
            navmeshAgent.isStopped = false;
            animator.PlayAnimation(AnimType.Walk);
        });
    }
    public void StopAnimal()
    {
        animator.PlayAnimation(AnimType.Idle);
        navmeshAgent.isStopped = true;
        navmeshAgent.enabled = false;
    }
}
