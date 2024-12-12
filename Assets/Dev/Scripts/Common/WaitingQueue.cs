using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using DG.Tweening;


public class WaitingQueue : MonoBehaviour
{
    public List<Transform> queue = new List<Transform>();
    public List<Patient> patientInQueue = new List<Patient>();
    [SerializeField] int queueIndex;
    [SerializeField] UnityEvent OnReachQueueEnd;

    public int QueueIndex
    {
        get => queueIndex;
        set
        {
            queueIndex = Mathf.Clamp(value, 0, queue.Count);
        }
    }

    public virtual void AddInQueue(Patient patient)
    {
        if (QueueIndex < queue.Count)
        {
            if (!patientInQueue.Contains(patient))
                patientInQueue.Add(patient);
            else
                return;



            int index = queueIndex;

            patient.NPCMovement.MoveToTarget(queue[QueueIndex], () =>
            {
                patient.transform.rotation = queue[index].rotation;
                OnReachedQueueAction(patient);

            });

            QueueIndex++;
        }
    }

    public bool bIsQueueFull()
    {
        return QueueIndex >= queue.Count;
    }

    public virtual void ReOrderQueue()
    {
        QueueIndex = 0;

        for (int i = 0; i < patientInQueue.Count; i++)
        {
            var patient = patientInQueue[i];

        
            if (patient == null)
            {
                return;
            }
                
            if (QueueIndex >= queue.Count)
            {
                Debug.LogWarning("Queue index exceeds queue size. Skipping reorder for remaining patients.");
                break;
            }

            patient.NPCMovement.MoveToTarget(queue[QueueIndex], () =>
            {
                if (QueueIndex < queue.Count)
                {
                    patient.transform.rotation = queue[QueueIndex].rotation;
                }
                OnReachedQueueAction(patient);
            });
            patient.MoveAnimal();

            QueueIndex++;
        }

        PatientManager.instance.StartSendingPatinet();
    }
    public virtual void RemoveFromQueue(Patient patient)
    {
        QueueIndex--;
        if (patientInQueue.Contains(patient))
        {
            patientInQueue.Remove(patient);
            ReOrderQueue();
        }
    }

    public virtual void OnReachedQueueAction(Patient patient)
    {
        patient.StartWatting();
    
        patient.waitingQueue = this;
        if (patientInQueue.Count > 0)
        {
            if (patient != null && patientInQueue[0] != null)
            {
                if (patient == patientInQueue[0])
                {
                    OnReachQueueEnd?.Invoke();
                }
            }
        }
    }


}