using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum DiseaseType
{
    Cough,
    Cold,
    Fever,
    Ear_Infection,
    Fleas_and_Ticks,
    Allergies,
    Dental_Disease,
    Skin_Infecction,
    Vomitting,
    Heartworm_Disease,
    Rabies,
    Bloat,
    Bladder_Stones,
    Fractures,
    Kidney_Disease,
    Asthma
}

[System.Serializable]
public enum Stafftype
{
    Intern,
    Junior,
    Senior,
    Chief,
}

[System.Serializable]
public class Disease
{
    public DiseaseType Type;
    public int juniorVeterinarianFee;
    public int VeterinarianFee;
    public int seniorVeterinarianFee;
    public int chiefVeterinarianFee;
}

[CreateAssetMenu(fileName = "NewDiseaseData", menuName = "Disease Data")]
public class DiseaseData : ScriptableObject
{
    public Disease[] diseases;
}