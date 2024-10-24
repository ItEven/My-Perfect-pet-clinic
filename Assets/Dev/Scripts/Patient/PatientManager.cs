using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class PatientManager : MonoBehaviour
{
    public static PatientManager instance;
    public AnimalData AnimalData;
    public ReceptionManager receptionManager;
    [SerializeField] GameObject[] patients;
    public Transform playerSpwanPos;
    public Transform animalSpwanPos;
    public float spwanDalay;





    [Header("Disease Data")]
    public List<DiseaseType> UnlocDiseases;
    public int currntDiseaseIndex
    {
        get
        {
            return PlayerPrefs.GetInt("CurrentDiseaseIndex", 0);
        }
        set
        {
            PlayerPrefs.SetInt("CurrentDiseaseIndex", value);
            PlayerPrefs.Save();
        }
    }
    public List<DiseaseType> Alldiseases;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartSpwanPatinet();
    }

    #region SpwanPatient

    public GameObject GetRandomPatients()
    {
        int randomIndex = Random.Range(0, patients.Length);
        return patients[randomIndex];
    }

    public Patient SpwanNewPatinet()
    {
        GameObject gameObject = Instantiate(GetRandomPatients(), playerSpwanPos.position, Quaternion.identity, playerSpwanPos);
        return gameObject.GetComponent<Patient>();
    }

    private Coroutine takeMoneyCoroutine;

    private void StartSpwanPatinet()
    {
        if (takeMoneyCoroutine == null)
        {
            takeMoneyCoroutine = StartCoroutine(SpwaningPatinet());
        }
    }

    private void StopSpwanPatinet()
    {
        if (takeMoneyCoroutine != null)
        {
            StopCoroutine(takeMoneyCoroutine);
            takeMoneyCoroutine = null;
        }
    }

    float lastSub = 0f;
    private IEnumerator SpwaningPatinet()
    {
        while (true)
        {


            if (!receptionManager.waitingQueue.bIsQueueFull())
            {
                GameObject gameObject = Instantiate(GetRandomPatients(), playerSpwanPos.position, Quaternion.identity, playerSpwanPos);

                var p = gameObject.GetComponent<Patient>();


                GameObject gameObject_2 = Instantiate(GetRandomAnimalObj(), p.animalFollowPos.position, Quaternion.identity, playerSpwanPos);

                var a = gameObject_2.GetComponent<Animal>();
                //a.player = p.animalFollowPos;

                p.diseaseType = GetRandomDisease();
                p.animal = a;
                p.NPCMovement.Init();
                a.Init();
                receptionManager.waitingQueue.AddInQueue(p);
                p.MoveAnimal();
            }
            else
            {
                StopSpwanPatinet();
                yield break;
            }

            yield return new WaitForSeconds(spwanDalay);

        }
    }
    public GameObject GetRandomAnimalObj()
    {
        int randomIndex = Random.Range(0, AnimalData.animales.Length);
        int ranIndex = Random.Range(0, AnimalData.animales[randomIndex].Animal.Length);
        return AnimalData.animales[randomIndex].Animal[ranIndex];
    }

    #endregion


    #region Disease Machiniacs
    public void UpdateDisease()
    {
        for (int i = 0; i < currntDiseaseIndex; i++)
        {
            AddDisease(Alldiseases[i]);
        }
    }

    public void AddDisease(DiseaseType disease)
    {
        if (!UnlocDiseases.Contains(disease))
        {
            UnlocDiseases.Add(disease);
        }
    }

    public DiseaseType GetRandomDisease()
    {
        if (UnlocDiseases.Count > 0)
        {
            int randomIndex = Random.Range(0, UnlocDiseases.Count);
            return UnlocDiseases[randomIndex];
        }
        else
        {
            AddDisease(DiseaseType.Cough);
            return DiseaseType.Cough;
        }
    }

    #endregion
}
