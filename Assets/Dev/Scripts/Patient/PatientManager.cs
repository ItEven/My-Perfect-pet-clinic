using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class PatientManagerData
{
    public bool bIsUnlock;
    public bool bCanSendPatient;
    public int curentUnlockDisease;
    public List<DiseaseType> diseaseTypes = new List<DiseaseType>();
}

public class PatientManager : MonoBehaviour
{
    public static PatientManager instance;

    [Header("TrafficBehavior Porpertis ")]
    public float spawnTimeDelay;

    public AnimalData animalData;
    public ReceptionManager receptionManager;
    public TrafficBehavior trafficBehavior;

    [Header("LeftSideTraffic Line")]
    public Transform leftSideIn;
    public Transform leftSideOut;

    [Header("RightSideTraffic Line")]
    public Transform rightSideIn;
    public Transform rightSideOut;

    [Header("Patient prefabes")]
    [SerializeField] private GameObject[] patientsPrefabe;

    [Header("Patients")]
    public List<Patient> patients = new List<Patient>();
    public Transform hospitalEntryPoint;
    public float spwanDalay;
    internal bool bIsUnlock;
    internal bool bCanSendPatient = false;

    [Header("Disease Data")]
    public List<DiseaseType> UnlocDiseases = new List<DiseaseType>();
    public int currntDiseaseIndex;
    public List<DiseaseType> Alldiseases;

    private void Awake()
    {
        instance = this;
    }
    #region Initializers

    SaveManager saveManager;
    UiManager uiManager;

    private void OnEnable()
    {
        UpdateInitializers();
    }

    public void UpdateInitializers()
    {

        saveManager = SaveManager.instance;

        uiManager = saveManager.uiManager;
    }

