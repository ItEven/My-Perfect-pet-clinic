using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Patient : MonoBehaviour
{
    public NavMeshAgent navmeshAgent;
    public Animator animator;
    public Transform animalFollowPos;

    public Action onCompleteAction;

    #region Aniamtions State
    [Header("Animation State Hash")]
    public static int walking = Animator.StringToHash("Walking");
    public static int idle = Animator.StringToHash("Idle");

    #endregion

    public void Init()
    {

    }

    private void OnDisable()
    {

    }

    public void PerformUpdate()
    {
        if (!navmeshAgent.enabled)
            return;

        if (navmeshAgent.isStopped)
        {
            animator.Play(idle);
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

                    animator.Play(idle);

                    //DOVirtual.DelayedCall(300, () => MakeSad()).SetId(details.id);

                    navmeshAgent.isStopped = true;
                    navmeshAgent.enabled = false;
                    onCompleteAction?.Invoke();
                }
            }
            else
            {
                animator.Play(walking);
            }
        }
    }


    public void MoveToTarget(Transform target, Action onComplete = null)
    {
        //if (!emotion_change)
        //    MakeHappy();

        onCompleteAction = null;
        navmeshAgent.enabled = true;
        navmeshAgent.isStopped = false;
        // emotion_change = false;
        navmeshAgent.SetDestination(target.position);
        onCompleteAction = onComplete;
    }


    
}
