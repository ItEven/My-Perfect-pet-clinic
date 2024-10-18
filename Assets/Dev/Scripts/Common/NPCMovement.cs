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

    public void Start()
    {
        navmeshAgent = GetComponent<NavMeshAgent>();
    }
    private void OnEnable()
    {
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
            animator.PlayAnimation(AnimType.Idle);
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

                    animator.PlayAnimation(AnimType.Idle);
                    StopNpc();
                    onCompleteAction?.Invoke();
                }
            }
            else
            {
                Debug.LogError("YOYOYOYO");
                animator.PlayAnimation(AnimType.Walk);
                animator.controller.SetFloat("Velocity", GetVelocity());


            }
        }
    }
    public float GetVelocity()
    {
        float velocity = navmeshAgent.velocity.magnitude;


        return Mathf.Clamp01(velocity);
    }
    [Button("test")]
    public void MoveToTarget(Transform target, Action onComplete = null)
    {
        DOVirtual.DelayedCall(1f, () =>
        {
            if (navmeshAgent != null)
            {
                onCompleteAction = null;
                navmeshAgent.enabled = true;
                navmeshAgent.isStopped = false;
                animator.PlayAnimation(AnimType.Npc_Walk);

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
        navmeshAgent.isStopped = true;
        navmeshAgent.enabled = false;

    }
}
