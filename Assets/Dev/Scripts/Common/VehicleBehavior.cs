using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;


public class VehicleBehavior : MonoBehaviour
{
    public NavMeshAgent navmeshAgent;
    public Action onCompleteAction;

    public virtual void Init()
    {
        navmeshAgent = GetComponent<NavMeshAgent>();
        PatientUpdater.patientAIUpdate += PerformUpdate;
    }


    private void OnDisable()
    {
        PatientUpdater.patientAIUpdate -= PerformUpdate;
    }

    public void PerformUpdate()
    {
        if (!navmeshAgent.enabled)
        {
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
                    onCompleteAction?.Invoke();
                }
            }
        }
    }

    public void MoveToTarget(Transform target, Action onComplete = null)
    {
        DOVirtual.DelayedCall(0.2f, () =>
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
