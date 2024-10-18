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
                    navmeshAgent.isStopped = true;
                    navmeshAgent.enabled = false;
                    onCompleteAction?.Invoke();
                }
            }
            else
            {
                animator.PlayAnimation(AnimType.Walk);

            }
        }
    }

    [Button("test")]
    public virtual void MoveToTarget(Transform target, Action onComplete = null)
    {
        DOVirtual.DelayedCall(2f, () =>
        {
            if (navmeshAgent != null)
            {
                onCompleteAction = null;
                navmeshAgent.enabled = true;
                navmeshAgent.isStopped = false;
                navmeshAgent.SetDestination(target.position);
                onCompleteAction = onComplete;
            }
            else
            {
                Debug.LogError(navmeshAgent);
            }
        });
    }


}
