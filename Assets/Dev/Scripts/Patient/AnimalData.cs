using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum AnimalType
{
    Dog,
    Cat,
    Raccon,
    Panda
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
}
