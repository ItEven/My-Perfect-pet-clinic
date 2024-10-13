using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatientUpdater : MonoBehaviour
{
    public static Action patientAIUpdate;

    void Update()
    {
        patientAIUpdate?.Invoke();
    }
}