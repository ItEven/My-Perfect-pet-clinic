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
        yield return new WaitForSeconds(spwanDalay);

        if (receptionManager.waitingQueue.bIsQueueFull())
        {
            GameObject gameObject = Instantiate(GetRandomPatients(), playerSpwanPos.position, Quaternion.identity, playerSpwanPos);

            var p = gameObject.GetComponent<Patient>();


            GameObject gameObject_2 = Instantiate(GetRandomAnimalObj(), animalSpwanPos.position, Quaternion.identity, animalSpwanPos);

            var a = gameObject_2.GetComponent<Animal>();
            a.player = p.animalFollowPos;


            receptionManager.waitingQueue.AddInQueue(p);
        }
        else
        {
            StopSpwanPatinet();
            yield break;
        }

    }
    #endregion


    public GameObject GetRandomAnimalObj()
    {
        int randomIndex = Random.Range(0, AnimalData.animales.Length);
        int ranIndex = Random.Range(0, AnimalData.animales[randomIndex].Animal.Length);
        return AnimalData.animales[randomIndex].Animal[ranIndex];
    }

    
}
