using System.Collections.Generic;
using UnityEngine;

public abstract class WaitingQueue : MonoBehaviour
{
    public List<Transform> queue = new List<Transform>();
    public List<Patient> patientInQueue = new List<Patient>();
    [SerializeField] int queueIndex;

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

            patient.MoveToTarget(queue[QueueIndex], () =>
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

        patient.MoveToTarget(queue[QueueIndex], () =>
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
        //Will execute by drive class
    }

    public virtual void CheckForFreeSlots()
    {
        patientInQueue.RemoveAll(x => x == null);

        if (patientInQueue.Count != 0)
        {
            for (int i = 0; i < patientInQueue.Count; i++)
            {
                //customerInQueue[i].CheckForFreeEquipement();
            }
        }
        else
        {
            // AssignCustomerFromOtherQueue();
        }
    }


}