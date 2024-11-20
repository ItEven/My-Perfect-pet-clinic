using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    public Camera mainCamera;

    void Start()
    {
        // If no specific camera is assigned, use the main camera
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    void LateUpdate()
    {
        // Make the UI face the camera directly
        transform.forward = mainCamera.transform.forward;
    }
}
