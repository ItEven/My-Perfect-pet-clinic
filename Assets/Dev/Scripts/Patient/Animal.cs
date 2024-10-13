using System.Collections;
using System;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;

public class Animal : MonoBehaviour
{
    public NavMeshAgent navmeshAgent;
    //public Animator animator;

    public Transform player;


    public Action onCompleteAction;

    #region Aniamtions State
    [Header("Animation State Hash")]
    public static int walking = Animator.StringToHash("Walking");
    public static int idle = Animator.StringToHash("Idle");
    #endregion

    private void Start()
    {
        Init();
    }

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
        //if (player != null)
        //{
        //    MoveToTarget(player);
        //}

        if (!navmeshAgent.enabled)
            return;

        if (navmeshAgent.isStopped)
        {
          //  animator.Play(idle);
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

                   // animator.Play(idle);

                    navmeshAgent.isStopped = true;
                    navmeshAgent.enabled = false;
                    onCompleteAction?.Invoke();
                }
            }
            else
            {
                //animator.Play(walking);
            }
        }
    }

    
    public void MoveToTarget(Transform target)
    {

        navmeshAgent.enabled = true;
        navmeshAgent.isStopped = false;
        
        navmeshAgent.SetDestination(target.position);
    }
}