    #endregion
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
            StartTrafficSystem();
            if (bCanSendPatient)
            {
                StartSendingPatinet();
            }
        }
        else
        {
            gameObject.SetActive(false);
        }

    }
    public void StartTrafficSystem()
    {
        trafficBehavior.StartSpwaning();
        StartCoroutine(SpawningPatient());
    }


    #region SpwanPatient
    public GameObject GetRandomPatients()
    {
        int randomIndex = Random.Range(0, patientsPrefabe.Length);
        return patientsPrefabe[randomIndex];
    }

    private Coroutine sendingPatinetCoroutine;

    public void StartSendingPatinet()
    {
        if (sendingPatinetCoroutine == null)
        {
            sendingPatinetCoroutine = StartCoroutine(SendingPatinetOnStart());

        }
    }

    private void StopSpwanPatinet()
    {
        if (sendingPatinetCoroutine != null)
        {
            StopCoroutine(sendingPatinetCoroutine);
            sendingPatinetCoroutine = null;
        }
    }

    private IEnumerator SendingPatinetOnStart()
    {
        while (true)
        {
            if (!receptionManager.waitingQueue)
            {
                Debug.LogError("Reception manager or waiting queue is not properly initialized.");
                yield break;
            }

            if (!receptionManager.waitingQueue.bIsQueueFull())
            {
                SendPatientToHospital();
            }
            else
            {
                StopSpwanPatinet();
                yield break;
            }
            yield return new WaitForSeconds(spwanDalay);
        }
    }

    private IEnumerator SpawningPatient()
    {
        while (true)
        {
            // Spawn patients at each position
            SpawnPatientAt(leftSideIn, leftSideOut, addToPatients: false);
            SpawnPatientAt(leftSideOut, leftSideIn, addToPatients: false);
            SpawnPatientAt(rightSideIn, rightSideOut, addToPatients: true);
            SpawnPatientAt(rightSideOut, rightSideIn, addToPatients: true);

            yield return new WaitForSeconds(spawnTimeDelay);
        }
    }

    private void SpawnPatientAt(Transform startPos, Transform endPos, bool addToPatients)
    {
        // Instantiate patient at the starting position
        GameObject patientObj = Instantiate(GetRandomPatients(), startPos.position, startPos.rotation, startPos);
        Patient patient = patientObj.GetComponent<Patient>();

        if (patient == null)
        {
            Debug.LogWarning("Patient component missing!");

            Destroy(patientObj);
            return;
        }

        // Add to the patients list only for left-side patients
        if (addToPatients)
        {
            patients.Add(patient);
        }

        // Assign random disease and spawn animal

        GameObject animalObj = Instantiate(GetRandomAnimalObj(), patient.RightFollowPos.position, Quaternion.identity, hospitalEntryPoint);
        Animal animal = animalObj.GetComponent<Animal>();

        if (animal != null)
        {
            patient.animal = animal;
            InitializePatientAndAnimal(patient, animal);
        }
        else
        {
            Debug.LogWarning("Animal component missing!");
            Destroy(animalObj);
        }

        // Define movement path and destruction callback
        patient.NPCMovement.MoveToTarget(endPos, () =>
        {
            RemovePatient(patient);
            Destroy(patient.animal.gameObject);
            Destroy(patient.gameObject);
        });
    }

    private void InitializePatientAndAnimal(Patient patient, Animal animal)
    {
        patient.NPCMovement.Init();
        animal.Init();
        patient.MoveAnimal();
    }

    public void SendPatientToHospital()
    {

        Patient patient = GetPatient();
        if (patient != null)
        {
            RemovePatient(patient);
            patient.diseaseType = GetRandomDisease();
            patient.sloganText.text = animalData.GetSlogan(patient.animal.animalType, patient.diseaseType);
            receptionManager.waitingQueue.AddInQueue(patient);
        }
        else
        {
            Debug.Log("Patient is null");
        }
    }

    public Patient GetPatient()
    {

        if (patients == null || patients.Count == 0)
        {
            Debug.LogWarning("No patients available.");
            return null;
        }

        Patient closestPatient = null;
        float closestDistance = float.MaxValue;


        foreach (Patient patient in patients)
        {
            if (patient == null) continue;

            float distance = Vector3.Distance(hospitalEntryPoint.position, patient.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestPatient = patient;
            }
        }

        return closestPatient;
    }
    public void RemovePatient(Patient patient)
    {
        if (patients.Contains(patient))
        {
            patients.Remove(patient);
        }
    }
    public GameObject GetRandomAnimalObj()
    {
        if (animalData.animales == null || animalData.animales.Length == 0)
        {
            Debug.LogWarning("Animal data array is empty or null!");
            return null;
        }
        int randomIndex = Random.Range(0, animalData.animales.Length);

        if (animalData.animales[randomIndex].Animal == null || animalData.animales[randomIndex].Animal.Length == 0)
        {
            Debug.LogWarning($"Animal array at index {randomIndex} is empty or null!");
            return null;
        }

        int ranIndex = Random.Range(0, animalData.animales[randomIndex].Animal.Length);

        return animalData.animales[randomIndex].Animal[ranIndex];
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
            if (disease != DiseaseType.Toy)
            {
                uiManager.AddIllnessesUi(disease.ToString());
            }
            UnlocDiseases.Add(disease);
        }
    }

    int diseaseIndex;
    public DiseaseType GetRandomDisease()
    {
        //if (diseaseIndex < UnlocDiseases.Count)
        //{
        //    diseaseIndex++;
        //    if (diseaseIndex >= UnlocDiseases.Count)
        //    {
        //        diseaseIndex = 0;
        //        return UnlocDiseases[diseaseIndex];

        //    }
        //    else
        //    {

        //        return UnlocDiseases[diseaseIndex];
        //    }

        //}
        if (UnlocDiseases.Count > 0)
        {
            int randomIndex = Random.Range(0, UnlocDiseases.Count);
            return UnlocDiseases[randomIndex];
        }
        else
        {
            //diseaseIndex = 0;
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
        data.bCanSendPatient = bCanSendPatient;
        data.diseaseTypes.AddRange(UnlocDiseases);
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
        bCanSendPatient = receivefile.bCanSendPatient;
        currntDiseaseIndex = receivefile.curentUnlockDisease;
        UnlocDiseases = new List<DiseaseType>(receivefile.diseaseTypes);
        foreach (var item in UnlocDiseases)
        {
            if (item != DiseaseType.Toy)
            {
                uiManager.AddIllnessesUi(item.ToShortString());
            }
        }
        //UpdateDisease();
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
