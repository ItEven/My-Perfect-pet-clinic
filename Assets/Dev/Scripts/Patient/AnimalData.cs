using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


[System.Serializable]
public enum AnimalType
{
    Dog,
    Cat,
    Raccon,
    Panda
}

[System.Serializable]
public class AnimalNames
{
    public AnimalType animalType;
    public string[] animalNames;
}
[System.Serializable]
public class DiseaseSloganData
{
    public DiseaseType diseaseType;
    public string[] slogans;
}

[System.Serializable]
public class Animales
{
    public AnimalType animalType;
    public GameObject[] Animal;
}

[CreateAssetMenu(fileName = "NewAnimalsData", menuName = "Animals Data")]
public class AnimalData : ScriptableObject
{
    public Animales[] animales;
    public AnimalNames[] animalNameData;
    public DiseaseSloganData[] diseaseSloganDatas;

    public string GetSlogan(AnimalType animalType, DiseaseType diseaseType)
    {
        if (GetRandomSlogan(diseaseType) == DiseaseType.Toy.ToString())
        {
            return GetRandomSlogan(diseaseType);
        }
        else
        {
            return "MY" + " " + GetAnimalName(animalType) + " " + GetRandomSlogan(diseaseType);
        }
    }

    public string GetAnimalName(AnimalType animalType)
    {
        foreach (var data in animalNameData)
        {
            if (data.animalType == animalType)
            {
                int random = Random.Range(0, data.animalNames.Length);
                return data.animalNames[random];
            }
        }
        return null;
    }

    public string GetRandomSlogan(DiseaseType diseaseType)
    {
        foreach (var data in diseaseSloganDatas)
        {
            if (data.diseaseType == diseaseType)
            {
                int random = Random.Range(0, data.slogans.Length);
                return data.slogans[random];
            }
        }
        return null;
    }
}
