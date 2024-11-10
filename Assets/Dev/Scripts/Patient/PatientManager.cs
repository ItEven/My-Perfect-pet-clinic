using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using Random = UnityEngine.Random;

public class PatientManagerData
{
    public bool bIsUnlock;
    public int curentUnlockDisease;
}
public class PatientManager : MonoBehaviour
{
    public static PatientManager instance;

    public AnimalData AnimalData;
    public ReceptionManager receptionManager;

    [SerializeField] private GameObject[] patients;

    public Transform playerSpwanPos;
    public Transform animalSpwanPos;
    public float spwanDalay;

    internal bool bIsUnlock;





    [Header("Disease Data")]
    public List<DiseaseType> UnlocDiseases;
    public int currntDiseaseIndex;
    public List<DiseaseType> Alldiseases;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (PlayerPrefs.HasKey("PatientManager"))
        {
            LoadSaveData();
        }
        else
        {
            LoadData();
        }

    }

    public void LoadData()
    {
        if (bIsUnlock)
        {
            StartSpwanPatinet();
        }
        else
        {
            gameObject.SetActive(false);
        }
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

    public void StartSpwanPatinet()
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
                GameObject randomPatientPrefab = GetRandomPatients();
                if (randomPatientPrefab != null)
                {
                    GameObject gameObject = Instantiate(randomPatientPrefab, playerSpwanPos.position, Quaternion.identity, playerSpwanPos);

                    Patient p = gameObject.GetComponent<Patient>();
                    if (p != null)
                    {
                        GameObject randomAnimalPrefab = GetRandomAnimalObj();
                        if (randomAnimalPrefab != null)
                        {
                            if (p.animalFollowPos != null)
                            {

                                GameObject gameObject_2 = Instantiate(randomAnimalPrefab, p.animalFollowPos.position, Quaternion.identity, playerSpwanPos);

                                Animal a = gameObject_2.GetComponent<Animal>();
                                if (a != null)
                                {
                                    p.diseaseType = GetRandomDisease();
                                    p.animal = a;
                                    p.NPCMovement.Init();
                                    a.Init();
                                    receptionManager.waitingQueue.AddInQueue(p);
                                    p.MoveAnimal();
                                }
                                else
                                {
                                    Debug.LogWarning("Animal component missing!");
                                }
                            }
                            else
                            {
                                Destroy(p.gameObject);
                                Debug.LogWarning(" animalFollowPos is null!");
                            }
                        }
                        else
                        {
                            Destroy(p.gameObject);
                            Debug.LogWarning("Animal prefab is null ");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Patient component missing!");
                    }
                }
                else
                {
                    Debug.LogWarning("Patient prefab is null!");
                }
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
        if (AnimalData.animales == null || AnimalData.animales.Length == 0)
        {
            Debug.LogWarning("Animal data array is empty or null!");
            return null;
        }

        int randomIndex = Random.Range(0, AnimalData.animales.Length);

        if (AnimalData.animales[randomIndex].Animal == null || AnimalData.animales[randomIndex].Animal.Length == 0)
        {
            Debug.LogWarning($"Animal array at index {randomIndex} is empty or null!");
            return null;
        }

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


    #region Data Functions

    public void SaveData()
    {
        PatientManagerData data = new PatientManagerData();

        data.bIsUnlock = bIsUnlock;
        data.curentUnlockDisease = UnlocDiseases.Count;

        string JsonData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString("PatientManager", JsonData);
    }
    public void LoadSaveData()
    {
        string JsonData = PlayerPrefs.GetString("PatientManager", string.Empty);
        if (string.IsNullOrEmpty(JsonData))
        {
            // Handle the case where no data has been saved yet
            return;
        }
        PatientManagerData receivefile = JsonUtility.FromJson<PatientManagerData>(JsonData);
        bIsUnlock = receivefile.bIsUnlock;
        currntDiseaseIndex = receivefile.curentUnlockDisease;
        UpdateDisease();
        LoadData();

    }

    void OnApplicationQuit()
    {
        SaveData();
    }
    void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            SaveData();
        }
    }
    #endregion

}
