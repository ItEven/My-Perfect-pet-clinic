using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public Transform target;
    public float range = 10f;
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
            transform.LookAt(target.transform);
        }
    }


    public void SetTarget(Transform newTarget, float Range)
    {
        target = newTarget;
        range = Range;
    }
}
