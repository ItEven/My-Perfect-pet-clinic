using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;
using DG.Tweening;
using UnityEngine.UI;
using Sirenix.OdinInspector;


public class NPCMovement : MonoBehaviour
{
    public NavMeshAgent navmeshAgent;
    public AnimationController animator;
    public Action onCompleteAction;
    internal AnimType idleAnimType = AnimType.Idle;
    internal AnimType walkingAnimType = AnimType.Walk;
    internal bool bIsMoving;

    public virtual void Init()
    {
        navmeshAgent = GetComponent<NavMeshAgent>();
        PatientUpdater.patientAIUpdate += PerformUpdate;
    }

    private void OnDisable()
    {
        PatientUpdater.patientAIUpdate -= PerformUpdate;
    }
    public virtual void PerformUpdate()
    {
        if (!navmeshAgent.enabled)
            return;

        if (navmeshAgent.isStopped)
        {
            animator.PlayAnimation(idleAnimType);
            return;
        }

        if (!navmeshAgent.pathPending)
        {
            if (navmeshAgent.remainingDistance <= navmeshAgent.stoppingDistance)
            {
                if (!navmeshAgent.hasPath || navmeshAgent.velocity.sqrMagnitude == 0f)
                {
                    if (navmeshAgent.isActiveAndEnabled)
                        navmeshAgent.ResetPath();

                    animator.PlayAnimation(idleAnimType);
                    StopNpc();
                    onCompleteAction?.Invoke();
                }
            }
            else
            {

                animator.PlayAnimation(walkingAnimType);
                animator.controller.SetFloat("Velocity", GetVelocity());


            }
        }
    }
    public float GetVelocity()
    {
        float velocity = navmeshAgent.velocity.magnitude;
        return Mathf.Clamp01(velocity);
    }


    public void MoveToTarget(Transform target, Action onComplete = null)
    {
        DOVirtual.DelayedCall(0.2f, () =>
        {
            if (navmeshAgent != null)
            {
                bIsMoving = true;
                onCompleteAction = null;
                navmeshAgent.enabled = true;
                navmeshAgent.isStopped = false;
                animator.PlayAnimation(walkingAnimType);

                navmeshAgent.SetDestination(target.position);
                onCompleteAction = onComplete;
            }
            else
            {
                Debug.LogError(navmeshAgent);
            }
        });
    }

    public void StopNpc()
    {
        bIsMoving = false;

        navmeshAgent.isStopped = true;
        navmeshAgent.enabled = false;
    }
}
