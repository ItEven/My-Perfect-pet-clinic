using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TrafficBehavior : MonoBehaviour
{
    [Header("TrafficBehavior Porpertis ")]
    public float spwanTimeDalay;

    [Header("LeftSideTraffic Line")]
    public Transform leftSideIn;
    public Transform leftSideOut;

    [Header("RightSideTraffic Line")]
    public Transform rightSideIn;
    public Transform rightSideOut;

    [Header("Vehicals")] 
    public GameObject[] vehicles;

    public GameObject GetRandomVehicles()
    {
        int randomIndex = Random.Range(0, vehicles.Length);
        return vehicles[randomIndex];
    }

    [Button("StartSpwan")]
    public void StartSpwaning()
    {
        StartCoroutine(SpwaningVehicals());
    }
    private IEnumerator SpwaningVehicals()
    {
        while (true)
        {
            GameObject gameObjectLeft = Instantiate(GetRandomVehicles(), leftSideIn.position, Quaternion.identity, leftSideIn);
            GameObject gameObjectRight = Instantiate(GetRandomVehicles(), rightSideIn.position, Quaternion.identity, rightSideIn);
            VehicleBehavior vehicleLeft = gameObjectLeft.GetComponent<VehicleBehavior>();
            VehicleBehavior vehicleRight = gameObjectRight.GetComponent<VehicleBehavior>();
            if (vehicleLeft != null)
            {   
                vehicleLeft.transform.rotation = leftSideIn.rotation;
                vehicleLeft.Init();
                vehicleLeft.MoveToTarget(leftSideOut, () =>
                {
                    Destroy(vehicleLeft.gameObject);
                });
            }
            if (vehicleRight != null)
            {
                vehicleRight.transform.rotation = rightSideIn.rotation;
                vehicleRight.Init();
                vehicleRight.MoveToTarget(rightSideOut, () =>
                {
                    Destroy(vehicleRight.gameObject);
                });
            }
            yield return new WaitForSeconds(spwanTimeDalay);
        }
    }
}
