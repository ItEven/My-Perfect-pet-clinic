using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public Transform target;
    public float range = 3.5f;
    public GameObject arrowIcon;

    private void Update()
    {
        if (target == null) return;

        float distanceToTarget = Vector3.Distance(transform.position, target.position);

        if (distanceToTarget <= range)
        {
            arrowIcon.SetActive(false);
        }
        else
        {
            arrowIcon.SetActive(true);

            // Constrain rotation to Y-axis (ignoring X-axis movement)
            Vector3 direction = target.position - transform.position;
            direction.y = 0; // Keep the rotation level
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }
    }

    public void SetTarget(Transform newTarget, float Range)
    {
        target = newTarget;
        range = Range;
    }
}
