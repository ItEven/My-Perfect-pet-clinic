using System.Collections;
using System;
using UnityEngine;
using UnityEngine.AI;
using Sirenix.OdinInspector;
using DG.Tweening;

public enum MoodType { GlassHappy, Happy, Lovely, Vomaite, Sad, Shok, Angry }
public class Animal : MonoBehaviour
{
    public NavMeshAgent navmeshAgent;
    public AnimationController animator;
    public Transform player;

    [Header("Partical")]
    public GameObject PT_GlassHappyObj;
    public GameObject PT_HappyObj;
    public GameObject PT_LovelyObj;
    public GameObject PT_VomaiteObj;
    public GameObject PT_SadObj;
    public GameObject PT_ShokObj;
    public GameObject PT_Angry;


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
                if (navmeshAgent.isActiveAndEnabled)
                    navmeshAgent.ResetPath();
                StopAnimal();
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
        DOVirtual.DelayedCall(0.5f, () =>
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
        //navmeshAgent.enabled = false;
    }

    public void SetPartical(MoodType moodType)
    {
        DisableAllPt();
        switch (moodType)
        {
            case MoodType.GlassHappy:
                PT_GlassHappyObj.SetActive(true);
                break;
            case MoodType.Lovely:
                PT_LovelyObj.SetActive(true);
                break;
            case MoodType.Sad:
                PT_SadObj.SetActive(true);
                break;
            case MoodType.Vomaite:
                PT_VomaiteObj.SetActive(true);
                break;
            case MoodType.Happy:
                PT_HappyObj.SetActive(true);
                break;
            case MoodType.Shok:
                PT_ShokObj.SetActive(true);
                break;
            case MoodType.Angry:
                PT_Angry.SetActive(true);
                break;

        }
    }

    public void DisableAllPt()
    {
        PT_GlassHappyObj.SetActive(false);
        PT_HappyObj.SetActive(false);
        PT_LovelyObj.SetActive(false);
        PT_VomaiteObj.SetActive(false);
        PT_SadObj.SetActive(false);
        PT_ShokObj.SetActive(false);
        PT_Angry.SetActive(false);
    }








}
