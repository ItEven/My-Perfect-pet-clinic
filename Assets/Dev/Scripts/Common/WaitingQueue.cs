using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;

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

            //patient.NPCMovement.MoveToTarget(queue[QueueIndex], null); 

            patient.NPCMovement.MoveToTarget(queue[QueueIndex], () =>
            {
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

        var patient = PatientManager.instance.SpwanNewPatinet();

        patient.NPCMovement.MoveToTarget(queue[QueueIndex], () =>
            {
                OnReachedQueueAction(patient);
            });

    }
    public virtual void RemoveFromQueue(Patient customer)
    {
        patientInQueue.Remove(customer);
        QueueIndex--;
        ReOrderQueue();
    }

    public virtual void OnReachedQueueAction(Patient customer)
    {
        if (customer != null)
        {
            if (customer == patientInQueue[0])
            {
                OnReachQueueEnd?.Invoke();
            }
        }
    }

  


}